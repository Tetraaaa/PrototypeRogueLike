using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : Singleton<AnimationManager>
{
    public GameObject ThunderFeetAnimation;

    public void ThunderFeet(Vector3 position)
    {
        Instantiate(ThunderFeetAnimation, position, Quaternion.identity, transform);
    }
}
