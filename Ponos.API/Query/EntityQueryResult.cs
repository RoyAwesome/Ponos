using Ponos.API.ECS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.API.Query
{
    public class EntityQueryResult
    {
        public ECS.Entity Entity;
    }

    public class EntityQueryResult<T> where T: struct
    {
        public ECS.Entity Entity;

        public ComponentRef<T> Component1;

    }

    public class EntityQueryResult<T, U> 
        where T : struct
        where U : struct
    {
        public ECS.Entity Entity;

        public ComponentRef<T> Component1;

        public ComponentRef<U> Component2;

    }
}
