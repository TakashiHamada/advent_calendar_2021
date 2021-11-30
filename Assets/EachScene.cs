using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class EachScene : MonoBehaviour
{
    [SerializeField] private GameObject model;
    private List<IDisposable> disposables = new List<IDisposable>();
    private Transform objectsRoot;

    void Awake()
    {
        this.objectsRoot = this.transform.Find("ROOT");
    }
    void OnEnable()
    {
        Observable.Interval(TimeSpan.FromSeconds(0.2f))
            .Where(_ => this.objectsRoot.childCount < 50)
            .Subscribe(_ =>
            {
                var copy = Instantiate(this.model, this.objectsRoot);
                // ランダムな位置と姿勢
                copy.transform.position = GetRandomPosition();
                copy.transform.rotation = Random.rotation;
            })
            .AddTo(this.disposables);
    }

    void Start()
    {
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(
            Random.Range(-15f, 15f),
            Random.Range(-10f, 10f),
            Random.Range(-6f, 6f));
    }

    private void OnDisable()
    {
        foreach (Transform child in this.objectsRoot)
            Destroy(child.gameObject);
            // --
        foreach (IDisposable disposable in this.disposables)
            disposable.Dispose();
    }
}