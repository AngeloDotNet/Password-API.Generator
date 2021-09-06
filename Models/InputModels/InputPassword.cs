using System.ComponentModel.DataAnnotations;

namespace API_Password_Generator.Models.InputModels
{
    public class InputPassword
    {
        [Required]
        public string Password { get; set; }
    }
}