using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAppiUser.Models;
using WebAppiUser.Models.Request;
using WebAppiUser.Models.Response;

namespace WebAppiUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        /// <summary>
        /// Mostrar todos los usuarios registrados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ViewUsuarios() {
            Respuesta<List<Usuarios>> oRespuesta = new Respuesta<List<Usuarios>>();
            try
            {
                using (usuariosContext db = new usuariosContext())
                {
                    var lst = db.Usuarios.ToList();
                    oRespuesta.Exito = 1;
                    oRespuesta.Mensaje = "Usuarios Registrados";
                    oRespuesta.Data = lst;
                }

            }
            catch (Exception ex)
            {
                oRespuesta.Mensaje = ex.Message;
            }
            Console.Write("Usuarios registrados");
            return Ok(oRespuesta);
        }

        /// <summary>
        /// Desplega datos especificos de un usuario 
        /// </summary>
        /// <param name="Cedula"></param>
        /// <returns></returns>
        [HttpGet("{Cedula}")]
        public IActionResult ViewClientes(string Cedula)
        {
            Respuesta<Usuarios> oRespuesta = new Respuesta<Usuarios>();
            try
            {
                using (usuariosContext db = new usuariosContext())
                {
                    var lst = db.Usuarios.Find(Cedula);
                    oRespuesta.Exito = 1;
                    oRespuesta.Data = lst;
                }

            }
            catch (Exception ex)
            {

                oRespuesta.Mensaje = ex.Message;
            }
            return Ok(oRespuesta);
        }


        /// <summary>
        /// Registar un usuario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddUser(UsuarioRequest model)
        {
            Respuesta<object> oRespuesta = new Respuesta<object>();
            try
            {
                using (usuariosContext db = new usuariosContext())
                {
                    Usuarios oUsuarios = new Usuarios();
                    Usuarios oUsuariosSec = db.Usuarios.Find(model.Cedula);
                    Usuarios oUsuariosTer = db.Usuarios.Find(model.Correo);
                    if (oUsuariosSec !=null || oUsuariosTer != null)
                    {
 
                        oRespuesta.Exito = 0;
                        oRespuesta.Mensaje = "El correo o la cedula ya estan registrados en el sistema";
                        return Ok(oRespuesta);
                    }
                    else
                    {
                        oUsuarios.Cedula = model.Cedula;
                        oUsuarios.Nombre = model.Nombre;
                        oUsuarios.Apellido = model.Apellido;
                        oUsuarios.Correo = model.Correo;
                        oRespuesta.Mensaje = "Usuario Registrado";
                        db.Usuarios.Add(oUsuarios);
                        db.SaveChanges();
                        oRespuesta.Exito = 1;      
                    }

                }
            }
            catch (Exception ex)
            {

                oRespuesta.Mensaje = ex.Message;
            }
            oRespuesta.Mensaje = "Proceso Exitoso";
            return Ok(oRespuesta);
        }



        /// <summary>
        /// Editar un usuario si existe
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult EditCliente(UsuarioRequest model)
        {
            Respuesta<object> oRespuesta = new Respuesta<object>();
            try
            {
                using (usuariosContext db = new usuariosContext())
                {
                    Usuarios oUsuariosSec = db.Usuarios.Find(model.Cedula);
                    if (oUsuariosSec !=null)
                    {
                        oRespuesta.Exito = 0;
                        oRespuesta.Mensaje = "El Usuario no existe";
                        return Ok(oRespuesta);
                    }
                    Usuarios oUsuarios = new Usuarios();
                    oUsuarios.Cedula = model.Cedula;
                    oUsuarios.Nombre = model.Nombre;
                    oUsuarios.Apellido = model.Apellido;
                    oUsuarios.Correo = model.Correo;
                    db.Entry(oUsuarios).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                    oRespuesta.Exito = 1;
                }
            }
            catch (Exception ex)
            {

                oRespuesta.Mensaje = ex.Message;
            }
            oRespuesta.Mensaje = "Proceso Exitoso";
            return Ok(oRespuesta);
        }

        /// <summary>
        /// Eliminar usuario si existe
        /// </summary>
        /// <param name="Cedula"></param>
        /// <returns></returns>
        [HttpDelete("Cedula")]
        public IActionResult DeleteCliente(string Cedula)
        {
            Respuesta<object> oRespuesta = new Respuesta<object>();
            try
            {
                using (usuariosContext db = new usuariosContext())
                {
                    Usuarios oUsuariosSec = db.Usuarios.Find(Cedula);
                    if (oUsuariosSec == null)
                    {
                        oRespuesta.Exito = 0;
                        oRespuesta.Mensaje = "El Usuario no existe";
                        return Ok(oRespuesta);
                    }
                    Usuarios oUsuarios = db.Usuarios.Find(Cedula);
                    db.Remove(oUsuarios);
                    db.SaveChanges();
                    oRespuesta.Exito = 1;
                   
                }
            }
            catch (Exception ex)
            {

                oRespuesta.Mensaje = ex.Message;
            }

            return Ok(oRespuesta);
        }
    }
}
