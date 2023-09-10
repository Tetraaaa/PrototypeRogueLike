using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamage : MonoBehaviour
{
    private float durationLeft = 1f;
    private float floatingSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        float xOffset = Random.Range(-0.5f, 0.5f);
        transform.position += new Vector3(xOffset, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 0.5f), 1f * Time.deltaTime); ;
        durationLeft -= Time.deltaTime;
        if (durationLeft <= 0) Destroy(gameObject);
    }

    public void SetText(string text, Color color)
    {
        TextMesh textMesh = GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.color = color;
    }
}
