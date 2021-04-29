using APIReporteGastos.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static APIReporteGastos.Models.Reporte;

namespace APIReporteGastos.Data
{
    public class ReporteRepositorio
    {
        private readonly string _connectionString;
        public ReporteRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("defaultConnection");
        }

        public async Task<List<ReporteList>> GetAll()
        {
            var response = new List<ReporteList>();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_REPORTE_LISTA", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(MapToList(reader));
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

        private ReporteList MapToList(SqlDataReader reader)
        {
            return new ReporteList()
            {
                reporteList_id = (int)reader["RG_ID"],
                reporteList_concepto = reader["RG_CONCEPTO"].ToString(),
                reporteList_total = (decimal)reader["RG_TOTAL_GASTO"],
                reporteList_aprobado_por = reader["RG_APROBADO_POR"].ToString(),
                reporteList_empleado_id = (int)reader["EM_ID"],
                reporteList_empleado = (string)reader["EMPLEADO"]
            };
        }

        public async Task<ReporteDetalle> GetById(int id)
        {
            ReporteDetalle response = new ReporteDetalle();
            ReporteDetalle response1 = new ReporteDetalle();
            ReporteDetalle response2 = new ReporteDetalle();
            try
            {
                response1 = await CreateMaster(id);
                response2 = await CreateDetail(id);

                response.reporteDetalle_id = response1.reporteDetalle_id;
                response.reporteDetalle_concepto = response1.reporteDetalle_concepto;
                response.reporteDetalle_total = response1.reporteDetalle_total;
                response.reporteDetalle_aprobado_por = response1.reporteDetalle_aprobado_por;
                response.reporteDetalle_empleado_id = response1.reporteDetalle_empleado_id;
                response.reporteDetalle_empleado = response1.reporteDetalle_empleado;
                response.reporteDetalle_Lista = response2.reporteDetalle_Lista;

                return response;
            }
            catch (Exception e)
            {
                return response;
            }
        }

        private async Task<ReporteDetalle> CreateMaster(int id)
        {
            ReporteDetalle response = new ReporteDetalle();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_REPORTE_DETALLES_MAESTRO", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ID", id));
                        await sql.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.reporteDetalle_id = (int)reader["RG_ID"];
                                response.reporteDetalle_concepto = reader["RG_CONCEPTO"].ToString();
                                response.reporteDetalle_total = (decimal)reader["RG_TOTAL_GASTO"];
                                response.reporteDetalle_aprobado_por = reader["RG_APROBADO_POR"].ToString();
                                response.reporteDetalle_empleado_id = (int)reader["EM_ID"];
                                response.reporteDetalle_empleado = (string)reader["EMPLEADO"];
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

        private async Task<ReporteDetalle> CreateDetail(int id)
        {
            ReporteDetalle response = new ReporteDetalle();
            List<Gastos_Detalle> dg_obj = new List<Gastos_Detalle>();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_REPORTE_DETALLES_DETALLE", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ID", id));
                        await sql.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {

                            /*while (await reader.ReadAsync())
                            {
                                response.reporteDetalle_Lista = ;
                            }*/
                            foreach (var item in reader)
                            {
                                var asignar = new Gastos_Detalle
                                {
                                    gd_id = (int)reader["GD_ID"],
                                    gd_descripcion = reader["GD_DESCRIPCION"].ToString(),
                                    gd_fecha = (DateTime)reader["GD_FECHA"],
                                    gd_cuenta = reader["GD_CUENTA"].ToString(),
                                    gd_gasto_individual = (decimal)reader["GD_GASTO_INDIVIDUAL"],
                                    gd_reporte_id = (int)reader["RG_ID"]
                                };
                                dg_obj.Add(asignar);
                            }
                        }
                        response.reporteDetalle_Lista = dg_obj;
                        return response;
                    }
                }
            }
            catch (Exception e)
            {
                return response;
            }
        }

        public async Task<GenericResponse> Insert(Reporte reporte_obj)
        {
            GenericResponse gr = new GenericResponse();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_REPORTE_CREAR", sql))
                    {

                        //[Parámetro de salida del Procedimiento Almacenado]
                        SqlParameter id_reporte_creado = new SqlParameter("@ID_REPORTE_CREADO", SqlDbType.Int);
                        id_reporte_creado.Direction = ParameterDirection.Output;
                        //
                        SqlParameter exito = new SqlParameter("@EXITO", SqlDbType.Bit);
                        exito.Direction = ParameterDirection.Output;

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(id_reporte_creado);
                        cmd.Parameters.Add(exito);
                        cmd.Parameters.Add(new SqlParameter("@CONCEPTO", reporte_obj.reporte_concepto));
                        cmd.Parameters.Add(new SqlParameter("@FECHA_INICIAL", reporte_obj.reporte_fecha_inicial));
                        cmd.Parameters.Add(new SqlParameter("@FECHA_FINAL", reporte_obj.reporte_fecha_final));
                        cmd.Parameters.Add(new SqlParameter("@TOTAL_REPORTE", reporte_obj.reporte_gasto));
                        cmd.Parameters.Add(new SqlParameter("@APROBADO_POR", reporte_obj.reporte_aprobado_por));
                        cmd.Parameters.Add(new SqlParameter("@ID_EMPLEADO", reporte_obj.reporte_empleado_id));

                        /*cmd.Parameters.Add(new SqlParameter("@ID_REPORTE_CREADO", 0));
                        cmd.Parameters.Add(new SqlParameter("@EXITO", false));*/

                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        gr.NumeroReporte = (int)id_reporte_creado.Value;
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
        public async Task<GenericResponse> Update(ReporteActualizar ra_obj)
        {
            GenericResponse gr = new GenericResponse();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_REPORTE_ACTUALIZAR", sql))
                    {

                        //[Parámetro de salida del Procedimiento Almacenado]
                        SqlParameter exito = new SqlParameter("@EXITO", SqlDbType.Bit);
                        exito.Direction = ParameterDirection.Output;

                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(exito);
                        cmd.Parameters.Add(new SqlParameter("@ID", ra_obj.reporteActualizar_id));
                        cmd.Parameters.Add(new SqlParameter("@CONCEPTO", ra_obj.reporteActualizar_concepto));
                        cmd.Parameters.Add(new SqlParameter("@FECHA_INICIAL", ra_obj.reporteActualizar_fecha_inicial));
                        cmd.Parameters.Add(new SqlParameter("@FECHA_FINAL", ra_obj.reporteActualizar_fecha_final));
                        cmd.Parameters.Add(new SqlParameter("@APROBADO_POR", ra_obj.reporteActualizar_aprobado_por));
                        cmd.Parameters.Add(new SqlParameter("@EM_ID", ra_obj.reporteActualizar_empleado_id));

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
                    using (SqlCommand cmd = new SqlCommand("SP_REPORTE_ELIMINAR", sql))
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
