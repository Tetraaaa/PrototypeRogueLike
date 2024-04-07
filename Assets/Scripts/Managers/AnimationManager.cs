using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : Singleton<AnimationManager>
{
    public GameObject ThunderFeetAnimation;
    public GameObject ForbiddenArtsAnimation;

    public void ThunderFeet(Vector3 position)
    {
        Instantiate(ThunderFeetAnimation, position, Quaternion.identity, transform);
    }

    public void ForbiddenArts(Vector3 position)
    {
        Instantiate(ForbiddenArtsAnimation, position, Quaternion.identity, transform);
    }
}
