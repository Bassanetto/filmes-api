using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]

public class FilmeController : ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;


    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para a criação de um filme</param>
    /// <returns></returns>
    
    [HttpPost]
    public IActionResult AdicionarFilme([FromBody] CreateFilmeDto filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return Created(nameof(ObterPorId), new { id = filme.Id, filme });
    }

    /// <summary>
    /// Obtem todos os filmes que estão cadastrados no banco de dados
    /// </summary>
    /// <param name="skip">Objeto com os campos necessários para a criação de um filme</param>
    /// <returns></returns>

    [HttpGet]
    public IEnumerable<ReadFilmeDto> ObterTodosFilmes([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take));
    }

    /// <summary>
    /// Obtem todos os filmes por Id que estão cadastrados no banco de dados
    /// </summary>
    /// <param name="id">Objeto com os campos necessários para a criação de um filme</param>
    /// <returns></returns>

    [HttpGet("{id}")]
    public IActionResult ObterPorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
        return Ok(filme);
    }

    /// <summary>
    /// Atualiza todos os dados de um filme que ja estao cadastrados no banco de dados
    /// </summary>
    /// <param name="id">Objeto com os campos necessários para a criação de um filme</param>
    /// <returns></returns>

    [HttpPut("{id}")]
    public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto) 
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

        if(filme == null) return NotFound();
        _mapper.Map(filmeDto, filme);
        _context.SaveChanges(); 
        return NoContent();
    }

    /// <summary>
    /// Atualiza um dado especifico de um filme que ja esta cadastrado no banco de dados
    /// </summary>
    /// <param name="patch">Objeto com os campos necessários para a criação de um filme</param>
    /// <returns></returns>

    [HttpPatch("{id}")]

    public IActionResult AtualizaFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

        if (filme == null) return NotFound();

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);

        patch.ApplyTo(filmeParaAtualizar, ModelState);

        if(!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Deleta um dado que ja esta cadastrado no banco de dados
    /// </summary>
    /// <param name="id">Objeto com os campos necessários para a criação de um filme</param>
    /// <returns></returns>

    [HttpDelete("{id}")]
    public IActionResult DeletaFilme(int id) 
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

        if (filme == null) return NotFound();
        _context.Filmes.Remove(filme);
        _context.SaveChanges();
        return NoContent();
    }
}