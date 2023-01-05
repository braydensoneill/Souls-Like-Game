using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class AmbushState : State
    {
        public bool isSleeping;
        public float detectionRadius = 2;
        public string sleepAnimation;
        public string wakeAnimation;

        public LayerMask detectionLayer;

        public EnemyPursueTargetState pursueTargetState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            if (enemyManager.isInteracting)
                return this;

            if (isSleeping && enemyManager.isInteracting == false)
            {
                enemyAnimatorHandler.PlayTargetAnimation(sleepAnimation, true);
            }

            #region Handler Target Detection
            Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats charactersStats = colliders[i].transform.GetComponent<CharacterStats>();

                if(charactersStats != null)
                {
                    Vector3 targetsDirection = charactersStats.transform.position - enemyManager.transform.position;
                    float viewableAngle = Vector3.Angle(targetsDirection, enemyManager.transform.forward);

                    if(viewableAngle > enemyManager.minimumDetectionAngle &&
                        viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = charactersStats;
                        isSleeping = false;
                        enemyAnimatorHandler.PlayTargetAnimation(wakeAnimation, true);
                    }
                }
            }
            #endregion

            #region Handle State Change
            if(enemyManager.currentTarget != null)
            {
                return pursueTargetState;
            }

            else
            {
                return this;
            }
            #endregion
        }
    }
}
