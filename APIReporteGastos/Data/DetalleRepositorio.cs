using APIReporteGastos.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Threading.Tasks;

namespace APIReporteGastos.Data
{
    public class DetalleRepositorio
    {
        private readonly string _connectionString;
        public DetalleRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("defaultConnection");
        }

        //LISTAR UN DETALLE DEL REPORTE
        public async Task<Gastos_Detalle> GetById(int Id)
        {
            Gastos_Detalle response = null;
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_DETALLE_REPORTE", sql))
                    {
                        //[Parámetro de salida del Procedimiento Almacenado]
                        SqlParameter exito = new SqlParameter("@EXITO", SqlDbType.Bit);
                        exito.Direction = ParameterDirection.Output;

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(exito);
                        cmd.Parameters.Add(new SqlParameter("@ID", Id));

                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response = MapToValue(reader);
                            }
                        }
                        return response;
                    }
                }
            }
            catch (Exception e)
            {
                return response;
            }
        }
        private Gastos_Detalle MapToValue(SqlDataReader reader)
        {
            return new Gastos_Detalle()
            {
                gd_id = (int)reader["GD_ID"],
                gd_descripcion = reader["GD_DESCRIPCION"].ToString(),
                gd_fecha = (DateTime)reader["GD_FECHA"],
                gd_cuenta = reader["GD_CUENTA"].ToString(),
                gd_gasto_individual  = (decimal)reader["GD_GASTO_INDIVIDUAL"],
                gd_reporte_id = (int)reader["RG_ID"],
            };
        }


        public async Task<GenericResponse> Insert(Gastos_Detalle gd_obj)
        {
            GenericResponse gr = new GenericResponse();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_DETALLE_CREAR", sql))
                    {

                        //[Parámetro de salida del Procedimiento Almacenado]
                        SqlParameter exito = new SqlParameter("@EXITO", SqlDbType.Bit);
                        exito.Direction = ParameterDirection.Output;

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(exito);
                        cmd.Parameters.Add(new SqlParameter("@DESCRIPCION", gd_obj.gd_descripcion));
                        cmd.Parameters.Add(new SqlParameter("@FECHA", gd_obj.gd_fecha));
                        cmd.Parameters.Add(new SqlParameter("@CUENTA", gd_obj.gd_cuenta));
                        cmd.Parameters.Add(new SqlParameter("@GASTO_INDIVIDUAL", gd_obj.gd_gasto_individual));
                        cmd.Parameters.Add(new SqlParameter("@ID_REPORTE", gd_obj.gd_reporte_id));

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        gr.exito = (bool)exito.Value;

                        return gr;
                    }
                }
            }
            catch (Exception e)
            {
                return gr;
            }
        }

        /*UPDATE*/
        public async Task<GenericResponse> Update(Gastos_Detalle gd_obj)
        {
            GenericResponse gr = new GenericResponse();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_DETALLE_ACTUALIZAR", sql))
                    {

                        //[Parámetro de salida del Procedimiento Almacenado]
                        SqlParameter exito = new SqlParameter("@EXITO", SqlDbType.Bit);
                        exito.Direction = ParameterDirection.Output;

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(exito);
                        cmd.Parameters.Add(new SqlParameter("@ID", gd_obj.gd_id));
                        cmd.Parameters.Add(new SqlParameter("@DESCRIPCION", gd_obj.gd_descripcion));
                        cmd.Parameters.Add(new SqlParameter("@FECHA", gd_obj.gd_fecha));
                        cmd.Parameters.Add(new SqlParameter("@CUENTA", gd_obj.gd_cuenta));
                        cmd.Parameters.Add(new SqlParameter("@GASTO_INDIVIDUAL", gd_obj.gd_gasto_individual));
                        cmd.Parameters.Add(new SqlParameter("@RG_ID", gd_obj.gd_reporte_id));

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        gr.exito = (bool)exito.Value;

                        return gr;
                    }
                }
            }
            catch (Exception e)
            {
                return gr;
            }
        }


        //eliminar detalle
        public async Task<GenericResponse> DeleteById(int Id)
        {
            GenericResponse gr = new GenericResponse();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_DETALLE_ELIMINAR", sql))
                    {
                        SqlParameter exito = new SqlParameter("@EXITO", SqlDbType.Bit);
                        exito.Direction = ParameterDirection.Output;

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(exito);
                        cmd.Parameters.Add(new SqlParameter("@ID", Id));
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        gr.exito = (bool)exito.Value;
                        return gr;
                    }
                }
            }
            catch (Exception e)
            {
                return gr;
            }
        }
    }
}
