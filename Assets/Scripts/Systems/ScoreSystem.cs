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
    public class ScoreSystem : IComponentSystem
    {
        public EntityManager CurrentEntityManager { get; set; }

        public void OnUpdate()
        {
            var scores = CurrentEntityManager.Entities.Where(e => e.GetComponent<ScoreComponent>()).ToArray();
            int len = scores.Count();
            if (len > 0)
            {
                var players = CurrentEntityManager.Entities.Where(e => e.GetComponent<PlayerComponent>());
                for (int i = 0; i < len; i++)
                {
                    foreach (var player in players)
                    {
                        player.GetComponent<PlayerComponent>().Score += scores[i].GetComponent<ScoreComponent>().Score;
                    }

                    CurrentEntityManager.DestroyEntity(scores[i]);
                }
            }
        }

        public void OnPostUpdate()
        {

        }
    }
}
