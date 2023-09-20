using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public HealthBar healthBar;
    public GameObject shopPanel;
    public GameObject perksContainer;
    public GameObject PerkPrefab;

    public void Init()
    {
        healthBar.Init();
    }

    public void UpdatePlayerHealth()
    {
        healthBar.UpdatePlayerHealth();
    }

    public void OpenPerksMenu()
    {
        shopPanel.SetActive(true);
        GameObject PerkCard = Instantiate(PerkPrefab, perksContainer.transform);
        PerkCard.GetComponent<PerkCard>().ShowOnScreen(new EnergyDrinkPerk(GameManager.Instance.Player));
    }
}
