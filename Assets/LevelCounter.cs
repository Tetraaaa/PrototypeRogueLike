using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelCounter : MonoBehaviour
{
    private TextMeshProUGUI text;

    public void Init()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateLevelCounter()
    {
        text.text = "Niveau " + GameManager.Instance.Player.level;
    }
}
