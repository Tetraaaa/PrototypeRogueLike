using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int maxHP = 20;
    private int currentHp;
    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        this.currentHp -= damage;
        if (this.currentHp <= 0) Destroy(gameObject); 
    }
}
