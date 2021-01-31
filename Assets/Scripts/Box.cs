using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{

    public float PushSpeed = 0.5f;

    public void BePush(Vector3 fromPos)
    {
        var dir = (transform.position - fromPos).normalized;
        transform.position += dir * Time.deltaTime * PushSpeed;
    }
}
