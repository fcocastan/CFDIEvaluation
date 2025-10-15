using MasterEDI.CFDI.Domain.DTO;
using MasterEDI.CFDI.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterEDI.CFDI.Domain.Validators
{
    /// <summary>
    /// Valida los datos de una factura (<see cref="Bill"/>).
    /// Verifica que los campos obligatorios estén presentes y tengan valores válidos.
    /// </summary>
    public class BillValidator : IBillValidator
    {
        /// <summary>
        /// Valida todos los campos de una instancia de <see cref="Bill"/>.
        /// </summary>
        /// <param name="bill">La factura a validar.</param>
        /// <returns>
        /// Una colección de mensajes de error. Si la factura es válida, la colección estará vacía, se esta pensando en mejorar
        /// usando fluent validation (Verificar las mejoras)
        /// </returns>
        /// <remarks>
        /// Los campos validados son:
        /// <list type="bullet">
        /// <item><description>Folio (obligatorio, no vacío)</description></item>
        /// <item><description>Receptor (obligatorio, no nulo)</description></item>
        /// <item><description>Concepts (al menos un concepto)</description></item>
        /// <item><description>SubTotal (mayor que 0)</description></item>
        /// <item><description>Total (mayor que 0)</description></item>
        /// <item><description>Date (debe ser válido)</description></item>
        /// </list>
        /// </remarks>
        public IEnumerable<string> ValidateAll(Bill bill)
        {
            var errors = new List<string>();

            if (bill == null)
            {
                errors.Add("La factura no puede ser null");
                return errors;
            }

            if (string.IsNullOrWhiteSpace(bill.Folio))
                errors.Add("El Folio es obligatorio");

            if (bill.Receptor == null)
                errors.Add("Receptor no puede ser null");

            if (bill.Concepts == null || !bill.Concepts.Any())
                errors.Add("Concepts debe tener cuando menos un concepto");

            if (bill.SubTotal <= 0)
                errors.Add("El Subtotal debe ser mayor que 0");

            if (bill.Total <= 0)
                errors.Add("El Total debe ser mayor que 0");

            if (bill.Date == default)
                errors.Add("La Fecha no es valida");

            return errors;
        }
    }
}
