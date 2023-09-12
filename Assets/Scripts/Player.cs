using System.Collections;
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
    public bool hasThunderFeet = false;


    // Start is called before the first frame update
    void Start()
    {
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
            GameManager.Instance.StartNextTurnAndPerformSideEffects();
            turnEnded = false;
        }
        HandleMovement();
    }

    void HandleMovement()
    {

        if (turnEnded) return;

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0f)
            {
                if (isHoldingKey) return;
                isHoldingKey = true;
                RaycastHit2D hit = Physics2D.Linecast(new Vector2(movePoint.position.x, movePoint.position.y), new Vector2(movePoint.position.x + Input.GetAxisRaw("Horizontal"), movePoint.position.y));
                if (hit.transform)
                {
                    if (hit.transform.gameObject.tag == "Enemy")
                    {
                        hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(attack, this);
                        turnEnded = true;
                    }
                    return;
                }

                if (hasThunderFeet) ApplyThunderFeet();
                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);

                turnEnded = true;
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0f)
            {
                if (isHoldingKey) return;
                isHoldingKey = true;
                RaycastHit2D hit = Physics2D.Linecast(new Vector2(movePoint.position.x, movePoint.position.y), new Vector2(movePoint.position.x, movePoint.position.y + Input.GetAxisRaw("Vertical")));
                if (hit.transform)
                {
                    if (hit.transform.gameObject.tag == "Enemy")
                    {
                        hit.transform.gameObject.GetComponent<Enemy>().TakeDamage(attack, this);
                        turnEnded = true;
                    }
                    return;
                }

                if (hasThunderFeet) ApplyThunderFeet();
                movePoint.position += new Vector3(0, Input.GetAxisRaw("Vertical"), 0);
                turnEnded = true;
            }
            else
            {
                isHoldingKey = false;
            }
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

    public void ApplyThunderFeet()
    {
        Vector2 currentPosition = movePoint.position;

        RaycastHit2D leftTarget = Physics2D.Linecast(currentPosition, currentPosition + Vector2.left);
        RaycastHit2D rightTarget = Physics2D.Linecast(currentPosition, currentPosition + Vector2.right);
        RaycastHit2D upTarget = Physics2D.Linecast(currentPosition, currentPosition + Vector2.up);
        RaycastHit2D downTarget = Physics2D.Linecast(currentPosition, currentPosition + Vector2.down);

        Instantiate(GameManager.Instance.ThunderFeetAnimation, currentPosition + Vector2.left, Quaternion.identity, transform);
        Instantiate(GameManager.Instance.ThunderFeetAnimation, currentPosition + Vector2.right, Quaternion.identity, transform);
        Instantiate(GameManager.Instance.ThunderFeetAnimation, currentPosition + Vector2.up, Quaternion.identity, transform);
        Instantiate(GameManager.Instance.ThunderFeetAnimation, currentPosition + Vector2.down, Quaternion.identity, transform);

        if (leftTarget.transform && leftTarget.transform.gameObject.tag == "Enemy")
        {
            leftTarget.transform.gameObject.GetComponent<Enemy>().TakeDamage(2, this);
        }
        if (rightTarget.transform && rightTarget.transform.gameObject.tag == "Enemy")
        {
            rightTarget.transform.gameObject.GetComponent<Enemy>().TakeDamage(2, this);
        }
        if (upTarget.transform && upTarget.transform.gameObject.tag == "Enemy")
        {
            upTarget.transform.gameObject.GetComponent<Enemy>().TakeDamage(2, this);
        }
        if (downTarget.transform && downTarget.transform.gameObject.tag == "Enemy")
        {
            downTarget.transform.gameObject.GetComponent<Enemy>().TakeDamage(2, this);
        }

    }

    public void AddThunderFeetPerk()
    {
        hasThunderFeet = true;
    }

}
