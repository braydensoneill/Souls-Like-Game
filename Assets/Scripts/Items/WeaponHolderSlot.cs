using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parentOverride;
        public WeaponItem currentWeapon;
        public bool isLeftHandSlot;
        public bool isRightHandSlot;
        #region Create sheath WeaponSlot variables
        public bool isSheathHeavy01;
        public bool isSheathHeavy02;
        public bool isSheathLightRight01;
        public bool isSheathLightRight02;
        public bool isSheathLightLeft01;
        public bool isSheathLightLeft02;
        public bool isSheathShield01;
        public bool isSheathShield02;
        public bool isSheathBow01;
        public bool isSheathBow02;
        public bool isBackSlot;
        #endregion
        public GameObject currentWeaponModel;

        public void UnloadWeapon()
        {
            if(currentWeaponModel != null)
            {
                currentWeaponModel.SetActive(false);
            }
        }

        public void UnloadWeaponAndDestroy()
        {
            if(currentWeaponModel != null)
            {
                Destroy(currentWeaponModel);
            }
        }

        public void LoadWeaponModel(WeaponItem weaponItem)
        {
            UnloadWeaponAndDestroy();

            if (weaponItem == null)
            {
                UnloadWeapon();
                return;
            }

            GameObject _model = Instantiate(weaponItem.modelPrefab);
            if (_model != null)
            {
                if(parentOverride != null)
                {
                    _model.transform.parent = parentOverride;
                }
                else
                {
                    _model.transform.parent = transform;
                }

                _model.transform.localPosition = Vector3.zero;
                _model.transform.localRotation = Quaternion.identity;
                _model.transform.localScale = Vector3.one;
            }

            currentWeaponModel = _model;
        }
    }
}
