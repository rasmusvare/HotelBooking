using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;

namespace WebApp.ApiControllers;

[Route("api/[controller]")]
[ApiController]
public class GuestController : ControllerBase
{
    private readonly AppDbContext _context;

    public GuestController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Guest
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Guest>>> GetGuests()
    {
        if (_context.Guests == null)
        {
            return NotFound();
        }
        return await _context.Guests.ToListAsync();
    }

    // GET: api/Guest/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Guest>> GetGuest(Guid id)
    {
        if (_context.Guests == null)
        {
            return NotFound();
        }
        var guest = await _context.Guests.FindAsync(id);

        if (guest == null)
        {
            return NotFound();
        }

        return guest;
    }

    // PUT: api/Guest/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGuest(Guid id, Guest guest)
    {
        if (id != guest.Id)
        {
            return BadRequest();
        }

        _context.Entry(guest).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GuestExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Guest
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Guest>> PostGuest(Guest guest)
    {
        if (_context.Guests == null)
        {
            return Problem("Entity set 'AppDbContext.Guests'  is null.");
        }
        _context.Guests.Add(guest);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetGuest", new { id = guest.Id }, guest);
    }

    // DELETE: api/Guest/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGuest(Guid id)
    {
        if (_context.Guests == null)
        {
            return NotFound();
        }
        var guest = await _context.Guests.FindAsync(id);
        if (guest == null)
        {
            return NotFound();
        }

        _context.Guests.Remove(guest);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool GuestExists(Guid id)
    {
        return (_context.Guests?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}