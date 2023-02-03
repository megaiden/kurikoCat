using System;
using UnityEngine;

public class KeyPannel : MonoBehaviour
{
    public Transform parent;
    public Transform initPosition;
    public Key keyPrefab;
    public int numberOfKeys;
    public int rows;
    public int columns;
    public float xSpace;
    public float ySpace;
    public bool keyWasFound;
    public bool isGameOver;
    public bool isOnScreen;

    [Header("Dialogues")]
    public Dialogue winnerDialogue;
    public string winButtonText;
    public Dialogue looserDialogue;
    public string looseButtonText;

    [Header("Scape Tilemap")]
    public GameObject scapeTilemapGO;

    private int openDoorKey;
    public int OpenDoorKey => openDoorKey;
    public static Action OnKeyFound;

    public void Initialize()
    {
        DialogueManager.OnDialogEnded -= HidePannel;
        DialogueManager.OnDialogEnded += HidePannel;

        openDoorKey = UnityEngine.Random.Range(1, numberOfKeys + 1);
        Debug.Log($"FOR TESTING PURPOSES: The right key is {openDoorKey}");
        isGameOver = false;

        if (!parent.gameObject.activeSelf)
            ShowPannel(true);

        for (int i = 0; i < numberOfKeys; i++)
        {
            var key = Instantiate(keyPrefab, new Vector3( initPosition.position.x + xSpace * (i % columns), 
                                                initPosition.position.y + (-ySpace * (i / columns))), 
                        Quaternion.identity,
                        parent);

            key.SetText(i+1, this);
        }
    }

    public void HidePannel()
    {
        if(isGameOver)
            ShowPannel(false);
    }

    public void ShowPannel(bool value)
    {
        parent.gameObject.SetActive(value);
        isOnScreen = value;
    }

    public void Reset()
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        { 
            Destroy(parent.GetChild(i).gameObject);
        }

        Initialize();
    }

    public void WinChallenge()
    {
        Debug.Log("YOU WIN!!! Do something!. Open the door??");
        OnKeyFound?.Invoke();
        scapeTilemapGO.SetActive(true);
    }

    public static void CheckKey(Key key, KeyPannel pannel)
    {
        var openDoorKey = pannel.OpenDoorKey;
        if (key.ID == openDoorKey)
        {
            Debug.Log("YOU HAVE THE RIGHT KEY!");
            key.ChangeColor(Color.green);
            pannel.keyWasFound = true;
            AudioManager.PlaySound(AudioManager.Sound.rightSelection, false);
            pannel.WinChallenge();
        }
        else
        {
            Debug.Log($"Wrong key... the right one is {openDoorKey}");
            key.ChangeColor(Color.red);
            AudioManager.PlaySound(AudioManager.Sound.wrongSelection, false);
        }

        pannel.SendResultDialogue(key.ID, pannel);
    }

    private void SendResultDialogue(int keyID, KeyPannel pannel)
    {
        if (keyID == pannel.OpenDoorKey)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(winnerDialogue, true, winButtonText, DialogueManager.ActionType.WinChallenge);
        }
        else
        {
            FindObjectOfType<DialogueManager>().StartDialogue(looserDialogue, true, looseButtonText, DialogueManager.ActionType.ResetKeyPannel, pannel.openDoorKey.ToString());
        }

        isGameOver = true;
    }
}
