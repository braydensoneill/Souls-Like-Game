using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class PlayerAttacker : MonoBehaviour
    {
        private AnimatorHandler animatorHandler;
        private InputHandler inputHandler;
        public string lastAttack;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            inputHandler = GetComponent<InputHandler>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if(inputHandler.flag_Combo)
            {
                animatorHandler.animator.SetBool("canDoCombo", false);

                if (lastAttack == weapon.OH_Right_Sword_Attack_01)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Right_Sword_Attack_02, true);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            animatorHandler.PlayTargetAnimation(weapon.OH_Right_Sword_Attack_01, true);
            lastAttack = weapon.OH_Right_Sword_Attack_01;
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_01, true);
            lastAttack = weapon.OH_Right_Sword_Attack_01;
        }
    }
}
