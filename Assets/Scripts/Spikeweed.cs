using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikeweed : MonoBehaviour
{
    public float OnTime = 1f;
    public float OffTime = 1.5f;
    [SerializeField]
    private Animator _animator;

    void Start()
    {
        _animator.SetBool("flag", _isOn);
    }

    private bool _isOn = false;
    private float _timeCount = 0;
    void Update()
    {
        _timeCount += Time.deltaTime;
        if (_isOn)
        {
            if (_timeCount > OnTime)
            {
                _isOn = false;
                _timeCount = 0;
                _animator.SetBool("flag", _isOn);
            }
        }
        else
        {
            if (_timeCount > OffTime)
            {
                _isOn = true;
                _timeCount = 0;
                _animator.SetBool("flag", _isOn);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    { 
        if (_isOn)
        {
            if (GameManager.Instance.Status == GameManager.GameStatus.Playing)
            {
                var player = other.gameObject.GetComponent<Robot.RobotPlayer>();
                if (player != null)
                {
                    player.BeAttack();
                }

            }
        }
    }

}
