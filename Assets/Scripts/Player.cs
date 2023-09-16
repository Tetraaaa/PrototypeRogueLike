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
    public int attack = 20;
    private int maxHP = 20;
    private int currentHp;
    private bool turnEnded = false;
    private Color damageColor = new Color(240, 0, 0);
    public int xpNeededForLevelUp = 50;
    public int level = 1;
    public bool hasFireFists = false;
    public bool hasKnockback = false;
    public int knockbackHitCounter = 0;
    public GameTile CurrentTile = null;

    public Action<GameTile> OnMove;
    public Action<Enemy> OnHit;

    public List<Perk> perks = new List<Perk>();


    // Start is called before the first frame update
    void Start()
    {
        perks.Add(new FireFistsPerk(this));
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
            targetTile.entity.GetComponent<Enemy>().TakeDamage(attack, this);
            OnHit?.Invoke(targetTile.entity.GetComponent<Enemy>());
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

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        GameManager.Instance.PlayHitSound();
        GameManager.Instance.ShowFloatingDamage(transform.position, damage, damageColor);
        if (currentHp <= 0) Destroy(gameObject);
    }

    public void GainExp(int xp)
    {
        this.xpNeededForLevelUp -= xp;
        if(xpNeededForLevelUp <= 0)
        {
            level++;
            xpNeededForLevelUp += 50*level;
            GameManager.Instance.ChooseNewPerks();
        }
    }

}
