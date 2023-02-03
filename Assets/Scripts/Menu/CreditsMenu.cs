using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenu : MonoBehaviour
{
    public GameObject menuContainer;
    public void GoBack()
    { 
        menuContainer.SetActive(false);
    }
}
