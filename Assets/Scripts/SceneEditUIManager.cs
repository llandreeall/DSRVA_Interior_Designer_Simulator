using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEditUIManager : MonoBehaviour
{
    public GameObject mainEditPanel;
    public GameObject objectEditPanel;
    public GameObject colorWheelPanel;
    public GameObject settingsPanel;
    public GameObject exitEditPanel;

    private void Start()
    {
        GoToMainEditPanel();
    }
    public void GoToMainEditPanel()
    {
        mainEditPanel.SetActive(true);
        objectEditPanel.SetActive(false);
        colorWheelPanel.SetActive(false);
        settingsPanel.SetActive(false);
        exitEditPanel.SetActive(false);
    }

    public void GoToObjectEditPanel()
    {
        mainEditPanel.SetActive(false);
        objectEditPanel.SetActive(true);
        colorWheelPanel.SetActive(false);
        settingsPanel.SetActive(false);
        exitEditPanel.SetActive(false);
    }

    bool isColorWheelActive = false;
    public void DisplayColorWheel()
    {
        if(isColorWheelActive == true)
            colorWheelPanel.SetActive(false);
        else
            colorWheelPanel.SetActive(true);
        isColorWheelActive = !isColorWheelActive;
    }

    bool isSettingsActive = false;
    public void DisplaySettings()
    {
        if (isSettingsActive == true)
            settingsPanel.SetActive(false);
        else
            settingsPanel.SetActive(true);
        isSettingsActive = !isSettingsActive;
    }

    public void HideSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void DisplayExitPanel()
    {
        exitEditPanel.SetActive(true);
    }

    public void HideExitPanel()
    {
        exitEditPanel.SetActive(false);
    }

    public void LoadMainmenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
