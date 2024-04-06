using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RNG
{

    public static float Get()
    {
        return Random.value;
    }

    public static float Range(float min, float max)
    {
        return Random.Range(min, max);
    }

    public static int Outcome(params float[] odds)
    {
        float rand = Range(0, odds.Sum());
        float currentProbability = 0;
        for (int i = 0; i < odds.Length; i++)
        {
            currentProbability += odds[i];
            if (rand <= currentProbability)
                return i;
        }

        return 0;

    }
}
