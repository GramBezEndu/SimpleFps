using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private Gun[] weapons;

    [SerializeField]
    private int defaultWeaponIndex = 0;

    private int currentWeaponIndex;

    public Gun CurrentWeapon => weapons[currentWeaponIndex];

    public event EventHandler<Gun> onWeaponSwitched;

    private void Start()
    {
        SetWeapon(defaultWeaponIndex);   
    }

    public void SetWeapon(int index)
    {
        if (weapons == null)
        {
            Debug.LogWarning("Weapons field is null");
            return;
        }

        if (index < 0 || index >= weapons.Length)
        {
            Debug.LogWarning("Invalid weapon index");
            return;
        }

        currentWeaponIndex = index;
        onWeaponSwitched?.Invoke(this, CurrentWeapon);

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(currentWeaponIndex == i);
        }
    }

    public void TryShoot(DestructibleObstacle obstacle)
    {
        CurrentWeapon.TryShoot(obstacle);
    }
}
