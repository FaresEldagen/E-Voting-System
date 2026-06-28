using E_Voting_System.Entities;
using E_Voting_System.Models;
using E_Voting_System.Services.Interfaces;
using E_Voting_System.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace E_Voting_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ISendImageService _sendImageService;

        public HomeController(ILogger<HomeController> logger, SignInManager<User> signInManager, UserManager<User> userManager, ISendImageService sendImageService)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _sendImageService = sendImageService;
        }

        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Vote");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("Index", vm);

            try
            {
                // In the future, this is where you would process the ID card using the AI model
                // and extract the ID number. For now, we simulate finding or creating the user.
                
                string userId =await _sendImageService.SendImageAsync(vm.IdImage,vm.SelfieImage,threshold:.45);

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    user = new User { Id = userId, UserName = userId, Vote = 0 };
                    var createResult = await _userManager.CreateAsync(user);
                    if (!createResult.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, string.Join(", ", createResult.Errors.Select(e => e.Description)));
                        return View("Index", vm);
                    }
                }

                // Securely sign in the user using Identity (encrypted claim-based cookie)
                await _signInManager.SignInAsync(user, isPersistent: true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Index", vm);
            }

            return RedirectToAction("Index", "Vote");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }


        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
