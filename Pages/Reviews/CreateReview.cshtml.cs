using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using proiect.Data;
using proiect.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using proiect.Areas.Identity.Data;

namespace proiect.Pages.Reviews
{
    public class CreateReviewModel : PageModel
    {
        private readonly proiectContext _context;
        private readonly LibraryIdentityContext _identityContext;
        private readonly UserManager<IdentityUser> _userManager;

        public string UserId { get; set; }

        public CreateReviewModel(proiectContext context, LibraryIdentityContext identityContext, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _identityContext = identityContext;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));  // Null check to catch if it's not injected
        }

        [BindProperty]
        public Review Review { get; set; }

        public List<SelectListItem> MovieSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Get the current logged-in user's ID
            UserId = _userManager.GetUserId(User);

            // Populate the movie selection list
            MovieSelectList = await _context.Movie
                .Select(m => new SelectListItem
                {
                    Value = m.ID.ToString(),
                    Text = m.Title
                }).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("OnPostAsync triggered!");

            // Get current user ID
            var userId = _userManager.GetUserId(User);
            Console.WriteLine($"Current User ID: {userId}");

            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError(string.Empty, "User is not logged in.");
                Console.WriteLine("User is not logged in.");
                await PopulateDropdownAsync(); // Repopulate dropdown
                return Page();
            }

            // Assign the User and UserId fields
            Review.UserId = userId;

            var currentUser = await _userManager.FindByIdAsync(userId);
            if (currentUser != null)
            {
                Review.User = currentUser.UserName; // Assign username (email)
                Console.WriteLine($"User found: {currentUser.UserName}");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "User not found.");
                Console.WriteLine("User not found.");
                await PopulateDropdownAsync(); // Repopulate dropdown
                return Page();
            }

            // Ensure Movie is assigned
            var movie = await _context.Movie.FirstOrDefaultAsync(m => m.ID == Review.MovieID);
            if (movie != null)
            {
                Review.Movie = movie.Title;
                Console.WriteLine($"Movie found: {movie.Title}, MovieID: {movie.ID}");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Movie not found.");
                Console.WriteLine("Movie not found.");
                await PopulateDropdownAsync(); // Repopulate dropdown
                return Page();
            }

            // Revalidate the model after setting User and Movie
            if (!TryValidateModel(Review, nameof(Review)))
            {
                Console.WriteLine("Model validation failed after setting User and Movie.");
                ModelState.Remove("Review.User");
                ModelState.Remove("Review.Movie");
                if (!TryValidateModel(Review))
                {
                    foreach (var entry in ModelState)
                    {
                        foreach (var error in entry.Value.Errors)
                        {
                            Console.WriteLine($"Key: {entry.Key}, Error: {error.ErrorMessage}");
                        }
                    }
                    await PopulateDropdownAsync(); // Repopulate dropdown
                    return Page();
                }
            }

            Console.WriteLine($"Review: UserId={Review.UserId}, User={Review.User}, MovieID={Review.MovieID}, Movie={Review.Movie}, Comment={Review.Comment}, Rating={Review.Rating}");

            _context.Review.Add(Review);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Movies/Details", new { id = Review.MovieID });
        }

        // Helper method to populate the dropdown list
        private async Task PopulateDropdownAsync()
        {
            MovieSelectList = await _context.Movie
                .Select(m => new SelectListItem
                {
                    Value = m.ID.ToString(),
                    Text = m.Title
                }).ToListAsync();
            Console.WriteLine("Dropdown list repopulated.");
        }
    }
}