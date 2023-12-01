using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    #region Custom Methods

    [Header("Audio")] 
    public AudioSource button;

    // Public Function that change the scene to the menu. Single mode.
    public void SceneMenu()
    {
        button.Play();
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    
    
    // Public Function that change the scene to the chosen map. Single mode.
    public void SceneMap(string mapName)
    {
        button.Play();
        SceneManager.LoadScene(mapName, LoadSceneMode.Single);
    }


    // Public Function that change the scene to the end scene. Single mode.
    public void EndScene()
    {
        SceneManager.LoadScene("EndScene", LoadSceneMode.Single);
    }

    #endregion
}
