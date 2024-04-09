using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextManager : Singleton<FloatingTextManager>
{
    public FloatingDamage FloatingDamagePrefab;
    public FloatingDamage FloatingLevelUpPrefab;

    public void ShowFloatingDamage(Vector3 position, int damage, Color color, bool crits=false)
    {
        FloatingDamage floatingDamage = Instantiate(FloatingDamagePrefab, position, Quaternion.identity, null);
        string text = damage.ToString();
        if (crits) text += "!";
        floatingDamage.SetText(text, color);
        floatingDamage.gameObject.SetActive(true);
    }

    public FloatingDamage ShowFloatingText(Vector3 position, string text, Color color)
    {
        FloatingDamage floatingDamage = Instantiate(FloatingDamagePrefab, position, Quaternion.identity, null);
        floatingDamage.SetText(text, color);
        floatingDamage.gameObject.SetActive(true);
        return floatingDamage;
    }

    public void ShowLevelUpText(Vector3 position)
    {
        FloatingDamage floatingDamage = Instantiate(FloatingLevelUpPrefab, position, Quaternion.identity, null);
        floatingDamage.gameObject.SetActive(true);
    }
}
