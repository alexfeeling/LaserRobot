using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDesdination : MonoBehaviour
{

    void Start()
    {
        transform.DORotate(new Vector3(0, 90, 0), 1f, RotateMode.LocalAxisAdd).SetLoops(-1);
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<Robot.RobotPlayer>();
        if (player != null)
        {
            GameManager.Instance.LevelSucc();
        }
    }
}
