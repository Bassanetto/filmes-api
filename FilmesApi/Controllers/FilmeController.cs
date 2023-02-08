using FilmesApi.Data;
using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]

public class FilmeController : ControllerBase
{
    private FilmeContext _context;

    public FilmeController(FilmeContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult AdicionarFilme([FromBody] Filme filme)
    {
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(ObterPorId), new { id = filme.Id, filme });
    }

    [HttpGet]
    public IEnumerable<Filme> ObterTodosFilmes([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        return _context.Filmes.Skip(skip).Take(take);
    }
    [HttpGet("{id}")]
    public IActionResult ObterPorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        return Ok(filme);
    }
}