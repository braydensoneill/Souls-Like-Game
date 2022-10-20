using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    [CreateAssetMenu(menuName = "AI/ Enemy Actions/ Attack Action")]
    public class EnemyAttackAction : EnemyAction
    {
        public int attackScore = 3; // Liklihood of attack to occur
        public float recoveryTime = 2;  // Time to recover after attack is thrown

        public float maximumAttackAngle = 35;   // Minimum angle player must be for enemy to throw attack
        public float minimumAttackAngle = -35;  // Maximum angle player must be for enemy to throw attack

        public float minimumDistanceNeededToAttack = 0; // Minimum distance to attack
        public float maximumDistanceNeededToAttack = 3; // Maximum distance to attack
    

    }
}
