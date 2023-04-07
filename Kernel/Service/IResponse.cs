using Kernel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Service
{
    public interface IResponse
    {
        bool HasExceptions();
        IEnumerable<DomainException> Exceptions { get; }
        void AddException(DomainException domainException);
    }
}
