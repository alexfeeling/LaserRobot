using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTower : MonoBehaviour
{

    public bool TriggerFlag = false;
    public SwitchReceiver[] Targets;
    public float SwitchTime = 1f;
    public Transform Head;

    private float _timeCount = 0;

    public void Switch()
    {
        if (_timeCount <= 0)
        {
            _timeCount = SwitchTime;
            TriggerFlag = !TriggerFlag;
            if (Targets != null)
            {
                for (int i = 0; i < Targets.Length; i++)
                    Targets[i]?.SetSwitch(TriggerFlag);
            }
            if (Head != null)
                Head.DOPunchScale(Vector3.one * 1.2f, 0.5f).OnComplete(() => Head.localScale = Vector3.one);//.SetEase(Ease.OutElastic);
        }
    }

    private void Update()
    {
        if (_timeCount > 0)
        {
            _timeCount -= Time.deltaTime;
        }
    }

}
