using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web = System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static ForgeAppTesting.Controllers.DataManagementController;
using System.IO;
using ForgeAppTesting.Models;
using Newtonsoft.Json;
using System.Dynamic;
using ForgeAppTesting.Services;
using System.Security.Cryptography.X509Certificates;

namespace ForgeAppTesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Bim5dController : ControllerBase
    {
        private readonly PartidaCertificacionService _pcService;

        public Bim5dController(PartidaCertificacionService pcService)
        {
            _pcService = pcService;
        }
        private static List<Concepto> Data { get; set; }
        private static MedicionesJson MedicionesJson { get; set; }

        [HttpGet("tree")]
        public async Task<IList<jsTreeNode>> GetTreeNodeAsync([web.FromUri]string id, [web.FromUri]string guid, [web.FromUri]string urn)
        {
            var matriz = Base64Decode(urn).Split('.');
            var fichero = matriz[matriz.Length - 1].Split('?')[0];

            IList<jsTreeNode> nodes = new List<jsTreeNode>();
            await GetJsonData(fichero);
            await GetCsvData(guid);

            if (id == "#")
            {
                return await GetCapitulosPrincipales();
            }
            else
            {
                string[] idParams = id.Split('/');
                string resource = idParams[idParams.Length - 1];
                switch (resource)
                {
                    case "capitulo":
                        return await GetCapitulos(id);
                }
            }

            return nodes;
        }

        [HttpGet("partidas")]
        public async Task<dynamic> GetPartidaAsync([web.FromUri]string id)
        {
            foreach (var medicionToWrite in MedicionesJson.MedicionesToWrite)
            {
                if (medicionToWrite.Hijo.Codigo == id)
                {
                    return medicionToWrite.ModelMediciones[0].ModelMediciones;
                }
            }

            return null;
        }

        [HttpGet("partidas/info")]
        public async Task<dynamic> GetPartidaInfoAsync([web.FromUri] string id)
        {
            dynamic data = new ExpandoObject();
            var precio = Data
                .FirstOrDefault(x => x.Tipo.ToLower() == "partida" && x.Codigo == id)
                .Precio;
            data.precio = Math.Round(precio, 2).ToString() + " €";
            foreach (var medicionToWrite in MedicionesJson.MedicionesToWrite)
            {
                if (medicionToWrite.Hijo.Codigo == id)
                {
                    var cantidad = 0.00;
                    var strCantidad = Convert.ToString(medicionToWrite.CantidadTotal);
                    double.TryParse(strCantidad, out cantidad);
                    data.unidad = medicionToWrite.Hijo.Unidad;
                    data.cantidad = Math.Round(cantidad, 2);
                    data.importe = Math.Round(cantidad * precio, 2).ToString() + " €";
                    return data;
                }
            }

            return data;
        }

        [HttpPost("partidas/cantidad")]
        public async Task<dynamic> GetCantidadesFromPartida([web.FromBody]CantidadBody data)
        {
            //var appData = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "App_Data");
            //var fichero = Path.Combine(appData, "certificacion.json");
            //var json = ReadFile(fichero);
            //CertificarPartidas = JsonConvert.DeserializeObject<List<CertificarPartida>>(json);

            var lista = new List<ExpandoObject>();

            var grupos = data.Values
                .GroupBy(x => x.Value);
            foreach (var grupo in grupos)
            {
                var cantidad = Math.Round(grupo.Sum(x => GetCantidadFromPartida(data.Partida, x.ExternalId)), 2);
                var avance = Math.Round(grupo.Sum(x => GetAvanceFromPartida(data.Partida, x.ExternalId)), 2);
                dynamic item = new ExpandoObject();
                item.grupo = grupo.Key;
                item.cantidad = cantidad;
                item.avance = avance;
                lista.Add(item);
            }
            lista = lista.OrderByDescending(x => ((dynamic)x).cantidad).ToList();

            return lista;
        }

        [HttpPost("partidas/certificar")]
        public async Task<dynamic> CertificarPartidaAsync([web.FromBody]CertificarBody data)
        {
            var cantidad = 0.00;

            var pc = _pcService.GetByHijo(data.HijoId);
            if (pc == null)
            {
                pc = _pcService.Create(new PartidaCertificacion(data.HijoId, data.ExternalIds));
            }
            else
            {
                var ids = _pcService.AddCertificaciones(data.HijoId, data.ExternalIds);
                cantidad = ids.Sum(x => GetCantidadFromPartida(data.HijoId, x));
            }


            return cantidad;
        }

        public static double GetCantidadFromPartida(string id, string externalId)
        {
            var modelMedicion = MedicionesJson
                .MedicionesToWrite
                .FirstOrDefault(x => x.Hijo.Codigo == id);
            if (modelMedicion != null)
            {
                var cantidad = modelMedicion.ModelMediciones
                    .SelectMany(x => x.ModelMediciones)
                    .Where(x => x.ExternalId == externalId)
                    .Sum(x => x.Cantidad);

                return cantidad;
            }
            else
            {
                return 0.00;
            }
        }
        private double GetAvanceFromPartida(string partida, string externalId)
        {
            var pc = _pcService.GetByHijo(partida);
            if (pc != null)
            {
                var _externalId = pc.Certificaciones.FirstOrDefault(x => x == externalId);
                if (_externalId != null)
                {
                    return GetCantidadFromPartida(partida, externalId) * 1; // TODO: Porcentaje certificacion.
                }
            }

            return 0.00;
        }

        private async Task<IList<jsTreeNode>> GetCapitulos(string href)
        {
            var idParams = href.Split('/');
            var codigo = idParams[idParams.Length - 2];
            var conceptos = Data.Where(x => x.Padre == codigo);
            var nodes = new List<jsTreeNode>();
            foreach (var concepto in conceptos)
            {
                if (concepto.Tipo.ToLower() == "capitulo")
                {
                    var id = $"{concepto.Codigo}/capitulo";
                    nodes.Add(new jsTreeNode(id, $"{concepto.Codigo} - {concepto.Nombre}", "capitulo", true));
                }
                else
                {
                    var id = $"{concepto.Codigo}/partida";
                    nodes.Add(new jsTreeNode(id, $"{concepto.Codigo} - {concepto.Unidad}|{concepto.Nombre}", "partida", false));
                }
            }

            return nodes;
        }

        private async Task<IList<jsTreeNode>> GetCapitulosPrincipales()
        {
            var capitulosPrincipales = Data.Where(x => x.Tipo.ToLower() == "capitulo" && x.Padre == "");

            var nodes = new List<jsTreeNode>();
            foreach (var capitulo in capitulosPrincipales)
            {
                var id = $"{capitulo.Codigo}/capitulo";
                nodes.Add(new jsTreeNode(id, $"{capitulo.Codigo} - {capitulo.Nombre}", "capitulo", true));
            }

            return nodes;
        }

        private async Task GetCsvData(string guid)
        {
            var appData = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
            await Task.Run(() =>
            {
                var lista = new List<Concepto>();
                var fichero = Path.Combine(appData, guid + ".tsv");
                var lineas = System.IO.File.ReadAllLines(fichero);
                for (int i = 1; i < lineas.Length; i++)
                {
                    var linea = lineas[i];
                    lista.Add(new Concepto(linea));

                }

                    Data = lista;
            });
        }
        private async Task GetJsonData(string guid)
        {
            var appData = Path.Combine(Directory.GetCurrentDirectory(), "App_Data");
            await Task.Run(() =>
            {
                string fichero = Path.Combine(appData, guid + ".json");
                using (var reader = new StreamReader(fichero))
                {
                    var json = reader.ReadToEnd();
                    MedicionesJson = JsonConvert.DeserializeObject<MedicionesJson>(json);
                }
            });
        }

        private string Base64Decode(string plainText)
        {
            var test = plainText.Replace('-', '+').Replace('_', '/').PadRight(4 * ((plainText.Length + 3) / 4), '=');
            var plainTextBytes = System.Convert.FromBase64String(test);
            return System.Text.Encoding.UTF8.GetString(plainTextBytes);
        }
    }

    public class Concepto
    {
        public string Tipo { get; set; }
        public string Codigo { get; set; }
        public string Unidad { get; set; }
        public string Nombre { get; set; }
        public string Padre { get; set; }
        public double Precio { get; set; }

        public Concepto(string linea)
        {
            var lista = new List<string>();
            var celdas = linea.Split('	');

            this.Tipo = celdas[0];
            this.Codigo = celdas[1];
            this.Unidad = celdas[2];
            this.Nombre = celdas[3];
            this.Padre = celdas[4];
            var precio = 0.00;
            double.TryParse(celdas[8], out precio);
            this.Precio = precio;
        }
    }

    public class CertificarBody
    {
        public string PadreId { get; set; }
        public string HijoId { get; set; }
        public List<string> ExternalIds { get; set; }
        public CertificarBody(string padreId, string hijoId, List<string> externalIds)
        {
            this.PadreId = padreId;
            this.HijoId = hijoId;
            this.ExternalIds = externalIds;
        }
    }

    public class CantidadBody
    {
        public string Partida { get; set; }
        public List<Valor> Values { get; set; }
        public CantidadBody(string partida, List<Valor> values)
        {
            this.Partida = partida;
            this.Values = values;
        }
    }

    public class Valor
    {
        public int DbId { get; set; }
        public string ExternalId { get; set; }
        public string Value { get; set; }
        public Valor(int dbId, string externalId, string value)
        {
            this.DbId = dbId;
            this.ExternalId = externalId;
            this.Value = value;
        }
    }
}