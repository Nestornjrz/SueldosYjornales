﻿using SYJ.Application.Dto;
using SYJ.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SueldosYjornales.Controllers.Api
{
    public class EmpleadosController : ApiController
    {
        // GET: api/Empleados
        public HttpResponseMessage Get()
        {
            EmpleadosManagers em = new EmpleadosManagers();
            List<EmpleadoDto> listado = em.ListadoEmpleados();
            return Request.CreateResponse<List<EmpleadoDto>>(HttpStatusCode.OK, listado);
        }

        // GET: api/Empleados/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Empleados
        public HttpResponseMessage Post(EmpleadoDto eDto)
        {
            EmpleadosManagers em = new EmpleadosManagers();
            MensajeDto mensaje = em.CargarEmpleado(eDto);
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
