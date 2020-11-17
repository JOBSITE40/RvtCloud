using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeAppTesting.Models.Bc3
{
    public class Descomposicion
    {
        public string Padre { get; set; }
        public string Hijo { get; set; }
        public string Factor { get; set; }
        public string Rendimiento { get; set; }

        public Descomposicion(string padre, string hijo, string factor, string rendimiento)
        {
            this.Padre = padre;
            this.Hijo = hijo;
            this.Factor = factor;
            this.Rendimiento = rendimiento;
        }
    }
}
