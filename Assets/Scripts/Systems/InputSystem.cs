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
    public class InputSystem : IComponentSystem
    {
        private const float FireTimeout = 0.3f;
        private const float RocketTimeout = 3f;

        public EntityManager CurrentEntityManager { get; set; }

        private float fireTimer = 0f;
        private float rocketTimer = 0f;

        public void OnUpdate()
        {
            int[] inputVals = new int[] { 0, 0, 0, 0 };
            if (Input.GetAxis("Horizontal") < 0)
                inputVals[0] = 1;
            else if (Input.GetAxis("Horizontal") > 0)
                inputVals[0] = -1;

            if (Input.GetAxis("Vertical") < 0)
                inputVals[1] = -1;
            else if (Input.GetAxis("Vertical") > 0)
                inputVals[1] = 1;

            if (Input.GetKeyUp("space") && fireTimer <= 0)
            {
                inputVals[2] = 1;
                fireTimer = FireTimeout;
            }
            if (Input.GetKeyUp(KeyCode.R) && rocketTimer <= 0)
            {
                inputVals[3] = 1;
                rocketTimer = RocketTimeout;
            }

            if (inputVals[0] != 0 || inputVals[1] != 0)
            {
                var inputEntity = CurrentEntityManager.CreateEntity("inputMoveEntity", typeof(InputMoveComponent));
                inputEntity.GetComponent<InputMoveComponent>().Vertical = inputVals[1];
                inputEntity.GetComponent<InputMoveComponent>().Rotation = new Vector3(0, 0, inputVals[0]);
            }
            if (inputVals[2] != 0 || inputVals[3] != 0)
            {
                var inputEntity = CurrentEntityManager.CreateEntity("inputFireEntity", typeof(InputFireComponent));
                inputEntity.GetComponent<InputFireComponent>().Fire = inputVals[2];
                inputEntity.GetComponent<InputFireComponent>().Rocket = inputVals[3];
            }

            fireTimer -= Time.deltaTime;
            rocketTimer -= Time.deltaTime;
        }

        public void OnPostUpdate()
        {
            
        }
    }
}
