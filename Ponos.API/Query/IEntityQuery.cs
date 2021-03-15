using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.API.Query
{
    public class EntityQuery
    {
        internal protected Type[] IncludeComponents;
        internal protected Type[] ExcludeComponents;
        internal protected Type[] SelectComponents;

        internal EntityQuery()
        {
            
        }

    }

    public static class EntityQueryStatics
    {
        public static EntityQuery SelectAllComponents(this EntityQuery query)
        {
            query.SelectComponents = null;

            return query;
        }

        public static EntityQuery SelectComponents<T>(this EntityQuery query) where T : struct
        {
            query.SelectComponents = new Type[] { typeof(T) };
            return query;
        }

        public static EntityQuery Include<T>(this EntityQuery query) where T : struct
        {
            query.IncludeComponents = new Type[] { typeof(T) };

            return query;
        }

        public static EntityQuery Exclude<T>(this EntityQuery query) where T: struct
        {
            query.ExcludeComponents = new Type[] { typeof(T) };
            return query;
        }
    }


}
