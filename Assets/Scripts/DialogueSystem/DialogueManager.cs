using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Animator animator;
    public float timeBetweenLetters;
    public bool isOnScreen;
    
    [Header("Action Button")]
    public UnityEngine.UI.Button actionButton;
    public TextMeshProUGUI actionButtonText;
    public static Action OnDialogEnded;

    private Queue<string> sentences;
    private string replaceText;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue, bool showActionAtEndDialogue = false, string actionButtonText = "", ActionType actionType = ActionType.None, string replaceText = "")
    {

        isOnScreen = true;
        this.actionButtonText.text = actionButtonText;
        actionButton.onClick.AddListener(() => CallAction(actionType));
        actionButton.gameObject.SetActive(false);
        this.replaceText = replaceText;

        animator.SetBool("IsOpen", true);
        nameText.text = dialogue.name + " :";
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        { 
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence(showActionAtEndDialogue);
    }

    public void DisplayNextSentence(bool showActionAtEndDialogue)
    {
        if (sentences.Count == 1)
        {
            actionButton.gameObject.SetActive(showActionAtEndDialogue);
        }
        else if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        if (!string.IsNullOrEmpty(replaceText) && sentence.Contains("-1"))
        {
            sentence = sentence.Replace("-1", replaceText);
            replaceText = "";
        }
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(timeBetweenLetters);
        }
    }

    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        isOnScreen = false;
        OnDialogEnded?.Invoke();
    }

    #region Actions for ActionButton
    public enum ActionType
    { 
        None = 0,
        ResetKeyPannel = 1,
        WinChallenge = 2,
        OpenMiniGame = 3,
    }

    public void CallAction(ActionType type)
    {
        switch (type)
        {
            case ActionType.None:
                break;
            case ActionType.ResetKeyPannel:
                ResetKeyPannel();
                break;
            case ActionType.WinChallenge:
                WinChallenge();
                break;
            case ActionType.OpenMiniGame:
                OpenMiniGame();
                break;
        }
    }

    private void ResetKeyPannel()
    {
        var pannel = FindObjectOfType<KeyPannel>();
        pannel.Reset();
        EndDialogue();
    }

    private void WinChallenge()
    {
        var pannel = FindObjectOfType<KeyPannel>();
        pannel.WinChallenge();
        EndDialogue();
    }

    private void OpenMiniGame()
    {
        var pannel = FindObjectOfType<KeyPannel>();
        pannel.Initialize();
        EndDialogue();
        pannel.GetComponent<DialogueTrigger>().TriggerDialogue();
    }
    #endregion
}
