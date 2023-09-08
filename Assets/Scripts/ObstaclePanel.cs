using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObstaclePanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI materialText;

    [SerializeField]
    private TextMeshProUGUI healthText;

    [SerializeField]
    private PlayerController playerController;

    private DestructibleObstacle currentObstacle;

    private void OnEnable()
    {
        playerController.onObstacleChanged += OnObstacleChanged;
    }

    private void OnDisable()
    {
        playerController.onObstacleChanged -= OnObstacleChanged;
    }

    private void ChangeObstacle(DestructibleObstacle obstacle)
    {
        UnsubscribeFromEvent();
        currentObstacle = obstacle;
        SubscribeToHpChange();
        UpdateText(obstacle);
    }

    private void SubscribeToHpChange()
    {
        if (currentObstacle != null)
        {
            currentObstacle.onHealthChanged += OnHpChanged;
        }
    }

    private void UnsubscribeFromEvent()
    {
        if (currentObstacle != null)
        {
            currentObstacle.onHealthChanged -= OnHpChanged;
        }
    }

    private void UpdateText(DestructibleObstacle obstacle)
    {
        if (obstacle == null || obstacle.Health <= 0)
        {
            materialText.text = "Material: -";
            healthText.text = "Health: -";
            return;
        }

        materialText.text = $"Material: {obstacle.Material}";
        healthText.text = $"Health: {obstacle.Health}/{obstacle.MaxHealth}";
    }

    private void OnObstacleChanged(object sender, DestructibleObstacle e)
    {
        ChangeObstacle(e);
    }

    private void OnHpChanged(object sender, int e)
    {
        UpdateText(currentObstacle);
    }
}
