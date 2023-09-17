using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burning:Debuff
{
    private Enemy target;
    private int damage = 1;

    public Burning(Enemy target)
    {
        this.target = target;
        this.target.OnTurnStart = OnTurnStart;
        this.target.OnDeath = Remove;

        target.GetComponent<SpriteRenderer>().color = new Color32(246, 152, 85, 255);
    }

    public void OnTurnStart()
    {
        target.TakeDamage(damage, GameManager.Instance.Player);
    }

    public void Remove()
    {
        target.GetComponent<SpriteRenderer>().color = Color.clear;
    }
}
