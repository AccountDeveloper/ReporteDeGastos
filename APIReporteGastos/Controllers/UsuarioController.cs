using APIReporteGastos.Data;
using APIReporteGastos.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static APIReporteGastos.Models.Usuario;

namespace APIReporteGastos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {
        private readonly UsuarioRepositorio _repositorio;
        public UsuarioController(UsuarioRepositorio repositorio)
        {
            this._repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UsuarioCrear us_obj)
        {
            return Json(await _repositorio.Insert(us_obj));
        }
    }
}
