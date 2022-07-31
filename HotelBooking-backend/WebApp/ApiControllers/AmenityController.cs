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
    /// <summary>
    /// Returns all amenities of the specified hotel
    /// </summary>
    /// <param name="hotelId">Id of the hotel</param>
    /// <returns>List of amenities</returns>
    [HttpGet("{hotelId:guid}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.Amenity>), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Amenity>>> GetAmenities(Guid hotelId)
    {
        var hotelAmenities = (await _bll.Amenities.GetAllAsync(hotelId))
            .Select(x => _amenityMapper.Map(x)!)
            .ToList();

        return Ok(hotelAmenities);
    }

    // GET: api/Amenity/details/5
    /// <summary>
    /// Returns the details of the specified amenity
    /// </summary>
    /// <param name="id">Id of the amenity</param>
    /// <returns>Amenity details</returns>
    [HttpGet("details/{id:guid}")]
    [AllowAnonymous]
    [Produces("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Amenity), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
    
    /// <summary>
    /// Updates the properties of the specified amenity
    /// </summary>
    /// <param name="id">Id of the amenity</param>
    /// <param name="amenity">Updated amenity properties</param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    /// <summary>
    /// Creates a new amenity with the properties specified
    /// </summary>
    /// <param name="amenity">Properties of the amenity</param>
    /// <returns>Created amenity</returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.Amenity), StatusCodes.Status200OK)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Amenity>> PostAmenity(Amenity amenity)
    {
        var bllEntity = _amenityMapper.Map(amenity)!;

        _bll.Amenities.Add(bllEntity);
        await _bll.SaveChangesAsync();
        bllEntity.Id = _bll.Amenities.GetIdFromDb(bllEntity);

        return CreatedAtAction("GetAmenity", new { id = bllEntity.Id }, bllEntity);
    }

    // DELETE: api/Amenity/5
    /// <summary>
    /// Deletes the specified amenity
    /// </summary>
    /// <param name="id">Id of the amenity</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesErrorResponseType(typeof(RestErrorResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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