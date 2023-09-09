using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform movePoint;
    public float moveSpeed = 7f;

    private bool isHoldingKey = false;
    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        //if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0) 
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0f)
            {
                RaycastHit2D hit = Physics2D.Linecast(new Vector2(movePoint.position.x, movePoint.position.y), new Vector2(movePoint.position.x + Input.GetAxisRaw("Horizontal"), movePoint.position.y));
                if (hit.transform) return;
                if (isHoldingKey) return;
                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
                GameManager.Instance.StartNextTurnAndPerformSideEffects();
                isHoldingKey = true;
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0f)
            {
                RaycastHit2D hit = Physics2D.Linecast(new Vector2(movePoint.position.x, movePoint.position.y), new Vector2(movePoint.position.x, movePoint.position.y + Input.GetAxisRaw("Vertical")));
                if (hit.transform) return;
                if (isHoldingKey) return;
                movePoint.position += new Vector3(0, Input.GetAxisRaw("Vertical"), 0);
                GameManager.Instance.StartNextTurnAndPerformSideEffects();
                isHoldingKey = true;
            }
            else
            {
                isHoldingKey = false;
            }
        }
    }

}
