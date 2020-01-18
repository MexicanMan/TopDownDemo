using Assets.Scripts.Components;
using Assets.Scripts.ECSHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace Assets.Scripts.Systems
{
    class UISystem : IComponentSystem
    {
        public EntityManager CurrentEntityManager { get; set; }

        private TMP_Text scoreText;

        public UISystem(TMP_Text scoreText)
        {
            this.scoreText = scoreText;
        }

        public void OnUpdate()
        {
            var players = CurrentEntityManager.Entities.Where(e => e.GetComponent<PlayerComponent>());
            int score = 0;

            foreach (var player in players)
            {
                score += player.GetComponent<PlayerComponent>().Score;
            }

            scoreText.text = score.ToString();
        }

        public void OnPostUpdate()
        {

        }
    }
}
