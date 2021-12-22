using System;
using System.Collections;
using System.Collections.Generic;
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
    private float time;

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
        this.time = 0f;

        Observable.EveryFixedUpdate()
            .Where(_ => this.isRewind)
            .Select(_ => this.player.time)
            .Where(t => t <= 0)
            .Subscribe(_ =>
            {
                this.player.pitch = 1f;
                this.player.Stop();
                this.isRewind = false;
            })
            .AddTo(this);

        Observable.EveryFixedUpdate()
            .Where(_ => this.isSppedyRewind)
            .Where(_ => 0 <= this.time)
            .Subscribe(_ =>
            {
                this.time -= 0.05f;
                if (this.time < 0)
                {
                    this.time = 0f;
                }
            });

        Observable.EveryUpdate()
            .Subscribe(_ => this.timeIndicator.text = this.time.ToString())
            .AddTo(this);
    }


    public void Rewind()
    {
        if (this.player.isPlaying)
        {
            this.player.pitch = -5;
            this.isRewind = true;
        }
        else
        {
            this.time = this.player.time;
            this.isSppedyRewind = true;
        }
    }

    private void SetPlayMode(bool isPlay)
    {
        var player2 = GetComponent<AudioSource>();
        if (isPlay && !this.buttons.IsBtnPause)
        {
            player2.Play();
        }
        else
        {
            player2.Pause();
        }
    }
}