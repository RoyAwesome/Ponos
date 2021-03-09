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

    public interface IEntityQuery<T> where T : struct
    {
        public IEnumerable<EntityQueryResult<T>> QueryResults
        {
            get;
            set;
        }
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



}
