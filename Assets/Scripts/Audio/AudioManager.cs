using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static GameObject _soundCreated = null;
    public enum Sound 
    {
        None = 0,
        PlayerMove = 1,
        PlayerDamaged = 2,
        PlayerOnePercent = 3,
        PlayerDied = 4,
        LightSwitch = 5,

        GameRestart = 10,
        MainMenu = 11,
        Background = 12,
        rightSelection = 13,
        wrongSelection = 14,
    }

    private static Dictionary<Sound, float> soundTimerDictionary;
    public static void Initialize()
    { 
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.PlayerMove] = 0f;
    }

    /// <summary>
    /// To be called from any part in the game where sound is needed. Example: AudioManager.PlaySound(AudioManager.Sound.rightSelection, false);
    /// </summary>
    public static void PlaySound(Sound sound, bool isLoop = false)
    {
        if (CanPlaySound(sound))
        {
            if (!_soundCreated)
            {
                _soundCreated = new GameObject("Sound");
            }

            AudioSource audioSource = _soundCreated.AddComponent<AudioSource>();
            if (isLoop)
                audioSource.loop = true;
            else
                audioSource.loop = false;

            audioSource.clip = GetAudioClip(sound);
            audioSource.Play();
        }
    }

    public static void PlaySound(Sound sound, Vector3 position, bool isLoop = false)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.transform.position = position;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            if (isLoop)
                audioSource.loop = true;
            else
                audioSource.loop = false;

            audioSource.clip = GetAudioClip(sound);
            audioSource.Play();
        }
    }

    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            case Sound.PlayerMove:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float playerMoveTimeMax = 0.25f; // walkspeed-ish
                    if (lastTimePlayed + playerMoveTimeMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return true;
                    
            default:
                return true;
        }
    }

    private static AudioClip GetAudioClip(Sound sound) 
    {
        foreach (SoundAudioClip clip in GameAssets.Instance.soundAudioClips)
        {
            if (clip.sound == sound)
                return clip.audioclip;
        }

        Debug.Log($"Error: Sound {sound} was not found...");
        return null;
    }
}
