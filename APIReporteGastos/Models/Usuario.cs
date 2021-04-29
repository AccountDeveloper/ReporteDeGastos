using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIReporteGastos.Models
{
    public class Usuario
    {
        public int usuario_id { get; set; }
        public string usuario_nombre { get; set; }
        public string usuario_apellido { get; set; }
        public string usuario_identificacion { get; set; }
        public string usuario_posicion { get; set; }
        public string usuario_departamento { get; set; }
        public string usuario_supervisor { get; set; }
        public string usuario_usuario { get; set; }
        public string usuario_password { get; set; }
        public int usuario_tipo_usuario { get; set; }
        public int usuario_status { get; set; }

        public class UsuarioCrear
        {
            public int usuario_id { get; set; }
            public string usuario_nombre { get; set; }
            public string usuario_apellido { get; set; }
            public string usuario_identificacion { get; set; }
            public string usuario_posicion { get; set; }
            public string usuario_departamento { get; set; }
            public string usuario_supervisor { get; set; }
            public string usuario_usuario { get; set; }
            public string usuario_password { get; set; }
            public int usuario_tipo_usuario { get; set; }
            public int usuario_status { get; set; }
        }
    }
}
