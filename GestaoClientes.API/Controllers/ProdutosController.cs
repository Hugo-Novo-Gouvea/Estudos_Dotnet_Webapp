using GestaoClientes.API.Data;
using GestaoClientes.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoClientes.API.Controllers;

[ApiController]
[Route("api/[controller]")] // A rota será: https://localhost:xxxx/api/clientes
public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _context;

    // Construtor: Recebe o acesso ao banco (Injeção de Dependência)
    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }

    // 1. GET: api/clientes (Listar todos)
    [HttpGet]
    public async Task<ActionResult<List<Produto>>> GetTodos()
    {
        // Vai no banco, pega a lista e transforma em JSON
        return await _context.Produtos.ToListAsync();
    }

    // 2. GET: api/clientes/5 (Buscar por ID)
    [HttpGet("{id}")]
    public async Task<ActionResult<Produto>> GetPorId(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);

        if (produto == null)
            return NotFound("Produto não encontrado!");

        return produto;
    }

    // 3. POST: api/clientes (Criar novo)
    [HttpPost]
    public async Task<ActionResult<Produto>> CriarNovo(Produto novoProduto)
    {
        _context.Produtos.Add(novoProduto);
        await _context.SaveChangesAsync(); // O "Commit" no banco

        return CreatedAtAction(nameof(GetPorId), new { id = novoProduto.Id }, novoProduto);
    }

    // 4. PUT: api/clientes/5 (Atualizar)
    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, Produto produtoEditado)
    {
        if (id != produtoEditado.Id)
            return BadRequest();

        // Avisa o EF que esse objeto foi modificado
        _context.Entry(produtoEditado).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Produtos.Any(e => e.Id == id))
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
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null)
            return NotFound();

        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}