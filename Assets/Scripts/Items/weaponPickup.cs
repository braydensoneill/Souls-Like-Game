using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BON
{
    public class WeaponPickup : Interactable
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
            PlayerAnimatorHandler animatorHandler;

            playerInventory = playerManager.GetComponent<PlayerInventory>();
            playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            animatorHandler = playerManager.GetComponentInChildren<PlayerAnimatorHandler>();

            //playerLocomotion.rigidbody.linearVelocity = Vector3.zero; // Stops the player from moving while pickup up an item
            //animatorHandler.PlayTargetAnimation("Pickup", true);    //  Plays the animation of looting the item
            playerInventory.weaponsInventory.Add(weapon);

            playerManager.itemPopUp.GetComponentInChildren<TextMeshProUGUI>().text = weapon.itemName;
            playerManager.itemPopUp.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
            playerManager.itemPopUp.gameObject.SetActive(true);
            Debug.Log("Item found: " + weapon.itemName);
            Destroy(gameObject);
        }
    }
}

