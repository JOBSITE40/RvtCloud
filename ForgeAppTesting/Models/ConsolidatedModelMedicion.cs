using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeAppTesting.Models.Bc3
{
    public class ConsolidatedModelMedicion
    {
        public Concepto Padre { get; set; }
        public Concepto Hijo { get; set; }
        public double CantidadTotal { get; set; }
        public List<ComentarioModelMedicion> ModelMediciones { get; }

        public ConsolidatedModelMedicion(Concepto padre, Concepto hijo, double cantidadTotal, List<ComentarioModelMedicion> modelMediciones)
        {
            this.Padre = padre;
            this.Hijo = hijo;
            this.CantidadTotal = cantidadTotal;
            this.ModelMediciones = modelMediciones;
        }
    }
}
