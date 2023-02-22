using System.Collections;
using System.Linq;
using Behaviors;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Behaviours
{
    public  class  HearthsBehaviour : MonoBehaviour
    {
        [SerializeField] public Transform hearthParent;
        [SerializeField] public GameObject prefabHearth;
        [SerializeField] private Image blackOutImage;
        [SerializeField] private Image gameOverImage;

        private bool _isGameOver,_imagesAppeared;
        private PlayerActionsBehaviour _playerActionsBehaviour;

        private void Start()
        {
            _playerActionsBehaviour = transform.GetComponent<PlayerActionsBehaviour>();
        }

        public void OnLosingHearth()
        {

            var activeHearths = hearthParent.Cast<Transform>().Count(x => x.gameObject.activeInHierarchy);
            if (activeHearths > 0)
            {
                hearthParent.Cast<Transform>().Last(x => x.gameObject.activeInHierarchy).gameObject.SetActive(false);    
            }
            else if(!_isGameOver)
            {
                _playerActionsBehaviour.isGameOver = true;
                blackOutImage.enabled = true;
                gameOverImage.enabled = true;
                _isGameOver = true;
            }
        }
        
        public void OnRecoveringHearthPlayer()
        {
            hearthParent.Cast<Transform>().First(x => x.gameObject.activeInHierarchy == false).gameObject.SetActive(true);
        }
        public int GetActivePlayerHearths()
        {
            return hearthParent.Cast<Transform>().Count(x => x.gameObject.activeInHierarchy);
        }

        private void FixedUpdate()
        {
            if (_isGameOver)
            {
                if (FadeInImage(blackOutImage,1.5f))
                {
                    if ( FadeInImage(gameOverImage, .45f))
                    {
                        StartCoroutine(LoadMenu(3f));
                    }
                }
            }
        }
        
        private IEnumerator LoadMenu(float waitTime)
        {
            yield return new WaitForSeconds( waitTime );
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }

        private bool FadeInImage(Image imageToShow, float timeToFadeIn)
        {
            var alpha = imageToShow.color.a + (Time.deltaTime * timeToFadeIn);
       
            if (alpha <= 2.2f)
            {
                imageToShow.color = new Color(imageToShow.color.r, imageToShow.color.g, imageToShow.color.b, alpha);
                return false;
            }

            return true;
        }
    }
}