using CRUD.Data;
using CRUD.Hashado;
using CRUD.Models;
using CRUD.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace CRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MusicaController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private ServicesToApiHttp _services;

        public MusicaController(AppDbContext appDbContext, ServicesToApiHttp services)
        {
            _appDbContext = appDbContext;
            _services = services;
        }
        /// <summary>
        /// POST /api/musicas
        /// </summary>
        [HttpPost("Enviar")]
        public async Task<IActionResult> AddMusica([FromBody] Musica musica)
        {

            var existe = await _appDbContext.MusicasLike.AnyAsync(m => m.Id == musica.Id);

            if (existe)
                return Conflict("Este ID já existe, escolha outro ID.");
            if (musica.Id == 0)
                return Conflict("O ID não pode ser igual a zero");
            if (string.IsNullOrWhiteSpace(musica.MusicTop))
                return Conflict("Defina em que posição está sua música");
            if (await _appDbContext.MusicasLike.AnyAsync(m => m.MusicTop == musica.MusicTop))
                return Conflict("Não pode ter dois itens na mesma colocação");
            if (string.IsNullOrWhiteSpace(musica.MusicArtist))
                return Conflict("Defina o artista");
            if (string.IsNullOrWhiteSpace(musica.MusicName))
                return Conflict("Qual o nome da música? Você esqueceu de definir");


            _appDbContext.MusicasLike.Add(musica);
            await _appDbContext.SaveChangesAsync();
            return Ok("Salvo");
        }
        /// <summary>
        /// GET /api/musicas?name=
        ///  /// </summary>
        [HttpGet("Mostrar")]
        public async Task<IActionResult> GetMusicas([FromQuery(Name = "Nome da música")] string MusicName)
        {

            var musicas = await _services.GetMusicByName(MusicName);

            if (musicas is null)
            {
                return NoContent();
            }
            /*if (!string.IsNullOrEmpty(MusicTop))
            {

                musicas = musicas.Where(m => m.Id.ToString() == MusicTop).ToList();
            }

            if(musicas.Count == 0)
            {
                return NotFound("Essa música não existe aqui");
            }*/


            return Ok(musicas);
        }
        /// <summary>
        /// DELETE /api/musicas/{Id}
        /// </summary>
        [HttpDelete("Deletar")]
        public async Task<ActionResult> DeleteMusica([FromQuery(Name = "Id da música que deseja deletar")] int? code)
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
        /// <summary>
        /// PUT /api/musicas/{Id}
        /// </summary>
        [HttpPut("Atualizar")]
        public async Task<IActionResult> EditarMusica([FromBody] Musica musica)
        {
            var existe = await _appDbContext.MusicasLike.FindAsync(musica.Id);

            existe.MusicTop = musica.MusicTop;
            existe.MusicName = musica.MusicName;
            existe.MusicArtist = musica.MusicArtist;

            await _appDbContext.SaveChangesAsync();

            return Ok(musica);
        }

        [HttpPost("Email")]
        public async Task<IActionResult> SendUser([FromBody] User.EmailDTO _emailAddress, string senha)
        {
            try
            {

                var hash = new Hash(SHA512.Create());
                var resultado = hash.CriptografarSenha(senha);
                var user = new User
                {
                    Email = _emailAddress.Email,
                    HSenha = resultado

                };
                _appDbContext.UsersPW.Add(user);
                await _appDbContext.SaveChangesAsync();
                return Ok("Email e senha válidos");
            } 
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}

/*[HttpPost("Senha")]
    public async Task<IActionResult> GerarHash([FromBody] string senha)
    {

    var hash = new Hash(SHA512.Create());
        var resultado = hash.CriptografarSenha(senha);
    var UsersPW = new User
    {
        HSenha = resultado
    };

    _appDbContext.UsersPW.Add(UsersPW);
    await _appDbContext.SaveChangesAsync();

    return Ok(resultado);
    }*/


