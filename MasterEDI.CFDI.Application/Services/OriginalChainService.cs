using MasterEDI.CFDI.Domain.DTO;
using MasterEDI.CFDI.Domain.Interface;
using MasterEDI.CFDI.Domain.Utils;
using MasterEDI.CFDI.Domain.Validators;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MasterEDI.CFDI.Application.Services
{
    /// <summary>
    /// Servicio encargado de generar la cadena original de una factura electrónica (CFDI).
    /// Realiza validaciones sobre la factura y sus conceptos antes de construir la cadena.
    /// </summary>
    public class OriginalChainService : IOriginalChainService
    {
        /// <summary>
        /// Genera la cadena original a partir de los datos de la factura.
        /// Valida la factura y sus conceptos, y lanza una excepción si existen errores.
        /// </summary>
        /// <param name="data">Instancia de <see cref="Bill"/> con los datos de la factura.</param>
        /// <returns>
        /// Cadena original construida con los datos normalizados de la factura y sus conceptos.
        /// </returns>
        /// <exception cref="ArgumentNullException">Si <paramref name="data"/> es nulo.</exception>
        /// <exception cref="InvalidDataException">Si la factura o sus conceptos contienen errores de validación.</exception>
        /// <remarks>
        /// La cadena original incluye los siguientes campos:
        /// <list type="bullet">
        /// <item><description>Serie</description></item>
        /// <item><description>Folio</description></item>
        /// <item><description>Fecha</description></item>
        /// <item><description>RFC y nombre del receptor</description></item>
        /// <item><description>Conceptos ordenados por IdProduct</description></item>
        /// <item><description>Subtotal y Total</description></item>
        /// </list>
        /// </remarks>
        public string GenerateOriginalChain(Bill data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            IBillValidator billValidator = new BillValidator();
            IEnumerable<string> errors = new List<string>();
            errors = billValidator.ValidateAll(data);
            IConceptValidator conceptValidator = new ConceptValidator();
            var conceptErrors = conceptValidator.ValidateAllItems(data.Concepts);
            errors.Concat(conceptErrors);

            if (errors.Any())
            {
                throw new InvalidDataException(string.Join(" | ", errors));
            }


            var sb = new StringBuilder();
            sb.Append($"SERIE:{StringUtils.Normalize(data.SerialNumber)}|");
            sb.Append($"FOLIO:{StringUtils.Normalize(data.Folio)}|");
            sb.Append($"FECHA:{data.Date.ToString("o")}|");
            sb.Append($"RECEPTOR_RFC:{StringUtils.Normalize(data.Receptor.Rfc)}|");
            sb.Append($"RECEPTOR_NOMBRE:{StringUtils.Normalize(data.Receptor.Name)}|");

            var conceptosOrdenados = data.Concepts
                .OrderBy(c => c.IdProduct ?? string.Empty)
                .Select(c => $"{StringUtils.Normalize(c.IdProduct)}~{StringUtils.Normalize(c.Description)}~{c.Unity}:{c.UnityValue}");

            sb.Append("CONCEPTOS:<[");
            sb.Append(string.Join(";", conceptosOrdenados));
            sb.Append("]>|");
            sb.Append($"SUBTOTAL:{data.SubTotal:F2}|TOTAL:{data.Total:F2}");

            return sb.ToString();
        }
    }
}
