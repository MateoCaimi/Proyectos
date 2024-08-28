using Azure;
using LogicaAccesoDatos.EF;
using LogicaNegocio.Entidades;
using LogicaNegocio.Entidades.DTOs;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using LogicaNegocio.ViewModel;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneOf.Types;
using PdfSharp.Pdf.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace LogicaAccesoDatos.Repositorios
{
    internal class RepositorioEmpleado : IRepositorioEmpleado
    {
        public ProyectoContext Context { get; set; }
        //private readonly HttpClient _httpClient;
        public RepositorioEmpleado()
        {
            this.Context = new ProyectoContext();
        }
        public void Agregar(Empleado item)
        {
            try
            {
                item.Validar();
                if(this.BuscarPorNombreYCedula("", item.Cedula) != null)
                {
                    throw new EmpleadoException("Ya existe un empleado con esta cédula en el sistema.");
                }
                item.Activo = true;
                Context.Empleados.Add(item);
                Context.SaveChanges();
            }
            catch (EmpleadoException ee)
            {
                throw new EmpleadoException(ee.Message);
            }
        }

        public void AgregarEmp(Empleado item, bool desdeForm)
        {
            try
            {
                if (desdeForm)
                {
                    item.ValidarNulos();
                }
                this.Agregar(item);
            }
            catch (EmpleadoException ee)
            {
                throw new EmpleadoException(ee.Message);
            }
        }

        public Empleado Buscar(int id)
        {
            return Context.Empleados.Where(e => e.Id == id).Include(e => e.TipoEmpleado).FirstOrDefault();
        }

        public void Eliminar(Empleado item)
        {
            throw new NotImplementedException();
        }

        public void Modificar(Empleado item)
        {
            Empleado empleado = this.Buscar(item.Id);
            if (empleado == null)
            {
                throw new EmpleadoException("No se encontró el empleado para modificar.");
            }
            item.Validar(); //Seria item validar no empleado validar porque valida los datos viejos sino
            empleado.CuentaBanco = item.CuentaBanco;
            empleado.Banco = item.Banco;
            empleado.FechaIngreso = item.FechaIngreso;
            empleado.IdTipoEmpleado = item.IdTipoEmpleado;
            empleado.Nombre = item.Nombre;
            empleado.Cedula = item.Cedula;
            empleado.Activo = item.Activo;
            empleado.IncentivoXHora = item.IncentivoXHora;
            Context.SaveChanges();
        }

        public IEnumerable<Empleado> TomarTodos()
        {
            return Context.Empleados.Include(e => e.TipoEmpleado).ToList();
        }

        public async Task<string> LlamadaCloudtimes(DateTime desde, DateTime hasta)
        {
            try
            {
                string inicio = desde.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                string fin = hasta.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                using (HttpClient cliente = new HttpClient())
                {

                    Uri uri = new Uri($"https://apicloudtimes.uy/apiclientes/v1/obtenerMarcas?inicio={inicio}&fin={fin}");
                    HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
                    solicitud.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbXByZXNhIjoiQm9kZWdhIFBpZWRyYWZpdGEifQ.TkaIdrD6xet2u9TbuoqV9BPxlEQ9AakyYNn5P27_fJw");

                    Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                    respuesta.Wait();
                    Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                    return await response;
                }
            }
            catch (EmpleadoException ee)
            {
                throw new EmpleadoException(ee.Message);
            }
        }

        public void Precarga()
        {
           // borrarTodasLasMarcas();
            
            int cantTipos = this.GetTipos().Count;
            if (cantTipos == 0)
            {
                TipoEmpleado tipoGenerico = new TipoEmpleado();
                tipoGenerico.Compensacion = 0;
                tipoGenerico.Categoría = "<<A INGRESAR>>";
                tipoGenerico.Presentismo = 0;
                tipoGenerico.ValorHora = 120;

                Context.TiposEmpleados.Add(tipoGenerico);
                Context.SaveChanges();
            }
            int cantCapataces = this.GetUsuariosObra().Count;
            if (cantCapataces == 0)
            {
                UDeObra capatazGenerico = new UDeObra();
                capatazGenerico.Nombre = "<<A INGRESAR>>";
                capatazGenerico.Contrasenia = "ContraseniaGenerica123456";
                capatazGenerico.IntentosFallidos = 0;
                capatazGenerico.NombreUsuario = "CapatazGenerico123";
                capatazGenerico.CambioContrasenia = false;

                Context.Usuarios.Add(capatazGenerico);

                Context.SaveChanges();
            }



        }
        //Esta como paara probar y ejecutar previo a todo
        public void borrarTodasLasMarcas()
        {
            foreach(Marca m in Context.Marcas)
            {
                Context.Marcas.Remove(m);
            }
            Context.SaveChanges(); 
        }

        public void PrecargaMarcasDelAño()
        {
            DateTime hoy = DateTime.Now;
            DateTime desde = new DateTime();
            DateTime hasta = new DateTime();
            int mesInicio = 1;

            if (hoy.Month > 2) //Porque cloudTimes no acepta mas de 5 llamadas
            {
             mesInicio = hoy.Month - 2;
            }

            //Carga los ultimos 2 meses hasta la actualidad o desde principio de año

            for (int i = hoy.Month; i >= mesInicio; i--)
            {
                if(i == hoy.Month)
                {
                    desde = new DateTime(hoy.Year, i, 01, 00, 00, 01);
                    hasta = new DateTime(hoy.Year, i, hoy.Day, hoy.Hour, hoy.Minute, hoy.Second);
                }
                else if (i == 4 || i == 6 || i == 9 || i == 11)
                {
                    desde = new DateTime(hoy.Year, i, 01, 00, 00, 01);
                    hasta = new DateTime(hoy.Year, i, 30, 23, 59, 59);
                }
                else if (i == 2)
                {
                    if (hoy.Year % 4 == 0)
                    {
                        desde = new DateTime(hoy.Year, i, 01, 00, 00, 01);
                        hasta = new DateTime(hoy.Year, i, 28, 23, 59, 59);
                    }

                    desde = new DateTime(hoy.Year, i, 01, 00, 00, 01);
                    hasta = new DateTime(hoy.Year, i, 29, 23, 59, 59);
                }
                else
                {
                    desde = new DateTime(hoy.Year, i, 01, 00, 00, 01);
                    hasta = new DateTime(hoy.Year, i, 31, 23, 59, 59);

                }

                AgregarEmpleadosAObraDTO(desde,hasta);
                ConseguirTodasLasMarcas(desde, hasta);


            }

        }



        private List<Usuario> GetUsuariosObra()
        {
            List<Usuario> usuarios = Context.Usuarios.Where(u => u is UDeObra).ToList();
            return usuarios;
        }

        private List<TipoEmpleado> GetTipos()
        {
            return Context.TiposEmpleados.ToList();
        }

        public bool AgregarEmpleadosAObraDTO(DateTime desde, DateTime hasta)
        {
            DateTime hastaDato;
            DateTime desdeDato;
            bool anomalias = false;

            if (desde.Year == 0001 || hasta.Year == 0001)
            {
                hastaDato = DateTime.Now;

                desdeDato = new DateTime(hastaDato.Year, hastaDato.Month, hastaDato.Day - hastaDato.Day + 1);

            }
            else
            {
                hastaDato = SetHoraA0(hasta);
                desdeDato = SetHoraA0(desde);
            }
            if (this.DiferenciaDias(desde, hasta) > 45)
            {
                throw new EmpleadoException("Debe ingresar un rango de días menor a 45.");
            }
            string response = "";
            try
            {
                response = LlamadaCloudtimes(desdeDato, hastaDato).Result; //Formatear la respuesta cloudtimes.
                if (response.Contains("IP"))
                {
                    throw new EmpleadoException("No se pueden generar marcas ahora mismo, intentar en unos minutos.");
                }
            }
            catch (EmpleadoException ee)
            {
                throw new EmpleadoException("No se pueden generar marcas ahora mismo, intentar en unos minutos.");
            }

            ListadoEmpleadosDTO listado = JsonConvert.DeserializeObject<ListadoEmpleadosDTO>(response);

            foreach (EmpleadoDTO emp in listado.Empleados)
            {
                Empleado empleado = this.BuscarPorNombreYCedula(emp.Nombre, emp.Cedula);
                Obra obra = null;
                string nombreObra = "";
                if (empleado == null)
                {
                    empleado = new Empleado();
                    empleado.Nombre = emp.Nombre;
                    empleado.Cedula = emp.Cedula;
                    empleado.Banco = "<<A INGRESAR>>";
                    empleado.CuentaBanco = "<<A INGRESAR>>";
                    empleado.Activo = true;
                    empleado.IdTipoEmpleado = BuscarTipoXNombre("<<A INGRESAR>>").Id; //Tenemos que tener una precarga con TipoEmpleado genérico
                    this.AgregarEmp(empleado, false);
                }
                if (emp.Marcas.Count() > 0)
                {
                    if (emp.Marcas.First().NombreLector != null)
                    {
                        nombreObra = emp.Marcas.First().NombreLector;
                        obra = GetObraPorNombre(emp.Marcas.First().NombreLector);
                    }
                    else
                    {
                        bool encontroObra = false;
                        string comentario = emp.Marcas.First().Comentario;
                        if (comentario == null)
                        {
                            anomalias = true;
                            continue; //si no hay comentario ni nombre lector continuar sin grabar
                        }

                        foreach (var o in TomarTodasLasObras())
                        {
                            comentario = comentario.ToLower();

                            if (comentario.Contains(o.Nombre.ToLower()))
                            {
                                obra = GetObraPorNombre(o.Nombre);
                                encontroObra = true;
                                break;
                            }
                        }

                        if (!encontroObra)
                        {
                            anomalias = true;
                            continue; // SI EL COMENTARIO NO TIENE LA OBRA NO SE AGREGA ESA MARCA
                        }
                    }
                }
                if (obra == null && nombreObra != "")
                {
                    Fachada f = new Fachada();
                    obra = new Obra();
                    obra.Nombre = nombreObra;
                    obra.Direccion = "<<A INGRESAR>>";
                    obra.FechaInicio = new DateTime(0001, 01, 01);
                    obra.Finalizada = false;
                    obra.NombreCronograma = "<<A INGRESAR>>";
                    obra.IdACargo = 1; //Tenemos que tener una precarga con un usuario de obra genérico    
                    f.AgregarObra(obra);
                }
                AgregarEmpleado(empleado, obra); //SI EL EMPLEADO CAMBIA DE OBRA NO SE AGREGA EL OBRA EMPLEADO NUEVAMENTE. ESO PROVOCA QUE EL METODO DE MARCAS ROMPA. NO EXISTE UN OBRAEMPLEADO NUEVO
            }
            return anomalias;
        }

        private TipoEmpleado BuscarTipoXNombre(string nombre)
        {
            return Context.TiposEmpleados.Where(te => te.Categoría == nombre).FirstOrDefault();
        }

        private int DiferenciaDias(DateTime desde, DateTime hasta)
        {
            return (int)(hasta - desde).TotalDays;
        }

        private bool MarcasEnFecha(Empleado emp, DateTime dia)
        {
            DateTime diaHoraCero = this.SetHoraA0(dia);
            List<Marca> marcasEmpleado = this.MarcasEmpleadoRango(emp, dia, new DateTime(dia.Year, dia.Month, dia.Day + 1, 0, 0, 0));
            if (marcasEmpleado != null && marcasEmpleado.Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        //public void AgregarEmpleadosAObra()
        //{
        //    DateTime desde = new DateTime(2024, 06, 01);
        //    DateTime hasta = new DateTime(2024, 07, 01);
        //    string response = LlamadaCloudtimes(desde, hasta).Result; //Formatear la respuesta cloudtimes.
        //    var root = JObject.Parse(response);
        //    int cantidadEmpleados = (int)root["cantidadEmpleados"];

        //    for (int i = 0; i < cantidadEmpleados; i++)
        //    {
        //        if (!root["empleados"][i]["marcas"].HasValues)//PROBAR
        //        {
        //            continue; // Si no tiene marcas pasa al siguiente empleado
        //        }
        //        string empleadoNom = root["empleados"][i]["nombre"].ToString();
        //        string empleadoCed = root["empleados"][i]["cedula"].ToString();
        //        string nombreObra = root["empleados"][i]["marcas"][0]["nombreLector"].ToString(); //Saco del lector. Nunca será null.
        //        Empleado empleado = this.BuscarPorNombreYCedula(empleadoNom, empleadoCed);
        //        if (empleado == null) //Si no fue añadido ya añadilo.
        //        {
        //            empleado = new Empleado();
        //            empleado.Nombre = empleadoNom;
        //            empleado.Cedula = empleadoCed;
        //            this.Agregar(empleado);
        //        }
        //        Obra obra = ObraPorNombre(nombreObra);
        //        AgregarEmpleado(empleado, obra);
        //        //El resto de variables las modifican manualmente.
        //    }
        //}








        public void AgregarEmpleado(Empleado empleado, Obra obra)
        {

            if (!EstaEnObra(empleado.Cedula, obra.Nombre))
            {
                //throw new ObraException("El empleado ya está en la obra seleccionada.");
                ObraEmpleado oe = new ObraEmpleado();
                oe.IdEmpleado = empleado.Id;
                oe.IdObra = obra.IdObra;
                oe.FechaIngreso = DateTime.Today; // ver esto luego
                oe.Validar();
                Context.ObrasEmpleados.Add(oe);
                Context.SaveChanges();
            }
        }




        public DateTime SetHoraA0(DateTime fecha)
        {
            return new DateTime(fecha.Year, fecha.Month, fecha.Day, 0, 0, 0);
        }


        public async Task<bool> ConseguirTodasLasMarcas(DateTime desde, DateTime hasta)
        // SI EXISTE SOLO UNA MARCA DEL DIA LA MARCA DE ESE DIA QUEDA DESFAZADA.
        {
            DateTime hastaDato;
            DateTime desdeDato;
            bool anomalias = false;

            if (desde.Year == 0001 || hasta.Year == 0001)
            {
                hastaDato = DateTime.Now;

                desdeDato = new DateTime(hastaDato.Year, hastaDato.Month, hastaDato.Day - hastaDato.Day + 1);

            }
            else
            {
               hastaDato = SetHoraA0(hasta);
               desdeDato = SetHoraA0(desde);
            }
            string response;

            //Cambie el desde y hasta(daato)

            try
            {
                response = LlamadaCloudtimes(desde, hasta).Result; //Formatear la respuesta cloudtimes.
                if (response.Contains("IP"))
                {
                    throw new EmpleadoException("No se pueden generar marcas ahora mismo, intentar en unos minutos.");
                }
            }
            catch (Exception e)
            {
                throw new EmpleadoException("No se pueden generar marcas ahora mismo, intentar en unos minutos.");
            }


            ListadoEmpleadosDTO listado = JsonConvert.DeserializeObject<ListadoEmpleadosDTO>(response);
            foreach (EmpleadoDTO emp in listado.Empleados)
            {
                if (emp.Marcas.Count() == 1)
                {
                    continue; //Si no tiene marcas pasa al siguiente empleado
                }
                Empleado empleado = BuscarPorNombreYCedula(emp.Nombre, emp.Cedula);

                int marcasPorDia = 1;

                for (int i = 0; i < emp.Marcas.Count(); i = i + 1)
                {


                    Obra obra = new Obra();

                    if (emp.Marcas.First().NombreLector != null)
                    {
                        obra = GetObraPorNombre(emp.Marcas.First().NombreLector);

                    }
                    else
                    {
                        bool encontroObra = false;
                        string comentario = emp.Marcas.First().Comentario;


                        foreach (var o in TomarTodasLasObras())
                        {
                            comentario = comentario.ToLower();

                            if (comentario.Contains(o.Nombre.ToLower()))
                            {
                                obra = GetObraPorNombre(o.Nombre);
                                encontroObra = true;
                                break;
                            }
                        }

                        if (!encontroObra)
                        {
                            anomalias = true;
                            continue; // SI EL COMENTARIO NO TIENE LA OBRA NO SE AGREGA ESA MARCA
                        }
                    }

                    Marca m = new Marca();

                    if (i > 0)
                    {
                        if (emp.Marcas[i].HoraMarcaje.Day == emp.Marcas[i - 1].HoraMarcaje.Day)
                        {
                            marcasPorDia++;
                        }
                        else
                        {
                            marcasPorDia = 1;
                        }



                        if (marcasPorDia > 2)
                        {
                            m.Entrada = emp.Marcas[i - marcasPorDia + 1].HoraMarcaje;
                            m.Salida = emp.Marcas[i].HoraMarcaje;


                            m.IdEmpleado = empleado.Id;
                            m.IdObra = obra.IdObra;
                            m.HorasLluvia = 0;


                        }
                        else
                        {

                            m.Entrada = emp.Marcas.ElementAt(i - 1).HoraMarcaje;
                            m.Salida = emp.Marcas.ElementAt(i).HoraMarcaje;


                            m.IdEmpleado = empleado.Id;
                            m.IdObra = obra.IdObra;
                            m.HorasLluvia = 0;


                        }
                    }
                    else
                    {
                        m.Entrada = emp.Marcas.ElementAt(i).HoraMarcaje; //Primera marca que trae la api

                    }
                    if (i != emp.Marcas.Count() - 1) // si no se llego al ultimo dia que entre a agregar en caso que el dia haya cambiado
                    {

                        if (!this.ExisteMarca(m) && emp.Marcas.ElementAt(i).HoraMarcaje.Day != emp.Marcas.ElementAt(i + 1).HoraMarcaje.Day)
                        {
                            Empleado empTest = this.Buscar(m.IdEmpleado);
                            Obra obraTest = this.GetObra(m.IdObra);
                            m.Empleado = this.GetEmpleadoObra(m.IdEmpleado, m.IdObra);
                            if (m.Salida.Year == 0001 || m.Entrada.Year == 0001)
                            {
                                anomalias = true;
                                continue;
                            }
                            if (m.Empleado != null && !this.MarcasEnFecha(m.Empleado.Empleado, m.Entrada))
                            {
                                this.AgregarMarca(m);
                            }
                        }

                    }
                    else
                    {
                        if (!this.ExisteMarca(m))
                        {
                            Empleado empTest = this.Buscar(m.IdEmpleado);
                            Obra obraTest = this.GetObra(m.IdObra);
                            m.Empleado = this.GetEmpleadoObra(m.IdEmpleado, m.IdObra);
                            if (empTest != null && obraTest != null && m.Empleado != null)
                            {
                                this.AgregarMarca(m);
                            }
                            else
                            {
                                anomalias = true;
                                if (m.Salida.Year == 0001)
                                {
                                    m.Salida = new DateTime(m.Entrada.Year, m.Entrada.Month, m.Entrada.Day, m.Entrada.Hour + 1, 0, 0);
                                }
                            }
                        }
                    }

                }

            }
            return anomalias;

        }

        private Obra GetObra(int idObra)
        {
            return Context.Obras.Where(o => o.IdObra == idObra).FirstOrDefault();
        }

        public IEnumerable<Obra> TomarTodasLasObras()
        {
            return Context.Obras.ToList();
        }


        private Obra GetObraPorNombre(string nombreLector)
        {
            return Context.Obras.Where(o => o.Nombre == nombreLector).FirstOrDefault();
        }

        public async void ConseguirMarcasDelEmpleadoDTO(ObraEmpleado empleadoObra)
        {
            DateTime desde = new DateTime(2024, 06, 01);
            DateTime hasta = new DateTime(2024, 07, 01);

            string response = LlamadaCloudtimes(desde, hasta).Result; //Formatear la respuesta cloudtimes.
            ListadoEmpleadosDTO listado = JsonConvert.DeserializeObject<ListadoEmpleadosDTO>(response);
            bool flag = false;
            foreach (EmpleadoDTO emp in listado.Empleados)
            {
                if (emp.Marcas.Count() == 0)
                {
                    continue; //Si no tiene marcas pasa al siguiente empleado
                }
                if (empleadoObra.Empleado.Cedula == emp.Cedula && empleadoObra.Obra.Nombre == emp.Marcas.First().NombreLector)
                {
                    flag = true;
                    for (int i = 0; i < emp.Marcas.Count(); i = i + 2)
                    {
                        Marca m = new Marca();
                        m.Entrada = emp.Marcas.ElementAt(i).HoraMarcaje; //horaMarcaje: Se asume 2 marcas por día. 
                        m.Salida = emp.Marcas.ElementAt(i + 1).HoraMarcaje; //horaMarcaje
                        m.IdEmpleado = empleadoObra.IdEmpleado;
                        m.IdObra = empleadoObra.IdObra;
                        m.HorasLluvia = 0;
                        if (!this.ExisteMarca(m))
                        {
                            this.AgregarMarca(m);
                        }

                    }
                }
            }
        }

        //public async void ConseguirMarcasDelEmpleado(ObraEmpleado empleadoObra)
        //{
        //    DateTime desde = new DateTime(2024, 06, 01);
        //    DateTime hasta = new DateTime(2024, 07, 01);

        //    string response = LlamadaCloudtimes(desde, hasta).Result; //Formatear la respuesta cloudtimes.
        //    var root = JObject.Parse(response);
        //    //int cant = root.response.count();
        //    int cantidadEmpleados = (int)root["cantidadEmpleados"];
        //    bool flag = false;
        //    for (int i = 0; i < cantidadEmpleados && !flag; i++)
        //    {
        //        if (root["empleados"][i]["marcas"][0] == null)
        //        {
        //            continue; // Si no tiene marcas pasa al siguiente empleado
        //        }
        //        string empleadoNom = root["empleados"][i]["nombre"].ToString();
        //        string empleadoCed = root["empleados"][i]["cedula"].ToString();
        //        string nombreObra = root["empleados"][i]["marcas"][0]["nombreLector"].ToString(); //Saco del lector. Nunca será null.
        //        if (empleadoObra.Empleado.Cedula == empleadoCed && empleadoObra.Obra.Nombre == nombreObra)
        //        {
        //            flag = true;
        //            int num = 0;
        //            foreach (var marca in root["empleados"][i]["marcas"])
        //            {
        //                if (num % 2 == 0)
        //                {
        //                    Marca m = new Marca();
        //                    m.Entrada = (DateTime)marca.First; //horaMarcaje: Se asume 2 marcas por día. 
        //                    m.Salida = (DateTime)marca.Next.First; //horaMarcaje
        //                    m.IdEmpleado = empleadoObra.IdEmpleado;
        //                    m.IdObra = empleadoObra.IdObra;
        //                    m.HorasLluvia = 0;
        //                    if (!this.ExisteMarca(m))
        //                    {
        //                        this.AgregarMarca(m);
        //                    }
        //                }
        //                num++;
        //            }
        //        }
        //    }
        //}

        public Dictionary<ObraEmpleado, decimal> Liquidar(DateTime desde, DateTime hasta, Obra? obra, Empleado? empleado, bool inactivos)
        {
            if (obra != null && empleado != null)
            {
                ObraEmpleado oe = this.GetEmpleadoObra(empleado.Id, obra.IdObra);
                return LiquidacionObraEmpleado(oe, desde, hasta, inactivos);
            }
            else if (empleado != null)
            {
                return LiquidacionEmpleado(empleado, desde, hasta, inactivos);
            }
            else if (obra != null)
            {
                return LiquidacionObra(obra, desde, hasta, inactivos);
            }
            else return LiquidacionTotal(desde, hasta, inactivos);

        }

        public Dictionary<ObraEmpleado, decimal> LiquidacionEmpleado(Empleado empleado, DateTime desde, DateTime hasta, bool inactivos)
        {
            if (inactivos || (!inactivos && empleado.Activo))
            {
                decimal liquidacionNominal;
                int horasTotales = 0;
                ObraEmpleado oe = new ObraEmpleado();
                oe.Empleado = empleado;
                Dictionary<ObraEmpleado, decimal> ret = new Dictionary<ObraEmpleado, decimal>();
                //int horasLluvia = 0;
                //int horasExtra = 0;

                List<Marca> marcasEmpRango = this.MarcasEmpleadoRango(empleado, desde, hasta);
                foreach (Marca m in marcasEmpRango)
                {
                    horasTotales += m.HorasTrabajadas(); // Para los bonos(Forma de pago de la mepresa) solo se utilizan horas trabajadas
                                                         //horasLluvia += m.HorasLluvia;
                                                         //horasExtra += m.HorasExtra;
                }
                liquidacionNominal = CalcularNominal(oe.Empleado, horasTotales);
                ret.Add(oe, liquidacionNominal);
                return ret;
            }
            else{
                ObraEmpleado oe = new ObraEmpleado();
                oe.Empleado = empleado;
                Dictionary<ObraEmpleado, decimal> ret2 = new Dictionary<ObraEmpleado, decimal>();
                ret2.Add(oe, 0);
                return ret2;
            }

        }

        private List<Marca> MarcasEmpleadoRango(Empleado empleado, DateTime desde, DateTime hasta)
        {
            return Context.Marcas.Where(marc => marc.Entrada.Day >= desde.Day
                        && marc.Salida.Day <= hasta.Day && marc.Entrada.Month == desde.Month
                        && marc.IdEmpleado == empleado.Id).ToList();
        }

        public Dictionary<ObraEmpleado, decimal> LiquidacionObraEmpleado(ObraEmpleado oe, DateTime desde, DateTime hasta, bool inactivos) //Dos firmas, para el manejo desde controller y desde repo
        {
            if(inactivos || (!inactivos && oe.Empleado.Activo))
            {
                decimal liquidacionNominal;
                Dictionary<ObraEmpleado, decimal> ret = new Dictionary<ObraEmpleado, decimal>();
                int horasTotales = 0;
                //int horasLluvia = 0;
                //int horasExtra = 0;

                List<Marca> marcasEmpRango = this.MarcasEmpObraRango(oe, desde, hasta);
                foreach (Marca m in marcasEmpRango)
                {
                    horasTotales += m.HorasTrabajadas(); // Para los bonos(Forma de pago de la empresa) solo se utilizan horas trabajadas
                                                         //horasLluvia += m.HorasLluvia;
                                                         //horasExtra += m.HorasExtra;
                }
                liquidacionNominal = CalcularNominal(oe.Empleado, horasTotales);
                ret.Add(oe, liquidacionNominal);
                return ret;
            }
            else
            {
                Dictionary<ObraEmpleado, decimal> ret2 = new Dictionary<ObraEmpleado, decimal>();
                ret2.Add(oe, 0);
                return ret2;
            }


        }

        private decimal CalcularNominal(Empleado empleado, int horasTotales)
        {
            decimal nominal;
            // Lo separo en otro metodo por ser importante, preguntar regla de negocio de pagos.
            decimal valorHora = empleado.TipoEmpleado.ValorHora + empleado.IncentivoXHora;
            decimal compensacion = empleado.TipoEmpleado.Compensacion;
            decimal presentismo = empleado.TipoEmpleado.Presentismo;
            if (presentismo != 0)
            {
                presentismo = (valorHora + compensacion) * presentismo / 100;
            }
            nominal = (valorHora + compensacion + presentismo + empleado.IncentivoXHora) * horasTotales;
            return nominal;

        }

        public Dictionary<ObraEmpleado, decimal> LiquidacionObra(Obra obra, DateTime desde, DateTime hasta, bool inactivos)
        {
            List<ObraEmpleado> empleadosObra = this.GetEmpleadosObra(obra); //Repetición de métodos entre repositorios. Que los repos se llamen está mal, pero no sé como organizarlo todavía
            Dictionary<ObraEmpleado, decimal> liqPorEmp = new Dictionary<ObraEmpleado, decimal>();
            foreach (ObraEmpleado oe in empleadosObra)
            {
                var liqEmpleado = this.LiquidacionObraEmpleado(oe, desde, hasta, inactivos);
                foreach (var item in liqEmpleado)
                {
                    liqPorEmp[item.Key] = item.Value;
                }
            }
            return liqPorEmp;
        }

        public Dictionary<ObraEmpleado, decimal> LiquidacionTotal(DateTime desde, DateTime hasta, bool inactivos)
        {
            Dictionary<ObraEmpleado, decimal> liqPorObras = new Dictionary<ObraEmpleado, decimal>();
            List<Obra> obras = this.GetObras(); //Repetición de métodos entre repositorios. Que los repos se llamen está mal, pero no sé como organizarlo todavía

            foreach (Obra o in obras)
            {
                var liqObra = this.LiquidacionObra(o, desde, hasta, inactivos);
                foreach (var item in liqObra)
                {
                    liqPorObras[item.Key] = item.Value;
                }
            }
            return liqPorObras;
        }

        private List<Obra> GetObras()
        {
            return Context.Obras.ToList();
        }

        private List<ObraEmpleado> GetEmpleadosObra(Obra obra)
        {
            return Context.ObrasEmpleados.Where(oe => oe.IdObra == obra.IdObra).Include(oe => oe.Empleado).Include(oe => oe.Empleado.TipoEmpleado).Include(oe => oe.Obra).ToList();
        }

        private List<Marca> MarcasEmpObraRango(ObraEmpleado oe, DateTime desde, DateTime hasta)
        {
            return Context.Marcas.Where(marc => marc.Entrada.Day >= desde.Day
            && marc.Salida.Day <= hasta.Day && marc.IdObra == oe.IdObra && marc.Entrada.Month == desde.Month
            && marc.IdEmpleado == oe.IdEmpleado).ToList();
        }

        private bool ExisteMarca(Marca m)
        {
            return Context.Marcas.Where(mar => mar.Entrada == m.Entrada
            && mar.Salida == m.Salida && mar.IdObra == m.IdObra
            && mar.IdEmpleado == m.IdEmpleado).Any();
        }

        private void AgregarMarca(Marca m)
        {
            try
            {
                m.Validar();
                Context.Marcas.Add(m);
                Context.SaveChanges();
            }
            catch (EmpleadoException ee)
            {
                throw new EmpleadoException(ee.Message);
            }
        }

        private bool EstaEnObra(string empleadoCed, string nombreObra)
        {
            bool esta = false;
            if (Context.ObrasEmpleados.Where(o => o.Obra.Nombre == nombreObra && o.Empleado.Cedula == empleadoCed).FirstOrDefault() != null)
            {
                esta = true;
            }
            return esta;
        }

        private Obra ObraPorNombre(string nombreObra)
        {
            return Context.Obras.Where(o => o.Nombre == nombreObra).FirstOrDefault();

        }

        private Empleado BuscarPorNombreYCedula(string empleadoNom, string empleadoCed)
        {
            return Context.Empleados.Where(e => (e.Nombre == empleadoNom && e.Cedula == empleadoCed) || (e.Cedula == empleadoCed)).FirstOrDefault();
        }

        public void AgregarTipo(TipoEmpleado tipo)
        {
            try
            {
                Context.TiposEmpleados.Add(tipo);
                Context.SaveChanges();
            }
            catch (EmpleadoException ee)
            {
                throw new EmpleadoException(ee.Message);
            }
        }

        public void ModificarTipo(TipoEmpleado tipo)
        {
            TipoEmpleado tipoEmpleado = this.BuscarTipo(tipo.Id);
            if (tipoEmpleado == null)
            {
                throw new EmpleadoException("No se encontró el tipo de empleado para modificar.");
            }
            tipo.Validar();
            //Context.TiposEmpleados.Update(tipo);
            tipoEmpleado.Categoría = tipo.Categoría;
            tipoEmpleado.ValorHora = tipo.ValorHora;
            tipoEmpleado.Compensacion = tipo.Compensacion;
            tipoEmpleado.Presentismo = tipo.Presentismo;
            Context.SaveChanges();
        }

        internal IEnumerable<TipoEmpleado> BuscarTipos()
        {
            return Context.TiposEmpleados;
        }

        internal TipoEmpleado BuscarTipo(int id)
        {
            return Context.TiposEmpleados.Find(id);
        }

        internal List<Empleado> TomarEmpleadosDeObra(Obra obra)
        {
            List<ObraEmpleado> oEmps = this.GetEmpleadosObra(obra);
            List<Empleado> emps = new List<Empleado>();
            foreach (ObraEmpleado oe in oEmps)
            {
                emps.Add(oe.Empleado);
            }
            return emps;
        }

        internal List<Marca> MarcasDelEmpleadoEnLaObra(List<Marca> marcas, Obra obra)
        {
            return marcas.Where(mar => mar.IdObra == obra.IdObra).ToList();
        }

        internal List<Marca> MarcasDelRangoDeFecha(List<Marca> marcas, DateTime desde, DateTime hasta)
        {
            return marcas.Where(mar => mar.Entrada.Date >= desde && mar.Entrada.Date <= hasta).ToList();
        }

        internal List<Marca> TraerTodasMarcas(Empleado empleado)
        {
            return Context.Marcas.Where(mar => mar.IdEmpleado == empleado.Id).ToList();
        }

        internal ObraEmpleado GetEmpleadoObra(int idEmpleado, int idObra)
        {
            return Context.ObrasEmpleados.Where(oe => oe.IdObra == idObra && oe.IdEmpleado == idEmpleado).Include(oe => oe.Empleado).Include(oe => oe.Obra).FirstOrDefault();
        }

        internal void AgregarTodasLasMarcasPorIdEmpleado(int idEmpleado)
        {

            Fachada f = new Fachada();

            Empleado empl = f.BuscarEmpleado(idEmpleado);
            DateTime desde = new DateTime(2024, 06, 01);
            DateTime hasta = new DateTime(2024, 06, 30);


            string response = LlamadaCloudtimes(desde, hasta).Result; //Formatear la respuesta cloudtimes.
            ListadoEmpleadosDTO listado = JsonConvert.DeserializeObject<ListadoEmpleadosDTO>(response);
            bool flag = false;
            foreach (EmpleadoDTO empDTO in listado.Empleados)
            {
                if (empDTO.Marcas.Count() == 0)
                {
                    continue; //Si no tiene marcas pasa al siguiente empleado
                }

                Obra obra = f.BuscarObraPorNombre(empDTO.Marcas.First().NombreLector);

                foreach (Empleado emp in Context.Empleados)
                {






                    if (emp.Cedula == empDTO.Cedula)
                    {
                        flag = true;
                        for (int i = 0; i < empDTO.Marcas.Count(); i = i + 2)
                        {
                            Marca m = new Marca();
                            m.Entrada = empDTO.Marcas.ElementAt(i).HoraMarcaje; //horaMarcaje: Se asume 2 marcas por día. 
                            m.Salida = empDTO.Marcas.ElementAt(i + 1).HoraMarcaje; //horaMarcaje
                            m.IdEmpleado = emp.Id;
                            m.IdObra = obra.IdObra;
                            m.HorasLluvia = 0;
                            if (!this.ExisteMarca(m))
                            {
                                this.AgregarMarca(m);
                            }

                        }
                    }
                }





            }







            ///////////////////////////////////////////////////////////////////////////////

            //Fachada f = new Fachada();

            //Empleado emp = f.BuscarEmpleado(idEmpleado);
            //DateTime desde = new DateTime(2024, 06, 01);
            //DateTime hasta = new DateTime(2024, 07, 01);

            //string response = LlamadaCloudtimes(desde, hasta).Result; //Formatear la respuesta cloudtimes.
            //var root = JObject.Parse(response);
            ////int cant = root.response.count();
            //int cantidadEmpleados = (int)root["cantidadEmpleados"];
            //bool flag = false;
            //for (int i = 0; i < cantidadEmpleados && !flag; i++)
            //{
            //    if (root["empleados"][i]["marcas"][0] == null)
            //    {
            //        continue; // Si no tiene marcas pasa al siguiente empleado
            //    }
            //    string empleadoNom = root["empleados"][i]["nombre"].ToString();
            //    string empleadoCed = root["empleados"][i]["cedula"].ToString();
            //    if(root["empleados"][i]["marcas"][0]["nombreLector"] == null) { continue; }
            //    string nombreObra = root["empleados"][i]["marcas"][0]["nombreLector"].ToString(); //Saco del lector. Nunca será null.

            //    Obra obra = f.BuscarObraPorNombre(nombreObra);
            //    if (emp.Cedula == empleadoCed)
            //    {
            //        flag = true;
            //        int num = 0;
            //        foreach (var marca in root["empleados"][i]["marcas"])
            //        {
            //            if (num % 2 == 0)
            //            {
            //                Marca m = new Marca();
            //                m.Entrada = (DateTime)marca.First; //horaMarcaje: Se asume 2 marcas por día.
            //                if(marca.Next == null)
            //                {
            //                    m.Salida = (DateTime)marca.First;
            //                }
            //                else
            //                {
            //                m.Salida = (DateTime)marca.Next.First; //horaMarcaje

            //                }
            //                m.IdEmpleado = emp.Id;
            //                m.IdObra = obra.IdObra;
            //                m.HorasLluvia = 0;
            //                if (!this.ExisteMarca(m))
            //                {
            //                    this.AgregarMarca(m);
            //                }
            //            }
            //            num++;
            //        }
            //    }
            //}


        }

        internal int HorasTotales(List<Marca> marcas)
        {
            int total = 0;
            foreach (Marca m in marcas)
            {
                total += m.HorasTrabajadas();
            }
            return total;
        }

        internal void ModificarObraEmpleado(ObraEmpleado obraEmpleadoNuevo)
        {
            try
            {
                ObraEmpleado obraEmpleado = this.GetEmpleadoObra(obraEmpleadoNuevo.IdEmpleado, obraEmpleadoNuevo.IdObra);
                if (obraEmpleado == null)
                {
                    throw new EmpleadoException("No se encontró el empleado en la obra para modificar.");
                }
                obraEmpleadoNuevo.Validar(); //Seria item validar no empleado validar porque valida los datos viejos sino

                obraEmpleado.FechaIngreso = obraEmpleadoNuevo.FechaIngreso;
                obraEmpleado.FechaEgreso = obraEmpleadoNuevo.FechaEgreso;
                Context.SaveChanges();
            }
            catch (EmpleadoException ee)
            {
                throw new EmpleadoException(ee.Message);
            }

        }
    }
}

