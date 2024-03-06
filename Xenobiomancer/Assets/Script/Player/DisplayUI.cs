using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private GameObject DeathScreen;
    [SerializeField] private Image healthBar;


    private void Start()
    {
        DisableDeathScreen();
    }

    public void UpdateHealthUI(float health, float maxHealth)
    {
        healthText.text = $"{health}/{maxHealth}";
        healthBar.fillAmount = health/maxHealth;

    }

    public void UpdateCurrencyUI(float currency)
    {
        currencyText.text = $"{currency}";
    }

    public void SetUI(float health, float maxHealth, float currency)
    {
        healthText.text = $"{health}/{maxHealth}";
        healthBar.fillAmount = health / maxHealth;
        currencyText.text = $"{currency}";
    }

    public void EnableDeathScreen()
    {
        DeathScreen.SetActive(true);
    }

    public void DisableDeathScreen()
    {
        DeathScreen.SetActive(false);
    }
}
