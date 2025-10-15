using MasterEDI.CFDI.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterEDI.CFDI.Domain.Interface
{
    public interface ICertifiedService
    {
        CertifiedInfo LoadCertificate(string alias);
    }
}
