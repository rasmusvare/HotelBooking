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
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
public class HotelController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly HotelMapper _hotelMapper;

    public HotelController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _hotelMapper = new HotelMapper(mapper);
    }

    // GET: api/Hotel
    /// <summary>
    /// Returns all the hotels in hte booking system
    /// </summary>
    /// <returns>List of hotels</returns>
    [HttpGet]
    [AllowAnonymous]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Hotel>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Hotel>>> GetHotels()
    {
        var res = (await _bll.Hotels.GetAllAsync()).Select(a => _hotelMapper.Map(a)!).ToList();
        return Ok(res);
    }

    // GET: api/Hotel/5
    /// <summary>
    /// Returns the details of the specified hotel
    /// </summary>
    /// <param name="id">Id of the hotel</param>
    /// <returns>Hotel</returns>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Hotel), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Hotel>> GetHotel(Guid id)
    {
        var hotel = await _bll.Hotels.FirstOrDefaultAsync(id);

        if (hotel == null)
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
                        $"Hotel with id {id} not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        return Ok(_hotelMapper.Map(hotel)!);
    }

    // PUT: api/Hotel/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754


    /// <summary>
    /// Update the properties of the specified hotel
    /// </summary>
    /// <param name="id">Id of the hotel</param>
    /// <param name="hotel">Properties of the updated hotel</param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutHotel(Guid id, Hotel hotel)
    {
        if (id != hotel.Id)
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

        if (!await _bll.Hotels.ExistsAsync(id))
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
                        $"Hotel with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var bllEntity = _hotelMapper.Map(hotel)!;
        _bll.Hotels.Update(bllEntity);
        await _bll.SaveChangesAsync();
        return Ok();
    }

    // POST: api/Hotel
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Creates a new hotel
    /// </summary>
    /// <param name="hotel">Properties of the hotel</param>
    /// <returns>Created hotel</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Hotel), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Hotel>> PostHotel(Hotel hotel)
    {
        var bllEntity = _hotelMapper.Map(hotel)!;

        _bll.Hotels.Add(bllEntity);
        await _bll.SaveChangesAsync();
        bllEntity.Id = _bll.Hotels.GetIdFromDb(bllEntity);

        return CreatedAtAction("GetHotel", new {id = bllEntity.Id}, bllEntity);
    }

    // DELETE: api/Hotel/5
    /// <summary>
    /// Deletes the specified hotel along with all other data like rooms, bookings, etc. connected with the hotel
    /// </summary>
    /// <param name="id">Id of the hotel</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteHotel(Guid id)
    {
        //TODO: Check permissions

        var hotel = await _bll.Hotels.FirstOrDefaultAsync(id);
        if (hotel == null)
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
                        $"Hotel with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        _bll.Hotels.Remove(hotel);
        await _bll.SaveChangesAsync();

        return NoContent();
    }
}