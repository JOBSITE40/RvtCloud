using ForgeAppTesting.Models.Bc3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeAppTesting.Models
{
    public class MedicionesJson
    {
        public List<Concepto> Conceptos { get; set; }
        public List<Descomposicion> DescomposicionesToWrite { get; set; }
        public List<ConsolidatedModelMedicion> MedicionesToWrite { get; set; }

        public MedicionesJson(List<Concepto> conceptos, List<Descomposicion> descomposicionesToWrite, List<ConsolidatedModelMedicion> medicionesToWrite)
        {
            this.Conceptos = conceptos;
            this.DescomposicionesToWrite = descomposicionesToWrite;
            this.MedicionesToWrite = medicionesToWrite;
        }
    }
}
