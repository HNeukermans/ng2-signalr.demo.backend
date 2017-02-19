using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ng2signalR.Hubs
{
    internal class MachineKeyProtectionProvider : IDataProtectionProvider
    {
        public IDataProtector Create(params string[] purposes)
        {
            return new MachineKeyDataProtector(purposes);
        }
    }

    internal class MachineKeyDataProtector : IDataProtector
    {
        private readonly string[] _purposes;

        public MachineKeyDataProtector(string[] purposes)
        {
            _purposes = purposes;
        }

        public byte[] Protect(byte[] userData)
        {
            //return MachineKey.Protect(userData, _purposes);
            return userData;
        }

        public byte[] Unprotect(byte[] protectedData)
        {
            //return System.Web.Security.MachineKey.Unprotect(protectedData, _purposes);
            return protectedData;
        }
    }
}
