using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;
    public static GameAssets Instance
    {
        get
        {
            if (instance == null)
                instance = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return instance;
        }
    }

    public SoundAudioClip[] soundAudioClips;
}

[System.Serializable]
public class SoundAudioClip
{
    public AudioManager.Sound sound;
    public AudioClip audioclip;
}
