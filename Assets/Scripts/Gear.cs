using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{

    public Transform Model;

    public enum PropType
    {
        Movable,
        RedLaser,
        LaserLengthen,
        Jump
    }

    public PropType propType;

    void Start()
    {
        Model.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.LocalAxisAdd).SetLoops(-1).SetEase(Ease.Linear);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == GameManager.Instance.PlayerRobot.gameObject)
        {
            HitByRaser();
        }
    }

    public void HitByRaser()
    {
        AudioManager.Insatnce.PlaySound(AudioManager.Insatnce.Effect_Pick);
        var collider = GetComponent<Collider>();
        Destroy(collider);
        transform.DOPunchScale(Vector3.one * 2, 0.4f).OnComplete(() =>
          {
              var pos = GameManager.Instance.PlayerRobot.position;
              transform.DOMove(pos, 0.2f).SetEase(Ease.InCubic).OnComplete(()=> {
                  GameManager.Instance.PlayerRobot.GetComponent<Robot.RobotPlayer>().AddProp(this);
                  Destroy(gameObject);
              });
              transform.DOScale(0, 0.1f).SetEase(Ease.InCubic).SetDelay(0.1f);
          });
    }
}
