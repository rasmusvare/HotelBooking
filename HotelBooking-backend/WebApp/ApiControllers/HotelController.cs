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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[AllowAnonymous]
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
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.Hotel?>>> GetHotels()
    {
        var res = (await _bll.Hotels.GetAllAsync()).Select(a => _hotelMapper.Map(a)).ToList();
        return res;
    }

    // GET: api/Hotel/5
    [HttpGet("{id}")]
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
    [HttpPut("{id}")]
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
    [HttpPost]
    public async Task<ActionResult<Hotel>> PostHotel(Hotel hotel)
    {
        //TODO: Check permissions

        var bllEntity = _hotelMapper.Map(hotel)!;

        _bll.Hotels.Add(bllEntity);
        await _bll.SaveChangesAsync();
        bllEntity.Id = _bll.Hotels.GetIdFromDb(bllEntity);

        return CreatedAtAction("GetHotel", new {id = bllEntity.Id}, bllEntity);
    }

    // DELETE: api/Hotel/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHotel(Guid id)
    {
        //TODO: Check permissions
        if (_bll.Hotels == null)
        {
            return NotFound();
        }

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

        // TODO: Remove rooms etc...

        _bll.Hotels.Remove(hotel);
        await _bll.SaveChangesAsync();

        return NoContent();
    }
}