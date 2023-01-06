using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class HandleRagdollLimbCollision : MonoBehaviour
    {
        private CharacterStats characterStats;
        private Rigidbody limbRigidBody;

        private void Awake()
        {
            characterStats = GetComponentInParent<CharacterStats>();
        }

        // Start is called before the first frame update
        void Start()
        {
            limbRigidBody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(characterStats.isDead)
            {
                Debug.Log(collision.gameObject.name + " collided with " + gameObject.name);
                limbRigidBody.AddForce(collision.impulse, ForceMode.Impulse);
            }
        }
    }
}