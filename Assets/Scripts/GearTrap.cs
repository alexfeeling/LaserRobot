using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearTrap : MonoBehaviour
{

    Animator anim;
    Collider colid;
    int flagHash = Animator.StringToHash("flag");

    private void Start()
    {
        GetComponent<SwitchReceiver>().OnSwitch += OnSwitch;
        anim = GetComponent<Animator>();
        colid = GetComponent<Collider>();
    }

    private void OnSwitch(bool isOn)
    {

        colid.enabled = !isOn;
        anim.SetBool(flagHash, isOn);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<Robot.RobotPlayer>();
        if (player != null)
        {
            player.BeAttack();
        }
    }

}
