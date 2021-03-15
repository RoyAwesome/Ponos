using Ponos.API;
using Ponos.API.ECS;
using Ponos.API.Components;
using Ponos.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ponos.Samples.BasicRendering
{
    class GameInstance : IGameInstance, IApplicationEventListener
    {
        public string Version => "1.0.0.0";

        public string Name => "Sample Rendering";

        EntityWorld entityWorld;

        public GameInstance(EntityWorld entityWorld)
        {
            this.entityWorld = entityWorld;
        }

        public void OnApplicationEvent(ApplicationEvent Stage)
        {
            if(Stage == ApplicationEvent.Begin)
            {
                Entity e = entityWorld.NewEntity();
                entityWorld.GetOrAddComponent<Transform>(e);
            }
        }
    }
}
