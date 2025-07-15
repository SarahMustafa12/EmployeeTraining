using InternalTraining.Models.ViewModel;
using InternalTraining.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using InternalTraining.Email_Sender;
using System.Security.Claims;

namespace InternalTraining.Areas.Identity.Controllers
{
    [Area("Identity")]
    //[Authorize (Roles ="Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender emailSender;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._roleManager = roleManager;
            this.emailSender = emailSender;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (_roleManager.Roles.IsNullOrEmpty())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Company"));
                await _roleManager.CreateAsync(new IdentityRole("Employee"));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVm registerVm)
        {

            if (ModelState.IsValid)
            {
                ApplicationUser appUser = new ApplicationUser
                {
                    UserName = registerVm.UserName,
                    Email = registerVm.Email,
                    ProfilePicturePath = "undraw_profile.svg"
                };

                var IsExist = await userManager.FindByEmailAsync(registerVm.Email);
                  if (IsExist != null) 
                  {
                    ModelState.AddModelError("Email", "Email is already taken.");
                    return View(registerVm);

                }

                var result = await userManager.CreateAsync(appUser, registerVm.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(appUser, false);
                    await userManager.AddToRoleAsync(appUser,"Admin");
                    return RedirectToAction("Login", "Account", new { area = "Identity" });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

            }
            return View(registerVm);
        }

        //public IActionResult Login()
        //{
        //    return View();
        //}

        public IActionResult Login()
        {
            var model = new LoginVm();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVm loginVm)
        {
            if (ModelState.IsValid)
            {
                var appUser = await userManager.FindByEmailAsync(loginVm.Email);

                if (appUser != null)
                {
                    var result = await userManager.CheckPasswordAsync(appUser, loginVm.Password);
                    if (result)
                    {
                        
                            var isInSelectedRole = await userManager.IsInRoleAsync(appUser, loginVm.UserType);
                            if (!isInSelectedRole)
                            {
                                ModelState.AddModelError("UserType", $"You are not authorized as {loginVm.UserType}.");
                                return View(loginVm);
                            }

                            await signInManager.SignInAsync(appUser, loginVm.RememberMe);

                            // ✅ Redirect based on user type
                            switch (loginVm.UserType)
                            {
                                case "Admin":
                                    return RedirectToAction("Index", "Home", new { area = "Admin" });

                                case "Company":
                                    return RedirectToAction("Index", "Home", new { area = "Company2" });

                                case "Employee":
                                    return RedirectToAction("Index", "Home", new { area = "Employee" });

                                default:
                                    // Optional: fallback to home if role isn't handled
                                    return RedirectToAction("Index", "Home", new { area = "Company" });
                            }
                        

                        // ✅ Redirect based on selected user type
                        //switch (loginVm.UserType)
                        //{
                        //    case "Admin":
                        //        return RedirectToAction("Index", "Home", new { area = "Admin" });

                        //    case "Company":
                        //        return RedirectToAction("Index", "Home", new { area = "Company" });

                        //    case "Employee":
                        //        return RedirectToAction("Index", "Home", new { area = "Employee" });

                        //    default:
                        //        ModelState.AddModelError("UserType", "Invalid user type selected.");
                        //        break;
                        //}
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Incorrect password.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Email address not found.");
                }
            }

            // If something fails, return the same view with validation errors
            return View(loginVm);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginVm loginVm)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var appUser = await userManager.FindByEmailAsync(loginVm.Email);

        //        if (appUser != null)
        //        {
        //            var result = await userManager.CheckPasswordAsync(appUser, loginVm.Password);
        //            if (result)
        //            {
        //                await signInManager.SignInAsync(appUser, loginVm.RememberMe);
        //                return RedirectToAction("Index", "Home", new { area = "Admin" });
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("Email", "Cannot Found The Email");
        //                ModelState.AddModelError("Password", "Donot Match The Password");
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("Email", "Cannot Found The Email");
        //            ModelState.AddModelError("Password", "Donot Match The Password");

        //        }
        //    }
        //    return View(loginVm);
        //}
        [HttpGet]
        public IActionResult ExternalLogin(string provider)
        {
            // إعداد رابط العودة بعد إتمام المصادقة الخارجية
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account");

            // إعداد خصائص المصادقة الخارجية
            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            // بدء عملية المصادقة مع مزود الخدمة الخارجي (مثل جوجل)
            return Challenge(properties, provider);
        }
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View("Login");
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Try to sign in the user with the external login
            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (result.Succeeded)
            {
                // ✅ Get the user
                var user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                if (user != null)
                {
                    // ✅ Get role
                    var role = (await userManager.GetRolesAsync(user)).FirstOrDefault();

                    // ✅ Redirect based on role
                    if (role == "Admin")
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    else if (role == "Company")
                        return RedirectToAction("Index", "Home", new { area = "Company2" });
                    else if (role == "Employee")
                        return RedirectToAction("Index", "Home", new { area = "Employee" });

                    // fallback
                    return RedirectToAction("Index", "Home");
                }
            }

            // ✅ If user doesn't exist, create a new one
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var userByEmail = await userManager.FindByEmailAsync(email);

            if (userByEmail == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };

                var createResult = await userManager.CreateAsync(newUser);
                if (createResult.Succeeded)
                {
                    // ✅ Optional: assign default role
                    await userManager.AddToRoleAsync(newUser, "Admin"); // or "Employee" etc.

                    // ✅ Link Google login to the user
                    await userManager.AddLoginAsync(newUser, info);

                    // ✅ Sign in and redirect
                    await signInManager.SignInAsync(newUser, isPersistent: false);
                    return RedirectToAction("Index", "Home", new { area = "Admin" }); // match the role you assigned
                }
                else
                {
                    // Show registration error
                    foreach (var error in createResult.Errors)
                        ModelState.AddModelError("", error.Description);
                    return View("Register");
                }
            }

            // ✅ If user exists by email but not linked, link it
            await userManager.AddLoginAsync(userByEmail, info);
            await signInManager.SignInAsync(userByEmail, isPersistent: false);

            var userRole = (await userManager.GetRolesAsync(userByEmail)).FirstOrDefault();
            if (userRole == "Admin")
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            else if (userRole == "Company")
                return RedirectToAction("Index", "Home", new { area = "Company2" });
            else if (userRole == "Employee")
                return RedirectToAction("Index", "Home", new { area = "Employee" });

            return RedirectToAction("Index", "Home");
        }
      
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "Home", new { area = "Company" });
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var appUser = await userManager.GetUserAsync(User);
            if (appUser == null)
            {
                ModelState.AddModelError("", "Sorry Something is Wrong");
            }

            var profileInfo = new ProfileVm
            {
                Id = appUser.Id,
                UserName = appUser.UserName,
            };
            return View(profileInfo);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileVm profileVm, IFormFile? file)
        {
            var user = await userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    if (file != null && file.Length > 0)
                    {
                        // Delete old image if it exists
                        if (!string.IsNullOrEmpty(user.ProfilePicturePath))
                        {
                            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/admin", user.ProfilePicturePath);
                            if (System.IO.File.Exists(oldPath))
                            {
                                System.IO.File.Delete(oldPath);
                            }
                        }

                        // Upload new image
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images/admin", fileName);

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await file.CopyToAsync(stream);
                        }

                        user.ProfilePicturePath = fileName;
                    }

                    // Update other fields
                    user.UserName = profileVm.UserName;
                    user.FirstName = profileVm.FirstName;
                    user.LastName = profileVm.LastName;

                    // Only update password if needed
                    if (profileVm.NewPassword!= null && profileVm.NewPassword == profileVm.ConfirmPassword)
                    {
                        var result = await userManager.ChangePasswordAsync(user, profileVm.CurrentPassword, profileVm.NewPassword);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                            ViewBag.ProfileImage = user.ProfilePicturePath;
                            return View(profileVm);
                        }
                    }

                    // Update user in DB
                    await userManager.UpdateAsync(user);

                    var role = (await userManager.GetRolesAsync(user)).FirstOrDefault(); ;
                    if (role == "Admin")
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    else if (role == "Company")
                    {
                        return RedirectToAction("Index", "Home", new { area = "Company2" });
                    }
                    else if (role == "Employee")
                    {
                        return RedirectToAction("Index", "Home", new { area = "Employee" });
                    }
                   
                }
            }

            ViewBag.ProfileImage = user?.ProfilePicturePath;
            return View(profileVm);
        }

        [HttpGet]
        public async Task<IActionResult> ForgetPassword()
        {

            return View(new ForgetPasswordVm());  
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVm model)
        {

            if (!ModelState.IsValid) return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "The Email Is Wrong!");
                return View();
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var link = Url.Action("ResetPassword", "Account", new { token, email = model.Email }, Request.Scheme);

            var body = $"Click the link to reset your password: <a href='{link}'>Reset Password</a>";
            await emailSender.SendEmailAsync(model.Email, "Reset Password", body);

            ModelState.AddModelError("Email", "Please Check Your Email");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var model = new ResetPasswordVm
            {
                Token = token,
                Email = email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVm resetPasswordVm)
        {
            if (!ModelState.IsValid) 
                return View(resetPasswordVm);
            var user = await userManager.FindByEmailAsync(resetPasswordVm.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Something went wrong. Please try again.");
                return View(resetPasswordVm);
            }
            var result = await userManager.ResetPasswordAsync(user, resetPasswordVm.Token, resetPasswordVm.ConfirmPassword);
            if (result.Succeeded)
            {
                
                return RedirectToAction("ResetPasswordConfirmation");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(resetPasswordVm);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation() // this if a page that display a success reset pass.
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetUserProfile()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            return Json(new
            {
                profilePicture = user.ProfilePicturePath
            });
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

