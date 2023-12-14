using efsane_oyun.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using efsane_oyun.Data;
using static efsane_oyun.Controllers.GameController;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace efsane_oyun.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        
        public async Task<IActionResult> Index(string Sort, int Tag)
        {
            var games = await _context.Games.ToListAsync();
            var ratings = await _context.Ratings.ToListAsync();
            switch (Sort)
            {
                case "HighestRated":
                    var games_with_average_rating = games.Select(game => new {
                        Game = game,
                        AverageRating = _context.Ratings
                            .Where(r => r.GameId == game.Id)
                            .Average(r => r.Score)
                    });

                    games = games_with_average_rating
                        .OrderByDescending(g => g.AverageRating)
                        .Select(g => g.Game)
                        .ToList();

                    break;

                case "Popular":
                    var games_with_popularity = games.Select(game => new {
                        Game = game,
                        RatingsCount = _context.Ratings.Count(r => r.GameId == game.Id),
                        CommentsCount = _context.Comments.Count(c => c.GameId == game.Id)
                    });

                    games = games_with_popularity
                        .OrderByDescending(g => g.RatingsCount + g.CommentsCount)
                        .Select(g => g.Game)
                        .ToList();

                    break;

                default:
                    games = games.OrderBy(g => g.Id).ToList();
                    break;
            }


            if (Tag != 0) {
                ViewBag.TagFilter = Tag;
                games = games
                .Join(
                    _context.GameTags,
                    game => game.Id,
                    tag => tag.GameId,
                    (game, tag) => new { Game = game, TagId = tag.TagId }
                )
                .Where(item => item.TagId == Tag)
                .Select(item => item.Game)
                .ToList();
            }

            //var user = await _userManager.GetUserAsync(User);

            //if (user != null)
            //{
            //    // Get the roles for the user
            //    var userRoles = await _userManager.GetRolesAsync(user);

            //    // Check if "Moderator" role is present
            //    if (userRoles.Contains("Moderator"))
            //    {
            //        // User has the "Moderator" role
            //        return View();
            //    }
            //    else
            //    {
            //        // User does not have the required role
            //        return Forbid(); // Or return an access denied view
            //    }
            //}
            //else
            //{
            //    // User is not authenticated
            //    return RedirectToAction("Login"); // Redirect to login page
            //}

            return View(games);
        }

        //public class GameWithRating
        //{
        //    public Games Game { get; set; }
        //    public int Rating { get; set; }
        //}


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
