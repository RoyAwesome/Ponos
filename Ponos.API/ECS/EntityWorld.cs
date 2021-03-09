using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.API.ECS
{
    public class EntityWorld
    {
        public struct Config
        {
            public int InitialEntityCacheSize;

            public int InitialComponentPoolsCacheSize;

            public int InitialComponentCacheSize;

            public const int DefaultInitialEntityCacheSize = 1024;

            public const int DefaultInitialComponentPoolCacheSize = 128;

            public const int DefaultInitialComponentCacheSize = 8;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        public struct EntityData
        {
            public ushort Generation;
            public short DoubleComponentsCount;
            public int[] Components;
        }

        protected readonly GrowOnlyList<uint> FreeEntities;
        protected EntityData[] Entities;
        protected uint TotalEntities;

        IComponentPool[] ComponentPools;
        int CreatedComponentPoolCount;

        Config WorldConfig;

        public EntityWorld(Config config = default)
        {
            WorldConfig = new()
            {
                InitialEntityCacheSize = config.InitialEntityCacheSize <= 0 ? Config.DefaultInitialEntityCacheSize : config.InitialEntityCacheSize,
                InitialComponentPoolsCacheSize = config.InitialComponentPoolsCacheSize <= 0 ? Config.DefaultInitialComponentPoolCacheSize : config.InitialComponentPoolsCacheSize,
                InitialComponentCacheSize = config.InitialComponentCacheSize <= 0 ? Config.DefaultInitialComponentCacheSize : config.InitialComponentCacheSize,
            };

            Entities = new EntityData[WorldConfig.InitialEntityCacheSize];
            FreeEntities = new GrowOnlyList<uint>(WorldConfig.InitialEntityCacheSize);
            ComponentPools = new IComponentPool[WorldConfig.InitialComponentPoolsCacheSize];
        }

        public Entity NewEntity()
        {
            Entity entity = new();
            entity.Owner = this;

            if(FreeEntities.Count > 0)
            {
                entity.Id = FreeEntities.Items[--FreeEntities.Count];

                ref var EntityData = ref Entities[entity.Id];
                entity.Generation = EntityData.Generation;
                EntityData.DoubleComponentsCount = 0;
            }
            else
            {
                if(TotalEntities == Entities.Length)
                {
                    Array.Resize(ref Entities, Entities.Length << 1);
                }
                entity.Id = TotalEntities++;
                ref var entityData = ref Entities[entity.Id];
                entityData.Components = new int[WorldConfig.InitialComponentCacheSize * 2];
                entityData.Generation = 1;
                entity.Generation = entityData.Generation;
                entityData.DoubleComponentsCount = 0;
            }

            return entity;
        }

        protected internal void RecycleEntity(uint id, ref EntityData entityData)
        {
            entityData.DoubleComponentsCount = -2;
            if(entityData.Generation < 0)
            {
                entityData.Generation = 0;
            }
            else
            {
                entityData.Generation++;
            }

            FreeEntities.Add(id);
        }

        public void Destroy(Entity entity)
        {
            ref var entityData = ref GetEntityData(entity);

            Entity savedData = new()
            {
                Id = entity.Id,
                Generation = entity.Generation,
                Owner = entity.Owner,
            };

            //recycle the components
            for(var i = entityData.DoubleComponentsCount - 2; i >= 0; i-=2)
            {
                savedData.Owner.ComponentPools[entityData.Components[i]].Recycle(entityData.Components[i + 1]);
                entityData.DoubleComponentsCount -= 2;
            }


            //recycle the entity
            entityData.DoubleComponentsCount = 0;
            RecycleEntity(entity.Id, ref entityData);

        }

        public ref T GetOrAddComponent<T>(in Entity entity) where T : struct
        {
            ref var entityData = ref entity.Owner.GetEntityData(entity);

            //Check if we have this component. If so, return it
            var typeIndex = ComponentType<T>.TypeIndex;
            for (int i = 0, iMax = entityData.DoubleComponentsCount; i < iMax; i+= 2)
            {
                if(entityData.Components[i] == typeIndex)
                {
                    return ref ((ComponentPool<T>)entity.Owner.ComponentPools[typeIndex]).Items[entityData.Components[i + 1]];
                }
            }

            //otherwise, create a new component and attach it.
            if(entityData.Components.Length == entityData.DoubleComponentsCount)
            {
                Array.Resize(ref entityData.Components, entityData.DoubleComponentsCount << 1);
            }
            entityData.Components[entityData.DoubleComponentsCount++] = typeIndex;

            var pool = entity.Owner.GetComponentPool<T>();

            var componentIndex = pool.CreateNew();

            entityData.Components[entityData.DoubleComponentsCount++] = componentIndex;

            return ref pool.GetItem(componentIndex);
        }

        protected internal ref EntityData GetEntityData(in Entity entity)
        {
            return ref Entities[entity.Id];
        }

        protected internal ComponentPool<T> GetComponentPool<T>() where T : struct
        {
            var typeIndex = ComponentType<T>.TypeIndex;
            if(ComponentPools.Length < typeIndex)
            {
                var len = ComponentPools.Length << 1;
                while(len <= typeIndex)
                {
                    len <<= 1;
                }
                Array.Resize(ref ComponentPools, len);
            }

            var pool = (ComponentPool<T>)ComponentPools[typeIndex];
            if(pool == null)
            {
                pool = new ComponentPool<T>();
                ComponentPools[typeIndex] = pool;
                CreatedComponentPoolCount++;
            }

            return pool;
        }
    }

   

    public class GrowOnlyList<T>
    {
        public T[] Items;
        public int Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GrowOnlyList(int capacity)
        {
            Items = new T[capacity];
            Count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            if(Items.Length == Count)
            {
                Array.Resize(ref Items, Items.Length << 1);
            }
            Items[Count++] = item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnsureCapacity(int count)
        {
            if(Items.Length < count)
            {
                var len = Items.Length << 1;
                while(len <= count)
                {
                    len <<= 1;
                }
                Array.Resize(ref Items, len);
            }
        }
    }
}
