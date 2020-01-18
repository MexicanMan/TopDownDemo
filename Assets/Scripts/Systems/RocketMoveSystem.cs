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
                if (rocketComponent.ClosestTarget != null)
                {
                    Vector3 dir = (rockets[i].GetComponent<RocketComponent>().ClosestTarget.transform.position - rockets[i].transform.position).normalized;
                    float distance = rockets[i].GetComponent<RocketComponent>().Speed * Time.deltaTime;
                    Vector3 newPos = rockets[i].transform.position + dir * distance;

                    Vector3 up = rockets[i].transform.up;
                    float rotateAngle = Mathf.Acos((up.x * dir.x + up.y * dir.y) / 
                        (Mathf.Sqrt(up.x * up.x + up.y * up.y) * Mathf.Sqrt(dir.x * dir.x + dir.y * dir.y))) * Mathf.Rad2Deg;
                    rockets[i].transform.Rotate(new Vector3(0, 0, 1), rotateAngle);

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
