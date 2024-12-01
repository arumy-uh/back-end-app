using EctoTec.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using EctoTec.Context;
using EctoTec.Custom;

namespace EctoTec.Controllers
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDBContenxt _context;
        private readonly Utilities _utils;

        public LoginController(AppDBContenxt context, Utilities utils)
        {
            _context = context;
            _utils = utils;
        }

        [HttpPost]
        public async Task<IActionResult> login(LoginDTO loginUser)
        {
            var userF = await _context.Users.Where(user => (user.UserName == loginUser.UserName
            && user.Password == loginUser.Password)).FirstOrDefaultAsync();

            if (userF == null)
                return StatusCode(StatusCodes.Status404NotFound, new { isSuccess = false, token = "" });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utils.generateJWT(userF) });
        }

        [HttpGet]
        [Route("validateToken")]
        public IActionResult validate([FromQuery]string token)
        {
            bool respuesta = _utils.validateToken(token);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }
    }
}
