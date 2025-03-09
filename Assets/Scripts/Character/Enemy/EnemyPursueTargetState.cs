using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class EnemyPursueTargetState : State
    {
        public EnemyCombatStanceState combatStanceState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            // Only allow the enemy to enter this state if they are not currently interacting
            if (enemyManager.isInteracting)
                return this;

            HandleRotationTowardsTarget(enemyManager);

            if (enemyManager.isInteracting)
            {
                enemyAnimatorHandler.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }
                
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position; // Look for the direction of the target
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward); // Enemy FOV

            if (distanceFromTarget > enemyManager.maximumAttackRange)
            {
                enemyAnimatorHandler.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            HandleRotationTowardsTarget(enemyManager);

            if(distanceFromTarget <= enemyManager.maximumAttackRange)
            {
                return combatStanceState;
            }
            
            else
            {
                return this;
            }
        }

        private void HandleRotationTowardsTarget(EnemyManager enemyManager)
        {
            // Rotate manually
            if (enemyManager.isInteracting)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }

            // Rotate with pathfinding (navmesh)
            else
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navmeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyManager.enemyRigidbody.linearVelocity;

                enemyManager.navmeshAgent.enabled = true;
                enemyManager.navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.enemyRigidbody.linearVelocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navmeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }
    }
}