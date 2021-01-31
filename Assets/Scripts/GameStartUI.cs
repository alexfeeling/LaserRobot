using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartUI : MonoBehaviour
{

    public GameObject StartContent;
    public GameObject OverContent;
    public GameObject WinContent;

    public void OnClickStart()
    {
        gameObject.SetActive(false);
        GameManager.Instance.GameStart();
    }

    public void OnClickQuit()
    {
        GameManager.Instance.Exit();
    }

    public void SetStart()
    {
        StartContent.SetActive(true);
        OverContent.SetActive(false);
        WinContent.SetActive(false);
    }

    public void SetOver()
    {
        StartContent.SetActive(false);
        OverContent.SetActive(true);
        WinContent.SetActive(false);
    }

    public void SetWin()
    {
        StartContent.SetActive(false);
        OverContent.SetActive(false);
        WinContent.SetActive(true);
    }
}
