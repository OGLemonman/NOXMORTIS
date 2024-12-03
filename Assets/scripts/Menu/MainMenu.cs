
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {
    public RectTransform controlsPanel;

    public void OnPlayClicked() {
        SceneManager.LoadScene("Level");
    }

    public void OnControlsClicked() {
        controlsPanel.gameObject.SetActive(true);
    }

    public void OnControlsBackClicked() {
        controlsPanel.gameObject.SetActive(false);
    }

    public void OnExitClicked() {
        Application.Quit();
    }
}
