using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponPanel : MonoBehaviour
{
    [SerializeField]
    private WeaponManager weaponManager;

    [SerializeField]
    private TextMeshProUGUI weaponText;

    [SerializeField]
    private TextMeshProUGUI breaksText;

    private Action<Gun> updateGunText;

    private void Awake()
    {
        updateGunText = (gun) => UpdateWeaponPanel(gun);
    }

    private void OnEnable()
    {
        weaponManager.onWeaponSwitched += updateGunText;
    }

    private void OnDisable()
    {
        weaponManager.onWeaponSwitched -= updateGunText;
    }

    private void UpdateWeaponPanel(Gun newGun)
    {
        weaponText.text = $"Weapon: {newGun.GunName}";
        breaksText.text = $"Breaks: {newGun.GetMaterialString()}";
    }
}
