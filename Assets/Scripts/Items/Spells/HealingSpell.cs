using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    [CreateAssetMenu(menuName = "Spells/Healing Spell")]
    public class HealingSpell : Spell
    {
        public float healAmount;

        public override void AttemptoCastSpell(PlayerAnimatorHandler playerAnimatorHandler, PlayerStats playerStats)
        {
            base.AttemptoCastSpell(playerAnimatorHandler, playerStats);
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, playerAnimatorHandler.transform);
            playerAnimatorHandler.PlayTargetAnimation(spellAnimation, true);
            Debug.Log("Attempted to cast spell");
        }

        public override void SuccessfullyCastSpell(PlayerAnimatorHandler playerAnimatorHandler, PlayerStats playerStats)
        {
            base.SuccessfullyCastSpell(playerAnimatorHandler, playerStats);
            GameObject instantiatedSpellFX = Instantiate(spellCastFX, playerAnimatorHandler.transform);
            playerStats.RestoreHealth(healAmount);
            Debug.Log("Successfully casted spell");
        }
    }
}

