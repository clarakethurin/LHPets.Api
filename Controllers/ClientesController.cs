using LHPets.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace LHPets.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    public static readonly List<Cliente> Clientes = new();
    private static int _proximoId = 1;

    [HttpGet]
    public ActionResult<IEnumerable<Cliente>> Get()
    {
        return Ok(Clientes);
    }

    [HttpPost]
    public ActionResult<Cliente> Post([FromBody] Cliente cliente)
    {
        if (string.IsNullOrWhiteSpace(cliente.Nome) ||
            string.IsNullOrWhiteSpace(cliente.Cpf) ||
            string.IsNullOrWhiteSpace(cliente.Email))
        {
            return BadRequest("Nome, Cpf e Email são obrigatórios.");
        }

        cliente.Id = _proximoId++;
        Clientes.Add(cliente);

        return CreatedAtAction(nameof(Get), new { id = cliente.Id }, cliente);
    }
}