using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity
{
    public Transform movePoint;
    public float moveSpeed = 7f;

    protected Animator animator;

    protected int maxHP = 20;
    private int currentHp;
    protected int attack = 2;
    public List<string> logs = new List<string>();
    private Color damageColor = new Color(255, 255, 255);
    public GameTile CurrentTile = null;
    public GameTile? playerPosition;
    public List<Debuff> debuffs = new List<Debuff>();

    public Action OnTurnStart;
    public Action OnDeath;
    // Start is called before the first frame update
    public virtual void Start()
    {
        movePoint.parent = null;
        currentHp = maxHP;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
    }

    public void SetAttack(int attack)
    {
        this.attack = attack;
    }

    public abstract void PlayTurn();
    public void WalkTowardsPlayer()
    {
        if (playerPosition != null) return;
        List<GameTile> path = HomeMadePathfinder.FindPath(CurrentTile, GameManager.Instance.Player.CurrentTile);
        if (path == null)
        {
            return;
        }
        movePoint.position = path[0].worldPos;
        GameManager.Instance.GameBoard.MoveEntity(CurrentTile, GameManager.Instance.GameBoard.Get(movePoint.position));
        CurrentTile = GameManager.Instance.GameBoard.Get(movePoint.position);
    }

    public void CheckIfPlayerIsInContact()
    {
        List<GameTile> neighbors = GameManager.Instance.GameBoard.GetNeighbors(CurrentTile);

        if(neighbors.Contains(GameManager.Instance.Player.CurrentTile))
        {
            playerPosition = GameManager.Instance.Player.CurrentTile;
        }
    }

    public void HitPlayerIfStillThere()
    {
        if(GameManager.Instance.Player.CurrentTile == playerPosition)
        {
            GameManager.Instance.Player.TakeDamage(attack, this);
        }
        else
        {
            playerPosition = null;
        }
    }

    public bool TakeDamage(int damage, GameObject attacker, bool crits=false)
    {
        this.currentHp -= damage;
        SoundManager.Instance.Hit();
        FloatingTextManager.Instance.ShowFloatingDamage(transform.position, damage, damageColor, crits);
        GetComponent<ParticleSystem>().Play();

        if (currentHp <= 0)
        {
            OnDeath?.Invoke();
            CurrentTile.entity = null;
            Destroy(movePoint.gameObject);
            WaveManager.Instance.RemoveEnemy(gameObject);
            if (attacker.GetComponent<Player>() != null) attacker.GetComponent<Player>().GainExp(10);
            return true;
        }

        return false;
    }

    public override string ToString()
    {
        return "Ennemi : " + CurrentTile.ToString();
    }

}
