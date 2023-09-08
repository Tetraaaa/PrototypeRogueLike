using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform movePoint;
    public float moveSpeed = 7f;
    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0f)
            {
                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
            }

            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0f)
            {
                movePoint.position += new Vector3(0, Input.GetAxisRaw("Vertical"), 0);
            }

        }

    }
}
