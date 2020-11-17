using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeAppTesting.Models.Bc3
{
    public class ComentarioModelMedicion
    {
        public string Comentario { get; set; }
        public double CantidadParcial { get; set; }
        public List<ModelMedicion> ModelMediciones { get; set; }

        public ComentarioModelMedicion(string comentario, double cantidadParcial, List<ModelMedicion> modelMediciones)
        {
            this.Comentario = comentario;
            this.CantidadParcial = cantidadParcial;
            this.ModelMediciones = modelMediciones;
        }
    }
}
