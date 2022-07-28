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
public class AmenityController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly AmenityMapper _amenityMapper;

    public AmenityController(IAppBLL bll, IMapper mapper)
    {
        _bll = bll;
        _amenityMapper = new AmenityMapper(mapper);
    }

    // GET: api/Amenity
    [HttpGet("{hotelId}")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Amenity>>> GetAmenities(Guid hotelId)
    {
        var hotelAmenities = (await _bll.Amenities.GetAllAsync(hotelId))
            .Select(x => _amenityMapper.Map(x))
            .ToList();

        return hotelAmenities;

    }

    // GET: api/Amenity/5
    [HttpGet("details/{id}")]
    [AllowAnonymous]

    public async Task<ActionResult<Amenity>> GetAmenity(Guid id)
    {
        var amenity = await _bll.Amenities.FirstOrDefaultAsync(id);

        if (amenity == null)
        {
            return NotFound();
        }

        return _amenityMapper.Map(amenity)!;
    }

    // PUT: api/Amenity/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [AllowAnonymous]

    public async Task<IActionResult> PutAmenity(Guid id, Amenity amenity)
    {
        if (id != amenity.Id)
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
        
        if (!await _bll.Amenities.ExistsAsync(id))
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
                        $"Amenity with the id {id} was not found"
                    }
                }
            };
            return NotFound(errorResponse);
        }

        var bllEntity = _amenityMapper.Map(amenity)!;
        _bll.Amenities.Update(bllEntity);
        await _bll.SaveChangesAsync();
        return Ok();
    }

    // POST: api/Amenity
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<Amenity>> PostAmenity(Amenity amenity)
    {
        var bllEntity = _amenityMapper.Map(amenity)!;

        _bll.Amenities.Add(bllEntity);
        await _bll.SaveChangesAsync();
        bllEntity.Id = _bll.Amenities.GetIdFromDb(bllEntity);

        return CreatedAtAction("GetAmenity", new { id = bllEntity.Id }, bllEntity);
    }

    // DELETE: api/Amenity/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAmenity(Guid id)
    {
        var amenity = await _bll.Amenities.FirstOrDefaultAsync(id);
        if (amenity == null)
        {
            return NotFound();
        }

        _bll.Amenities.Remove(amenity);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

}