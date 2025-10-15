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
    /// Valida los datos de un concepto (<see cref="Concept"/>).
    /// Verifica que los campos obligatorios estén presentes y tengan valores válidos.
    /// </summary>
    public class ConceptValidator : IConceptValidator
    {
        /// <summary>
        /// Valida todos los campos de una instancia de <see cref="Concept"/>.
        /// </summary>
        /// <param name="concept">El concepto a validar.</param>
        /// <returns>
        /// Una colección de mensajes de error. Si el concepto es válido, la colección estará vacía.
        /// </returns>
        /// <remarks>
        /// Los campos validados son:
        /// <list type="bullet">
        /// <item><description>IdProduct (obligatorio, no vacio)</description></item>
        /// <item><description>Description (obligatoria, no vacía)</description></item>
        /// <item><description>Unity (mayor que cero)</description></item>
        /// <item><description>UnityValue (no negativo)</description></item>
        /// </list>
        /// </remarks>
        public IEnumerable<string> ValidateAll(Concept concept)
        {
            var errors = new List<string>();

            if (concept == null)
            {
                errors.Add("El concepto no puede ser nulo.");
                return errors;
            }

            if (string.IsNullOrWhiteSpace(concept.IdProduct))
                errors.Add("IdProduct es obligatorio.");

            if (string.IsNullOrWhiteSpace(concept.Description))
                errors.Add("Description es obligatoria.");

            if (concept.Unity <= 0)
                errors.Add("Unity debe ser mayor que cero.");

            if (concept.UnityValue < 0)
                errors.Add("UnityValue no puede ser negativo.");

            return errors;
        }

        /// <summary>
        /// Valida todos los conceptos de una lista.
        /// </summary>
        /// <param name="concepts">Lista de conceptos a validar.</param>
        /// <returns>
        /// Una coleccion de mensajes de error para el ultimo concepto validado.
        /// <para>Nota: La implementación actual solo retorna los errores del ultimo elemento.</para>
        /// </returns>
        public IEnumerable<string> ValidateAllItems(List<Concept> concepts)
        {
            var errors = Enumerable.Empty<string>();
            foreach (var item in concepts)
            {
                errors = ValidateAll(item);
            }
            return errors;
        }
    }
}
