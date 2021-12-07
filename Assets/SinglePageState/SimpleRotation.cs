using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UniRx;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class SimpleRotation : MonoBehaviour
{
    void Start()
    {
        Observable.EveryUpdate()
            .Subscribe(_ => this.transform.Rotate(Vector3.right * Time.deltaTime * 100f)).AddTo(this);
    }
}
