using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class Spell : MonoBehaviour
    {
        public GameObject spellWarmUpFX;
        public GameObject spellCastFX;
        public string spellAnimation;

        [Header("Spell Type")]
        public bool isFaithSpell;
        public bool isMagicSpell;
        public bool isPyroSpell;

        [Header("Spell Description")]
        [TextArea] public string spellDescription;

        public virtual void AttemptoCastSpell()
        {
            Debug.Log("You attempt to cast a spell!");
        }

        public virtual void SuccessfullyCastSpell()
        {
            Debug.Log("You successfully to cast a spell!");
        }
    }
}


