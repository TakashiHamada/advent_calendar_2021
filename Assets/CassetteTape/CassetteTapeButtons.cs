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
    [SerializeField] private Button btnInsertCassetteTape;

    private readonly Vector2 NORMAL_SIZE = new Vector2(80, 60);
    private readonly Vector2 PUSH_SIZE = new Vector2(80, 40);

    public bool IsBtnPause => this.btnPause.GetComponent<RectTransform>().sizeDelta == PUSH_SIZE;
    public bool IsBtnPlay => this.btnPlay.GetComponent<RectTransform>().sizeDelta == PUSH_SIZE;
    public bool IsBtnRewind => this.btnRewind.GetComponent<RectTransform>().sizeDelta == PUSH_SIZE;
    public bool IsBtnForward => this.btnForward.GetComponent<RectTransform>().sizeDelta == PUSH_SIZE;
    public bool CanReadCassetteTape => !this.btnInsertCassetteTape.interactable;

    private bool IsStopMode => !IsBtnPlay && !IsBtnRewind && !IsBtnForward;

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
                if (IsStopMode)
                {
                    this.btnInsertCassetteTape.interactable = true;
                }
                else
                {
                    Switch(this.btnStop, true);
                    Reset();
                }
            })
            .AddTo(this);

        this.btnStop.OnPointerUpAsObservable()
            .Subscribe(_ => Switch(this.btnStop, false))
            .AddTo(this);

        // 早送り
        this.btnRewind.OnPointerUpAsObservable()
            .Subscribe(_ =>
            {
                Switch(this.btnRewind, true);
                if (IsBtnForward) Switch(this.btnForward, false);
            })
            .AddTo(this);

        // 巻き戻し
        this.btnForward.OnPointerUpAsObservable()
            .Subscribe(_ =>
            {
                Switch(this.btnForward, true);
                if (IsBtnRewind) Switch(this.btnRewind, false);
            })
            .AddTo(this);
        
        // カセットテープ入れる
        this.btnInsertCassetteTape.OnClickAsObservable()
            .Subscribe(_ =>
            {
                this.btnInsertCassetteTape.interactable = false;

            })
            .AddTo(this);
    }

    private void Switch(Button target, bool isPush)
    {
        target.GetComponent<RectTransform>().sizeDelta = isPush ? PUSH_SIZE : NORMAL_SIZE;
    }

    public void Reset()
    {
        Switch(this.btnPlay, false);
        Switch(this.btnRewind, false);
        Switch(this.btnForward, false);
    }
}