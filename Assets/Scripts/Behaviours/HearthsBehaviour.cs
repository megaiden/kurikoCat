using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public  class  HearthsBehaviour : MonoBehaviour
    {
        [SerializeField] public List<GameObject> hearthParent;



        public void OnDamagePlayer()
        {
            hearthParent.RemoveAt(hearthParent.Count);
        }
    }
}