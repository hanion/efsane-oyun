using efsane_oyun.Data;
using efsane_oyun.Data.Migrations;
using efsane_oyun.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace efsane_oyun.Controllers
{
    public class GameController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GameController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            string? game_id_string = HttpContext.Request.Query["i"];
            
            int game_id;
            if (string.IsNullOrEmpty(game_id_string) || !int.TryParse(game_id_string, out game_id))
            {
                return RedirectToAction("Index");
            }

            var game_detail = _context.Games.Find(game_id);
            if (game_detail == null) {
                return RedirectToAction("Index");
            }


            
            ViewBag.Id          = game_detail.Id;
            ViewBag.Title       = game_detail.Title;
            ViewBag.Description = game_detail.Description;
            ViewBag.CoverImage  = game_detail.CoverImage;
                    
                    
            ViewBag.EmbedValid  = !string.IsNullOrEmpty(game_detail.EmbedSource);
            ViewBag.EmbedSource = game_detail.EmbedSource;


            ViewBag.Tags = _context.GameTags
                .Where(gt => gt.GameId == game_detail.Id)
                .Join(
                    _context.Tags,
                    combined => combined.TagId,
                    tag => tag.Id,
                    (combined, tag) => new { Id = tag.Id, Tag = tag.TagLabel }
                )
                .ToList();


            // Retrieve comments with user information
            var game_comments = _context.Comments
                .Where(comment => comment.GameId == game_id)
                .Join(
                    _context.Users,
                    comment => comment.UserId,
                    user => user.Id,
                    (comment, user) => new CommentWithUser
                    {
                        Comment = comment,
                        UserName = user.UserName
                    })
                .ToList();

            ViewBag.UserComments = game_comments;


            var game_ratings = _context.Ratings.Where(r => r.GameId == game_id).ToList();
            ViewBag.AverageRating = game_ratings.Any() ? game_ratings.Average(r => r.Score) : 0;



            
            // Retrieve the user's rating for the game, if available
            if (User.Identity.IsAuthenticated)
            {
                string user_id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                ViewBag.UserRating = _context.Ratings
                    .Where(r => r.GameId == game_id && r.UserId == user_id)
                    .Select(r => r.Score)
                    .FirstOrDefault();

            }


            return View();
        }

        public class CommentWithUser {
            public Comments Comment { get; set; }
            public string UserName { get; set; }
        }


        [HttpPost]
        public IActionResult AddComment(int game_id, string content)
        {
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated) {
                // Get the user ID
                string user_id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Create a new comment
                Comments comment = new Comments {
                    GameId  = game_id,
                    UserId  = user_id,
                    Content = content,
                };

                // Add the comment to the database
                _context.Comments.Add(comment);
                _context.SaveChanges();
            }

            // Redirect back to the game page
            return RedirectToAction("Index","Game", new { i = game_id });
        }

        [HttpPost]
        public IActionResult RateGame(int game_id, int user_rating)
        {
            if (User.Identity.IsAuthenticated)
            {
                string user_id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Check if the user has already rated the game
                var existing_rating = _context.Ratings
                    .FirstOrDefault(r => r.GameId == game_id && r.UserId == user_id);

                if (existing_rating != null)
                {
                    // Update the existing rating
                    existing_rating.Score = user_rating;
                }
                else
                {
                    // Create a new rating
                    Ratings rating = new Ratings
                    {
                        GameId = game_id,
                        UserId = user_id,
                        Score  = user_rating
                    };

                    _context.Ratings.Add(rating);
                }

                _context.SaveChanges();
            }

            // Redirect back to the game page
            return RedirectToAction("Index", "Game", new { i = game_id });
        }

    }
}
