﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.API.Interfaces
{
    public interface IApplicationEventListener
    {
        public void OnApplicationEvent(ApplicationEvent Stage);
    }
}
