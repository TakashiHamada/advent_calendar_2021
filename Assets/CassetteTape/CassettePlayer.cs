using System;
using System.Collections;
using TMPro;
using UniRx;
using UnityEngine;

public class CassettePlayer : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private TextMeshProUGUI timeIndicator;
    private AudioSource player;

    [SerializeField] private CassetteTapeButtons buttons;

    void Start()
    {
        var SPEEDY = 20f;
        var SLOW = 4;
        
        this.player = GetComponent<AudioSource>();
        this.player.pitch = 0f;
        this.player.Play();

        // カセットテープの挿入
        Observable.EveryUpdate()
            .Select(_ => this.buttons.CanReadCassetteTape)
            .DistinctUntilChanged()
            .Subscribe(f => this.player.clip = f ? clip : null)
            .AddTo(this);

        var onPlay = Observable.EveryFixedUpdate()
            .Where(_ => this.player.clip != null)
            .Publish().RefCount();

        // 再生ボタン
        onPlay.Select(_ => this.buttons.IsBtnPlay)
            .DistinctUntilChanged()
            .Skip(1) // 起動の一発目は無視
            .Subscribe(isPlay => SetPlayMode(isPlay)) // 停止
            .AddTo(this);

        // 一時停止ボタン
        onPlay.Select(_ => this.buttons.IsBtnPause)
            .DistinctUntilChanged()
            .Skip(1) // 起動の一発目は無視
            .Subscribe(isPause => SetPlayMode(!isPause))
            .AddTo(this);

        // 巻き戻しボタン
        onPlay.Select(_ => this.buttons.IsBtnRewind)
            .Where(f => f)
            .Subscribe(isBtnRewind => this.player.pitch = this.buttons.IsBtnPlay ? -SLOW : -SPEEDY)
            .AddTo(this);

        // 早送りボタン
        onPlay.Select(_ => this.buttons.IsBtnForward)
            .Where(f => f)
            .Subscribe(isBtnForward => this.player.pitch = this.buttons.IsBtnPlay ? SLOW : SPEEDY)
            .AddTo(this);

        // 巻き戻しと早送りが離されたとき
        onPlay.Select(_ => this.buttons)
            .Select(b => !b.IsBtnForward && !b.IsBtnRewind)
            .DistinctUntilChanged()
            .Where(f => f)
            .Subscribe(_ => SetPlayMode(this.buttons.IsBtnPlay))
            .AddTo(this);

        // 限界判定
        onPlay.Select(_ => this.player.isPlaying)
            .DistinctUntilChanged()
            .Where(f => !f)
            // なぜかピッチをマイナスにして0を下回ると、最大値になる
            .Where(_ => this.player.time >= this.player.clip.length)
            .Subscribe(_ =>
            {
                this.player.time = this.buttons.IsBtnRewind
                    ? 0f
                    : this.player.clip.length - 0.001f; // ちょうどだとエラー

                this.player.pitch = 0;
                this.player.Play(); // Playしないとtimeが更新されない

                this.buttons.Reset();
            })
            .AddTo(this);

        Observable.EveryUpdate()
            .Select(_ => this.timeIndicator)
            .Subscribe(target => target.text =
                this.player.clip == null ? "-- --" : this.player.time.ToString("F2"))
            .AddTo(this);
    }

    private void SetPlayMode(bool isPlay)
    {
        var playable = isPlay && !this.buttons.IsBtnPause;

        this.player.pitch = playable ? 1f : 0f;
        this.player.volume = playable ? 1f : 0f;

        if (!this.player.isPlaying)
            this.player.Play();
    }
}