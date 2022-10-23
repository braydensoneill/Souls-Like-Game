using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        public NavMeshAgent navmeshAgent;
        public Rigidbody enemyRigidbody;

        [Header("Enemy Flags ")]
        public bool isInteracting;

        [Header("Movement Settings")]
        public float rotationSpeed = 15;
        public float maximumAttackRange = 2;

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
            navmeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRigidbody = GetComponent<Rigidbody>();
            backStabCollider = GetComponentInChildren<BackStabCollider>();
        }

        private void Start()
        {
            navmeshAgent.enabled = false;
            enemyRigidbody.isKinematic = false;
        }

        private void Update()
        {
            HandleRecoveryTime();

            isInteracting = enemyAnimatorHandler.animator.GetBool("isInteracting");
            enemyAnimatorHandler.animator.SetBool("isDead", enemyStats.isDead);
        }

        private void FixedUpdate()
        {
            HandleStateMachine();
        }

        private void HandleStateMachine()
        {
            if (enemyStats.isDead)
                return;

            if (currentState != null)
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
    }
}