﻿using Microsoft.AspNet.Identity;
using SYJ.Application.Dto;
using SYJ.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SueldosYjornales.Controllers.Api
{
    //[Authorize]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class EmpleadosController : ApiController
    {
        // GET: api/Empleados
        public HttpResponseMessage Get()
        {
            EmpleadosManagers em = new EmpleadosManagers();
            List<EmpleadoDto> listado = em.ListadoEmpleadosConMarcaDeActivo();
            return Request.CreateResponse<List<EmpleadoDto>>(HttpStatusCode.OK, listado);
        }

        [HttpGet]
        [Route("api/Empleados/SegunUbicacionSucursal")]
        public HttpResponseMessage GetSegunUbicacionSucursal()
        {
            EmpleadosManagers em = new EmpleadosManagers();
            List<EmpleadoDto> listado = new List<EmpleadoDto>();
            if (User.Identity.IsAuthenticated)
            {
                listado = em.ListadoEmpleadosSegunUbicacionSucursal(Guid.Parse(User.Identity.GetUserId()));
            }
            return Request.CreateResponse<List<EmpleadoDto>>(HttpStatusCode.OK, listado);
        }

        [HttpGet]
        [Route("api/Empleados/SegunUbicacionSucursal")]
        public HttpResponseMessage GetSegunUbicacionSucursalMesYear(int mes, int year)
        {
            EmpleadosManagers em = new EmpleadosManagers();
            List<EmpleadoDto> listado = new List<EmpleadoDto>();
            if (User.Identity.IsAuthenticated)
            {
                listado = em.ListadoEmpleadosSegunUbicacionSucursal(Guid.Parse(User.Identity.GetUserId()), mes, year);
            }
            return Request.CreateResponse<List<EmpleadoDto>>(HttpStatusCode.OK, listado);
        }
        [HttpGet]
        [Route("api/Empleados/InactivosSegunUbicacionSucursal")]
        public HttpResponseMessage GetInactivosSegunUbicacionSucursalMesYear()
        {
            EmpleadosManagers em = new EmpleadosManagers();
            List<EmpleadoDto> listado = new List<EmpleadoDto>();
            if (User.Identity.IsAuthenticated)
            {
                listado = em.ListadoEmpleadosInactivosSegunUbicacionSucursal(Guid.Parse(User.Identity.GetUserId()));
            }
            return Request.CreateResponse<List<EmpleadoDto>>(HttpStatusCode.OK, listado);
        }

        // GET: api/Empleados/5
        public HttpResponseMessage Get(int id)
        {
            EmpleadosManagers em = new EmpleadosManagers();
            MensajeDto mensaje = em.RecuperarEmpleado(id);
            return Request.CreateResponse(HttpStatusCode.OK, mensaje);
        }

        // POST: api/Empleados
        public HttpResponseMessage Post(EmpleadoDto eDto)
        {
            EmpleadosManagers em = new EmpleadosManagers();
            MensajeDto mensaje = em.CargarEmpleado(eDto, Guid.Parse(User.Identity.GetUserId()));
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }

        // PUT: api/Empleados/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Empleados/5
        public HttpResponseMessage Delete(int id)
        {
            EmpleadosManagers em = new EmpleadosManagers();
            MensajeDto mensaje = em.EliminarEmpleado(id);
            return Request.CreateResponse(HttpStatusCode.Created, mensaje);
        }
    }
}
