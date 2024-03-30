using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public WaveCounter waveCounter;
    public HealthBar healthBar;
    public GameObject shopPanel;
    public GameObject perksContainer;
    public GameObject PerkPrefab;

    private List<GameObject> perkCardsOnScreen = new List<GameObject>();

    public void Init()
    {
        healthBar.Init();
        waveCounter.Init();
    }

    public void UpdatePlayerHealth()
    {
        healthBar.UpdatePlayerHealth();
    }

    public void UpdateCurrentWave()
    {
        waveCounter.UpdateWaveCounter();
    }

    public void OpenPerksMenu(List<Perk> perks)
    {
        shopPanel.SetActive(true);
        foreach (Perk perk in perks)
        {
            GameObject PerkCard = Instantiate(PerkPrefab, perksContainer.transform);
            PerkCard.GetComponent<PerkCard>().ShowOnScreen(perk);
            perkCardsOnScreen.Add(PerkCard);
        }
    }

    public void ClosePerksMenu()
    {
        shopPanel.SetActive(false);
        foreach(GameObject perkCard in perkCardsOnScreen)
        {
            Destroy(perkCard);
        }
        UpdatePlayerHealth();
    }
}
