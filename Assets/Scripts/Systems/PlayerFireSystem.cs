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
    public class PlayerFireSystem : IComponentSystem
    {
        public EntityManager CurrentEntityManager { get; set; }

        private Type[] bulletArchetype = new Type[] { typeof(SpriteRenderer),
            typeof(BulletComponent) };

        private Type[] rocketArchetype = new Type[] { typeof(SpriteRenderer),
            typeof(RocketComponent) };

        private Sprite bulletSprite;
        private Sprite rocketSprite;
        private float speed;

        public PlayerFireSystem(Sprite bulletSprite, Sprite rocketSprite, float speed)
        {
            this.bulletSprite = bulletSprite;
            this.rocketSprite = rocketSprite;
            this.speed = speed;
        }

        public void OnUpdate()
        {
            var inputs = CurrentEntityManager.Entities.Where(e => e.GetComponent<InputFireComponent>()).ToArray();
            int iLen = inputs.Length;
            if (iLen > 0)
            {
                var players = CurrentEntityManager.Entities.Where(e => e.GetComponent<PlayerComponent>()).ToArray();
                int pLen = players.Length;
                for (int i = 0; i < iLen; i++)
                {
                    for (int j = 0; j < pLen; j++)
                    {
                        var fireComponent = inputs[i].GetComponent<InputFireComponent>();

                        if (fireComponent.Fire == 1)
                        {
                            var bulletEntity = CurrentEntityManager.CreateEntity("bulletEntity", bulletArchetype);

                            Vector3 bulletDir = new Vector3(
                                -Mathf.Sin(Mathf.Deg2Rad * players[j].transform.eulerAngles.z) * players[j].GetComponent<PlayerComponent>().PlayerOffset,
                                Mathf.Cos(Mathf.Deg2Rad * players[j].transform.eulerAngles.z) * players[j].GetComponent<PlayerComponent>().PlayerOffset);

                            bulletEntity.transform.SetPositionAndRotation(players[j].transform.position + bulletDir,
                                players[j].transform.rotation);
                            bulletEntity.GetComponent<SpriteRenderer>().sprite = bulletSprite;
                            bulletEntity.GetComponent<SpriteRenderer>().sortingOrder = 3;
                            bulletEntity.GetComponent<BulletComponent>().Speed = speed;
                            bulletEntity.GetComponent<BulletComponent>().Direction = bulletDir;
                        }

                        if (fireComponent.Rocket == 1)
                        {
                            var rocketEntity = CurrentEntityManager.CreateEntity("rocketEntity", rocketArchetype);

                            Vector3 rocketDir = new Vector3(
                                -Mathf.Sin(Mathf.Deg2Rad * players[j].transform.eulerAngles.z) * players[j].GetComponent<PlayerComponent>().PlayerOffset,
                                Mathf.Cos(Mathf.Deg2Rad * players[j].transform.eulerAngles.z) * players[j].GetComponent<PlayerComponent>().PlayerOffset);

                            rocketEntity.transform.SetPositionAndRotation(players[j].transform.position + rocketDir,
                                players[j].transform.rotation);
                            rocketEntity.GetComponent<SpriteRenderer>().sprite = rocketSprite;
                            rocketEntity.GetComponent<SpriteRenderer>().sortingOrder = 3;
                            rocketEntity.GetComponent<RocketComponent>().Speed = speed;

                            var targets = CurrentEntityManager.Entities.Where(e => e.GetComponent<TargetComponent>());
                            if (targets.Count() > 0)
                            {
                                GameObject closestTarget = targets.First();
                                foreach (var target in targets)
                                {
                                    if (Vector3.Distance(closestTarget.transform.position, rocketEntity.transform.position) >
                                        Vector3.Distance(target.transform.position, rocketEntity.transform.position))
                                        closestTarget = target;
                                }

                                rocketEntity.GetComponent<RocketComponent>().ClosestTarget = closestTarget;
                            }
                            else
                                rocketEntity.GetComponent<RocketComponent>().ClosestTarget = null;
                        }
                    }

                    CurrentEntityManager.DestroyEntity(inputs[i]);
                }
            }
        }

        public void OnPostUpdate()
        {

        }
    }
}
