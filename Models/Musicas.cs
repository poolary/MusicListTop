using System.ComponentModel.DataAnnotations;

namespace CRUD.Models
{
    public class Musica
    {
        public int Id { get; set; }
        public string? MusicName { get; set; }
        public string? MusicArtist { get; set; }
        public string? MusicTop { get; set; }
    }
}