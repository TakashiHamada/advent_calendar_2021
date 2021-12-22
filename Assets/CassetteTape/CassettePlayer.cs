using System;
using System.Collections;
using TMPro;
using UniRx;
using UnityEngine;

public class CassettePlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private TextMeshProUGUI timeIndicator;

    private AudioSource player;
    private bool isRewind = false;
    private bool isSppedyRewind = false;

    [SerializeField] private CassetteTapeButtons buttons;

    void Start()
    {
        this.player = GetComponent<AudioSource>();
        this.player.clip = this.clips[0];
        this.player.pitch = 0f;
        this.player.Play();

        Observable.EveryUpdate()
            .Select(_ => this.buttons.CanReadCassetteTape)
            .DistinctUntilChanged()
            .Subscribe(f => this.player.clip = this.clips[f ? 0 : 1])
            .AddTo(this);

        // 再生ボタン
        Observable.EveryUpdate()
            .Select(_ => this.buttons.IsBtnPlay)
            .DistinctUntilChanged()
            .Skip(1) // 起動の一発目は無視
            .Subscribe(isPlay => SetPlayMode(isPlay)) // 停止
            .AddTo(this);

        // 一時停止ボタン
        Observable.EveryUpdate()
            .Select(_ => this.buttons.IsBtnPause)
            .DistinctUntilChanged()
            .Skip(1) // 起動の一発目は無視
            .Subscribe(isPause => SetPlayMode(!isPause))
            .AddTo(this);

        // 巻き戻しボタン
        Observable.EveryFixedUpdate()
            .Select(_ => this.buttons.IsBtnRewind)
            .Where(f => f)
            .Subscribe(isBtnRewind => { this.player.pitch = this.buttons.IsBtnPlay ? -4 : -40; })
            .AddTo(this);

        // 早送りボタン
        Observable.EveryFixedUpdate()
            .Select(_ => this.buttons.IsBtnForward)
            .Where(f => f)
            .Subscribe(isBtnForward => { this.player.pitch = this.buttons.IsBtnPlay ? 4 : 40; })
            .AddTo(this);

        // ピッチを戻す
        Observable.EveryFixedUpdate()
            .Select(_ => this.buttons)
            .Where(b => !b.IsBtnForward && !b.IsBtnRewind)
            .DistinctUntilChanged()
            .Subscribe(_ => { SetPlayMode(this.buttons.IsBtnPlay); })
            .AddTo(this);

        Observable.EveryFixedUpdate()
            .Select(_ => this.player.isPlaying)
            .DistinctUntilChanged()
            .Where(f => !f)
            // なぜかピッチをマイナスにして0を下回ると、最大値になる
            .Where(_ => this.player.time >= this.player.clip.length)
            .Subscribe(_ =>
            {
                this.player.time = this.buttons.IsBtnRewind ? 0f
                    : this.player.clip.length - 0.001f; // ちょうどだとエラー

                this.player.pitch = 0;
                this.player.Play(); // Playしないとtimeが更新されない

                this.buttons.Reset();
            })
            .AddTo(this);

        Observable.EveryUpdate()
            .Select(_ => this.timeIndicator)
            .Subscribe(target => target.text = this.player.time.ToString("F2"))
            .AddTo(this);
    }

    private void SetPlayMode(bool isPlay)
    {
        if (isPlay && !this.buttons.IsBtnPause)
        {
            this.player.pitch = 1f;
            this.player.volume = 1f;
        }
        else
        {
            this.player.pitch = 0f;
            this.player.volume = 0f;
        }

        if (!this.player.isPlaying)
            this.player.Play();
    }
}