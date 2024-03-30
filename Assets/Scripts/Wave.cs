using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
{
    public List<string> mobs { get; set; }

    public Wave(List<string> mobs)
    {
        this.mobs = mobs;
    }
}
