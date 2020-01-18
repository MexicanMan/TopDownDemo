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
    public class PlayerMovementSystem : IComponentSystem
    {
        public EntityManager CurrentEntityManager { get; set; }

        public void OnUpdate()
        {
            var inputs = CurrentEntityManager.Entities.Where(e => e.GetComponent<InputMoveComponent>()).ToArray();
            int len = inputs.Count();
            if (len > 0)
            {
                var players = CurrentEntityManager.Entities.Where(e => e.GetComponent<PlayerComponent>());
                for (int i = 0; i < len; i++)
                {
                    foreach (var player in players)
                    {
                        int dir = inputs[i].GetComponent<InputMoveComponent>().Vertical;
                        
                        // Rotate player
                        player.transform.Rotate(inputs[i].GetComponent<InputMoveComponent>().Rotation * (dir != 0 ? dir : 1), 
                            player.GetComponent<PlayerComponent>().PlayerSpeed);

                        // Calcs new position values
                        Vector3 moveDir = new Vector3(-Mathf.Sin(Mathf.Deg2Rad * player.transform.eulerAngles.z),
                            Mathf.Cos(Mathf.Deg2Rad * player.transform.eulerAngles.z)) * dir;
                        float distance = player.GetComponent<PlayerComponent>().PlayerSpeed * Time.deltaTime;
                        Vector3 newPos = player.transform.position + moveDir * distance;

                        // Checks if any of obstacles are on our way
                        RaycastHit2D raycastHit = Physics2D.Raycast(player.transform.position, 
                            moveDir, distance + player.GetComponent<PlayerComponent>().PlayerOffset);

                        if (raycastHit.collider == null)
                            player.transform.position = newPos; 
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
