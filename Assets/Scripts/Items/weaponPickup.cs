using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class weaponPickup : Interactable
    {
        public WeaponItem weapon;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventory playerInventory;
            PlayerLocomotion playerLocomotion;
            AnimatorHandler animatorHandler;

            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            animatorHandler = playerManager.GetComponentInChildren<AnimatorHandler>();

            playerLocomotion.rigidbody.velocity = Vector3.zero; // Stops the player from moving while pickup up an item
            animatorHandler.PlayTargetAnimation("Pickup", true);    //  Plays the animation of looting the item
            playerInventory.weaponsInventory.Add(weapon);
            Destroy(gameObject);
        }
    }
}

