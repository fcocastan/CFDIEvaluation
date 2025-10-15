using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterEDI.CFDI.Domain.DTO
{
    /// <summary>
    /// Representa un concepto o producto dentro de una factura electrónica (CFDI).
    /// </summary>
    public class Concept
    {
        /// <summary>
        /// Identificador del producto o servicio.
        /// </summary>
        public string IdProduct { get; set; }

        /// <summary>
        /// Descripción del producto o servicio.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Cantidad o unidad del producto o servicio.
        /// </summary>
        public decimal Unity { get; set; }

        /// <summary>
        /// Valor unitario del producto o servicio.
        /// </summary>
        public decimal UnityValue { get; set; }
    }
}
