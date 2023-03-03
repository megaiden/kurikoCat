using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static GameObject _soundCreated = null;
    private static GameObject _MusicStage = null;
    private static AudioSource _audioSource = null;
    private static bool _alreadyRemoved, _shouldPlayMusicStage;
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
        CatMeow1 = 15,
        CatMeow2 = 16,
        CatMeow3 = 17,
        CatMeow4 = 18,
        CatMeow5 = 19,
        CatMeow6 = 20,
        MenuMusic = 21,
        StageMusic = 22,
        GameOver = 23,
        EndingMusic = 24
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
    public static void PlaySound(Sound sound, bool isLoop = false, float volume = 1.0f)
    {
        if (CanPlaySound(sound))
        {
            if (!_soundCreated)
            {
                _soundCreated = new GameObject("Sound");
            }

            AudioSource audioSource = null;
            if (!_soundCreated.GetComponent<AudioSource>())
            {
                audioSource = _soundCreated.AddComponent<AudioSource>();    
            }
            else
            {
                audioSource = _soundCreated.GetComponent<AudioSource>();    
            }
            
            if (isLoop)
                audioSource.loop = true;
            else
                audioSource.loop = false;

            audioSource.clip = GetAudioClip(sound);
            audioSource.volume = volume;
            audioSource.Play();
        }
    }
    
    public static void PlayStageMusic(Sound sound, bool isLoop = false, float volume = 1.0f)
    {

        if (CanPlaySound(sound))
        {
            if (!_MusicStage)
            {
                _MusicStage = new GameObject("MusicStage");
            }


            if (!_MusicStage.GetComponent<AudioSource>())
            {
                _audioSource = _MusicStage.AddComponent<AudioSource>();    
            }
            else
            {
                _audioSource = _MusicStage.GetComponent<AudioSource>();    
            }
            
            if (isLoop)
                _audioSource.loop = true;
            else
                _audioSource.loop = false;

            _audioSource.clip = GetAudioClip(sound);
            _audioSource.volume = volume;
            _audioSource.Play();
        }
    }
    
    public static void StopStageMusic()
    {
        if (!_alreadyRemoved)
        {
            Destroy(_MusicStage);
            _alreadyRemoved = true;
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
