using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeAppTesting.Models.Bc3
{
    public class ModelMedicion
    {
        public string ExternalId { get; set; }
        public Concepto Padre { get; set; }
        public Concepto Hijo { get; set; }
        public double Cantidad { get; set; }
        public string Comentario { get; set; }
        public ModelMedicion(string externalId, Concepto padre, Concepto hijo, double cantidad, string comentario)
        {
            this.ExternalId = externalId;
            this.Padre = padre;
            this.Hijo = hijo;
            this.Cantidad = cantidad;
            this.Comentario = comentario;
        }
    }
}
