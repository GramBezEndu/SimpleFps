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

    private void OnEnable()
    {
        weaponManager.onWeaponSwitched += UpdateGunText;
    }

    private void OnDisable()
    {
        weaponManager.onWeaponSwitched -= UpdateGunText;
    }

    private void UpdateGunText(object sender, Gun newGun)
    {
        weaponText.text = $"Weapon: {newGun.GunName}";
        breaksText.text = $"Breaks: {newGun.GetMaterialString()}";
    }
}
