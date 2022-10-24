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
        private PlayerWeaponSlotManager playerWeaponSlotManager;

        public string lastAttack;
        private LayerMask backStabLayer = 1 << 13;

        private void Awake()
        {
            playerAnimatorHandler = GetComponent<PlayerAnimatorHandler>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerStats = GetComponentInParent<PlayerStats>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            // Unless the player has insufficient stamina
            if (playerStats.stamina_Current <= 0)
                return;

            if (inputHandler.flag_Combo)
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
            // Unless the player has insufficient stamina
            if (playerStats.stamina_Current <= 0)
                return;

            playerWeaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.flag_TwoHand)
            {
                playerAnimatorHandler.PlayTargetAnimation(weapon.TH_Sword_Light_Attack_01, true);
                lastAttack = weapon.TH_Sword_Light_Attack_01;
            }

            else
            {
                playerWeaponSlotManager.attackingWeapon = weapon;
                playerAnimatorHandler.PlayTargetAnimation(weapon.OH_Sword_Light_Attack_Right_01, true);
                lastAttack = weapon.OH_Sword_Light_Attack_Right_01;
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            // Unless the player has insufficient stamina
            if (playerStats.stamina_Current <= 0)
                return;

            playerWeaponSlotManager.attackingWeapon = weapon;

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
        public void HandleRBAction()
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

        public void AttemptBackStabOrRiposte()
        {
            // Unless the player has insufficient stamina
            if (playerStats.stamina_Current <= 0)
                return;

            RaycastHit hit;

            if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, 
                transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.transform.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

                if(enemyCharacterManager != null)
                {
                    // check for team id (avoid friendly fire)

                    // pull the character into a transform behind the enemy so the animation looks better
                    playerManager.transform.position = enemyCharacterManager.backStabCollider.backStabberStandPoint.position;

                    // rotate towards the transform
                    #region Rotate Towards the target transform
                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;
                    #endregion

                    // Handle Critical Damage
                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.CurrentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    // play the animation
                    playerAnimatorHandler.PlayTargetAnimation("Backstab_Stab", true);

                    // make the enemy play an animation
                    enemyCharacterManager.GetComponentInChildren<CharacterAnimatorHandler>().PlayTargetAnimation("Backstab_Stabbed", true);
                    
                    // do damage
                }
            }
        }
    }
}
