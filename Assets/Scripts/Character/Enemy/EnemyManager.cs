using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class EnemyManager : CharacterManager
    {
        private EnemyLocomotion enemyLocomotion;

        [Header("Enemy Flags ")]
        public bool isInteracting;

        [Header("A.I Settings")]
        public float detectionRadius = 20;
        public float minimumDetectionAngle = -50;   // Enemy min field of view angle
        public float maximumDetectionAngle = 50;    // Enemy max field of view angle

        private void Awake()
        {
            enemyLocomotion = GetComponent<EnemyLocomotion>();
        }

        private void FixedUpdate()
        {
            HandleCurrentAction();
        }

        private void HandleCurrentAction()
        {
            if (enemyLocomotion.currentTarget == null)
            {
                enemyLocomotion.HandleDectection();
            }

            else
            {
                enemyLocomotion.HandleMoveToTarget();
            }
        }
    }
}