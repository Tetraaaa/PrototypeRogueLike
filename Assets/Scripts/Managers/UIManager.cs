using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public WaveCounter waveCounter;
    public HealthBar healthBar;
    public LevelCounter levelCounter;
    public GameObject shopPanel;
    public GameObject perksContainer;
    public GameObject PerkPrefab;

    //Illustrations des cartes
    public Sprite commonCardImage;
    public Sprite uncommonCardImage;
    public Sprite mythicCardImage;
    public Sprite exoticCardImage;
    public Sprite corruptedCardImage;

    private List<GameObject> perkCardsOnScreen = new List<GameObject>();

    public void Init()
    {
        healthBar.Init();
        waveCounter.Init();
        levelCounter.Init();
    }

    public void UpdatePlayerHealth()
    {
        healthBar.UpdatePlayerHealth();
    }

    public void UpdateCurrentWave()
    {
        waveCounter.UpdateWaveCounter();
    }

    public void UpdatePlayerLevel()
    {
        levelCounter.UpdateLevelCounter();
    }

    public void OpenPerksMenu(List<Perk> perks)
    {
        shopPanel.SetActive(true);
        foreach (Perk perk in perks)
        {
            GameObject PerkCard = Instantiate(PerkPrefab, perksContainer.transform);
            var perkCardImage = PerkCard.GetComponent<Image>();
            switch (perk.Rarity)
            {
                case PerkRarity.Uncommon:
                    perkCardImage.sprite = uncommonCardImage;
                    break;
                case PerkRarity.Mythic:
                    perkCardImage.sprite= mythicCardImage;
                    break;
                case PerkRarity.Exotic:
                    perkCardImage.sprite = exoticCardImage;
                    break;
                case PerkRarity.Corrupted:
                    perkCardImage.sprite = corruptedCardImage;
                    break;
                default:
                    perkCardImage.sprite = commonCardImage;
                    break;
            }
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
