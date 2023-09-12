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
    private Vector2? playerPosition;
    private Color damageColor = new Color(255, 255, 255);
    private Seeker seeker;
    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        currentHp = maxHP;
        seeker = GetComponent<Seeker>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
    }

    public void PlayTurn()
    {
        if(playerPosition != null)
        {
            HitPlayerIfStillThere();
        }
        else
        {
            CheckIfPlayerIsInContact();
            WalkTowardsPlayer();
        }
    }

    public void WalkTowardsPlayer()
    {
        if (playerPosition != null) return;
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Path p = seeker.StartPath(transform.position, GameManager.Instance.player.transform.position);
        p.BlockUntilCalculated();
        if(p.error)
        {
            logs.Add("no player ?");
        }
        else
        {
            bool goingToCrashIntoEachOther = Physics2D.Linecast(currentPosition, p.vectorPath[1]).transform != null;
            if (goingToCrashIntoEachOther)
            {
                Debug.Log("wow y'a déjà quelqu'un ici !!");
                return;
            }
            movePoint.position = p.vectorPath[1];
        }
    }

    public void CheckIfPlayerIsInContact()
    {
        logs.Add("aaaah j'espère qu'il n'y a pas de joueur à mon cac :) ;)");
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D leftTarget = Physics2D.Linecast(currentPosition, currentPosition + Vector2.left);
        RaycastHit2D rightTarget = Physics2D.Linecast(currentPosition, currentPosition + Vector2.right);
        RaycastHit2D upTarget = Physics2D.Linecast(currentPosition, currentPosition + Vector2.up);
        RaycastHit2D downTarget = Physics2D.Linecast(currentPosition, currentPosition + Vector2.down);

        if (leftTarget.transform && leftTarget.transform.gameObject.tag == "Player")
        {
            playerPosition = currentPosition + Vector2.left;
        }
        else if (rightTarget.transform && rightTarget.transform.gameObject.tag == "Player")
        {
            playerPosition = currentPosition + Vector2.right;
        }
        else if (upTarget.transform && upTarget.transform.gameObject.tag == "Player")
        {
            playerPosition = currentPosition + Vector2.up;
        }
        else if (downTarget.transform && downTarget.transform.gameObject.tag == "Player")
        {
            playerPosition = currentPosition + Vector2.down;
        }

        if (playerPosition != null)
        {
            logs.Add("il y a un joueur a mon cac wtf ?????????");
        } 
    }

    public void HitPlayerIfStillThere()
    {
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D player = Physics2D.Linecast(currentPosition, (Vector2)playerPosition);
        if(player.transform && player.transform.gameObject.tag == "Player")
        {
            logs.Add("prends ça fumier");
            player.transform.gameObject.GetComponent<Player>().TakeDamage(attack);
        }
        else
        {
            logs.Add("ouf fausse alerte ;)");
            playerPosition = null;
        }
    }

    public void TakeDamage(int damage, Player attacker)
    {
        this.currentHp -= damage;
        GameManager.Instance.PlayHitSound();
        GameManager.Instance.ShowFloatingDamage(transform.position, damage, damageColor);
        GetComponent<ParticleSystem>().Play();


        if (this.currentHp <= 0)
        {
            Destroy(gameObject);
            attacker.GainExp(10);
        }
    }
}
