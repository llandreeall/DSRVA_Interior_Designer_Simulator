using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManagerUI : MonoBehaviour
{

    public void LoadNewScene()
    {
        SceneManager.LoadScene("NewSceneCreation", LoadSceneMode.Single);
    }

    public void LoadSavedScenes()
    {
        SceneManager.LoadScene("SavedScenes", LoadSceneMode.Single);
    }

    public void LoadGallery()
    {
        SceneManager.LoadScene("GalleryScene", LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
