using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterEDI.CFDI.Domain.DTO
{
    /// <summary>
    /// Representa una factura electrónica (CFDI) con sus datos principales.
    /// </summary>
    public class Bill
    {
        /// <summary>
        /// Serie de la factura.
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Folio de la factura.
        /// </summary>
        public string Folio { get; set; }

        /// <summary>
        /// Fecha de emisión de la factura.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Información del receptor de la factura.
        /// </summary>
        public Receptor Receptor { get; set; }

        /// <summary>
        /// Lista de conceptos incluidos en la factura.
        /// </summary>
        public List<Concept> Concepts { get; set; } = new();

        /// <summary>
        /// Subtotal de la factura antes de impuestos.
        /// </summary>
        public decimal SubTotal { get; set; }

        /// <summary>
        /// Total de la factura incluyendo impuestos.
        /// </summary>
        public decimal Total { get; set; }
    }
}
