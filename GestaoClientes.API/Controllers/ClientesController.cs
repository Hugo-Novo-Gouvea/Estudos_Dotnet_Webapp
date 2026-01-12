using GestaoClientes.API.Data;
using GestaoClientes.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoClientes.API.Controllers;

[ApiController]
[Route("api/[controller]")] // A rota será: https://localhost:xxxx/api/clientes
public class ClientesController : ControllerBase
{
    private readonly AppDbContext _context;

    // Construtor: Recebe o acesso ao banco (Injeção de Dependência)
    public ClientesController(AppDbContext context)
    {
        _context = context;
    }

    // 1. GET: api/clientes (Listar todos)
    [HttpGet]
    public async Task<ActionResult<List<Cliente>>> GetTodos()
    {
        // Vai no banco, pega a lista e transforma em JSON
        return await _context.Clientes.ToListAsync();
    }

    // 2. GET: api/clientes/5 (Buscar por ID)
    [HttpGet("{id}")]
    public async Task<ActionResult<Cliente>> GetPorId(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente == null)
            return NotFound("Cliente não encontrado!");

        return cliente;
    }

    // 3. POST: api/clientes (Criar novo)
    [HttpPost]
    public async Task<ActionResult<Cliente>> CriarNovo(Cliente novoCliente)
    {
        _context.Clientes.Add(novoCliente);
        await _context.SaveChangesAsync(); // O "Commit" no banco

        return CreatedAtAction(nameof(GetPorId), new { id = novoCliente.Id }, novoCliente);
    }

    // 4. PUT: api/clientes/5 (Atualizar)
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, Cliente clienteEditado)
    {
        if (id != clienteEditado.Id)
            return BadRequest();

        // Avisa o EF que esse objeto foi modificado
        _context.Entry(clienteEditado).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Clientes.Any(e => e.Id == id))
                return NotFound();
            else
                throw;
        }

        return NoContent(); // 204 (Deu certo, mas não tenho nada pra mostrar)
    }

    // 5. DELETE: api/clientes/5 (Excluir)
    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null)
            return NotFound();

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}