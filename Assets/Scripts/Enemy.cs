using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int maxHP = 20;
    private int currentHp;
    private int attack = 1;
    public List<string> logs = new List<string>();
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

    public void HitPlayerIfPossible()
    {
        logs.Add("Je vais tenter de tapaient :)");
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D leftTarget = Physics2D.Linecast(currentPosition, currentPosition + Vector2.left);
        RaycastHit2D rightTarget = Physics2D.Linecast(currentPosition, currentPosition + Vector2.right);
        RaycastHit2D upTarget = Physics2D.Linecast(currentPosition, currentPosition + Vector2.up);
        RaycastHit2D downTarget = Physics2D.Linecast(currentPosition, currentPosition + Vector2.down);

        logs.Add("Je detek quelque chose !!!!");
        if (leftTarget.transform && leftTarget.transform.gameObject.tag == "Player")
        {
            leftTarget.transform.gameObject.GetComponent<Player>().TakeDamage(attack);
        }
        else if (rightTarget.transform && rightTarget.transform.gameObject.tag == "Player")
        {
            rightTarget.transform.gameObject.GetComponent<Player>().TakeDamage(attack);
        }
        else if (upTarget.transform && upTarget.transform.gameObject.tag == "Player")
        {
            upTarget.transform.gameObject.GetComponent<Player>().TakeDamage(attack);
        }
        else if (downTarget.transform && downTarget.transform.gameObject.tag == "Player")
        {
            downTarget.transform.gameObject.GetComponent<Player>().TakeDamage(attack);
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
