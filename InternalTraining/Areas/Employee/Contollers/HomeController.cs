using InternalTraining.Models;
using InternalTraining.Unit_of_Work;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InternalTraining.Areas.Employee.Contollers
{
    [Area("Employee")]

    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var empCourse = unitOfWork.EmployeeCourses.Get(e => e.IsAccepted == true, includes: [e => e.Course]);
            return View(empCourse.ToList());
        }

        public IActionResult Inviations()
        {
            var currentUser = userManager.GetUserId(User);
            var userInfo = unitOfWork.ApplicationUsers.GetOne(e => e.Id == currentUser);
            var userEmail = userInfo.Email;

            var allInvitations = unitOfWork.CourseInvitations.Get(e => e.Email.ToLower() == userEmail.ToLower() && e.Paid == true, includes: [e => e.Course]);
            return View(allInvitations.ToList());
        }

        public IActionResult AcceptInvitaion(int id)
        {
            var invitation = unitOfWork.CourseInvitations.GetOne(e => e.Id == id && e.Paid == true);
            invitation.IsAccepted = true;
            unitOfWork.CourseInvitations.Update(invitation);

            var emp = unitOfWork.EmployeeCourses.GetOne(e => e.EmployeeUser.ApplicationUser.Email.ToLower() == invitation.Email.ToLower());
            emp.IsAccepted = true;
            unitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public IActionResult CourseContent(int id)
        {
            var currentUserId = userManager.GetUserId(User);
            var selectedCourse = unitOfWork.Courses.GetOne(e => e.Id == id, includes: [e => e.Chapters, e => e.Lessons]);
            var courseChapters = unitOfWork.Chapters.Get(e => e.CourseId == id);
            var courseLessonse = unitOfWork.Lessons.Get(e => e.CourseId == id);
            ViewBag.Chapters = courseChapters;
            ViewBag.Lessons = courseLessonse;

            var completedLessonIds = unitOfWork.EmployeeLessonsProgress
                                .Get(e => e.EmployeeUserId == currentUserId && e.IsCompleted && courseLessonse.Select(l => l.Id).Contains(e.LessonId))
                                .Select(e => e.LessonId)
                                .ToList();

            ViewBag.CompletedLessonIds = completedLessonIds;

            return View(selectedCourse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult MarkLessonCompleted(int lessonId)
        {
            var currentUserId = userManager.GetUserId(User);
            var selectedlesson = unitOfWork.EmployeeLessonsProgress.GetOne(e => e.EmployeeUserId == currentUserId && e.LessonId == lessonId);

            if (selectedlesson == null)
            {
                var lesson = unitOfWork.Lessons.GetOne(e => e.Id == lessonId);

                var progress = new EmployeeLessonProgress
                {
                    EmployeeUserId = currentUserId,
                    LessonId = lessonId,
                    IsCompleted = true,
                    CompletedOn = DateTime.Now
                };

                unitOfWork.EmployeeLessonsProgress.Create(progress);

                // Update or create EmployeeProgress summary
                var courseId = lesson.CourseId;
                var courseProgress = unitOfWork.EmployeesProgress.GetOne(e => e.EmployeeUserId == currentUserId && e.CourseId == courseId);

                if (courseProgress != null)
                {
                    courseProgress.LessonsCompleted++;
                    unitOfWork.EmployeesProgress.Update(courseProgress);
                }
                else
                {
                    unitOfWork.EmployeesProgress.Create(new EmployeeProgress
                    {
                        EmployeeUserId = currentUserId,
                        CourseId = courseId,
                        LessonsCompleted = 1,
                        ExamScore = 0
                    });
                }

                unitOfWork.Commit();
            }

            return Json(new { success = true });
        }

    }
}
