using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class EnemyManager : CharacterManager
    {
        private EnemyLocomotion enemyLocomotion;
        private EnemyAnimatorHandler enemyAnimatorHandler;
        private EnemyStats enemyStats;

        [Header("General")]
        public State currentState;
        public CharacterStats currentTarget;

        [Header("Enemy Flags ")]
        public bool isInteracting;

        [Header("A.I Settings")]
        public float detectionRadius = 20;
        public float minimumDetectionAngle = -50;   // Enemy min field of view angle
        public float maximumDetectionAngle = 50;    // Enemy max field of view angle

        public float currentRecoveryTime = 0;

        private void Awake()
        {
            enemyLocomotion = GetComponent<EnemyLocomotion>();
            enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
            enemyStats = GetComponent<EnemyStats>();
        }

        private void Update()
        {
            HandleRecoveryTime();
        }

        private void FixedUpdate()
        {
            HandleStateMachine();
        }

        private void HandleStateMachine()
        {
            if(currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimatorHandler);

                if(nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(State _state)
        {
            currentState = _state;
        }

        private void HandleRecoveryTime()
        {
            if(currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if(isInteracting)
            {
                if(currentRecoveryTime <= 0)
                {
                    isInteracting = false;
                }
            }
        }

        #region Attacks
        private void AttackTarget()
        {
            /*
            if (isInteracting)
                return;

            if(currentAttack == null)
            {
                GetNewAttack();
            }

            else
            {
                isInteracting = true;
                currentRecoveryTime = currentAttack.recoveryTime;
                enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
                currentAttack = null;
            }
            */
        }

        private void GetNewAttack()
        {
        /*
            Vector3 targetDirection = enemyLocomotion.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);
            enemyLocomotion.distanceFromTarget = Vector3.Distance(enemyLocomotion.currentTarget.transform.position, transform.position);

            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if(enemyLocomotion.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack &&
                    enemyLocomotion.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if(viewableAngle <= enemyAttackAction.maximumAttackAngle &&
                        viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int tempScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (enemyLocomotion.distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack &&
                    enemyLocomotion.distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle &&
                        viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (currentAttack != null)
                            return;

                        tempScore += enemyAttackAction.attackScore;

                        if(tempScore > randomValue)
                        {
                            currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        */
        }
        #endregion
    }
}