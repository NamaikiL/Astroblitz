using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Manager
{
    public class SceneScript : MonoBehaviour
    {
        #region Variables

        [Header("Audio")] 
        public AudioSource button;

        #endregion
    
        #region Custom Methods

        /**
         * <summary>
         * Function that change the scene to the menu. Single mode.
         * </summary>
         */
        public void SceneMenu()
        {
            button.Play();
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    
    
        /**
         * <summary>
         * Function that change the scene to the chosen map. Single mode.
         * </summary>
         */
        public void SceneMap(string mapName)
        {
            button.Play();
            SceneManager.LoadScene(mapName, LoadSceneMode.Single);
        }


        /**
         * <summary>
         * Function that change the scene to the end scene. Single mode.
         * </summary>
         */
        public void EndScene()
        {
            SceneManager.LoadScene("EndScene", LoadSceneMode.Single);
        }

        #endregion
    }
}
