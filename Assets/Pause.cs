using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private List<IDisposable> disposables = new List<IDisposable>();

    void Awake()
    {
    }

    void OnEnable()
    {
        // ポーズ開始
        Time.timeScale = 0f;

        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ => Debug.Log(" Click! <= " + this.gameObject.name))
            .AddTo(this.disposables);
    }

    void Start()
    {
    }

    private void OnDisable()
    {
        // ポーズ解除
        Time.timeScale = 1f;
        // --
        foreach (IDisposable disposable in this.disposables)
            disposable.Dispose();
    }
}