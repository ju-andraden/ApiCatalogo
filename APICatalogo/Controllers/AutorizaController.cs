using APICatalogo.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AutorizaController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AutorizaController(UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "AutorizaController :: Acessado em : "
                + DateTime.Now.ToLongDateString();
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUsr([FromBody] UsuarioDto usuarioDto)
        {
            /*if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            }*/

            var user = new IdentityUser
            {
                UserName = usuarioDto.Email,
                Email = usuarioDto.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, usuarioDto.PassWord);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _signInManager.SignInAsync(user, false);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UsuarioDto usuarioDto)
        {
            //verificar as credenciais do usuário e retorna um valor
            var result = await _signInManager.PasswordSignInAsync(usuarioDto.Email,
                usuarioDto.PassWord, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Login inválido.");

                return BadRequest(ModelState);
            }
        }

    }
}
