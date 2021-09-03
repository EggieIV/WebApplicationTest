using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationTest.Data;
using WebApplicationTest.Models;
using WebApplicationTest.Helper;

namespace WebApplicationTest.Controllers
{
    public class DailyQuotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DailyQuotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DailyQuotes
        public async Task<IActionResult> Index()
        {
            return View(await _context.DailyQuote.ToListAsync());
        }

        // GET: DailyQuotes/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View("ShowSearchForm");
        }

        // GET: DAilyQuotes/ShowSearchForm
        public async Task<IActionResult> ShowSearchResults(string SearchTerm)
        {

           // var viewResult = View("Index", await _context.DailyQuote.Where(d => d.Content.Contains(SearchTerm)
           //&& d.Creator.Contains(SearchTerm) &&
           //d.Day.ToString().Contains(SearchTerm) &&
           //d.Source.Contains(SearchTerm)).ToListAsync());

            return View("Index", await _context.DailyQuote.Where(d => d.Content.Contains(SearchTerm)
            | d.Creator.Contains(SearchTerm) |
            d.Day.Date.ToString().Contains(SearchTerm) |
            d.Source.Contains(SearchTerm)).ToListAsync());
        }


        // GET: DailyQuotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyQuote = await _context.DailyQuote
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailyQuote == null)
            {
                return NotFound();
            }

            return View(dailyQuote);
        }

        // GET: DailyQuotes/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: DailyQuotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Content,Source,Tag,Creator")] DailyQuote dailyQuote)
        {
            if (ModelState.IsValid)
            {
                MailHelper mail = new MailHelper();
                mail.SendMail("J.Eckert@rto.de",dailyQuote.Content + " - " + dailyQuote.Source);
                mail.SendMail("M.Fischer@rto.de", dailyQuote.Content + " - " + dailyQuote.Source);
                return RedirectToAction(nameof(Index));
            }

            return View(dailyQuote);

        }

        // GET: DailyQuotes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyQuote = await _context.DailyQuote.FindAsync(id);
            if (dailyQuote == null)
            {
                return NotFound();
            }
            return View(dailyQuote);
        }

        // POST: DailyQuotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,Source,Day,Creator")] DailyQuote dailyQuote)
        {
            if (id != dailyQuote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailyQuote);
                    dailyQuote.Creator = User.Identity.Name;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyQuoteExists(dailyQuote.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dailyQuote);
        }

        // GET: DailyQuotes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyQuote = await _context.DailyQuote
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailyQuote == null)
            {
                return NotFound();
            }

            return View(dailyQuote);
        }

        // POST: DailyQuotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dailyQuote = await _context.DailyQuote.FindAsync(id);
            _context.DailyQuote.Remove(dailyQuote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DailyQuoteExists(int id)
        {
            return _context.DailyQuote.Any(e => e.Id == id);
        }
    }
}
