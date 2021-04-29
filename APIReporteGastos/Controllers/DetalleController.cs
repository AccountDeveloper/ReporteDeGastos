using APIReporteGastos.Data;
using APIReporteGastos.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace APIReporteGastos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleController : Controller
    {
        private readonly DetalleRepositorio _repositorio;
        public DetalleController(DetalleRepositorio repositorio)
        {
            this._repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Gastos_Detalle>> Get(int id)
        {
            return Json(await _repositorio.GetById(id));
        }

        [HttpPost]//esto agrega un detalle
        public async Task<ActionResult> Post([FromBody] Gastos_Detalle gd_obj)
        {
            return Json(await _repositorio.Insert(gd_obj));
        }

        [HttpPut]//esto agrega un MODIFICA
        public async Task<ActionResult> Put([FromBody] Gastos_Detalle gd_obj)
        {
            return Json(await _repositorio.Update(gd_obj));
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return Json(await _repositorio.DeleteById(id));
        }
    }
}
