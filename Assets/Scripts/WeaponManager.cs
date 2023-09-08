using System;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private Gun[] weapons;

    [SerializeField]
    private int defaultWeaponIndex = 0;

    private int currentWeaponIndex;

    public event EventHandler<Gun> OnWeaponSwitched;

    public Gun CurrentWeapon => weapons[currentWeaponIndex];

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
        OnWeaponSwitched?.Invoke(this, CurrentWeapon);

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(currentWeaponIndex == i);
        }
    }

    public void TryShoot(DestructibleObstacle obstacle)
    {
        CurrentWeapon.TryShoot(obstacle);
    }

    private void Start()
    {
        SetWeapon(defaultWeaponIndex);
    }
}
