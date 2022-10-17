using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class PlayerAttacker : MonoBehaviour
    {
        private AnimatorHandler animatorHandler;
        private InputHandler inputHandler;
        private WeaponSlotManager weaponSlotManager;
        public string lastAttack;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            inputHandler = GetComponent<InputHandler>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if(inputHandler.flag_Combo)
            {
                animatorHandler.animator.SetBool("canDoCombo", false);

                if (lastAttack == weapon.OH_Sword_Light_Attack_Right_01)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Sword_Light_Attack_Right_02, true);
                }

                else if(lastAttack == weapon.TH_Sword_Light_Attack_01)
                {
                    animatorHandler.PlayTargetAnimation(weapon.TH_Sword_Light_Attack_02, true);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.flag_TwoHand)
            {
                animatorHandler.PlayTargetAnimation(weapon.TH_Sword_Light_Attack_01, true);
                lastAttack = weapon.TH_Sword_Light_Attack_01;
            }

            else
            {
                weaponSlotManager.attackingWeapon = weapon;
                animatorHandler.PlayTargetAnimation(weapon.OH_Sword_Light_Attack_Right_01, true);
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
                animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_01, true);
                lastAttack = weapon.OH_Sword_Light_Attack_Right_01;
            }
        }
    }
}
