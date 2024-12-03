using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    public AudioSource soundEffect;
    public AudioSource soundMusic;

    public SoundType[] Sounds;
    public bool isMute;
    public float Volume = 1f;

    private Dictionary<Sounds, float> soundCooldowns = new Dictionary<Sounds, float>();
    private float globalCooldown = 0.1f; // Global cooldown for sound effects (in seconds)

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetVolume(0.5f);
        PlayMusic(global::Sounds.Music);
    }

    public void Mute(bool status)
    {
        isMute = status;
    }

    public void SetVolume(float volume)
    {
        Volume = volume;
        //soundEffect.volume = Volume;
        soundEffect.volume = 0.8f;
        soundMusic.volume = Volume;
    }

    //Used for background music
    public void PlayMusic(Sounds sound)
    {
        if (isMute)
            return;

        AudioClip clip = getSoundClip(sound);

        if (clip != null)
        {
            soundMusic.clip = clip;
            soundMusic.Play();
        }
        else
        {
            Debug.LogError("Clip not found for sound type :" + sound);
        }
    }

    //Used for Sfx 
    public void Play(Sounds sound)
    {
        if (isMute)
            return;

        if (CanPlaySound(sound))
        {
            AudioClip clip = getSoundClip(sound);

            if (clip != null)
            {
                soundEffect.PlayOneShot(clip);
                UpdateSoundCooldown(sound);
            }
            else
            {
                Debug.LogError("Clip not found for sound type :" + sound);
            }
        }
    }


    private AudioClip getSoundClip(Sounds sound)
    {
        SoundType item = Array.Find(Sounds, i => i.soundType == sound);
        if (item != null)
            return item.soundClip;
        return null;
    }

    private bool CanPlaySound(Sounds sound)
    {
        if (!soundCooldowns.ContainsKey(sound))
        {
            soundCooldowns[sound] = 0f;
        }

        float lastPlayed = soundCooldowns[sound];
        return Time.time - lastPlayed >= globalCooldown;
    }

    private void UpdateSoundCooldown(Sounds sound)
    {
        soundCooldowns[sound] = Time.time;
    }
}

[Serializable]
public class SoundType
{
    public Sounds soundType;
    public AudioClip soundClip;
}

public enum Sounds
{
    ButtonClick,
    Music,
    PlayerMove,
    PlayerDeath,
    PlayerHurt,
    PlayerFootSteps,
    PlayerJump,
    PlayerSpikeImpact,
    ButtonPlay,
    PlayerCollectable,
    PlayerVictory,
    OpenDoor,
    EnemyDeath,
}
