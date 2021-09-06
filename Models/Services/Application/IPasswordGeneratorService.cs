using API_Password_Generator.Models.InputModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace API_Password_Generator.Models.Services.Application
{
    public interface IPasswordGeneratorService
    {
        string PasswordGeneratorAsync(InputPassword model, [FromServices] IWebHostEnvironment env);
        string PasswordRestoreAsync(InputPassword model, [FromServices] IWebHostEnvironment env);
    }
}