using LogicaAccesoDatos.EF;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using LogicaNegocio.ViewModel;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PdfSharp.Pdf.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
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
                Context.Empleados.Add(item);
                Context.SaveChanges();
            }
            catch (EmpleadoException ee)
            {
                throw new EmpleadoException(ee.Message);
            }
        }

        public Empleado Buscar(int id)
        {
            return Context.Empleados.Where(e => e.IdEmpleado == id).Include(e => e.TipoEmpleado).FirstOrDefault();
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
                throw new EmpleadoException("No se encontró el material para modificar.");
            }

            //empleado.Nombre = m.Nombre;
            //empleado.UnidadDeMedida = m.UnidadDeMedida;
            empleado.Validar();
            Context.Empleados.Update(empleado);
            Context.SaveChanges();
        }

        public IEnumerable<Empleado> TomarTodos()
        {
            return Context.Empleados.Include(e => e.TipoEmpleado).ToList();
        }

        public async Task<string> Liquidar()
        {
            //HttpClient cliente = new HttpClient();
            using (HttpClient cliente = new HttpClient())
            {

            Uri uri = new Uri("https://apicloudtimes.uy/apiclientes/v1/obtenerMarcas?inicio=01/04/2024&fin=02/04/2024");
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
            solicitud.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbXByZXNhIjoiQm9kZWdhIFBpZWRyYWZpdGEifQ.TkaIdrD6xet2u9TbuoqV9BPxlEQ9AakyYNn5P27_fJw");

            Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
            respuesta.Wait();
            Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
            //RespuestaApiModel responseObject = JsonConvert.DeserializeObject<ResponseModel>(responseContent);

                //string responseContent = await respuesta.Content.ReadAsStringAsync();
                //RespuestaApiModel[] respuesta = JsonConvert.DeserializeObject < RespuestaApiModel[]>
            return await response;

            }

            //using (var client = new HttpClient())
            //{
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbXByZXNhIjoiQm9kZWdhIFBpZWRyYWZpdGEifQ.TkaIdrD6xet2u9TbuoqV9BPxlEQ9AakyYNn5P27_fJw");

            //var apiUrl = "https://apicloudtimes.uy/apiclientes/v1/obtenerMarcas?inicio=01/04/2024&fin=30/04/2024"; // Reemplaza con tu URL de la API
            //var response = await client.GetAsync(apiUrl);
            //response.EnsureSuccessStatusCode();

            //return await response.Content.ReadAsStringAsync();
            //}
        }

        public async void AgregarEmpleadosAObra()
        {
            string response = Liquidar().Result; //Formatear la respuesta cloudtimes.
            var root = JObject.Parse(response);
            int cantidadEmpleados = (int)root["cantidadEmpleados"];
            
            for(int i = 0; i < cantidadEmpleados; i++)
            {
                string empleadoNom = root["empleados"][i]["nombre"].ToString();
                string empleadoCed = root["empleados"][i]["cedula"].ToString();
                string nombreObra = root["empleados"][i]["marcas"][0]["nombreLector"].ToString(); //Saco del lector. Nunca será null.
                Empleado empleado = this.BuscarPorNombreYCedula(empleadoNom, empleadoCed);
                if (empleado == null) //Si no fue añadido ya añadilo.
                {
                    empleado = new Empleado();
                    empleado.Nombre = empleadoNom;
                    empleado.Cedula = empleadoCed;
                    this.Agregar(empleado);
                }
                ObraEmpleado oe = new ObraEmpleado();
                oe.IdEmpleado = empleado.Id;
                oe.IdObra = this.ObraPorNombre(nombreObra).IdObra; //Se asume que las obras ya están ingresadas.
                this.AgregarEmpleado(oe);
                
                //El resto de variables las modifican manualmente.
            }
        }
        public void AgregarEmpleado(ObraEmpleado oe)
        {
            oe.Validar();
            if (this.BuscarPorCedulaYObra(oe.Empleado.Cedula, oe.Obra.Nombre) != null)
            {
                throw new ObraException("El empleado ya está en la obra seleccionada.");
            }
            Context.ObrasEmpleados.Add(oe);
            Context.SaveChanges();
        }

        public async void ConseguirMarcasDelEmpleado(ObraEmpleado empleadoObra)
        {
            string response = Liquidar().Result; //Formatear la respuesta cloudtimes.
            var root = JObject.Parse(response);
            int cantidadEmpleados = (int)root["cantidadEmpleados"];
            bool flag = false;
            for (int i = 0; i < cantidadEmpleados && !flag; i++)
            {
                string empleadoNom = root["empleados"][i]["nombre"].ToString();
                string empleadoCed = root["empleados"][i]["cedula"].ToString();
                string nombreObra = root["empleados"][i]["marcas"][0]["nombreLector"].ToString(); //Saco del lector. Nunca será null.
                if(empleadoObra.Empleado.Cedula == empleadoCed && empleadoObra.Obra.Nombre == nombreObra)
                {
                    flag = true;
                    int incremento = 0;
                    var marca = root["empleados"][i]["marcas"][incremento];
                    while (marca != null)
                    {
                        Marca m = new Marca();
                        m.Entrada = (DateTime)marca.First; //horaMarcaje: Se asume 4 marcas por día, o 2. 
                        m.Salida = (DateTime)marca.First.Next; //horaMarcaje
                        m.IdEmpleado = this.BuscarPorNombreYCedula(empleadoNom, empleadoCed).Id;
                        m.IdObra = this.ObraPorNombre(nombreObra).IdObra;
                        m.HorasLluvia = 0;
                        if (!this.ExisteMarca(m))
                        {
                            this.AgregarMarca(m);
                        }
                        
                        incremento += 2;
                    }
                }

                //El resto de variables las modifican manualmente.
            }
        }

        public double LiquidacionEmpleado(ObraEmpleado oe, DateTime desde, DateTime hasta)
        {
            double liquidacionNominal = 0;
            int horasTotales = 0;
            int horasLluvia = 0;
            int horasExtra = 0;

            List<Marca> marcasEmpRango = this.MarcasEmpRango(oe, desde, hasta);
            foreach(Marca m in marcasEmpRango)
            {
                horasTotales += m.HorasTrabajadas() - m.HorasLluvia;
                horasLluvia += m.HorasLluvia;
                horasExtra += m.HorasExtra;
            }
            liquidacionNominal = ((horasTotales) + (horasLluvia * 2) + (horasExtra * 4)) 
                * ((oe.Empleado.TipoEmpleado.ValorHora + oe.Empleado.TipoEmpleado.Compensacion) * oe.Empleado.TipoEmpleado.Presentismo);

            return liquidacionNominal;
        }

        public double LiquidacionObra(Obra obra, DateTime desde, DateTime hasta)
        {
            double liquidacionNominal = 0;
            List<ObraEmpleado> empleadosObra = this.GetEmpleadosObra(obra); //Repetición de métodos entre repositorios. Que los repos se llamen está mal, pero no sé como organizarlo todavía
            
            foreach(ObraEmpleado oe in empleadosObra)
            {
                liquidacionNominal += this.LiquidacionEmpleado(oe, desde, hasta);
            }
            return liquidacionNominal;
        }

        public double LiquidacionTotal(DateTime desde, DateTime hasta)
        {
            double liquidacionNominal = 0;
            List<Obra> obras = this.GetObras(); //Repetición de métodos entre repositorios. Que los repos se llamen está mal, pero no sé como organizarlo todavía

            foreach (Obra o in obras)
            {
                liquidacionNominal += this.LiquidacionObra(o, desde, hasta);
            }
            return liquidacionNominal;
        }

        private List<Obra> GetObras()
        {
            return Context.Obras.ToList();   
        }

        private List<ObraEmpleado> GetEmpleadosObra(Obra obra)
        {
            return Context.ObrasEmpleados.Where(oe => oe.IdObra == obra.IdObra).Include(oe => oe.Empleado).Include(oe => oe.Empleado.TipoEmpleado).Include(oe => oe.Obra).ToList();
        }

        private List<Marca> MarcasEmpRango(ObraEmpleado oe, DateTime desde, DateTime hasta)
        {
            return Context.Marcas.Where(marc => marc.Entrada.Day == desde.Day 
            && marc.Salida.Day == hasta.Day && marc.IdObra == oe.IdObra 
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

        private ObraEmpleado BuscarPorCedulaYObra(string empleadoCed, string nombreObra)
        {
            var Retorno = Context.ObrasEmpleados.Where(o => o.Obra.Nombre == nombreObra && o.Empleado.Cedula == empleadoCed).FirstOrDefault();
            return Retorno;
        }

        private Obra ObraPorNombre(string nombreObra)
        {
            var Retorno = Context.Obras.Where(o => o.Nombre == nombreObra).FirstOrDefault();
            return Retorno;
        }

        private Empleado BuscarPorNombreYCedula(string empleadoNom, string empleadoCed)
        {
            return Context.Empleados.Where(e => e.Nombre == empleadoNom && e.Cedula == empleadoCed).FirstOrDefault();
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

        internal IEnumerable<TipoEmpleado> BuscarTipos()
        {
            return Context.TiposEmpleados;
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
            return marcas.Where(mar => mar.Entrada.Date.Day >= desde.Day && mar.Entrada.Date.Day <= hasta.Day).ToList();
        }

        internal List<Marca> TraerTodasMarcas(Empleado empleado)
        {
            return Context.Marcas.Where(mar => mar.IdEmpleado == empleado.Id).ToList();
        }


        //  CabaniaModel[] cabanias = JsonConvert.DeserializeObject<CabaniaModel[]>(response.Result);

    }
}

