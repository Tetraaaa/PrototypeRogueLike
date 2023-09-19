using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform movePoint;
    public float moveSpeed = 7f;

    public int XCoordinate = 0;
    public int YCoordinate = 0;
    private bool isHoldingKey = false;
    private bool turnEnded = false;
    private Color damageColor = new Color(240, 0, 0);
    public int xpNeededForLevelUp = 50;
    public int level = 1;
    public GameTile CurrentTile = null;
    private bool lowHealthThresholdReached = false;

    //Stats
    public int maxHP = 20;
    public int currentHp;
    public int attack = 20;
    public float attackMultiplier = 1f;
    public int armor = 0;
    public float dodgeChance = 0f;
    public float parryChance = 0f;
    public float critChance = 0f;
    public float critDamageMultiplier = 1.5f;

    //Events
    public Action<GameTile> OnMove;
    public Action<Enemy> OnHit;
    public Action OnLowHealth;

    public List<Perk> perks = new List<Perk>();

    public int physicalDamage
    {
        get
        {
            return (int)(attack * attackMultiplier);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        perks.Add(new EnergyDrinkPerk(this));
        movePoint.parent = null;
        currentHp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        XCoordinate = Mathf.FloorToInt(transform.position.x);
        YCoordinate = Mathf.FloorToInt(transform.position.y);
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if(turnEnded && Vector3.Distance(transform.position, movePoint.position) == 0)
        {
            WaveManager.Instance.StartNextTurnAndPerformSideEffects();
            turnEnded = false;
        }
            
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 0f && Mathf.Abs(Input.GetAxisRaw("Vertical")) == 0f)
        {
            isHoldingKey = false;
        }
        HandleMovement();
    }

    void HandleMovement()
    {
        if (turnEnded || isHoldingKey) return;
        GameTile targetTile = null;

        if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0f)
        {
            isHoldingKey = true;
            targetTile = GameManager.Instance.GameBoard.Get(CurrentTile.x + (int)Input.GetAxisRaw("Horizontal"), CurrentTile.y);
        }
        else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0f)
        {
            isHoldingKey = true;    
            targetTile = GameManager.Instance.GameBoard.Get(CurrentTile.x , CurrentTile.y + (int)Input.GetAxisRaw("Vertical"));
        }

        if (targetTile == null) return;


        if (targetTile.entity)
        {
            bool attackCrits = UnityEngine.Random.Range(0f, 100f) <= critChance;
            int attackDamage = attack;
            if (attackCrits) attackDamage = (int)(attackDamage* critDamageMultiplier);
            targetTile.entity.GetComponent<Enemy>().TakeDamage(attackDamage, this);
            if(targetTile.entity != null)
            {
                OnHit?.Invoke(targetTile.entity.GetComponent<Enemy>());
            }
            turnEnded = true;
        }
        else
        {
            if (!targetTile.IsWalkable()) return;
            movePoint.position = targetTile.worldPos;
            GameManager.Instance.GameBoard.MoveEntity(CurrentTile, targetTile);
            CurrentTile = targetTile;
            OnMove?.Invoke(targetTile);

            turnEnded = true;
        }
    }

    public void TakeDamage(int damage, Enemy hitBy)
    {
        bool playerDodged = UnityEngine.Random.Range(0f, 100f) <= dodgeChance;
        if(playerDodged)
        {
            //TODO : Jouer le son du dodge
            FloatingTextManager.Instance.ShowFloatingText(transform.position, "DODGE", Color.white);
            bool playerParried = UnityEngine.Random.Range(0f, 100f) <= parryChance;
            if(playerParried)
            {
                hitBy.TakeDamage(physicalDamage + (int)(damage*0.2f), this);
                FloatingTextManager.Instance.ShowFloatingText(transform.position, "PARRY !", Color.white);
            }
            return;
        }
        int damageOnHp = damage - armor;
        if (damageOnHp < 0) damageOnHp = 0;
        currentHp -= damageOnHp;
        SoundManager.Instance.Hit();
        FloatingTextManager.Instance.ShowFloatingDamage(transform.position, damageOnHp, damageColor);
        UIManager.Instance.UpdatePlayerHealth();
        if (currentHp <= 0) Destroy(gameObject);
        if (!lowHealthThresholdReached && currentHp <= maxHP * 0.2)
        {
            OnLowHealth?.Invoke();
            lowHealthThresholdReached = true;
        }
    }

    public void GainExp(int xp)
    {
        this.xpNeededForLevelUp -= xp;
        if(xpNeededForLevelUp <= 0)
        {
            level++;
            xpNeededForLevelUp += 50*level;
            FloatingTextManager.Instance.ShowLevelUpText(transform.position);
            SoundManager.Instance.LevelUp();
            GameManager.Instance.ChooseNewPerks();
        }
    }

    public void Heal(int hp)
    {
        this.currentHp += hp;
        if (currentHp > maxHP) currentHp = maxHP;
        if (lowHealthThresholdReached && currentHp > maxHP * 0.2) lowHealthThresholdReached = false;
        FloatingTextManager.Instance.ShowFloatingDamage(transform.position, hp, new Color32(0, 145, 10, 255));
    }

}
