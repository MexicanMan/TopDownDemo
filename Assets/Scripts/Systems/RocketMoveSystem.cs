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
    public class RocketMoveSystem : IComponentSystem
    {
        public EntityManager CurrentEntityManager { get; set; }

        private Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        public void OnUpdate()
        {
            var rockets = CurrentEntityManager.Entities.Where(e => e.GetComponent<RocketComponent>()).ToArray();
            int len = rockets.Length;
            for (int i = 0; i < len; i++)
            {
                RocketComponent rocketComponent = rockets[i].GetComponent<RocketComponent>();

                // If rocket's target have been destroyed somehow - destroy rocket too
                if (rocketComponent.ClosestTarget != null)
                {
                    Vector3 dir = (rockets[i].GetComponent<RocketComponent>().ClosestTarget.transform.position - rockets[i].transform.position).normalized;
                    float distance = rockets[i].GetComponent<RocketComponent>().Speed * Time.deltaTime;
                    Vector3 newPos = rockets[i].transform.position + dir * distance;

                    // Rotate rocket in the direction to a target
                    Vector3 up = rockets[i].transform.up;
                    float rotateAngle = Vector3.SignedAngle(up, dir, new Vector3(0, 0, 1));

                    rockets[i].transform.Rotate(new Vector3(0, 0, 1), rotateAngle);

                    // Check if rocket collide with target
                    RaycastHit2D raycastHit = Physics2D.Raycast(rockets[i].transform.position, dir, distance);

                    if (raycastHit.collider == null || !raycastHit.collider.GetComponent<TargetComponent>())
                        rockets[i].transform.position = newPos;
                    else if (raycastHit.collider.GetComponent<TargetComponent>())
                    {
                        CurrentEntityManager.DestroyEntity(raycastHit.collider.gameObject);
                        CurrentEntityManager.DestroyEntity(rockets[i]);

                        var scoreEntity = CurrentEntityManager.CreateEntity("scoreEntity", typeof(ScoreComponent));
                        scoreEntity.GetComponent<ScoreComponent>().Score = 1;
                    }

                    // If rocket is not visible by main camera
                    if (!GeometryUtility.TestPlanesAABB(planes, rockets[i].GetComponent<SpriteRenderer>().bounds))
                        CurrentEntityManager.DestroyEntity(rockets[i]);
                }
                else
                    CurrentEntityManager.DestroyEntity(rockets[i]);
            }
        }

        public void OnPostUpdate()
        {

        }
    }
}
