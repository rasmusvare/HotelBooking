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
[AllowAnonymous]
public class RoomTypeController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly RoomTypeMapper _roomTypeMapper;
    private readonly SearchPropertiesMapper _searchPropertiesMapper;

    public RoomTypeController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _roomTypeMapper = new RoomTypeMapper(mapper);
        _searchPropertiesMapper = new SearchPropertiesMapper(mapper);
    }

    // GET: api/RoomType

    /// <summary>
    /// Returns the room types of the specified hotel
    /// </summary>
    /// <param name="hotelId">Id of the hotel</param>
    /// <returns>List of room types</returns>
    [HttpGet("{hotelId:guid}")]
    [AllowAnonymous]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.RoomType>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.RoomType>>> GetRoomTypes(Guid hotelId)
    {
        var hotelRoomTypes = (await _bll.RoomTypes.GetAllAsync(hotelId))
            .Select(x => _roomTypeMapper.Map(x)!)
            .ToList();

        return Ok(hotelRoomTypes);
    }

    // GET: api/Search

    /// <summary>
    /// Searches for available room types of the specified hotel between the start and end dates and based on the
    /// number of guests
    /// </summary>
    /// <param name="hotelId">Id of the hotel</param>
    /// <param name="startDate">Search start date (string format: YYYY-MM-DD)</param>
    /// <param name="endDate">Search end date (string format: YYYY-MM-DD)</param>
    /// <param name="noOfGuests">Number of guests</param>
    /// <returns>List of available room types based on the the search parameters</returns>
    [HttpGet("{hotelId}/search/startdate={startDate}&enddate={endDate}&guests={noOfGuests}")]
    [AllowAnonymous]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.RoomType>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.RoomType>>> SearchRooms(Guid hotelId, string startDate,
        string endDate, int noOfGuests)
    {
        var searchProperties = new SearchProperties
        {
            HotelId = hotelId,
            StartDate = startDate,
            EndDate = endDate,
            NoOfGuests = noOfGuests
        };

        var validationErrors = _bll.RoomTypes.ValidateSearch(_searchPropertiesMapper.Map(searchProperties)!);

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

        var searchResults =
            (await _bll.RoomTypes.SearchAvailableRooms(_searchPropertiesMapper.Map(searchProperties)!)).ToList();

        return Ok(searchResults.Select(x => _roomTypeMapper.Map(x)!));
    }

    // GET: api/RoomType/details/5
    /// <summary>
    /// Returns the details of the specified room type
    /// </summary>
    /// <param name="id">Id of the room type</param>
    /// <returns>Room type</returns>
    [HttpGet("details/{id:guid}")]
    [AllowAnonymous]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.RoomType), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoomType>> GetRoomType(Guid id)
    {
        var roomType = await _bll.RoomTypes.FirstOrDefaultAsync(id);

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
                        $"Room type with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        return Ok(_roomTypeMapper.Map(roomType)!);
    }


    // PUT: api/RoomType/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Updates the properties of the specified room type
    /// </summary>
    /// <param name="id">Id of the room type</param>
    /// <param name="roomType">Updated properties of the room type</param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutRoomType(Guid id, RoomType roomType)
    {
        if (id != roomType.Id)
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

        if (!await _bll.RoomTypes.ExistsAsync(id))
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
                        $"Room type with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var bllEntity = _roomTypeMapper.Map(roomType)!;

        await _bll.RoomTypes.CountRooms(bllEntity);
        await _bll.SaveChangesAsync();
        return Ok();
    }

    // POST: api/RoomType
    
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Creates a new room type for the specified hotel
    /// </summary>
    /// <param name="roomType">Parameters of the new room type</param>
    /// <returns>Created room type</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.RoomType), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<RoomType>> PostRoomType(RoomType roomType)
    {
        var bllEntity = _roomTypeMapper.Map(roomType)!;
        var newRoomType = _bll.RoomTypes.Add(bllEntity);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetRoomType", new {id = newRoomType.Id});
    }

    // DELETE: api/RoomType/5
    
    /// <summary>
    /// Deletes the room type specified
    /// </summary>
    /// <param name="id">Id of the room type</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRoomType(Guid id)
    {
        var roomType = await _bll.RoomTypes.FirstOrDefaultAsync(id);
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
                        $"Room type with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        _bll.RoomTypes.Remove(roomType);
        await _bll.SaveChangesAsync();

        return NoContent();
    }
}