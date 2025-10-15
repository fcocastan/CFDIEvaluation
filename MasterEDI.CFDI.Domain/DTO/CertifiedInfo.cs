using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterEDI.CFDI.Domain.DTO
{
    /// <summary>
    /// Representa la información de un certificado digital utilizado para la firma de comprobantes.
    /// </summary>
    public class CertifiedInfo
    {
        /// <summary>
        /// Número de serie del certificado.
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Archivo PFX del certificado en formato de arreglo de bytes.
        /// </summary>
        public byte[] Pfx { get; set; } 

        /// <summary>
        /// Clave privada simulada para pruebas o procesos no productivos.
        /// </summary>
        public string SimulatedPrivateKey { get; set; }
    }
}
