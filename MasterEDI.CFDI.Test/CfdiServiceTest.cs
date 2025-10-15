using System;
using System.Text;
using Xunit;
using Moq;
using MasterEDI.CFDI.Application.Services;
using MasterEDI.CFDI.Domain.DTO;
using MasterEDI.CFDI.Domain.Interface;

namespace MasterEDI.CFDI.Test
{
    public class CfdiServiceTest
    {
        [Fact]
        public void GenerarYFirmar_ReturnsCadenaAndSelloSimulado()
        {
            // Arrange
            var factura = new Bill
            {
                SerialNumber = "SER",
                Folio = "FOL",
                Date = DateTime.Now,
                Receptor = new Receptor { Rfc = "RFC", Name = "Name" },
                Concepts = new System.Collections.Generic.List<Concept>
                {
                    new Concept { IdProduct = "P1", Description = "Desc", Unity = 1, UnityValue = 10 }
                },
                SubTotal = 100,
                Total = 100
            };

            var cadenaEsperada = "CADENA_PRUEBA";
            var cert = new CertifiedInfo { SerialNumber = "123456" };

            var cadenaMock = new Mock<IOriginalChainService>();
            cadenaMock.Setup(x => x.GenerateOriginalChain(factura)).Returns(cadenaEsperada);

            var certMock = new Mock<ICertifiedService>();
            certMock.Setup(x => x.LoadCertificate("alias")).Returns(cert);

            var service = new CfdiService(cadenaMock.Object, certMock.Object);

            // Act
            var (cadena, sello) = service.GenerarYFirmar(factura, "alias");

            // Assert
            Assert.Equal(cadenaEsperada, cadena);

            // El sello debe ser el hash SHA256 en base64 de "CADENA_PRUEBA|123456"
            using var sha = System.Security.Cryptography.SHA256.Create();
            var data = Encoding.UTF8.GetBytes(cadenaEsperada + "|123456");
            var expectedSello = Convert.ToBase64String(sha.ComputeHash(data));
            Assert.Equal(expectedSello, sello);
        }

        [Fact]
        public void GenerarYFirmar_CertificateIsNull_SelloSimuladoUsesNullSerial()
        {
            // Arrange
            var factura = new Bill
            {
                SerialNumber = "SER",
                Folio = "FOL",
                Date = DateTime.Now,
                Receptor = new Receptor { Rfc = "RFC", Name = "Name" },
                Concepts = new System.Collections.Generic.List<Concept>
                {
                    new Concept { IdProduct = "P1", Description = "Desc", Unity = 1, UnityValue = 10 }
                },
                SubTotal = 100,
                Total = 100
            };

            var cadenaEsperada = "CADENA_PRUEBA";

            var cadenaMock = new Mock<IOriginalChainService>();
            cadenaMock.Setup(x => x.GenerateOriginalChain(factura)).Returns(cadenaEsperada);

            var certMock = new Mock<ICertifiedService>();
            certMock.Setup(x => x.LoadCertificate("alias")).Returns((CertifiedInfo)null);

            var service = new CfdiService(cadenaMock.Object, certMock.Object);

            // Act
            var (cadena, sello) = service.GenerarYFirmar(factura, "alias");

            // Assert
            Assert.Equal(cadenaEsperada, cadena);

            // El sello debe ser el hash SHA256 en base64 de "CADENA_PRUEBA|"
            using var sha = System.Security.Cryptography.SHA256.Create();
            var data = Encoding.UTF8.GetBytes(cadenaEsperada + "|");
            var expectedSello = Convert.ToBase64String(sha.ComputeHash(data));
            Assert.Equal(expectedSello, sello);
        }

        [Fact]
        public void GenerarYFirmar_CadenaOriginalIncompleta_RegresaSelloSimulado()
        {
            // Arrange
            var factura = new Bill
            {
                SerialNumber = "SER",
                Folio = "FOL",
                Date = DateTime.Now,
                Receptor = new Receptor { Rfc = "RFC", Name = "Name" },
                Concepts = new System.Collections.Generic.List<Concept>
                {
                    new Concept { IdProduct = "P1", Description = "Desc", Unity = 1, UnityValue = 10 }
                },
                SubTotal = 100,
                Total = 100
            };

            // Simula una cadena original incompleta (por ejemplo, vacía o con datos faltantes)
            var cadenaIncompleta = string.Empty;
            var cert = new CertifiedInfo { SerialNumber = "123456" };

            var cadenaMock = new Mock<IOriginalChainService>();
            cadenaMock.Setup(x => x.GenerateOriginalChain(factura)).Returns(cadenaIncompleta);

            var certMock = new Mock<ICertifiedService>();
            certMock.Setup(x => x.LoadCertificate("alias")).Returns(cert);

            var service = new CfdiService(cadenaMock.Object, certMock.Object);

            // Act
            var (cadena, sello) = service.GenerarYFirmar(factura, "alias");

            // Assert
            Assert.Equal(cadenaIncompleta, cadena);
            // El sello debe ser el hash SHA256 en base64 de "|123456"
            using var sha = System.Security.Cryptography.SHA256.Create();
            var data = Encoding.UTF8.GetBytes("|123456");
            var expectedSello = Convert.ToBase64String(sha.ComputeHash(data));
            Assert.Equal(expectedSello, sello);
        }
    }
}
