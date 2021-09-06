using System;
using API_Password_Generator.Models.InputModels;
using API_Password_Generator.Models.Services.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API_Password_Generator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordController : ControllerBase
    {
        private readonly ILogger<PasswordController> logger;
        private readonly IPasswordGeneratorService generatorService;
        private readonly IWebHostEnvironment env;

        public PasswordController(ILogger<PasswordController> logger, IPasswordGeneratorService generatorService, IWebHostEnvironment env)
        {
            this.logger = logger;
            this.generatorService = generatorService;
            this.env = env;
        }
        
        [HttpGet("Welcome")]
        public IActionResult Welcome()
        {
            return Ok(string.Concat("Ciao sono le ore: ", DateTime.Now.ToLongTimeString()));
        }

        [HttpPost("Password-Encrypt")]
        public IActionResult PasswordEncrypt([FromForm] InputPassword model)
        {
            try
            {
                string password = generatorService.PasswordGeneratorAsync(model, env);
                return Ok(password);
            }
            catch(Exception exc)
            {
                return StatusCode(500, exc.ToString());
            }
        }

        [HttpPost("Password-Decrypt")]
        public IActionResult PasswordDecrypt([FromForm] InputPassword model)
        {
            try
            {
                string password = generatorService.PasswordRestoreAsync(model, env);
                return Ok(password);
            }
            catch(Exception exc)
            {
                return StatusCode(500, exc.ToString());
            }
        }
    }
}