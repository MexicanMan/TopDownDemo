using Assets.Scripts.Components;
using Assets.Scripts.ECSHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Systems
{
    public class TargetMovementSystem : IComponentSystem
    {
        public EntityManager CurrentEntityManager { get; set; }

        private float[] targetYEdges;

        public TargetMovementSystem(float[] targetYEdges)
        {
            this.targetYEdges = targetYEdges;
        }

        public void OnUpdate()
        {
            var targets = CurrentEntityManager.Entities.Where(e => e.GetComponent<TargetComponent>()).ToArray();
            for(int i = 0; i < targets.Length; i++)
            {
                int dir = targets[i].GetComponent<TargetComponent>().TargetDirection;

                // Move target up or down
                targets[i].transform.position += new Vector3(0, dir) *
                    targets[i].GetComponent<TargetComponent>().TargetSpeed * Time.deltaTime;

                // Check if target has gone
                if ((targets[i].transform.position.y < targetYEdges[0] && dir == -1) || 
                    (targets[i].transform.position.y > targetYEdges[1] && dir == 1))
                    CurrentEntityManager.DestroyEntity(targets[i]);
            }
        }

        public void OnPostUpdate()
        {

        }
    }
}
