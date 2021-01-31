using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearTrap : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<Robot.RobotPlayer>();
        if (player != null)
        {
            player.BeAttack();
        }
    }

}
