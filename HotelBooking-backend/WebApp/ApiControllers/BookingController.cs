using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Public.DTO.v1;
using App.Public.DTO.v1.ErrorResponses;
using App.Public.DTO.v1.Mappers;
using AutoMapper;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[AllowAnonymous]
public class BookingController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly BookingMapper _bookingMapper;

    public BookingController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _bookingMapper = new BookingMapper(mapper);
    }

    // GET: api/Booking
    [HttpGet]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Booking>>> GetBookings()
    {
        var bookings = (await _bll.Bookings.GetAllUserAsync(User.GetUserId()))
            .Select(x => _bookingMapper.Map(x)!)
            .ToList();

        return bookings;
    }

    // GET: api/Booking
    [HttpGet("{hotelId}")]
    public async Task<ActionResult<IEnumerable<Booking>>> GetHotelBookings(Guid hotelId)
    {
        var bookings = (await _bll.Bookings.GetAllHotelAsync(hotelId))
            .Select(x => _bookingMapper.Map(x)!)
            .ToList();

        return bookings;
    }

    // GET: api/Booking/5
    [HttpGet("details/{id}")]
    public async Task<ActionResult<Booking>> GetBooking(Guid id)
    {
        var booking = await _bll.Bookings.GetBooking(id);

        if (booking == null)
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                Title = "Not found",
                Status = HttpStatusCode.NotFound,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Not found"] = new List<string>
                    {
                        $"Booking with id {id} not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        Console.WriteLine("in controller: " + booking.BookingHolder.Id);

        var bkö = _bookingMapper.Map(booking)!;

        Console.WriteLine("after mapping: " + bkö.BookingHolder.Id);
        Console.WriteLine("guest[0]: " + bkö.Guests!.ToList()[0].Id);

        return _bookingMapper.Map(booking)!;
    }


    // PUT: api/Booking/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBooking(Guid id, Booking booking)
    {
        if (id != booking.Id)
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "App error",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Id mismatch"] = new List<string>
                    {
                        "Id mismatch"
                    }
                }
            };
            return BadRequest(errorResponse);
        }

        if (!await _bll.Bookings.ExistsAsync(id))
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231/#section-6.5.4",
                Title = "Not found",
                Status = HttpStatusCode.NotFound,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Not found"] = new List<string>
                    {
                        $"Booking with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        //TODO: Check dates, if not admin
        Console.WriteLine(booking.DateFrom.ToString());
        if (!User.IsInRole("Admin") && DateOnly.FromDateTime(DateTime.Now).AddDays(3) > DateOnly.Parse(booking.DateFrom.ToString()) )
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "Bad request",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Bad request"] = new List<string>
                    {
                        $"Cannot change booking within 3 days from the booking start date"
                    }
                }
            };
            return BadRequest(errorResponse);
        }

        var bllEntity = _bookingMapper.Map(booking)!;
        _bll.Bookings.Update(bllEntity);
        await _bll.SaveChangesAsync();
        return Ok();
    }

    // POST: api/Booking
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Booking>> PostBooking(Booking booking)
    {
        var roomType = await _bll.RoomTypes.FirstOrDefaultAsync(booking.RoomTypeId);

        if (roomType == null)
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231/#section-6.5.4",
                Title = "Not found",
                Status = HttpStatusCode.NotFound,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Not found"] = new List<string>
                    {
                        $"Room type with the id {booking.RoomTypeId} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }
        
        var bllEntity = _bookingMapper.Map(booking)!;

        var validationErrors = await _bll.Bookings.ValidateBooking(bllEntity, roomType);

        if (validationErrors.Count != 0)
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "Bad request",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Validation errors"] = validationErrors
                }
            };
            return BadRequest(errorResponse);
        }

        _bll.Bookings.Add(bllEntity);
        await _bll.SaveChangesAsync();

        bllEntity.Id = _bll.Bookings.GetIdFromDb(bllEntity);

        return CreatedAtAction("GetBooking", new { id = bllEntity.Id }, bllEntity);
    }

    // DELETE: api/Booking/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(Guid id)
    {
        var booking = await _bll.Bookings.FirstOrDefaultAsync(id);
        if (booking == null)
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231/#section-6.5.4",
                Title = "Not found",
                Status = HttpStatusCode.NotFound,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Not found"] = new List<string>
                    {
                        $"Booking with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        //TODO: Check if can delete
        if (!User.IsInRole("Admin") && DateOnly.FromDateTime(DateTime.Now).AddDays(3) > DateOnly.Parse(booking.DateFrom.ToString()) )
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "Bad request",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Bad request"] = new List<string>
                    {
                        $"Cannot delete booking within 3 days from the booking start date"
                    }
                }
            };
            return BadRequest(errorResponse);
        }
        _bll.Bookings.Remove(booking);
        await _bll.SaveChangesAsync();

        return NoContent();
    }
}