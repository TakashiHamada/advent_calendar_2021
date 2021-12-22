using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class CassetteTapeButtons : MonoBehaviour
{
    [SerializeField] private Button btnPause;
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnStop;
    [SerializeField] private Button btnRewind;
    [SerializeField] private Button btnForward;
    
    private readonly Vector2 NORMAL_SIZE = new Vector2(80, 60);
    private readonly Vector2 PUSH_SIZE = new Vector2(80, 40);

    public bool IsBtnPause => this.btnPause.GetComponent<RectTransform>().sizeDelta == PUSH_SIZE;
    public bool IsBtnPlay => this.btnPlay.GetComponent<RectTransform>().sizeDelta == PUSH_SIZE;
    public bool IsBtnRewind => this.btnRewind.GetComponent<RectTransform>().sizeDelta == PUSH_SIZE;
    public bool IsBtnForward => this.btnForward.GetComponent<RectTransform>().sizeDelta == PUSH_SIZE;

    void Start()
    {
        // ポーズは独立
        this.btnPause.OnPointerDownAsObservable()
            .Subscribe(_ => Switch(this.btnPause, !IsBtnPause))
            .AddTo(this);
        
        // 再生
        this.btnPlay.OnPointerDownAsObservable()
            .Subscribe(_ => Switch(this.btnPlay, true))
            .AddTo(this);
        // 停止
        this.btnStop.OnPointerDownAsObservable()
            .Subscribe(_ =>
            {
                Switch(this.btnStop, true);
                // --
                // Reset
                Switch(this.btnPlay, false);
                Switch(this.btnRewind, false);
                Switch(this.btnForward, false);
            })
            .AddTo(this);
        
        this.btnStop.OnPointerUpAsObservable()
            .Subscribe(_ => Switch(this.btnStop, false))
            .AddTo(this);
        
        // Rewind
        this.btnRewind.OnPointerUpAsObservable()
            .Subscribe(_ =>
            {
                Switch(this.btnRewind, true);
                if (IsBtnForward) Switch(this.btnForward, false);
            })
            .AddTo(this);
        
        this.btnForward.OnPointerUpAsObservable()
            .Subscribe(_ =>
            {
                Switch(this.btnForward, true);
                if (IsBtnRewind) Switch(this.btnRewind, false);
            })
            .AddTo(this);
    }

    private void Switch(Button target, bool isPush)
    {
        target.GetComponent<RectTransform>().sizeDelta = isPush ? PUSH_SIZE : NORMAL_SIZE;
    }
}
