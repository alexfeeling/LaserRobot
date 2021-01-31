using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //将声音管理器写成单例模式
    public static AudioManager Insatnce;
    //音乐播放器
    public AudioSource MusicPlayer;
    //音效播放器
    public AudioSource SoundPlayer;

    public AudioClip MusicStartPanel;
    public AudioClip MusicGame;
    public AudioClip MusicGameWin;

    public AudioClip Effect_Move;
    public AudioClip Effect_LaserBlue;
    public AudioClip Effect_LaserRed;
    public AudioClip Effect_GameOver;
    public AudioClip Effect_Pick;
    public AudioClip Effect_AddGear;
    public AudioClip Effect_Explode;

    void Awake()
    {
        Insatnce = this;
        MusicPlayer = gameObject.AddComponent<AudioSource>();
        MusicPlayer.playOnAwake = false;
        MusicPlayer.volume = 0.25f;
        MusicPlayer.loop = true;
        SoundPlayer = gameObject.AddComponent<AudioSource>();
        SoundPlayer.playOnAwake = false;
    }

    private void OnDestroy()
    {
        Insatnce = null;
    }

    //播放音乐
    public void PlayMusic(string name)
    {
        if (MusicPlayer.isPlaying == false)
        {
            AudioClip clip = Resources.Load<AudioClip>(name);
            MusicPlayer.clip = clip;
            MusicPlayer.Play();
        }
        
    }

    //播放音效
    public void PlaySound(string name)
    {
        AudioClip clip = Resources.Load<AudioClip>(name);
        SoundPlayer.clip = clip;
        SoundPlayer.PlayOneShot(clip);
    }

    //播放音乐
    public void PlayMusic(AudioClip clip)
    {
        //if (MusicPlayer.isPlaying == false)
        MusicPlayer.Stop();
        {
            MusicPlayer.clip = clip;
            MusicPlayer.Play();
        }

    }

    public void StopMusic()
    {
        MusicPlayer.Stop();
    }

    //播放音效
    public void PlaySound(AudioClip clip)
    {
        SoundPlayer.clip = clip;
        SoundPlayer.PlayOneShot(clip);
    }

}
