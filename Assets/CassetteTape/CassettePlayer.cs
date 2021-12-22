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
        Observable.EveryUpdate()
            .Select(_ => this.buttons.IsBtnPlay)
            .DistinctUntilChanged()
            .Skip(1) // 起動の一発目は無視
            .Subscribe(isPlay => SetPlayMode(isPlay))
            .AddTo(this);

        Observable.EveryUpdate()
            .Select(_ => this.buttons.IsBtnPause)
            .DistinctUntilChanged()
            .Skip(1) // 起動の一発目は無視
            .Subscribe(isPause => SetPlayMode(!isPause))
            .AddTo(this);


        this.player = GetComponent<AudioSource>();
        this.player.clip = this.clips[0];
        this.player.pitch = 0f;
        this.player.Play();

        Observable.EveryUpdate()
            .Select(_ => this.buttons.CanReadCassetteTape)
            .DistinctUntilChanged()
            .Subscribe(f => this.player.clip = this.clips[f ? 0 : 1])
            .AddTo(this);

        Observable.EveryFixedUpdate()
            .Select(_ => this.buttons.IsBtnRewind)
            .Where(f => f)
            .Subscribe(isBtnRewind => { this.player.pitch = this.buttons.IsBtnPlay ? -4 : -40; })
            .AddTo(this);

        Observable.EveryFixedUpdate()
            .Select(_ => this.buttons.IsBtnForward)
            .Where(f => f)
            .Subscribe(isBtnForward => { this.player.pitch = this.buttons.IsBtnPlay ? 4 : 40; })
            .AddTo(this);

        // ピッチを戻す
        Observable.EveryFixedUpdate()
            .Select(_ => this.buttons)
            .Where(b => !b.IsBtnForward && !b.IsBtnRewind)
            .Subscribe(_ => { SetPlayMode(this.buttons.IsBtnPlay); })
            .AddTo(this);

        // ボタンリセット
        Observable.EveryFixedUpdate()
            .Select(_ => Playable())
            .DistinctUntilChanged()
            .Where(f => f)
            .Subscribe(_ => this.buttons.Reset())
            .AddTo(this);

        Observable.EveryUpdate()
            .Select(_ => this.timeIndicator)
            .Subscribe(target => target.text = this.player.time.ToString("F1"))
            .AddTo(this);
    }

    private bool Playable()
    {
        var t = this.player.time;
        return t <= 0 || this.player.clip.length - 1f <= t;
    }


    private void SetPlayMode(bool isPlay)
    {
        var player2 = GetComponent<AudioSource>();
        if (isPlay && !this.buttons.IsBtnPause)
        {
            player2.pitch = 1f;
            player2.volume = 1f;
        }
        else
        {
            player2.pitch = 0f;
            player2.volume = 0f;
        }

        if (!player2.isPlaying)
            player2.Play();
    }
}