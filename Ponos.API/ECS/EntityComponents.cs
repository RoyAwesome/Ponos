using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ponos.API.ECS
{

    public interface IComponentReset<T> where T: struct
    {
        void Reset(ref T component);
    }

    internal sealed class ComponentCounter
    {
        internal static int ComponentTypesCount;
    }

    public static class ComponentType<T> where T : struct
    {
        public static readonly int TypeIndex;
        public static readonly Type Type;
        public static bool HasResetter;

        static ComponentType()
        {
            TypeIndex = Interlocked.Increment(ref ComponentCounter.ComponentTypesCount);
            Type = typeof(T);
            HasResetter = typeof(IComponentReset<T>).IsAssignableFrom(Type);
        }
    }

    public struct ComponentRef<T> where T : struct
    {
        internal int Index;
        internal ComponentPool<T> Pool;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in ComponentRef<T> lhs, in ComponentRef<T> rhs)
        {
            return lhs.Index == rhs.Index && lhs.Pool == rhs.Pool;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in ComponentRef<T> lhs, in ComponentRef<T> rhs)
        {
            return !(lhs == rhs);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return Index;
        }

        public override bool Equals(object obj)
        {
            return obj is ComponentRef<T> other && Equals(other);
        }

        public bool Equals(ComponentRef<T> other)
        {
            return this == other;
        }
    }

    public static class ComponentRefExt
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T Unref<T>(in this ComponentRef<T> wrapper) where T : struct
        {
            return ref wrapper.Pool.GetItem(wrapper.Index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNull<T>(in this ComponentRef<T> wrapper) where T : struct
        {
            return wrapper.Pool == null;
        }
    }

    public interface IComponentPool
    {
        public Type ItemType
        {
            get;
        }
        public object GetItem(int index);
        public void Recycle(int index);
        public int CreateNew();
        public void CopyData(int SourceIndex, int DestinationIndex);
    }

    public sealed class ComponentPool<T> : IComponentPool where T : struct
    {
        delegate void ComponentResetHandler(ref T component);

        public Type ItemType
        {
            get;
        }

        public T[] Items = new T[128];
        int[] reservedItems = new int[128];
        int itemsCount = 0;
        int reservedItemsCount = 0;

        readonly ComponentResetHandler ResetComponent;

        internal ComponentPool()
        {
            ItemType = typeof(T);

            if(ComponentType<T>.HasResetter)
            {
                var method = typeof(T).GetMethod(nameof(IComponentReset<T>.Reset));

                if(method != null)
                {
                    ResetComponent = (ComponentResetHandler)Delegate.CreateDelegate(
                   typeof(ComponentResetHandler),
                   null,
                   method);
                }               
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T GetItem(int index)
        {
            return ref Items[index];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ComponentRef<T> Ref(int index)
        {
            return new ComponentRef<T>
            {
                Pool = this,
                Index = index
            };
        }

        public void SetCapacity(int newCapacity)
        {
            if(Items.Length < newCapacity)
            {
                Array.Resize(ref Items, newCapacity);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyData(int SourceIndex, int DestinationIndex)
        {
            Items[DestinationIndex] = Items[SourceIndex];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CreateNew()
        {
            int id;
            if(reservedItemsCount > 0)
            {
                id = reservedItems[--reservedItemsCount];
            }
            else
            {
                id = itemsCount;
                if(itemsCount == Items.Length)
                {
                    Array.Resize(ref Items, itemsCount << 1);
                }

                ResetComponent?.Invoke(ref Items[itemsCount]);
                itemsCount++;
            }

            return id;
        }

        object IComponentPool.GetItem(int index)
        {
            return GetItem(index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Recycle(int index)
        {
            if(ResetComponent != null)
            {
                ResetComponent(ref Items[index]);
            }
            else
            {
                Items[index] = default;

            }

            if (reservedItemsCount == reservedItems.Length)
            {
                Array.Resize(ref reservedItems, reservedItemsCount << 1);
            }
            reservedItems[reservedItemsCount++] = index;
        }
    }


}
