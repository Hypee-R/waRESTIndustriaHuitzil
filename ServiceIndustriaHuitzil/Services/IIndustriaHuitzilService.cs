using CoreIndustriaHuitzil.ModelsRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceIndustriaHuitzil.Services
{
    public interface IIndustriaHuitzilService
    {
        Task<object> auth(AuthUserRequest usuario);
    }
}
