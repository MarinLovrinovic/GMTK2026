using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float timeUntilRecovery = 20f;
    [SerializeField] private RectTransform healthIndicator;
    [SerializeField] private RectTransform regenerationIndicator;
    [SerializeField] private RectTransform healthPointIndicatorPrefab;
    [SerializeField] private float indicatorHeightFull = 20;
    [SerializeField] private float indicatorHeightEmpty = 5;
    [SerializeField] private AnimationCurve regenerationAnimationCurve;
    private readonly List<RectTransform> healthPointIndicators = new();
    private readonly List<float> healthPointIndicatorTargetHeights = new();
    
    private int currentHealth;
    
    private float currentTimeUntilRecovery = 0f;

    void Start()
    {
        currentHealth = maxHealth;
        for (int i = 0; i < maxHealth; i++)
        {
            RectTransform healthPointIndicator = Instantiate(healthPointIndicatorPrefab, healthIndicator);
            healthPointIndicator.anchoredPosition = new Vector2(10 + i * 60, -10);
            healthPointIndicators.Add(healthPointIndicator);
            healthPointIndicatorTargetHeights.Add(indicatorHeightFull);
        }
        regenerationIndicator.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
        ResetRegenerationIndicator();
    }

    private void Update()
    {
        UpdateHealthIndicators();
        if (currentTimeUntilRecovery > 0f)
        {
            currentTimeUntilRecovery -= Time.deltaTime;
            if (currentTimeUntilRecovery <= 0f)
            {
                currentTimeUntilRecovery = 0f;
                ResetHealth();
                ResetRegenerationIndicator();
            }
            else
            {
                UpdateRegenerationIndicator();
            }
        }
    }

    public void DamageTaken(int damage)
    {
        if (damage <= 0) return;
        
        bool gameLost = false;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            gameLost = true;
        }
        
        for (int i = currentHealth; currentHealth - damage < i; i--)
        {
            healthPointIndicatorTargetHeights[i] = indicatorHeightEmpty;
        }

        if (gameLost)
        {
            GameLost();
        }
        currentTimeUntilRecovery = timeUntilRecovery;
    }

    
    private void ResetHealth()
    {
        currentHealth = maxHealth;
        for (int i = 0; i < maxHealth; i++)
        {
            healthPointIndicatorTargetHeights[i] = indicatorHeightFull;
        }
    }

    private void UpdateHealthIndicators()
    {
        for (int i = 0; i < maxHealth; i++)
        {
            RectTransform healthPointIndicator = healthPointIndicators[i];
            Vector2 indicatorSize = healthPointIndicator.sizeDelta;
            indicatorSize += (healthPointIndicatorTargetHeights[i] - indicatorSize.y) * Time.deltaTime * 2f * Vector2.up;
            healthPointIndicator.sizeDelta = indicatorSize; 
        }
    }
    
    private void UpdateRegenerationIndicator()
    {
        float recoveryPercentage = Mathf.InverseLerp(timeUntilRecovery, 0f, currentTimeUntilRecovery);
        float indicatorWidth = Mathf.Lerp(0f, 60 * maxHealth - 10, recoveryPercentage);
        regenerationIndicator.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, indicatorWidth);
    }

    private void ResetRegenerationIndicator()
    {
        regenerationIndicator.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
    }
    
    private void GameLost()
    {
        Debug.Log("game lost!");
    }
}
