using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private Image icon;
    private List<IDisposable> disposables = new List<IDisposable>();
    void Awake()
    {
        GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.1f);
    }
    
    void OnEnable()
    {
        Time.timeScale = 0f;
        // --
        Observable.Timer(TimeSpan.FromMilliseconds(16), TimeSpan.Zero, Scheduler.MainThreadIgnoreTimeScale)
            .Subscribe(t =>
            {
                var alpha = (t * 0.02f) % 1f;
                this.icon.color = new Color(1f, 1f, 1f, alpha);
            })
            .AddTo(this.disposables);
    }

    void Start()
    {
        
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
        // --
        foreach (IDisposable disposable in this.disposables)
            disposable.Dispose();
    }
}
