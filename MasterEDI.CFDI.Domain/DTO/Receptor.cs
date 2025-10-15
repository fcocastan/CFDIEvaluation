using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterEDI.CFDI.Domain.DTO
{
    /// <summary>
    /// Representa la información del receptor de una factura electrónica (CFDI).
    /// </summary>
    public class Receptor
    {
        /// <summary>
        /// RFC (Registro Federal de Contribuyentes) del receptor.
        /// </summary>
        public string Rfc { get; set; }

        /// <summary>
        /// Nombre o razón social del receptor.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Domicilio fiscal del receptor.
        /// </summary>
        public string Domicile { get; set; }
    }
}
