using APIReporteGastos.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static APIReporteGastos.Models.Usuario;

namespace APIReporteGastos.Data
{
    public class UsuarioRepositorio
    {
        private readonly string _connectionString;
        public UsuarioRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("defaultConnection");
        }

        public async Task<Usuario> Insert(UsuarioCrear us_obj)
        {
            Usuario us = new Usuario();
            SHA256Managed sha = new SHA256Managed();
            byte[] byteContra = Encoding.Default.GetBytes(us_obj.usuario_password);//primero hay que dar en byte el texto, se coloca using System.text
            byte[] byteContraCifrado = sha.ComputeHash(byteContra);//ciframos lo introducido en la linea anterior y se almacena en una variable
            string cadenaCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");//se convierte a string los bytes cifrados, el cifrado trae guión y lo reemplazamos
            GenericResponse gr = new GenericResponse();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_USUARIO_CREAR", sql))
                    {

                        //[Parámetro de salida del Procedimiento Almacenado]
                        SqlParameter id_creado = new SqlParameter("@ID_CREADO", SqlDbType.Int);
                        id_creado.Direction = ParameterDirection.Output;

                        SqlParameter exito = new SqlParameter("@EXITO", SqlDbType.Bit);
                        exito.Direction = ParameterDirection.Output;

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(id_creado);
                        cmd.Parameters.Add(exito);
                        cmd.Parameters.Add(new SqlParameter("@NOMBRE", us_obj.usuario_nombre));
                        cmd.Parameters.Add(new SqlParameter("@APELLIDO", us_obj.usuario_apellido));
                        cmd.Parameters.Add(new SqlParameter("@IDENTIFICACION", us_obj.usuario_identificacion));
                        cmd.Parameters.Add(new SqlParameter("@POSICION", us_obj.usuario_posicion));
                        cmd.Parameters.Add(new SqlParameter("@DEPARTAMENTO", us_obj.usuario_departamento));
                        cmd.Parameters.Add(new SqlParameter("@SUPERVISOR", us_obj.usuario_supervisor));
                        cmd.Parameters.Add(new SqlParameter("@USUARIO", us_obj.usuario_usuario));
                        cmd.Parameters.Add(new SqlParameter("@PASSWORD", cadenaCifrada));

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        gr.exito = (bool)exito.Value;
                        if(gr.exito == true)
                        {
                            us.usuario_id = (int)id_creado.Value;
                            us.usuario_nombre = us_obj.usuario_nombre;
                            us.usuario_apellido = us_obj.usuario_apellido;
                            us.usuario_identificacion = us_obj.usuario_identificacion;
                            us.usuario_posicion = us_obj.usuario_posicion;
                            us.usuario_departamento = us_obj.usuario_departamento;
                            us.usuario_supervisor = us_obj.usuario_supervisor;
                            us.usuario_usuario = us_obj.usuario_usuario;
                            us.usuario_password = us_obj.usuario_password;
                            us.usuario_tipo_usuario = 2;
                            us.usuario_status = 1;
                        }
                        return us;
                    }
                }
            }
            catch (Exception e)
            {
                return us;
            }
        }
    }
}
