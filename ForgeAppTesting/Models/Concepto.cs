using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeAppTesting.Models.Bc3
{
    public class Concepto
    {
        public string Codigo { get; set; }
        public string Unidad { get; set; }
        public string Resumen { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; }
        public Concepto(string codigo, string unidad, string resumen, string descripcion, double precio)
        {
            this.Codigo = codigo;
            this.Unidad = unidad;
            this.Resumen = resumen;
            this.Descripcion = descripcion;
            this.Precio = precio;
        }
    }
}
