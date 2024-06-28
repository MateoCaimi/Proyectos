using LogicaAccesoDatos.EF;
using LogicaNegocio.Entidades;
using LogicaNegocio.Excepciones;
using LogicaNegocio.Interfaces;
using PdfSharp.Pdf.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;


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
            return Context.Empleados.Find(id);
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
            return Context.Empleados.ToList();
        }

        public async Task<string> Liquidar()
        {
            //HttpClient cliente = new HttpClient();
            using (HttpClient cliente = new HttpClient())
            {

            Uri uri = new Uri("https://apicloudtimes.uy/apiclientes/v1/obtenerMarcas?inicio=01/04/2024&fin=30/04/2024");
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


      //  CabaniaModel[] cabanias = JsonConvert.DeserializeObject<CabaniaModel[]>(response.Result);
               
    }
}

