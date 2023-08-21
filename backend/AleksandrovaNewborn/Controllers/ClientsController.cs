using AleksandrovaNewborn.Entities;
using AleksandrovaNewborn.Models;
using AleksandrovaNewborn.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AleksandrovaNewborn.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = Policies.Administrator)]
public class ClientsController : ControllerBase
{
    private readonly AleksandrovaNewbornContext _db;

    public ClientsController(AleksandrovaNewbornContext db)
    {
        _db = db;
    }
    
    /// <summary>
    /// Adds new client.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> AddClient([FromBody] ClientModel request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var client = new Client(request.ChildName);
        client.EmailAddress = request.EmailAddress;
        client.PhoneNumber = request.PhoneNumber;
        _db.Clients.Add(client);

        await _db.SaveChangesAsync();

        return Created($"/api/clients/{client.Id}", null);
    }

    /// <summary>
    /// Returns list of the clients
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetClients()
    {
        return Ok(await _db.Clients.ToListAsync());
    }
}