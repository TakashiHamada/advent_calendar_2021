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

    void Start()
    {
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

    public void PlayOrPause()
    {
        if (this.player.isPlaying)
            this.player.Pause();
        else
        {
            this.player.time = this.time;
            this.player.Play();
        }
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
}