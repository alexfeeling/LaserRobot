using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGear : MonoBehaviour
{

    public Vector3[] Path;
    public float MoveSpeed = 1f;
    public float IdleTime = 1f;
    public bool IsLoop = false;
    void Start()
    {
        MoveForward();
    }

    private void MoveForward()
    {
        if (Path != null && Path.Length >= 2)
        {
            var totalTime = 0f;
            for (int i = 1; i < Path.Length; i++)
            {
                var lastPos = Path[i - 1];
                var thisPos = Path[i];
                var length = (thisPos - lastPos).magnitude;
                totalTime += length / MoveSpeed;
            }
            if (IsLoop)
                transform.DOLocalPath(Path, totalTime, PathType.Linear).SetEase(Ease.Linear).OnComplete(MoveForward).SetDelay(IdleTime);
            else
                transform.DOLocalPath(Path, totalTime, PathType.Linear).SetEase(Ease.Linear).OnComplete(MoveBackward).SetDelay(IdleTime);
        }
    }

    private void MoveBackward()
    {
        if (Path != null && Path.Length >= 2)
        {
            var totalTime = 0f;
            for (int i = 1; i < Path.Length; i++)
            {
                var lastPos = Path[i - 1];
                var thisPos = Path[i];
                var length = (thisPos - lastPos).magnitude;
                totalTime += length / MoveSpeed;
            }

            transform.DOLocalPath(Path, totalTime, PathType.Linear).SetEase(Ease.Linear).OnComplete(MoveForward).SetDelay(IdleTime).PlayBackwards();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<Robot.RobotPlayer>();
        if (player != null)
        {
            player.BeAttack();
        }
    }

    private Color _drawColor = new Color(0, 1, 0, 0.5f);
    private Color _drawLineColor = new Color(0, 0, 0, 1);
    private void OnDrawGizmos()
    {
        if (Path != null && Path.Length >= 2)
        {
            Gizmos.color = _drawColor;
            for (int i = 0; i < Path.Length; i++)
            {
                var pos = Path[i] + transform.parent.position;
                pos.y += 0.25f;
                Gizmos.DrawSphere(pos, 0.25f);
            }
            Gizmos.color = _drawLineColor;
            for (int i = 0; i < Path.Length; i++)
            {
                if (i > 0)
                {
                    var lastPos = Path[i - 1] + transform.parent.position;
                    var thisPos = Path[i] + transform.parent.position;
                    lastPos.y += 0.25f;
                    thisPos.y += 0.25f;
                    Gizmos.DrawLine(lastPos, thisPos);
                }
            }
        }
    }

}
