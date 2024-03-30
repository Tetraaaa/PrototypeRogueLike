using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveCounter : MonoBehaviour
{
    private TextMeshProUGUI text;

    public void Init()
    {
        UpdateWaveCounter();
    }

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateWaveCounter()
    {
        text.text = "Vague " + WaveManager.Instance.CurrentWave ;
    }
}
