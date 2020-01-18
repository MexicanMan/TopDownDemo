using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ECSHelpers
{
    public class EntityManager : ScriptableObject
    {
        public List<GameObject> Entities { get; private set; }

        private List<IComponentSystem> systems;

        //private List<PostUpdateCommand> postUpdateCommands;

        public EntityManager()
        {
            Entities = new List<GameObject>();

            systems = new List<IComponentSystem>();
            //postUpdateCommands = new List<PostUpdateCommand>();
        }

        public void RegisterSystem(IComponentSystem system)
        {
            system.CurrentEntityManager = this;
            systems.Add(system);
        }

        public GameObject CreateEntity(string name, params Type[] components)
        {
            GameObject entity = new GameObject(name, components);
            Entities.Add(entity);

            return entity;
        }

        public void DestroyEntity(GameObject entity)
        {
            Entities.Remove(entity);
            Destroy(entity);
        }

        public void Update()
        {
            foreach(var system in systems)
            {
                system.OnUpdate();
            }
            
            PostUpdate();
        }

        public void PostUpdate()
        {
            foreach (var system in systems)
            {
                system.OnPostUpdate();
            }
        }
    }
}
