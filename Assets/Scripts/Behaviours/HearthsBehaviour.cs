
using System.Linq;
using UnityEngine;


namespace Behaviours
{
    public  class  HearthsBehaviour : MonoBehaviour
    {
        [SerializeField] public Transform hearthParent;
        [SerializeField] public GameObject prefabHearth;


        public void OnLosingHearth()
        {
            hearthParent.Cast<Transform>().Last(x => x.gameObject.activeInHierarchy).gameObject.SetActive(false);
        }
        
        public void OnRecoveringHearthPlayer()
        {
            Instantiate(prefabHearth, hearthParent, true);
        }
    }
}