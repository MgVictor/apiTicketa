using apiTicket.Helper;
using apiTicket.Models;
using apiTicket.Services;
using apiTicket.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace apiTicket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolSinController : ControllerBase
    {
        private readonly ISolSinService _SolSinService;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IConfiguration _Config;
        public SolSinController(ISolSinService solSinService, IHostingEnvironment hostEnvironment, IOptions<AppSettings> appSettings, IConfiguration config)
        {
            _SolSinService = solSinService;
            _HostEnvironment = hostEnvironment;
            _appSettings = appSettings;
            _Config = config;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "ticket1", "ticket2" };
        }

        [HttpPost]
        [Route("GetTokenSin")]
        public IActionResult GetToken(SessionUser session)
        {
            try
            {
                var secretKey = _Config.GetValue<string>("Jwt:SecretKey");
                var issuer = _Config.GetValue<string>("Jwt:Issuer");
                var audience = _Config.GetValue<string>("Jwt:Audience");
                var expirationMinutes = _Config.GetValue<int>("Jwt:ExpirationMinutes");

                var token = GenerateJwtToken2(secretKey, issuer, audience, session.username, expirationMinutes);
                return Ok(new { token });
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Route("GetTokenSinietro")]
        public string GenerateJwtToken2(string secretKey, string issuer, string audience, string username, int expirationMinutes)
        {
            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, username),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,

                expires: DateTime.Now.AddMinutes(expirationMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("S3Adjunto")]
        public IActionResult S3Adjunto([FromBody] List<Ab_Adjunto> Adjuntos)
        {
            var json = this._SolSinService.S3Adjuntar(Adjuntos, "360");
            if (json == null)
            {
                return NotFound();
            }
            return Ok(json);
        }

        [HttpPost("RegJira")]
        public IActionResult RegistraJIRA([FromBody] Ab_Contract request)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string logFilePath = Path.Combine(baseDirectory, "logfile.txt");

            var _objReturn = this._SolSinService.RegistraTicketJIRA(request, "360");
            try
            {
                if (_objReturn == null)
                {
                    return NotFound();
                }
                LogToFile(logFilePath, $"Registro exitoso - Respuesta {_objReturn.Codigo}");
                return Ok(_objReturn);
            }
            catch (Exception ex)
            {
                LogToFile(logFilePath, $"Error: {ex.Message}\nStack Trace: {ex.StackTrace}");
                throw;
            }

        }

        [HttpGet("Log")]
        public void LogToFile(string logFileName, string message)
        {
            string logFilePath = Path.Combine(_HostEnvironment.ContentRootPath, logFileName);

            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
        }
    }
}