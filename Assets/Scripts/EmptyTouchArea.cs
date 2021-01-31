using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTouchArea : MonoBehaviour
{

    public int ShowTextId;

    public bool ShowOnStart = false;

    private void Start()
    {
        var rdrs = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < rdrs.Length; i++)
            Destroy(rdrs[i].gameObject);

        if (ShowOnStart)
        {
            Show();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Show();
        }
    }

    private void Show()
    {
        UIPlayerMainCon.getInstance().playText(ShowTextId);
        Destroy(gameObject);
    }

}
