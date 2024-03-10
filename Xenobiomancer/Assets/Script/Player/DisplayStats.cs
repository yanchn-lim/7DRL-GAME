using Bioweapon;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStats : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private Image healthBar;

    [Header("bullet")]
    [SerializeField] private TextMeshProUGUI bulletAmountText;
    [SerializeField] private TextMeshProUGUI ammoLeftText;

    [Header("cost")]
    [SerializeField] private TextMeshProUGUI costText;

    [Header("GunImage")]
    [SerializeField] private Image gunImage;
    [SerializeField] private Sprite pistolImage;
    [SerializeField] private Sprite rifleImage;
    [SerializeField] private Sprite shotgunImage;
    [SerializeField] private Sprite LasergunImage;
    [SerializeField] private Sprite sniperImage;

    [Header("transfer area")]
    [SerializeField] private RectTransform circleForTransferShip;
    [SerializeField] private TextMeshProUGUI transferShipText;


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

    public void SetWeaponUI(int currentAmmo, int maxAmmo, int ammoLeft)
    {
        bulletAmountText.text = $"{currentAmmo}/{maxAmmo}";
        ammoLeftText.text = $"{ammoLeft}";
    }

    public void ChangeWeaponImageBasedOnWeaponType(GunType type)
    {
        Sprite spriteSelcted;
        switch(type)
        {
            case (GunType.Pistol): spriteSelcted = pistolImage;break;
            case(GunType.Sniper): spriteSelcted = sniperImage; break;
            case (GunType.Rifle): spriteSelcted = rifleImage; break;
            case (GunType.Laser): spriteSelcted = LasergunImage; break;
            case (GunType.ShotGun): spriteSelcted = shotgunImage; break;
            default: spriteSelcted = null; break;
        }
        gunImage.sprite = spriteSelcted;
        gunImage.preserveAspect = true;
    }

    public void ChangeCostText(int costOfWeapon)
    {
        costText.text = $"Q - Buy more ammo <b>({costOfWeapon})</b>" +
            $"\r\nH - Heal <b>(100)</b>";
    }


   

    public void ShowTransferShipText()
    {
        transferShipText.gameObject.SetActive(true);
    }

    public void HideTransferShipText()
    {
        transferShipText.gameObject.SetActive(false);
    }
}
