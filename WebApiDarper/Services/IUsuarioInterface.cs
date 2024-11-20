using WebApiDarper.Models;

namespace WebApiDarper.Services
{
    public interface IUsuarioInterface
    {
        // BUSCA UMA LISTA DE USUARIOS
        Task<ResponseModel<List<UsuarioListarDto>>> BuscarUsuarios();
        // BUSCAR USUARIO POR ID buscar 1 usuario
        Task<ResponseModel<UsuarioListarDto>> BuscarUsuariosporId(int id);

        //CRIANDO UM USUARIO - e me retona uma lissta para ver todos criados
        Task<ResponseModel<List<UsuarioListarDto>>> CriarUsuarioPorId(UsuarioCriarDto usuarioCriarDtox);

        //EDITAR USUARIOS - me retorna uma lista de usuarios inclusive meu usuari editado
        Task<ResponseModel<List<UsuarioListarDto>>> EditarUsuarioPorId(UsuarioEditarDto usuarioEditarDto);

        Task<ResponseModel<List<UsuarioListarDto>>> RemoverUsuarioPorId(int id);


    }
}
