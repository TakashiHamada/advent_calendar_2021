using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EachScene : MonoBehaviour
{
    [SerializeField] private GameObject model;
    private List<IDisposable> disposables = new List<IDisposable>();
    private Transform objectsRoot;

    void Awake()
    {
        this.objectsRoot = this.transform.Find("ROOT");
        GetComponent<Image>().enabled = false;
    }
    
    void OnEnable()
    {
        Observable.Interval(TimeSpan.FromSeconds(0.1f))
            .TakeWhile(_ => this.objectsRoot.childCount < 20)
            .Subscribe(_ =>
            {
                var copy = Instantiate(this.model, this.objectsRoot);
                // ランダムな位置と姿勢
                copy.transform.position = GetRandomPosition();
                copy.transform.rotation = Random.rotation;
            }, () =>
            {
                // 自らを非表示に
                this.gameObject.SetActive(false);
            })
            .AddTo(this.disposables);
        // --
        // クリックイベント(TimeScale=0で動かず)
        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .BatchFrame(0, FrameCountType.FixedUpdate) // InputはUpdateでないと正しく動かない
            .Subscribe(_ => Debug.Log(" Click! <= " + this.gameObject.name))
            .AddTo(this.disposables);
    }

    void Start()
    {
    }

    void Update()
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