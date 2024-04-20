using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] TMP_Text currentAmmoText;
    [SerializeField] TMP_Text maxAmmoText;
    [SerializeField] Image screenFlash;

    FPSController player;

    Color redTint = new Color(255, 0, 0, .7f);
    Color targetColor = new Color(255, 0, 0, 0);
    float flashDuration = 1.4f;

    Gun[] currentGuns;
    private void OnEnable()
    {
        if (FindObjectsOfType<Gun>() != null)
        {
            currentGuns = FindObjectsOfType<Gun>();
            foreach (Gun gun in currentGuns)
            {
                gun.Fired += ChangeAmmoText;
                gun.Reload += ChangeAmmoText;
            }
        }
    }

    void DecreaseHealth(float health)
    {
        healthBar.fillAmount = health;
        screenFlash.color = redTint;
    }

    void HealthSet(float value)
    {
        healthBar.fillAmount = value;
    }

    // Start is called before the first frame update
    void Start()
    {

        if (FindObjectOfType<FPSController>() && player == null)
            player = FindObjectOfType<FPSController>().GetComponent<FPSController>();
        player.WeaponSwapID += SwapChangeTexts;
        player.Damaged += DecreaseHealth;
        player.HealthLoaded += HealthSet;
        HealthSet(player.health);
    }

    void ChangeAmmoText(Gun gun)
    {
        int value = gun.ammo;
        if (value > 9)
            currentAmmoText.text = value.ToString();
        else
            currentAmmoText.text = "0" + value.ToString();
    }

    void SwapChangeMaxAmmoText(Gun gun)
    {
        int value = gun.maxAmmo;
        if (value > 9)
            maxAmmoText.text = value.ToString();
        else
            maxAmmoText.text = "0" + value.ToString();
    }

    void SwapChangeTexts(Gun gun)
    {
        ChangeAmmoText(gun);
        SwapChangeMaxAmmoText(gun);
    }

    private void Update()
    {
        var currentColor = screenFlash.color;
        if (currentColor != targetColor)
        {
            currentColor = Color.LerpUnclamped(currentColor, targetColor, flashDuration * Time.deltaTime);
            screenFlash.color = currentColor;
        }
    }
}
