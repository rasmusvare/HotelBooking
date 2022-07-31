using System.Diagnostics;
using System.Net;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using App.Public.DTO.v1;
using App.Public.DTO.v1.ErrorResponses;
using App.Public.DTO.v1.Mappers;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles="admin")]
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
    /// <summary>
    /// Returns all the bookings associated with the specified email
    /// </summary>
    /// <param name="email">Email of the booking owner</param>
    /// <returns>List of bookings</returns>
    [HttpGet("search/{email}")]
    [AllowAnonymous]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Booking>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Booking>>> GetBookings(string email)
    {
        var bookings = (await _bll.Bookings.SearchBookingsByEmail(email))
            .Select(x => _bookingMapper.Map(x)!)
            .ToList();

        return Ok(bookings);
    }

    // GET: api/Booking
    
    /// <summary>
    /// Returns all the bookings associated with the specified hotel
    /// </summary>
    /// <param name="hotelId">Id of the hotel</param>
    /// <returns>List of bookings</returns>
    [HttpGet("{hotelId:guid}")]
    [AllowAnonymous]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Booking>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<Booking>>> GetHotelBookings(Guid hotelId)
    {
        var bookings = (await _bll.Bookings.GetAllHotelAsync(hotelId))
            .Select(x => _bookingMapper.Map(x)!)
            .ToList();

        return bookings;
    }

    // GET: api/Booking/details/5
    
    /// <summary>
    /// Returns the details of the specified booking
    /// </summary>
    /// <param name="id">Id of the booking</param>
    /// <returns>Booking</returns>
    [HttpGet("details/{id:guid}")]
    [Produces("application/json")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Booking), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        return Ok(_bookingMapper.Map(booking)!);
    }


    // PUT: api/Booking/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    
    /// <summary>
    /// Updates the properties of the booking provided
    /// </summary>
    /// <param name="id">Id of the booking</param>
    /// <param name="booking">Properties of the updated booking</param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [AllowAnonymous]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        var bookingDb = await _bll.Bookings.GetBooking(booking.Id);

        if (bookingDb == null)
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

        if (!User.IsInRole("admin") && DateOnly.FromDateTime(DateTime.Now).AddDays(3) > DateOnly.Parse(bookingDb.DateFrom.ToString()) )
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
                        "Cannot change booking within 3 days from the booking start date"
                    }
                }
            };
            return BadRequest(errorResponse);
        }

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
        
        _bll.Bookings.Update(bllEntity);
        await _bll.SaveChangesAsync();
        return Ok();
    }

    // POST: api/Booking
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    
    /// <summary>
    /// Creates a new booking with the properties specified
    /// </summary>
    /// <param name="booking">Properties of the booking</param>
    /// <returns>Created booking</returns>
    [HttpPost]
    [AllowAnonymous]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Booking), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
    
    /// <summary>
    /// Deletes the specified booking
    /// </summary>
    /// <param name="id">Id of the booking</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        //TODO: CHECK IF PAST BOOKING
        //TODO: Check if can delete
        if (!User.IsInRole("admin") && DateOnly.FromDateTime(DateTime.Now).AddDays(3) > DateOnly.Parse(booking.DateFrom.ToString()) )
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
                        "Cannot delete booking within 3 days from the booking start date"
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