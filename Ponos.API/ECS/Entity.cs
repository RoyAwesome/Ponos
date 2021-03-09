using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.API.ECS
{
    public struct Entity : IEquatable<Entity>
    {
        internal uint Id;
        internal ushort Generation;
        internal EntityWorld Owner;

        public static readonly Entity Null = new()
        {
            Id = uint.MinValue,
            Generation = ushort.MinValue,
            Owner = null,
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator==(in Entity lhs, in Entity rhs)
        {
            return lhs.Id == rhs.Id && lhs.Generation == rhs.Generation && lhs.Owner == rhs.Owner;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator!=(in Entity lhs, in Entity rhs)
        {
            return !(lhs == rhs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return obj is Entity other && Equals(other);
        }

        public bool Equals(Entity other)
        {
            return Id == other.Id && Generation == other.Generation && Owner == other.Owner;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            int hashCode = ((int)Id * 397) ^ Generation.GetHashCode();
            hashCode = (hashCode * 397) ^ (Owner?.GetHashCode() ?? 0);

            return hashCode;
        }

#if DEBUG
        public override string ToString()
        {
            return $"Entity-{Id}:{Generation}";
        }
#endif
    }
}
