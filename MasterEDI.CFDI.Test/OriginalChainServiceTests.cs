using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MasterEDI.CFDI.Application.Services;
using MasterEDI.CFDI.Domain.DTO;

namespace MasterEDI.CFDI.Test
{
    public class OriginalChainServiceTests
    {
        [Fact]
        public void GenerateOriginalChain_NullBill_ThrowsArgumentNullException()
        {
            var service = new OriginalChainService();
            Assert.Throws<ArgumentNullException>(() => service.GenerateOriginalChain(null));
        }

        [Fact]
        public void GenerateOriginalChain_BillWithValidationErrors_ThrowsInvalidDataException()
        {
            var bill = new Bill
            {
                SerialNumber = "A",
                Folio = "1",
                Date = DateTime.Now,
                Receptor = new Receptor { Rfc = "RFC", Name = "Name" },
                Concepts = new List<Concept>(),
                SubTotal = 100,
                Total = 100
            };

            // BillValidator and ConceptValidator will return errors for empty Concepts
            var service = new OriginalChainService();
            var ex = Assert.Throws<InvalidDataException>(() => service.GenerateOriginalChain(bill));
            Assert.Contains("Concept", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void GenerateOriginalChain_ValidBill_ReturnsExpectedChain()
        {
            var bill = new Bill
            {
                SerialNumber = "SER123",
                Folio = "FOL456",
                Date = new DateTime(2024, 10, 14, 12, 0, 0, DateTimeKind.Utc),
                Receptor = new Receptor { Rfc = "RFC789", Name = "Carlos Adolfo" },
                Concepts = new List<Concept>
                {
                    new Concept { IdProduct = "P1", Description = "Desc1", Unity = 2, UnityValue = 10 },
                    new Concept { IdProduct = "P2", Description = "Desc2", Unity = 1, UnityValue = 20 }
                },
                SubTotal = 30,
                Total = 40
            };

            var service = new OriginalChainService();
            var chain = service.GenerateOriginalChain(bill);

            Assert.Contains("SERIE:SER123|", chain);
            Assert.Contains("FOLIO:FOL456|", chain);
            Assert.Contains("FECHA:2024-10-14T12:00:00.0000000Z|", chain);
            Assert.Contains("RECEPTOR_RFC:RFC789|", chain);
            Assert.Contains("RECEPTOR_NOMBRE:Carlos Adolfo|".ToUpper(), chain);
            Assert.Contains("CONCEPTOS:<[", chain);
            Assert.Contains("P1~Desc1~2:10".ToUpper(), chain);
            Assert.Contains("P2~Desc2~1:20".ToUpper(), chain);
            Assert.Contains("SUBTOTAL:30.00|TOTAL:40.00", chain);
        }

        [Fact]
        public void GenerateOriginalChain_ConceptsAreOrderedByIdProduct()
        {
            var bill = new Bill
            {
                SerialNumber = "S",
                Folio = "F",
                Date = DateTime.Now,
                Receptor = new Receptor { Rfc = "R", Name = "N" },
                Concepts = new List<Concept>
                {
                    new Concept { IdProduct = "B", Description = "DescB", Unity = 1, UnityValue = 1 },
                    new Concept { IdProduct = "A", Description = "DescA", Unity = 2, UnityValue = 2 }
                },
                SubTotal = 3,
                Total = 3
            };

            var service = new OriginalChainService();
            var chain = service.GenerateOriginalChain(bill);

            var idxA = chain.IndexOf("A~DescA~2:2".ToUpper());
            var idxB = chain.IndexOf("B~DescB~1:1".ToUpper());
            Assert.True(idxA < idxB, "Concepts should be ordered by IdProduct.");
        }
    }
}