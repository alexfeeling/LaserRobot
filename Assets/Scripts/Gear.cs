using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{

    public Transform Model;

    // Start is called before the first frame update
    void Start()
    {
        Model.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.LocalAxisAdd).SetLoops(-1).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HitByRaser()
    {
        var collider = GetComponent<Collider>();
        Destroy(collider);

        transform.DOScale(0, 0.6f).SetEase(Ease.InElastic).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
