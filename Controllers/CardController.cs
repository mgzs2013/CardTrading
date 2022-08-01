#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CardTrading.Data;
using CardTrading.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
namespace CardTrading.Controllers
{
    public class CardController : Controller
    {   
        private readonly UserManager<AppUser> _userManager;       
        
        private readonly ApplicationDbContext _context;

        public CardController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Card
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cards.Include(c => c.User).ToListAsync());
        }
        
        [Authorize]
        
        public async Task<IActionResult> Manage()
        {
            var CurrentUser = await _userManager.GetUserAsync(User);
            var CardsForUser = await _context.Cards.Where(c => c.User.Id == CurrentUser.Id).ToListAsync();
            return View(CardsForUser);
        }

        

        // GET: Card/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var CurrentUser = await _userManager.GetUserAsync(User);

            var card = await _context.Cards
                .FirstOrDefaultAsync(m => m.CardId == id && (m.User != null && m.User.Id == CurrentUser.Id
                ));
            if (card == null)
            {
                return RedirectToAction("Manage");
            }

            return View(card);
        }

        // GET: Card/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Card/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CardId,PokemonName,CardCondition,CardPhoto,ForSale")] Card card)
        {
            if (ModelState.IsValid)
            {
                var CurrentUser = await _userManager.GetUserAsync(User);
                card.User = CurrentUser;
                _context.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(card);
        }

        // GET: Card/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }
            return View(card);
        }

        // POST: Card/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CardId,PokemonName,CardCondition,CardPhoto,ForSale")] Card card)
        {
            if (id != card.CardId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(card);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardExists(card.CardId))
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
            return View(card);
        }

        // GET: Card/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards
                .FirstOrDefaultAsync(m => m.CardId == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        // POST: Card/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CardExists(int id)
        {
            return _context.Cards.Any(e => e.CardId == id);
        }
    }
}
