using CRUD.Models;
using Microsoft.EntityFrameworkCore;
using CRUD.Controllers;
using CRUD.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CRUD.Services
{
    public class ServicesToApiHttp
    {
        private readonly AppDbContext _appDbContext;

        public ServicesToApiHttp(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Musica?> GetMusicByName (string NameOfMusic)
        {
            var musica = await _appDbContext.MusicasLike.
                //Where(m => m.MusicName.Contains(NameOfMusic)).ToListAsync();
            FirstOrDefaultAsync(m => m.MusicName == NameOfMusic);
            return musica;
        }
    }
}

