using System;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How many bullets per second you can fire")]
    private float fireRate = 5f;

    [SerializeField]
    [Tooltip("Visible name")]
    private string gunName = "?";

    [SerializeField]
    private int damage = 25;

    [SerializeField]
    private Material penetrationMaterial;

    [SerializeField]
    private AudioClip fireSound;

    private float lastShootTime;

    private AudioSource audioSource;

    public string GunName => gunName;

    public void TryShoot(DestructibleObstacle obstacle)
    {
        float currentTime = Time.time;
        if (currentTime - lastShootTime >= 1f / fireRate)
        {
            lastShootTime = currentTime;
            audioSource.Play();

            if (obstacle != null)
            {
                if (penetrationMaterial.HasFlag(obstacle.Material))
                {
                    obstacle.TakeDamage(damage);
                }
                else
                {
                    Debug.Log("Can't damage this material with this gun");
                }
            }
        }
    }

    public string GetMaterialString()
    {
        StringBuilder result = new StringBuilder();
        Material[] materials = (Material[])Enum.GetValues(typeof(Material));

        foreach (Material material in materials)
        {
            if (material == Material.None)
            {
                continue;
            }

            if (penetrationMaterial.HasFlag(material))
            {
                if (result.Length > 0)
                {
                    result.Append(", ");
                }

                result.Append(material.ToString());
            }
        }

        return result.ToString();
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = fireSound;
    }
}
