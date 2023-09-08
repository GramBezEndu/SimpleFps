using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DestructibleObstacle : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;

    [SerializeField]
    private Material material;

    private int health;

    public Material Material => material;

    public int Health => health;

    public int MaxHealth => maxHealth;

    public event EventHandler<int> onHealthChanged;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage()
    {
        health -= 25;
        onHealthChanged?.Invoke(this, health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}

[Flags]
public enum Material
{
    None = 0,
    Metal = 1,
    Wood = 2,
    Plastic = 4,
}
