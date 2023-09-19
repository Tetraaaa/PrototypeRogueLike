using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public HealthBar healthBar;

    public void Init()
    {
        healthBar.Init();
    }

    public void UpdatePlayerHealth()
    {
        healthBar.UpdatePlayerHealth();
    }
}
