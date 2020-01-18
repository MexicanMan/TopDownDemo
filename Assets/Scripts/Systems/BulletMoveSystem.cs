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
    public class BulletMoveSystem : IComponentSystem
    {
        public EntityManager CurrentEntityManager { get; set; }

        private Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        public void OnUpdate()
        {
            var bullets = CurrentEntityManager.Entities.Where(e => e.GetComponent<BulletComponent>()).ToArray();
            int len = bullets.Length;
            for (int i = 0; i < len; i++)
            {
                Vector3 dir = bullets[i].GetComponent<BulletComponent>().Direction;
                float distance = bullets[i].GetComponent<BulletComponent>().Speed * Time.deltaTime;
                Vector3 newPos = bullets[i].transform.position + dir * distance;

                // Check if bullet collide with target
                RaycastHit2D raycastHit = Physics2D.Raycast(bullets[i].transform.position, dir, distance);

                if (raycastHit.collider == null || !raycastHit.collider.GetComponent<TargetComponent>())
                    bullets[i].transform.position = newPos;
                else if (raycastHit.collider.GetComponent<TargetComponent>())
                {
                    CurrentEntityManager.DestroyEntity(raycastHit.collider.gameObject);
                    CurrentEntityManager.DestroyEntity(bullets[i]);

                    var scoreEntity = CurrentEntityManager.CreateEntity("scoreEntity", typeof(ScoreComponent));
                    scoreEntity.GetComponent<ScoreComponent>().Score = 1;
                }

                // If bullet is not visible by main camera
                if (!GeometryUtility.TestPlanesAABB(planes, bullets[i].GetComponent<SpriteRenderer>().bounds))
                    CurrentEntityManager.DestroyEntity(bullets[i]);
            }
        }

        public void OnPostUpdate()
        {

        }
    }
}
