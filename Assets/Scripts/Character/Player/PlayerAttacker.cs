using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class PlayerAttacker : MonoBehaviour
    {
        private PlayerAnimatorHandler playerAnimatorHandler;
        private PlayerManager playerManager;
        private PlayerStats playerStats;
        private PlayerInventory playerInventory;
        private InputHandler inputHandler;
        private PlayerWeaponSlotManager weaponSlotManager;

        public string lastAttack;

        private void Awake()
        {
            playerAnimatorHandler = GetComponent<PlayerAnimatorHandler>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerStats = GetComponentInParent<PlayerStats>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            inputHandler = GetComponentInParent<InputHandler>();
            weaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if(inputHandler.flag_Combo)
            {
                playerAnimatorHandler.animator.SetBool("canDoCombo", false);

                if (lastAttack == weapon.OH_Sword_Light_Attack_Right_01)
                {
                    playerAnimatorHandler.PlayTargetAnimation(weapon.OH_Sword_Light_Attack_Right_02, true);
                }

                else if(lastAttack == weapon.TH_Sword_Light_Attack_01)
                {
                    playerAnimatorHandler.PlayTargetAnimation(weapon.TH_Sword_Light_Attack_02, true);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.flag_TwoHand)
            {
                playerAnimatorHandler.PlayTargetAnimation(weapon.TH_Sword_Light_Attack_01, true);
                lastAttack = weapon.TH_Sword_Light_Attack_01;
            }

            else
            {
                weaponSlotManager.attackingWeapon = weapon;
                playerAnimatorHandler.PlayTargetAnimation(weapon.OH_Sword_Light_Attack_Right_01, true);
                lastAttack = weapon.OH_Sword_Light_Attack_Right_01;
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.flag_TwoHand)
            {

            }

            else
            {
                playerAnimatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_01, true);
                lastAttack = weapon.OH_Sword_Light_Attack_Right_01;
            }
        }

        #region Input Actions
        public void HandleRTAction()
        {
            if(playerInventory.rightWeapon.isMeleeWeapon)
            {
                PerformRBMeleeAction();
            }

            else if (playerInventory.rightWeapon.isSpellCaster ||
                playerInventory.rightWeapon.isFaithCaster ||
                playerInventory.rightWeapon.isPyroCaster)
            {
                PerformRBMagicAction(playerInventory.rightWeapon);
            }
        }
        #endregion

        #region Attack Actions
        private void PerformRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.flag_Combo = true;
                HandleWeaponCombo(playerInventory.rightWeapon);
                inputHandler.flag_Combo = false;
            }

            else
            {
                if (playerManager.isInteracting)
                    return;

                if (playerManager.canDoCombo)
                    return;

                playerAnimatorHandler.animator.SetBool("isUsingRightHand", true);
                HandleLightAttack(playerInventory.rightWeapon);
            }
        }

        private void PerformRBMagicAction(WeaponItem _weapon)
        {
            // Don't allow to spam
            if (playerManager.isInteracting)
                return;

            // Check weapon type
            if(_weapon.isFaithCaster)
            {
                // Check if player has active spell and if it is a right weapon type
                if(playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
                {
                    // Check if the player has enough mana for the spell cost
                    if(playerStats.mana_Current >= playerInventory.currentSpell.manaCost)
                    {
                        // Cast the spell
                        playerInventory.currentSpell.AttemptoCastSpell(playerAnimatorHandler, playerStats);
                    }

                    else
                    {
                        playerAnimatorHandler.PlayTargetAnimation("Shrug", true);
                    }
                }
            }
        }

        private void SuccessfullyCastSpell() // This function only calls a function in another script. It is also here so it can be called in an animation event()
        {
            playerInventory.currentSpell.SuccessfullyCastSpell(playerAnimatorHandler, playerStats);
        }
        #endregion
    }
}
