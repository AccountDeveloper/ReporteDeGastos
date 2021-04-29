using System;

namespace APIReporteGastos.Models
{
    public class Gastos_Detalle
    {
        public int gd_id { get; set; }
        public string gd_descripcion { get; set; }
        public DateTime gd_fecha { get; set; }
        public string gd_cuenta { get; set; }
        public decimal gd_gasto_individual { get; set; }
        public int gd_status { get; set; }
        public int gd_reporte_id { get; set; }
    }
}
