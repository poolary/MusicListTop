using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace CRUD.Models
{
    public class User
    {
        [Key] public int Id { get; set; }
        [Required] [DataType(DataType.Password)] public string? HSenha { get; set; }
        public string Email { get; set; }
        public class EmailDTO
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
    }
}
