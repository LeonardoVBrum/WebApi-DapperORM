using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.ResponseCompression;
using System.Collections.Generic;
using System.Data.SqlClient;
using WebApiDarper.Models;
namespace WebApiDarper.Services
{
    public class UsuarioService : IUsuarioInterface
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public UsuarioService(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<ResponseModel<List<UsuarioListarDto>>> BuscarUsuarios()

        {

            ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuarioBanco = await connection.QueryAsync<Usuario>("SELECT * FROM Usuarios");

                if (usuarioBanco.Count() == 0)
                {
                    response.Memsagem = "Nenhum resultado encontrado!";
                    response.Status = false;
                    return response;
                }


                // TRANSFORMAR EM MAPEAMENTO
                var usuariomapper = _mapper.Map<List<UsuarioListarDto>>(usuarioBanco);
                response.Dados = usuariomapper;
                response.Memsagem = "Usuario Mapeado com sucesso!";
            }
            return response;

        }

        public async Task<ResponseModel<UsuarioListarDto>> BuscarUsuariosporId(int usuarioId)
        {
            //CRIAMOS UMA VARIAVEL RESPONSE TO TIPO RESPONSE MODEL USUARIOS DTO
            // ISSO FAZ QUE Q NOSSA MODEL RESPONDE RECEBA A NOSSA MODEL DTO PARA SER RETORNADA
            ResponseModel<UsuarioListarDto> response = new ResponseModel<UsuarioListarDto>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuarioBanco = await connection.QueryFirstOrDefaultAsync<Usuario>("select * from Usuarios where id = @Id", new { Id = usuarioId });
                if (usuarioBanco == null)
                {
                    response.Memsagem = "Nenhum resultado encontrado!";
                    response.Status = false;
                    return response;
                }

                var usuarioMapeado = _mapper.Map<UsuarioListarDto>(usuarioBanco);
                response.Dados = usuarioMapeado;
                response.Memsagem = "Usuario Localizado com sucesso!";
            }

            return response;
        }


        private static async Task<IEnumerable<Usuario>> ListarUsuarios(SqlConnection con)
        {
            return await con.QueryAsync<Usuario>("select * from Usuarios");
        }

        public async Task<ResponseModel<List<UsuarioListarDto>>> CriarUsuarioPorId(UsuarioCriarDto usuarioCriarDtox)
        {
            ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var usuarioBanco = await connection.ExecuteAsync("insert into Usuarios (NomeCompleto,Email,Cargo,Salario,CPF,Senha, Situacao) values (@NomeCompleto,@Email,@Cargo,@Salario,@CPF,@Senha,@Situacao)", usuarioCriarDtox);

                if (usuarioBanco == 0)
                {

                    response.Memsagem = "Ocorreu um erro no registro";
                    response.Status = false;
                    return response;




                }
                var usuarios = await ListarUsuarios(connection);

                var usuarioMapeados = _mapper.Map<List<UsuarioListarDto>>(usuarios);
                response.Dados = usuarioMapeados;
                response.Memsagem = "Usuario Criado com Sucesso!";
            }
            return response;
        }

        public async Task<ResponseModel<List<UsuarioListarDto>>> EditarUsuarioPorId(UsuarioEditarDto usuarioEditarDto)
        {
            ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {

                var usuarioBanco = await connection.ExecuteAsync("UPTDATE Usuarios SET NomeCompleto = @NomeCompleto," +
                    " Email = @Email," +
                    "Cargo = @Cargo," +
                    "Salario=@Salario," +
                    "CPF = @CPF WHERE Id = @Id", usuarioEditarDto);
                if (usuarioBanco == 0)
                {

                    response.Memsagem = "Ocorreu um erro no registro";
                    response.Status = false;
                    return response;




                }
                var usuarios = await ListarUsuarios(connection);
                var usuarioMapeados = _mapper.Map<List<UsuarioListarDto>>(usuarios);
                response.Dados = usuarioMapeados;
                response.Memsagem = "Usuario Alterado com sucesso!";
            }
            return response;


        }

        public async Task<ResponseModel<List<UsuarioListarDto>>> RemoverUsuarioPorId(int usuarioId)
        {
            ResponseModel<List<UsuarioListarDto>> response = new ResponseModel<List<UsuarioListarDto>>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                                                     // ExecuteAsync significa que ele vai executar uma operação no banco   
                var usuarioBanco = await connection.ExecuteAsync("DELETE FROM Usuarios WHERE Id = @Id", new {Id = usuarioId });

                if(usuarioBanco == 0)
                {
                    response.Memsagem = "Usuario não econtrado no sistema";
                    response.Status = false;
                    return response;
                }

                //LISTA OS USUARIOS DO BANCO QUE SOBRARAM
                var usuarios = await ListarUsuarios(connection);
                // MAPEIA USUARIOS
                var usuariosMapeados = _mapper.Map<List<UsuarioListarDto>>(usuarios);

                response.Memsagem = "Usuario deletado com sucesso!";
                response.Dados = usuariosMapeados;
                response.Status = true;
                
            }
            return response;
        }
    }
}
