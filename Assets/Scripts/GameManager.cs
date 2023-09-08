using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int currentTurn = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNextTurnAndPerformSideEffects()
    {
        currentTurn++;
        Debug.Log(currentTurn);
    }

}
