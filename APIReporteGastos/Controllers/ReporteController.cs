using APIReporteGastos.Data;
using APIReporteGastos.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static APIReporteGastos.Models.Reporte;

namespace APIReporteGastos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteController : Controller
    {
        private readonly ReporteRepositorio _repositorio;
        public ReporteController(ReporteRepositorio repositorio)
        {
            this._repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        //Lista de todos los reportes activos
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReporteList>>> Get()
        {
            return Json(await _repositorio.GetAll());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReporteDetalle>> Get(int id)
        {
           return Json(await _repositorio.GetById(id));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Reporte reporte_obj)
        {
            return Json(await _repositorio.Insert(reporte_obj));
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] ReporteActualizar ra_obj)
        {
            return Json(await _repositorio.Update(ra_obj));
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _repositorio.DeleteById(id);
        }
    }
}
