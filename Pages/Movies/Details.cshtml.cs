using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proiect.Areas.Identity.Data;
using proiect.Models;

namespace proiect.Pages.Movies
{
    public class DetailsModel : PageModel
    {
        private readonly LibraryIdentityContext _identityContext;
        private readonly proiect.Data.proiectContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DetailsModel(proiect.Data.proiectContext context, LibraryIdentityContext identityContext, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _identityContext = identityContext;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public Movie Movie { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (movie == null)
            {
                return NotFound();
            }
            else
            {
                Movie = movie;
            }

            var userId = _userManager.GetUserId(User); // Debugging the current user's ID
            Console.WriteLine($"Current User ID: {userId}"); // Output to console

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteReviewAsync(int reviewId, int movieId)
        {
            var review = await _context.Review.FindAsync(reviewId);

            if (review == null)
            {
                return NotFound();
            }

            _context.Review.Remove(review);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Movies/Details", new { id = movieId });
        }
    }
}
