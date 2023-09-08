using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField]
    private Gun[] weapons;

    [SerializeField]
    private int defaultWeaponIndex = 0;

    private void Start()
    {
        SetWeaponModel(defaultWeaponIndex);
    }

    public void SetWeaponModel(int index)
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

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(index == i);
        }
    }
}
