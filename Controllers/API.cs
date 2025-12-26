using CRUD.Data;
using CRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class API : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private Musica music = new Musica();

        public API(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        [HttpPost("Enviar")]
        public async Task<IActionResult> AddMusica([FromBody] Musica musica)
        {

            var existe = await _appDbContext.MusicasLike.AnyAsync(m => m.Id == musica.Id);

            if (existe)
            {
                return Conflict("Este ID já existe, escolha outro ID.");
            }

            _appDbContext.MusicasLike.Add(musica);
            await _appDbContext.SaveChangesAsync();
            return Ok("Salvo");
        }

        [HttpGet("Mostrar")]
        public async Task<IActionResult> GetMusicas([FromQuery] string? MusicTop)
        {

            var musicas = await _appDbContext.MusicasLike.ToListAsync();
            if (!string.IsNullOrEmpty(MusicTop))
            {
               
                musicas = musicas.Where(m => m.Id.ToString() == MusicTop).ToList();
            }

            if(musicas.Count == 0)
            {
                return NotFound("Essa música não existe aqui");
            }


            return Ok(musicas);
        }

        
        [HttpDelete("Deletar")]
        public async Task<ActionResult> DeleteMusica([FromQuery]int? code)
        {
            if (code == null || code == 0)
            {
                return BadRequest("Código inválido.");
            }
            var musica = await _appDbContext.MusicasLike.FindAsync(code.Value);
            if (musica == null)
            {
                return NotFound("Música não encontrada.");
            }

            _appDbContext.MusicasLike.Remove(musica);
            await _appDbContext.SaveChangesAsync();
            return Ok(musica);



        }

        [HttpPut("Atualizar")]
        public async Task<IActionResult> EditarMusica([FromBody] Musica musica)
        {
            var existe = await _appDbContext.MusicasLike.FindAsync(musica.Id);

            existe.MusicTop = musica.MusicTop;
            existe.Id = musica.Id;


            _appDbContext.MusicasLike.Update(musica);
            await _appDbContext.SaveChangesAsync();

            return Ok(musica);
        }

    }
}