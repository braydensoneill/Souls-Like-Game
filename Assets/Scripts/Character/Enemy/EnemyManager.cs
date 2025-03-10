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

        [Header("Combat Flags")]
        public bool canDoCombo;

        [Header("A.I Settings")]
        public float detectionRadius = 20;
        public float minimumDetectionAngle = -50;   // Enemy min field of view angle
        public float maximumDetectionAngle = 50;    // Enemy max field of view angle

        public float currentRecoveryTime = 0;

        protected override void Awake()
        {
            base.Awake(); // Calls the character maanger awake function
            enemyLocomotion = GetComponent<EnemyLocomotion>();
            enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
            enemyStats = GetComponent<EnemyStats>();
            navmeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyRigidbody = GetComponent<Rigidbody>();
            backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
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

            HandleStateMachine();   // This needs to go in between the isinteracting and candocombo variables for some reason

            canDoCombo = enemyAnimatorHandler.animator.GetBool("canDoCombo");
            enemyAnimatorHandler.animator.SetBool("isDead", enemyStats.isDead);

            HandleDeathState();
        }

        private void LateUpdate()
        {
            // This is a bug fix to stop the enemy from often spinning uncontrollably
            navmeshAgent.transform.localPosition = Vector3.zero;
            navmeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleStateMachine()
        {
            if (enemyStats.isDead)
                return;

            if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimatorHandler);

                if (nextState != null)
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
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isInteracting)
            {
                if (currentRecoveryTime <= 0)
                {
                    isInteracting = false;
                }
            }
        }

        public override void HandleDeathState()
        {
            base.HandleDeathState();

            // Handle death-related actions (like giving gold)
            enemyAnimatorHandler.AwardGoldOnDeath();
        }
    }
}