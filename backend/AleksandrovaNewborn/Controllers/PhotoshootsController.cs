using AleksandrovaNewborn.Entities;
using AleksandrovaNewborn.Models;
using AleksandrovaNewborn.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AleksandrovaNewborn.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Policies.Administrator)]
public class PhotoshootsController : ControllerBase
{
    private readonly AleksandrovaNewbornContext _db;

    public PhotoshootsController(AleksandrovaNewbornContext db)
    {
        _db = db;
    }
    
    /// <summary>
    /// Returns the list of photo-shoots.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetPhotoshoots()
    {
        var photoshoots = await _db.Photoshoots.ToListAsync();
        return Ok(photoshoots);
    }

    /// <summary>
    /// Adds new photoshoot
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> AddPhotoshoot([FromBody] PhotoshootModel request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var client = await _db.Clients.FirstOrDefaultAsync(x => x.Id == request.ClientId);
        if (client is null) return BadRequest(new { Message = "Client not found." });

        var photoshoot = new Photoshoot(client, request.Name, request.PhotoshootDate);
        _db.Photoshoots.Add(photoshoot);
        await _db.SaveChangesAsync();

        return Created($"/api/photoshoots/{photoshoot.Id}", null);
    }
}