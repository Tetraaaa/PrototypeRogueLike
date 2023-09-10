using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int maxHP = 20;
    private int currentHp;
    private int attack = 1;
    public List<string> logs = new List<string>();
    private Vector2? playerPosition;
    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.position.x, transform.position.y) + Vector2.left);
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

        if (playerPosition != null) logs.Add("il y a un joueur a mon cac wtf ?????????");
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

    public void TakeDamage(int damage)
    {
        this.currentHp -= damage;
        GameManager.Instance.PlayHitSound();
        GetComponent<ParticleSystem>().Play();

        if (this.currentHp <= 0) Destroy(gameObject); 
    }
}
