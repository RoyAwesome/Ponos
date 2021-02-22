using Ponos.API.Interfaces;
using System;

namespace Ponos.API
{
    public interface IEngine : INamed
    {
        public string Version
        {
            get;
        }
    }
}
