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
using System.Globalization;
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
                item.Validar();
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
            empleado.Validar();
            Context.Empleados.Update(empleado);
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

        public void AgregarEmpleadosAObra()
        {
            DateTime desde = new DateTime(2024, 04, 01);
            DateTime hasta = new DateTime(2024, 04, 02);
            string response = LlamadaCloudtimes(desde, hasta).Result; //Formatear la respuesta cloudtimes.
            var root = JObject.Parse(response);
            int cantidadEmpleados = (int)root["cantidadEmpleados"];

            for (int i = 0; i < cantidadEmpleados; i++)
            {
                if (root["empleados"][i]["marcas"].Count() == 0 )
                {
                    continue; // Si no tiene marcas pasa al siguiente empleado
                }
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
                Obra obra = ObraPorNombre(nombreObra);
                AgregarEmpleado(empleado, obra);
                //El resto de variables las modifican manualmente.
            }
        }
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

        public async void ConseguirMarcasDelEmpleado(ObraEmpleado empleadoObra)
        {
            DateTime desde = new DateTime(2024, 04, 01);
            DateTime hasta = new DateTime(2024, 04, 02);

            string response = LlamadaCloudtimes(desde, hasta).Result; //Formatear la respuesta cloudtimes.
            var root = JObject.Parse(response);
            //int cant = root.response.count();
            int cantidadEmpleados = (int)root["cantidadEmpleados"];
            bool flag = false;
            for (int i = 0; i < cantidadEmpleados && !flag; i++)
            {
                if (root["empleados"][i]["marcas"][0] == null)
                {
                    continue; // Si no tiene marcas pasa al siguiente empleado
                }
                string empleadoNom = root["empleados"][i]["nombre"].ToString();
                string empleadoCed = root["empleados"][i]["cedula"].ToString();
                string nombreObra = root["empleados"][i]["marcas"][0]["nombreLector"].ToString(); //Saco del lector. Nunca será null.
                if (empleadoObra.Empleado.Cedula == empleadoCed && empleadoObra.Obra.Nombre == nombreObra)
                {
                    flag = true;
                    int num = 0;
                    foreach (var marca in root["empleados"][i]["marcas"])
                    {
                        if (num % 2 == 0)
                        {
                            Marca m = new Marca();
                            m.Entrada = (DateTime)marca.First; //horaMarcaje: Se asume 2 marcas por día. 
                            m.Salida = (DateTime)marca.Next.First; //horaMarcaje
                            m.IdEmpleado = empleadoObra.IdEmpleado;
                            m.IdObra = empleadoObra.IdObra;
                            m.HorasLluvia = 0;
                            if (!this.ExisteMarca(m))
                            {
                                this.AgregarMarca(m);
                            }
                        }
                        num++;
                    }
                }
            }
        }

        public double Liquidar(DateTime desde, DateTime hasta, Obra? obra, Empleado? empleado)
        {
            double liquidacionNominal;
            if (obra != null && empleado !=null)
            {
                ObraEmpleado oe = this.GetEmpleadoObra(empleado.Id, obra.IdObra);
                liquidacionNominal = LiquidacionObraEmpleado(oe, desde, hasta);
                return liquidacionNominal;
            }

            if (empleado != null)
            {
                liquidacionNominal = LiquidacionEmpleado(empleado,desde,hasta);
                return liquidacionNominal;
            }
            if (obra != null)
            {
                liquidacionNominal = LiquidacionObra(obra,desde,hasta);
                return liquidacionNominal;
            }
             return LiquidacionTotal(desde,hasta);

        }

        private double LiquidacionEmpleado(Empleado empleado, DateTime desde, DateTime hasta)
        {
            double liquidacionNominal;
            int horasTotales = 0;
            //int horasLluvia = 0;
            //int horasExtra = 0;

            List<Marca> marcasEmpRango = this.MarcasEmpleadoRango(empleado, desde, hasta);
            foreach (Marca m in marcasEmpRango)
            {
                horasTotales += m.HorasTrabajadas(); // Para los bonos(Forma de pago de la mepresa) solo se utilizan horas trabajadas
                //horasLluvia += m.HorasLluvia;
                //horasExtra += m.HorasExtra;
            }
            liquidacionNominal = CalcularNominal(empleado, horasTotales);
            return liquidacionNominal;
        }

        private List<Marca> MarcasEmpleadoRango(Empleado empleado, DateTime desde, DateTime hasta)
        {
            return Context.Marcas.Where(marc => marc.Entrada.Day == desde.Day
                        && marc.Salida.Day == hasta.Day 
                        && marc.IdEmpleado == empleado.IdEmpleado).ToList();
        }

        public double LiquidacionObraEmpleado(ObraEmpleado oe, DateTime desde, DateTime hasta)
        {
            double liquidacionNominal;
            int horasTotales = 0;
            //int horasLluvia = 0;
            //int horasExtra = 0;

            List<Marca> marcasEmpRango = this.MarcasEmpObraRango(oe, desde, hasta);
            foreach (Marca m in marcasEmpRango)
            {
                horasTotales += m.HorasTrabajadas(); // Para los bonos(Forma de pago de la mepresa) solo se utilizan horas trabajadas
                //horasLluvia += m.HorasLluvia;
                //horasExtra += m.HorasExtra;
            }
            liquidacionNominal = CalcularNominal(oe.Empleado, horasTotales);
            return liquidacionNominal;
        }

        private double CalcularNominal(Empleado empleado, int horasTotales)
        {
            // Lo separo en otro metodo por ser importante, preguntar regla de negocio de pagos.
            double valorHora = empleado.TipoEmpleado.ValorHora;
            double compensacion = empleado.TipoEmpleado.Compensacion;
            double presentismo = empleado.TipoEmpleado.Presentismo;
            double nominal = horasTotales*valorHora;
            return nominal;

        }

        public double LiquidacionObra(Obra obra, DateTime desde, DateTime hasta)
        {
            double liquidacionNominal = 0;
            List<ObraEmpleado> empleadosObra = this.GetEmpleadosObra(obra); //Repetición de métodos entre repositorios. Que los repos se llamen está mal, pero no sé como organizarlo todavía

            foreach (ObraEmpleado oe in empleadosObra)
            {
                liquidacionNominal += this.LiquidacionObraEmpleado(oe, desde, hasta);
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

        private List<Marca> MarcasEmpObraRango(ObraEmpleado oe, DateTime desde, DateTime hasta)
        {
            return Context.Marcas.Where(marc => marc.Entrada.Day >= desde.Day
            && marc.Salida.Day <= hasta.Day && marc.IdObra == oe.IdObra
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

        internal ObraEmpleado GetEmpleadoObra(int idEmpleado, int idObra)
        {
            return Context.ObrasEmpleados.Where(oe => oe.IdObra == idObra && oe.IdEmpleado == idEmpleado).Include(oe => oe.Empleado).Include(oe => oe.Obra).FirstOrDefault();
        }
    }
}

