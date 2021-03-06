﻿using SYJ.Application.Dto;
using SYJ.Domain.Managers.Auxiliares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using SYJ.Application.Dto.Auxiliares;
using SYJ.Domain.Managers;

namespace SueldosYjornales.Controllers.Api.Auxiliares
{
    [Authorize(Roles = "LiquidadorSalario")]
    public class LiquidacionSalariosController : ApiController
    {
        // GET: api/LiquidacionSalarios
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/LiquidacionSalarios/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/LiquidacionSalarios
        public HttpResponseMessage Post(FormLiquidacionDto fldto)
        {
            LiquidacionSalariosManagers lsm = new LiquidacionSalariosManagers(fldto,
                Guid.Parse(User.Identity.GetUserId()));
            MensajeDto mensaje = lsm.GenerarLiquidacionesSalarios();
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // POST: api/LiquidacionSalarios/Detalles
        [HttpPost]
        [Route("api/LiquidacionSalarios/Detalles")]
        public HttpResponseMessage PostDetalles(FormLiquidacionDto fldto) {
            LiquidacionSalariosManagers lsm = new LiquidacionSalariosManagers(fldto,
                Guid.Parse(User.Identity.GetUserId()));
            MensajeDto mensaje = lsm.RecuperarDetalles();
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }
        [HttpPost]
        [Route("api/LiquidacionSalarios/DetallesUnSoloEmpleado")]
        public HttpResponseMessage PostDetallesUnSoloEmpleado(FormLiquidacionDto fldto) {
            LiquidacionSalariosManagers lsm = new LiquidacionSalariosManagers(fldto,
                Guid.Parse(User.Identity.GetUserId()));
            MensajeDto mensaje = lsm.RecuperarDetallesDeUnYearYunSoloEmpleado();
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // POST: api/LiquidacionSalarios/ParaImprimir
        [HttpPost]
        [Route("api/LiquidacionSalarios/ParaImprimir")]
        public HttpResponseMessage PostParaImprimir(FormLiquidacionDto fldto) {
            LiquidacionSalariosManagers lsm = new LiquidacionSalariosManagers(fldto,
                Guid.Parse(User.Identity.GetUserId()));
            MensajeDto mensaje = lsm.RecuperarDetallesParaImprimir();
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        [HttpPost]
        [Route("api/LiquidacionSalarios/ParaPlanillaSueldos")]
        public HttpResponseMessage PostParaImprimir(MesYearEmpresaSucursalesDto myesDto) {

            //Se crea el formulario que tiene la seleccion por empleado
            FormLiquidacionDto fldto = new FormLiquidacionDto();
            fldto.Year = myesDto.Year;
            fldto.Mes = myesDto.Mes;
            ///Se tiene que recuperar segun las sucursales los empleados activos
            //fldto.EmpleadosSeleccionados = Aqui se requiere empleados activos
            EmpleadosManagers em = new EmpleadosManagers();
            fldto.EmpleadosSeleccionados = em.EmpleadosSeleccionados(myesDto);

            LiquidacionSalariosManagers lsm = new LiquidacionSalariosManagers(fldto,
                Guid.Parse(User.Identity.GetUserId()));
            MensajeDto mensaje = lsm.RecuperarDetallesParaImprimir();
            mensaje = lsm.RecuperarDetallesSubtotalesPorSuc((List<LiquidacionSalarioDto>)mensaje.ObjetoDto);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/LiquidacionSalarios/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/LiquidacionSalarios/5
        public HttpResponseMessage Delete(int id)
        {
            MensajeDto mensaje = LiquidacionSalariosManagers.EliminarMovimiento(id);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }
    }
}
