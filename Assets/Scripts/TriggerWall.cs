using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TriggerWall : MonoBehaviour
{

    private void Start()
    {
        GetComponent<SwitchReceiver>().OnSwitch += OnSwitch;
    }

    private void OnSwitch(bool isOn)
    {
        transform.DOKill();
        if (isOn)
        transform.DOLocalMoveY(-1.4f, 0.5f).SetEase(Ease.InOutCubic);
        else
        transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.InOutCubic);
    }

}
