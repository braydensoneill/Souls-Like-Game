using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BON
{
    public class EnemyLocomotion : MonoBehaviour
    {
        private EnemyManager enemyManager;
        private EnemyAnimatorHandler enemyAnimatorHandler;
        private NavMeshAgent navmeshAgent;
        public Rigidbody enemyRigidbody;

        public LayerMask detectionLayer;

        public float distanceFromTarget;    // How far from target
        public float stoppingDistance = 2f;  // Distance to stop in front of target

        public float rotationSpeed = 15;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
            navmeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            navmeshAgent.enabled = false;
            enemyRigidbody.isKinematic = false;
        }

        public void HandleMoveToTarget()
        {
            if (enemyManager.isInteracting)
                return;

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position; // Look for the direction of the target
            distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward); // Enemy FOV

            // If performing an action, stop movement
            if (enemyManager.isInteracting)
            {
                enemyAnimatorHandler.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                navmeshAgent.enabled = false;
            }

            else
            {
                if (distanceFromTarget > stoppingDistance)
                {
                    enemyAnimatorHandler.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
                }

                // If close enought to the target, then attack
                else if(distanceFromTarget <= stoppingDistance)
                {
                    enemyAnimatorHandler.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                }
            }

            HandleRotationTowardsTarget();
            navmeshAgent.transform.localPosition = Vector3.zero;
            navmeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleRotationTowardsTarget()
        {
            // Rotate manually
            if(enemyManager.isInteracting)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if(direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed / Time.deltaTime);
            }

            // Rotate with pathfinding (navmesh)
            else
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(navmeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyRigidbody.velocity;

                navmeshAgent.enabled = true;
                navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);

                enemyRigidbody.velocity = targetVelocity;
                transform.rotation = Quaternion.Slerp(transform.rotation, navmeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);
            }
        }
    }
}