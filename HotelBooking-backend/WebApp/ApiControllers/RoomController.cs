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
public class RoomController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly RoomMapper _roomMapper;

    public RoomController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _roomMapper = new RoomMapper(mapper);
    }

    // GET: api/Room

    /// <summary>
    /// Returns all the rooms of the specified hotel
    /// </summary>
    /// <param name="hotelId">Id of the hotel</param>
    /// <returns>List of rooms</returns>
    [HttpGet("{hotelId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Room>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Room>>> GetRooms(Guid hotelId)
    {
        var hotelRooms = (await _bll.Rooms.GetAllAsync(hotelId))
            .Select(x => _roomMapper.Map(x)!)
            .ToList();

        return Ok(hotelRooms);
    }

    // GET: api/Room/details/5

    /// <summary>
    /// Returns the details of the specified room
    /// </summary>
    /// <param name="id">Id of the room</param>
    /// <returns>Room</returns>
    [HttpGet("details/{id:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Room), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Room>> GetRoom(Guid id)
    {
        var room = await _bll.Rooms.FirstOrDefaultAsync(id);

        if (room == null)
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
                        $"Room with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        return Ok(_roomMapper.Map(room)!);
    }

    // PUT: api/Room/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754


    /// <summary>
    /// Updates the properties of the specified room
    /// </summary>
    /// <param name="id">Id of the room</param>
    /// <param name="room">Updated properties of the room</param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutRoom(Guid id, Room room)
    {
        if (id != room.Id)
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

        if (!await _bll.Rooms.ExistsAsync(id))
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
                        $"Room with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var bllEntity = _roomMapper.Map(room)!;
        _bll.Rooms.Update(bllEntity);
        await _bll.SaveChangesAsync();
        return Ok();
    }

    // POST: api/Room
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    /// <summary>
    /// Creates a new room in a hotel
    /// </summary>
    /// <param name="room">Properties of the room</param>
    /// <returns>Created room</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Room), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Room>> PostRoom(Room room)
    {
        if (room.RoomNumber == "")
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "Bad request",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Id mismatch"] = new List<string>
                    {
                        "Please enter a room number"
                    }
                }
            };
            return BadRequest(errorResponse);
        }

        if (!await _bll.Rooms.CheckRoomNumberAvailability(_roomMapper.Map(room)!))
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "Bad request",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Id mismatch"] = new List<string>
                    {
                        "Room number already in use"
                    }
                }
            };
            return BadRequest(errorResponse);
        }

        var bllEntity = _roomMapper.Map(room)!;

        if (!await _bll.RoomTypes.ExistsAsync(bllEntity.RoomTypeId))
        {
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "Bad request",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Id mismatch"] = new List<string>
                    {
                        "Please select a valid room type"
                    }
                }
            };
            return BadRequest(errorResponse);
        }

        _bll.Rooms.Add(bllEntity);
        await _bll.SaveChangesAsync();

        var roomType = await _bll.RoomTypes.FirstOrDefaultAsync(room.RoomTypeId);
        await _bll.RoomTypes.CountRooms(roomType!);
        await _bll.SaveChangesAsync();

        bllEntity.Id = _bll.Rooms.GetIdFromDb(bllEntity);

        return CreatedAtAction("GetRoom", new {id = bllEntity.Id}, bllEntity);
    }

    // DELETE: api/Room/5

    /// <summary>
    /// Deletes the specified room
    /// </summary>
    /// <param name="id">Id of the room</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRoom(Guid id)
    {
        var room = await _bll.Rooms.FirstOrDefaultAsync(id);
        if (room == null)
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
                        $"Room with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var futureBookings = await _bll.Bookings.GetFutureBookings(room.RoomTypeId);

        foreach (var each in futureBookings)
        {
            _bll.Bookings.Remove(each.Id);
            await _bll.SaveChangesAsync();
        }

        _bll.Rooms.Remove(room);
        await _bll.SaveChangesAsync();

        return NoContent();
    }
}