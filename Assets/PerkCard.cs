using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PerkCard : MonoBehaviour
{
    public Perk Perk;

    public Image Image;
    public TextMeshProUGUI Description;

    private void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void OnPointerDownDelegate(PointerEventData data)
    {
        GameManager.Instance.Player.perks.Add(Perk);
        Perk.OnBuy(GameManager.Instance.Player);
        UIManager.Instance.ClosePerksMenu();
    }


    public void ShowOnScreen(Perk perk)
    {
        Perk = perk;
        Image.sprite = perk.image;
        Description.text = perk.Description;
    }

}
