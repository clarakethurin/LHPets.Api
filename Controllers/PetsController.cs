using LHPets.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace LHPets.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PetsController : ControllerBase
{
    private static readonly List<Pet> Pets = new();
    private static int _proximoId = 1;

    [HttpGet]
    public ActionResult<IEnumerable<Pet>> Get()
    {
        return Ok(Pets);
    }

    [HttpPost]
    public ActionResult<Pet> Post([FromBody] Pet pet)
    {
        if (string.IsNullOrWhiteSpace(pet.Nome) ||
            string.IsNullOrWhiteSpace(pet.Especie))
        {
            return BadRequest("Nome e Especie são obrigatórios.");
        }

        // Validação do vínculo: o cliente precisa existir
        var clienteExiste = ClientesController.Clientes.Any(c => c.Id == pet.ClienteId);
        if (!clienteExiste)
        {
            return NotFound($"Cliente com Id {pet.ClienteId} não encontrado.");
        }

        pet.Id = _proximoId++;
        Pets.Add(pet);

        return CreatedAtAction(nameof(Get), new { id = pet.Id }, pet);
    }

    [HttpGet("cliente/{clienteId}")]
    public ActionResult<IEnumerable<Pet>> GetPorCliente(int clienteId)
    {
        var pets = Pets.Where(p => p.ClienteId == clienteId).ToList();
        return Ok(pets);
    }
}