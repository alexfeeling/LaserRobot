using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchReceiver : MonoBehaviour
{

    public System.Action<bool> OnSwitch;

    public void SetSwitch(bool isOn)
    {
        OnSwitch?.Invoke(isOn);
    }
}
