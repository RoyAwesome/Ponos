using Ponos.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.API
{
    public interface IGameInstance : INamed
    {
        public string Version
        {
            get;
        }      
    }
}
