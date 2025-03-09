using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class OpenChest : Interactable
    {
        Animator animator;
        OpenChest openChest;

        public Transform playerStandingPoint;
        public Transform itemStandPoint;

        public GameObject itemSpawner;
        public WeaponItem itemInChest;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            openChest = GetComponent<OpenChest>();
        }

        public override void Interact(PlayerManager _playerManager)
        {
            /*
            // Rotate the player towards the chest
            #region Rotate the player towards the chest
            Vector3 rotationDirection = transform.position - _playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();
            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(_playerManager.transform.rotation, tr, 300 * Time.deltaTime);
            _playerManager.transform.rotation = targetRotation;
            #endregion
            */

            // Move the player in front of the chest
            //#region Lock the player's transform to a point in front of the chest
            //_playerManager.OpenChestInteraction(playerStandingPoint);
            //#endregion
            
            // Open the chest lid and animate the player
            animator.Play("Chest_Open");

            // Spawn an item inside the chest that the player can pick up
            StartCoroutine(SpawnItemInChest());

            WeaponPickup weaponPickup = itemSpawner.GetComponent<WeaponPickup>();

            if(weaponPickup != null)
            {
                weaponPickup.weapon = itemInChest;
            }
                
        }

        private IEnumerator SpawnItemInChest()
        {
            yield return new WaitForSeconds(1f);
            Instantiate(itemSpawner, itemStandPoint.transform);
            Destroy(openChest);
        }
    }
}
