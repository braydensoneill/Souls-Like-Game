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

        [Header("Character Collision")]
        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlockerCollider;

        public LayerMask detectionLayer;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
        }

        private void Start()
        {
            Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
        }
    }
}