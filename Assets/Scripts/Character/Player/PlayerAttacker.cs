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

        private float timeTo_HeavyAttack = 0.65f;

        private bool flag_HeavyAttack_Right;
        private bool flag_HeavyAttack_Left;

        private LayerMask backStabLayer = 1 << 13;
        private LayerMask parryLayer = 1 << 14;

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
        public void HandleRBAction(float _holdDuration = 0)
        {
            if(playerInventory.rightWeapon.isMeleeWeapon)
            {
                // perform a light attack if the timer ends between 0 and the decimal
                if (_holdDuration > 0 && _holdDuration < timeTo_HeavyAttack)
                {
                    flag_HeavyAttack_Right = false;
                    PerformRBMeleeAction(flag_HeavyAttack_Right);
                    inputHandler.timer_RB_Input = 0;
                }

                // perform a heavy attack if the timer is greater than the decimal
                else if (_holdDuration >= timeTo_HeavyAttack)
                {
                    flag_HeavyAttack_Right = true;
                    PerformRBMeleeAction(flag_HeavyAttack_Right);
                    inputHandler.timer_RB_Input = 0;
                }

                flag_HeavyAttack_Right = false;
            }

            if (playerInventory.rightWeapon.isShield)
            {
                PerformRBBlockAction();
            }

            if (playerInventory.rightWeapon.isSpellCaster ||
                playerInventory.rightWeapon.isFaithCaster ||
                playerInventory.rightWeapon.isPyroCaster)
            {
                PerformRBMagicAction(playerInventory.rightWeapon);
            }
        }

        public void HandleLBAction(float _holdDuration = 0)
        {
            if (playerInventory.leftWeapon.isMeleeWeapon)
            {
                // the decimal represents the delay holding down the input
                if (_holdDuration > 0 && _holdDuration < timeTo_HeavyAttack)
                {
                    flag_HeavyAttack_Left = false;
                    // PerformLBMeleeAction(inputHandler.flag_HeavyAttack_Left, _delta);
                }

                else if (_holdDuration >= timeTo_HeavyAttack)
                {
                    flag_HeavyAttack_Left = true;
                    // PerformLBMeleeAction(inputHandler.flag_HeavyAttack_Left, _delta);
                }

                flag_HeavyAttack_Left = false;
            }

            if (playerInventory.leftWeapon.isShield)
            {
                PerformLBBlockAction();
            }

            if (playerInventory.leftWeapon.isSpellCaster ||
                playerInventory.leftWeapon.isFaithCaster ||
                playerInventory.leftWeapon.isPyroCaster)
            {
                // PerformLBMagicAction(playerInventory.leftWeapon);
            }
        }

        public void HandleLTAction()
        {
            if(playerInventory.leftWeapon.isShield)
            {
                // Perform shield weapon art
                PerformLTWeaponArt(inputHandler.flag_TwoHand);
            }

            else if (playerInventory.leftWeapon.isMeleeWeapon)
            {
                // do a light attack (dunno where this came from)
            }
        }
        #endregion

        #region Attack Actions
        private void PerformRBMeleeAction(bool _isHeavyAttack)
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

                if (_isHeavyAttack)
                    Debug.Log("Performed heavy attack");

                else
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

        private void PerformLTWeaponArt(bool isTwoHanding)
        {
            if (playerManager.isInteracting)
                return;


            if(isTwoHanding)
            {
                // if we are two handing perform weapon art for right weapon
                
            }

            else
            {
                // else perform weapon art for left handed weapon
                playerAnimatorHandler.PlayTargetAnimation(playerInventory.leftWeapon.weapon_art, true);
            }
        }

        private void SuccessfullyCastSpell() // This function only calls a function in another script. It is also here so it can be called in an animation event()
        {
            playerInventory.currentSpell.SuccessfullyCastSpell(playerAnimatorHandler, playerStats);
        }
        #endregion

        #region Defence Actions
        private void PerformRBBlockAction()
        {
            // Ignore this if the player is interacting or already blocking
            if (playerManager.isInteracting || playerManager.isBlocking)
                return;

            // Add functionality for if the shield is in the left hand here
            playerAnimatorHandler.PlayTargetAnimation("Block_Shield_Right_Start", false, true);

            playerManager.isBlockingRightHand = true;
        }

        private void PerformLBBlockAction()
        {
            // Ignore this if the player is interacting or already blocking
            if (playerManager.isInteracting || playerManager.isBlocking)
                return;

            // Add functionality for if the shield is in the right hand here
            playerAnimatorHandler.PlayTargetAnimation("Block_Shield_Left_Start", false, true);

            playerManager.isBlockingLeftHand = true;
        }
        #endregion

        public void AttemptBackStabOrParry()
        {
            // Unless the player has insufficient stamina
            if (playerStats.stamina_Current <= 0)
                return;

            RaycastHit hit;

            if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
            {
                EnemyManager enemyManager = hit.transform.gameObject.GetComponentInParent<EnemyManager>();
                DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

                if(enemyManager != null)
                {
                    // check for team id (avoid friendly fire)

                    // pull the character into a transform behind the enemy so the animation looks better
                    playerManager.transform.position = enemyManager.backStabCollider.critialDamageStandPosition.position;

                    // rotate towards the transform
                    #region Rotate towards the target transform
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
                    enemyManager.pendingCriticalDamage = criticalDamage;

                    // play the animation
                    playerAnimatorHandler.PlayTargetAnimation("Backstab_Stab", true);
                    
                    // make the enemy play an animation
                    enemyManager.GetComponentInChildren<EnemyAnimatorHandler>().PlayTargetAnimation("Backstab_Stabbed", true);
                    
                    // do damage
                }
            }

            else if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.7f, parryLayer))
            {
                EnemyManager enemyManager = hit.transform.gameObject.GetComponentInParent<EnemyManager>();
                DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

                if (enemyManager != null && enemyManager.canBeRiposted)
                {
                    // check for team id (avoid friendly fire)

                    // pull the character into a transform in front of the enemy so the animation looks better
                    playerManager.transform.position = enemyManager.parryCollider.critialDamageStandPosition.position;

                    // rotate towards the transform
                    #region Rotate towards the target transform
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
                    enemyManager.pendingCriticalDamage = criticalDamage;

                    // play the animation
                    playerAnimatorHandler.PlayTargetAnimation("Parry_Stab", true);

                    // make the enemy play an animation
                    enemyManager.GetComponentInChildren<EnemyAnimatorHandler>().PlayTargetAnimation("Parry_Stabbed", true);

                    // do damage
                }
            }
        }
    }
}
