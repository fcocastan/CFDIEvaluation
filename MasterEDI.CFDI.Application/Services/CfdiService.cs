using MasterEDI.CFDI.Domain.DTO;
using MasterEDI.CFDI.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MasterEDI.CFDI.Application.Services
{
    /// <summary>
    /// Servicio para la generación de la cadena original y la simulación de la firma de un comprobante fiscal (CFDI).
    /// </summary>
    public class CfdiService
    {
        private readonly IOriginalChainService _cadenaGenerator;
        private readonly ICertifiedService _certService;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="CfdiService"/>.
        /// </summary>
        /// <param name="cadenaGenerator">Servicio para generar la cadena original del CFDI.</param>
        /// <param name="certService">Servicio para cargar la información del certificado digital.</param>
        public CfdiService(IOriginalChainService cadenaGenerator, ICertifiedService certService)
        {
            _cadenaGenerator = cadenaGenerator;
            _certService = certService;
        }

        /// <summary>
        /// Genera la cadena original y simula la firma digital usando el certificado especificado.
        /// </summary>
        /// <param name="factura">Factura (<see cref="Bill"/>) a procesar.</param>
        /// <param name="aliasCert">Alias del certificado digital a utilizar.</param>
        /// <returns>
        /// Una tupla con la cadena original generada y el sello simulado en base64.
        /// </returns>
        /// <remarks>
        /// El sello simulado se genera aplicando SHA256 sobre la cadena original concatenada con el número de serie del certificado.
        /// </remarks>
        public (string Cadena, string SelloSimulado) GenerarYFirmar(Bill factura, string aliasCert)
        {
            var cadena = _cadenaGenerator.GenerateOriginalChain(factura);
            var cert = _certService.LoadCertificate(aliasCert);
            // Simula "firma" con SHA256 + serial del cert
            using var sha = SHA256.Create();
            var data = Encoding.UTF8.GetBytes(cadena + "|" + cert?.SerialNumber);
            var hash = sha.ComputeHash(data);
            var sello = Convert.ToBase64String(hash);

            return (cadena, sello);
        }
    }
}
