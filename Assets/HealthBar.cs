using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider healthSlider;

    void Start()
    {
        healthSlider = GetComponent<Slider>();

    }

    public void Init()
    {
        UpdatePlayerHealth();
    }

    public void UpdatePlayerHealth()
    {
        healthSlider.maxValue = GameManager.Instance.Player.maxHP;
        healthSlider.value = GameManager.Instance.Player.currentHp;
    }

}
