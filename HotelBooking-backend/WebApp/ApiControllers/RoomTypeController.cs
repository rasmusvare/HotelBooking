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
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[AllowAnonymous]

public class RoomTypeController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly RoomTypeMapper _roomTypeMapper;

    public RoomTypeController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _roomTypeMapper = new RoomTypeMapper(mapper);
    }

    // GET: api/RoomType
    [HttpGet("{hotelId}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.RoomType>>> GetRoomTypes(Guid hotelId)
    {
        var hotelRoomTypes = (await _bll.RoomTypes.GetAllAsync(hotelId))
            .Select(x => _roomTypeMapper.Map(x))
            .ToList();

        return hotelRoomTypes;
    }

    // GET: api/Search
    [HttpGet("{hotelId}/search/startdate={startDate}&enddate={endDate}&guests={noOfGuests}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.RoomType>>> SearchRooms(Guid hotelId, string startDate,
        string endDate, int noOfGuests)
    {
        var searchResults =
            (await _bll.RoomTypes.SearchAvailableRooms(hotelId, startDate, endDate, noOfGuests)).ToList();

        return searchResults.Select(x => _roomTypeMapper.Map(x)!)
            .ToList();
    }

    // GET: api/RoomType/5
    [HttpGet("details/{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<RoomType>> GetRoomType(Guid id)
    {
        var roomType = await _bll.RoomTypes.FirstOrDefaultAsync(id);

        if (roomType == null)
        {
            return NotFound();
        }

        return _roomTypeMapper.Map(roomType)!;
    }


    // PUT: api/RoomType/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
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
        // _bll.RoomTypes.Update(bllEntity);
        // await _bll.SaveChangesAsync();
        return Ok();
    }



    // POST: api/RoomType
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<RoomType>> PostRoomType(RoomType roomType)
    {

        var bllEntity = _roomTypeMapper.Map(roomType)!;
        var newRoomType = _bll.RoomTypes.Add(bllEntity);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetRoomType", new {id = newRoomType.Id});
    }

    // DELETE: api/RoomType/5
    [HttpDelete("{id}")]
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