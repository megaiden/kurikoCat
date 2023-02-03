using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class OpenChest : MonoBehaviour
{
    public Canvas SpaceBar;
    public int number;
    [SerializeField] private ChestAugment[] ChestContents;
    private ChestAugment CurrentAugment;
    // Start is called before the first frame update
    void Start()
    {   
        SpaceBar.enabled = false;
        CurrentAugment = ChestContents[Random.Range(0,2)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnter(){
        SpaceBar.enabled = true;
    }

    public void OnExit(){
        SpaceBar.enabled = false;
    }
    public ChestAugment GetAugment(){
        return CurrentAugment;
    }

    [System.Serializable]
    public struct ChestAugment {
        [SerializeField] public Sprite _SpriteAugment;
        [SerializeField] public string _NameAugment;
    }
}
