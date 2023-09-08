using System;
using UnityEngine;
using UnityEngine.Events;

public class DestructibleObstacle : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;

    [SerializeField]
    private Material material;

    private int health;

    public event EventHandler OnObjectDestroyed;

    public event EventHandler<int> OnHealthChanged;

    public Material Material => material;

    public int Health => health;

    public int MaxHealth => maxHealth;

    public void TakeDamage(int damage)
    {
        health -= damage;
        OnHealthChanged?.Invoke(this, health);
        if (health <= 0)
        {
            OnObjectDestroyed?.Invoke(this, new EventArgs());
            Debug.Log("On Object Destroyed event called");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        health = maxHealth;
    }
}
