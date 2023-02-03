using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI text;
    public int ID;
    public KeyPannel pannel;

    public void SetText(int id, KeyPannel pannel)
    {
        text.text = id.ToString();
        ID = id;
        this.pannel = pannel;
    }

    public int GetID()
    {
        return ID;
    }

    public void ChangeColor(Color color)
    { 
        image.color = color;
    }

    public void OnClick()
    {
        var DialogueManager = FindObjectOfType<DialogueManager>();
        if (!DialogueManager.isOnScreen)
        {
            Debug.Log($"Hi. I'm the key {ID}");
            KeyPannel.CheckKey(this, pannel);
        }
    }
}
