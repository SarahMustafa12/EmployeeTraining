using InternalTraining.Data.Enums;
using InternalTraining.Models;
using InternalTraining.Unit_of_Work;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Linq.Expressions;

namespace InternalTraining.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ExamController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public ExamController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var AllExams = unitOfWork.Exams.Get(includes: [e => e.Chapter, e => e.Chapter.Course]);

            return View(AllExams.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            var allCourses = unitOfWork.Courses.Get().ToList();
            var allChapters = unitOfWork.Chapters.Get(e => e.Id != e.Exam.ChapterId, includes: [e => e.Exam]).ToList();


            ViewBag.Chapters = allChapters;
            ViewBag.Courses = allCourses;

            return View();
        }
       
        [HttpPost]
        public IActionResult Create(Exam exam)
        {
            if (ModelState.IsValid)
            {
                var newExam = new Exam
                {
                    Name = exam.Name,
                    ChapterId = exam.ChapterId
                };

                unitOfWork.Exams.Create(newExam);
                unitOfWork.Commit();

                foreach (var question in exam.Questions)
                {
                    var newQuestion = new Question
                    {
                        Text = question.Text,
                        QuestionType = question.QuestionType,
                        CorrectAnswer = question.CorrectAnswer,
                        ExamId = newExam.Id
                    };
                    unitOfWork.Questions.Create(newQuestion);
                    unitOfWork.Commit();

                    foreach (var option in question.Options)
                    {
                        var newOption = new Option
                        {
                            Text = option.Text,
                            QuestionId = newQuestion.Id
                        };
                        unitOfWork.Options.Create(newOption);
                    }
                }

                unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            var allCourses = unitOfWork.Courses.Get().ToList();
            var allChapters = unitOfWork.Chapters.Get().ToList();

            ViewBag.Chapters = allChapters;
            ViewBag.Courses = allCourses;
            return View(exam);
        }


        [HttpGet]
        public IActionResult GetChaptersByCourse(int courseId, int? currentChapterId = null)
        {
            var chapters = unitOfWork.Chapters.Get(
                 e => e.CourseId == courseId && (e.Exam == null || e.Id == currentChapterId),
                includes: new[] { (Expression<Func<Chapter, object>>)(e => e.Exam) }
            )
            .OrderBy(c => c.Name) // ممكن تغيّره لـ .OrderBy(c => c.Number) لو فيه ترتيب رقمي
            .Select(c => new { c.Id, c.Name })
            .ToList();

            return Json(chapters);
        }

        [HttpGet]
        public IActionResult Edit(int id)

        {
            var exam = unitOfWork.Exams.GetOne(e => e.Id == id, includes: [e=>e.Chapter, e=>e.Chapter.Course]);
            if (exam == null) return NotFound();

            var questions = unitOfWork.Questions.Get(q => q.ExamId == id).ToList();
            foreach (var question in questions)
            {
                question.Options = unitOfWork.Options.Get(o => o.QuestionId == question.Id).ToList();
            }

            exam.Questions = questions;

            var allCourses = unitOfWork.Courses.Get().ToList();
            var allChapters = unitOfWork.Chapters.Get(e=>e.Exam == null || e.Exam.Id == id).ToList();

            ViewBag.Courses = allCourses;
            ViewBag.Chapters = allChapters;

            return View(exam);
        }

        [HttpPost]
        public IActionResult Edit(Exam exam)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Courses = unitOfWork.Courses.Get().ToList();
                ViewBag.Chapters = unitOfWork.Chapters.Get().ToList();
                return View(exam);
            }

            var existingExam = unitOfWork.Exams.GetOne(e => e.Id == exam.Id, includes: [e => e.Chapter, e => e.Chapter.Course]);
            if (existingExam == null)
                return NotFound();

            // Update exam fields
            existingExam.Name = exam.Name;
            existingExam.ChapterId = exam.ChapterId;
            unitOfWork.Exams.Update(existingExam);
            unitOfWork.Commit();

            // Load existing questions from DB
            var existingQuestions = unitOfWork.Questions.Get(q => q.ExamId == exam.Id).ToList();

            // Delete removed questions
            foreach (var dbQ in existingQuestions)
            {
                if (!exam.Questions.Any(q => q.Id == dbQ.Id))
                {
                    var dbOptions = unitOfWork.Options.Get(o => o.QuestionId == dbQ.Id).ToList();
                    foreach (var opt in dbOptions)
                    {
                        unitOfWork.Options.Delete(opt);
                    }

                    unitOfWork.Questions.Delete(dbQ);
                }
            }

            // Process submitted questions
            foreach (var question in exam.Questions)
            {
                if (question.Id == 0)
                {
                    // New Question
                    var newQuestion = new Question
                    {
                        Text = question.Text,
                        QuestionType = question.QuestionType,
                        CorrectAnswer = question.CorrectAnswer,
                        ExamId = exam.Id
                    };

                    unitOfWork.Questions.Create(newQuestion);
                    unitOfWork.Commit();

                    foreach (var option in question.Options)
                    {
                        var newOption = new Option
                        {
                            Text = option.Text,
                            QuestionId = newQuestion.Id
                        };
                        unitOfWork.Options.Create(newOption);
                    }
                }
                else
                {
                    // Existing Question
                    var dbQuestion = existingQuestions.FirstOrDefault(q => q.Id == question.Id);
                    if (dbQuestion != null)
                    {
                        dbQuestion.Text = question.Text;
                        dbQuestion.QuestionType = question.QuestionType;
                        dbQuestion.CorrectAnswer = question.CorrectAnswer;
                        unitOfWork.Questions.Update(dbQuestion);

                        // Delete old options
                        var dbOptions = unitOfWork.Options.Get(o => o.QuestionId == dbQuestion.Id).ToList();
                        foreach (var opt in dbOptions)
                        {
                            unitOfWork.Options.Delete(opt);
                        }

                        // Add updated options
                        foreach (var option in question.Options)
                        {
                            var newOption = new Option
                            {
                                Text = option.Text,
                                QuestionId = dbQuestion.Id
                            };
                            unitOfWork.Options.Create(newOption);
                        }
                    }
                }
            }

            unitOfWork.Commit();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var selectedExam = unitOfWork.Exams.GetOne(e => e.Id == id);
            if (selectedExam != null)
            {
                unitOfWork.Exams.Delete(selectedExam);
                unitOfWork.Commit();

            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult ShowExam(int id)
        {
            var exam = unitOfWork.Exams.GetOne(e => e.Id == id, includes: [e => e.Chapter, e => e.Chapter.Course]);

            if (exam == null)
                return NotFound();

            var questions = unitOfWork.Questions.Get(q => q.ExamId == id).ToList();

            foreach (var question in questions)
            {
                question.Options = unitOfWork.Options.Get(o => o.QuestionId == question.Id).ToList();
            }

            exam.Questions = questions;

            return View(exam);

        }

    }
}
