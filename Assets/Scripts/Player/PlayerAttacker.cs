using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class PlayerAttacker : MonoBehaviour
    {
        AnimatorHandler animatorHandler;
        InputHandler inputHandler;
        public string lastAttack;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            inputHandler = GetComponent<InputHandler>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if(inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);

                if (lastAttack == weapon.oh_right_sword_attack_01)
                {
                    animatorHandler.PlayTargetAnimation(weapon.oh_right_sword_attack_02, true);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            animatorHandler.PlayTargetAnimation(weapon.oh_right_sword_attack_01, true);
            lastAttack = weapon.oh_right_sword_attack_01;
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            animatorHandler.PlayTargetAnimation(weapon.oh_heavy_attack_01, true);
            lastAttack = weapon.oh_right_sword_attack_01;
        }
    }
}
