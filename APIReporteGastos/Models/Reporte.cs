using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APIReporteGastos.Models
{
    public class Reporte
    {
        public int reporte_id { get; set; }
        [Required]
        public string reporte_concepto { get; set; }
        [Required]
        public DateTime reporte_fecha_inicial { get; set; }
        [Required]
        public DateTime reporte_fecha_final { get; set; }
        [Required]
        public decimal reporte_gasto { get; set; }
        [Required]
        public string reporte_aprobado_por { get; set; }
        [Required]
        public int reporte_status { get; set; }
        [Required]
        public int reporte_empleado_id { get; set; }

        public class ReporteList
        {
            public int reporteList_id { get; set; }
            public string reporteList_concepto { get; set; }
            public decimal reporteList_total { get; set; }
            public string reporteList_aprobado_por { get; set; }
            public int reporteList_empleado_id { get; set; }
            public string reporteList_empleado { get; set; }
        }
        public class ReporteDetalle
        {
            public int reporteDetalle_id { get; set; }
            public string reporteDetalle_concepto { get; set; }
            public decimal reporteDetalle_total { get; set; }
            public string reporteDetalle_aprobado_por { get; set; }
            public int reporteDetalle_empleado_id { get; set; }
            public string reporteDetalle_empleado { get; set; }
            public List<Gastos_Detalle> reporteDetalle_Lista { get;set;}
        }

        public class ReporteActualizar
        {
            [Required]
            public int reporteActualizar_id { get; set; }
            [Required]
            public string reporteActualizar_concepto { get; set; }
            [Required]
            public DateTime reporteActualizar_fecha_inicial { get; set; }
            [Required]
            public DateTime reporteActualizar_fecha_final { get; set; }
            [Required]
            public string reporteActualizar_aprobado_por { get; set; }
            [Required]
            public int reporteActualizar_empleado_id { get; set; }
        }
    }
}
