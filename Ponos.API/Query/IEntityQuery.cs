using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.API.Query
{
    public interface IEntityQuery
    {
        public IEnumerable<EntityQueryResult> QueryResults
        {
            get;
            set;
        }
    }

    public class EntityQueryResult
    {
        public ECS.Entity Entity;
    }


    public interface IEntityQuery<T> where T : struct
    {
        public IEnumerable<EntityQueryResult<T>> QueryResults
        {
            get;
            set;
        }
    }

    public class EntityQueryResult<T> where T : struct
    {
        public ECS.Entity Entity;

        public ECS.ComponentRef<T> Component1;

    }

    public interface IEntityQuery<T, U>
        where T : struct
        where U : struct
    {
        public IEnumerable<EntityQueryResult<T, U>> QueryResults
        {
            get;
            set;
        }
    }

    public class EntityQueryResult<T, U>
        where T : struct
        where U : struct
    {
        public ECS.Entity Entity;

        public ECS.ComponentRef<T> Component1;

        public ECS.ComponentRef<U> Component2;

    }



}
