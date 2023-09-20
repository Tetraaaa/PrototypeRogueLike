using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PerkCard : MonoBehaviour
{
    public Perk Perk;

    public Image Image;
    public TextMeshProUGUI Description;


    public void ShowOnScreen(Perk perk)
    {
        Perk = perk;
        Image.sprite = perk.image;
        Description.text = perk.Description;
    }

}
