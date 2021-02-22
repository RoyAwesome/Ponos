using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.API.Jobs
{
    public enum JobCreateOptions : byte
    {
        None        = 0,
        NoParent    = 0x01,
    }
}
