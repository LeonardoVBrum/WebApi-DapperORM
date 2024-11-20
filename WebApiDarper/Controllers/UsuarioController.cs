using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiDarper.Models;
using WebApiDarper.Services;

namespace WebApiDarper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioInterface _usuariointerface;
        public UsuarioController(IUsuarioInterface usuariointerface)
        {
            _usuariointerface = usuariointerface;
        }

        [HttpGet]

        public async Task<IActionResult> BuscarUsuarios()
        {
            var usuarios = await _usuariointerface.BuscarUsuarios();

            if (usuarios.Status == false)
            {
                return NotFound(usuarios);
            }

            return Ok(usuarios);

        }

        [HttpGet("{usuarioId}")]

        public async Task<IActionResult> BuscarUsuarioPorId(int usuarioId)
        {
            var usuario = await _usuariointerface.BuscarUsuariosporId(usuarioId);
            if (usuario.Status == false)
            {
                return NotFound(usuario);
            }

            return Ok(usuario); 
        }


        [HttpPost]

        public async Task<IActionResult> CriarUsuarioPorId(UsuarioCriarDto usuarioCriarDtox)
        {
            var usuarios = await _usuariointerface.CriarUsuarioPorId(usuarioCriarDtox);

            if(usuarios.Status == null)
            {
                return NotFound(usuarios);
            }

            return Ok(usuarios);
        }

        [HttpPut]
        public async Task<IActionResult> EditarUsuarioPorId(UsuarioEditarDto usuarioEditarDto)
        {
            var usuarios = await _usuariointerface.EditarUsuarioPorId(usuarioEditarDto);

            if(usuarios.Status == null)
            {
                return NotFound(usuarios);
            }
            return Ok(usuarios);
        }

        [HttpDelete]

        public async Task<IActionResult> RemoverUsuarios(int usuarioId)
        {
            var usuarios = await _usuariointerface.RemoverUsuarioPorId(usuarioId);

            if (usuarios.Status == null)
            {
                return BadRequest(usuarios);
            }
            return Ok(usuarios);
        }
    }
}
