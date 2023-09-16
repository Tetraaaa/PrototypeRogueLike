using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    public Transform movePoint;
    public float moveSpeed = 7f;


    private int maxHP = 20;
    private int currentHp;
    private int attack = 1;
    public List<string> logs = new List<string>();
    private Color damageColor = new Color(255, 255, 255);
    public bool isBurning;
    public GameTile CurrentTile = null;
    public GameTile? playerPosition;
    public bool willAttackPlayerNextTurn = false;
    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        currentHp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBurning) GetComponent<SpriteRenderer>().color = new Color32(246, 152, 85, 255);
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
    }

    public void PlayTurn()
    {
        if (isBurning) TakeDamage(1, GameManager.Instance.Player);
        if(willAttackPlayerNextTurn)
        {
            HitPlayerIfStillThere();
            willAttackPlayerNextTurn = false;
        }
        else if (playerPosition != null)
        {
            willAttackPlayerNextTurn = true;
            //Ici : On pourrait changer le sprite du mob en son animation d'attaque
        }
        else
        {
            CheckIfPlayerIsInContact();
            if(playerPosition == null) WalkTowardsPlayer();
        }
    }

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
            GameManager.Instance.Player.TakeDamage(attack);
        }
        else
        {
            playerPosition = null;
        }
    }

    public void TakeDamage(int damage, Player attacker)
    {
        this.currentHp -= damage;
        GameManager.Instance.PlayHitSound();
        GameManager.Instance.ShowFloatingDamage(transform.position, damage, damageColor);
        GetComponent<ParticleSystem>().Play();


        if (currentHp <= 0)
        {
            CurrentTile.entity = null;
            Destroy(movePoint.gameObject);
            WaveManager.Instance.RemoveEnemy(gameObject);
            attacker.GainExp(10);
        }
    }

}
