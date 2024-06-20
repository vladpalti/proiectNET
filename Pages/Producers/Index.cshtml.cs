using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using proiect.Data;
using proiect.Models;
using proiect.Models.ViewModels;

namespace proiect.Pages.Producers
{
    public class IndexModel : PageModel
    {
        private readonly proiect.Data.proiectContext _context;

        public IndexModel(proiect.Data.proiectContext context)
        {
            _context = context;
        }

        public IList<Producer> Producer { get;set; } = default!;
        public ProducerIndexData PublisherData { get; set; }
        public int ProducerID { get; set; }
        public int MovieID { get; set; }


        public async Task OnGetAsync(int? id, int? movieID)
        {
            PublisherData = new ProducerIndexData();
            PublisherData.Producers = await _context.Producer
            .Include(i => i.Movies)
                .ThenInclude(c => c.Director)
            .OrderBy(i => i.ProducerName)
            .ToListAsync();

            if (id != null)
            {
                ProducerID = id.Value;
                Producer producer = PublisherData.Producers
                     .Where(i => i.ID == id.Value).Single();
                PublisherData.Movies = producer.Movies;
            }
        }
    }
}
