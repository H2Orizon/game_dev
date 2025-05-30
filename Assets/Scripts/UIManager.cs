using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour {
    public GameObject mainMenuCanvas;
    public GameObject serverCanvas;
    public GameObject registerCanvas;
    public GameObject waitingCanvas;
    public GameObject gameCanvas;
    public GameObject scoreboardCanvas;

    public TMP_InputField[] droneInputFields;

    void Start() {
        SetDefaultInputs();
        ShowMainMenu(); // Починаємо з головного меню
    }

    public void SetDefaultInputs() {
        foreach (var input in droneInputFields) {
            input.text = "0";
        }
    }

    public void ShowMainMenu() {
        mainMenuCanvas.SetActive(true);
        serverCanvas.SetActive(false);
        registerCanvas.SetActive(false);
        waitingCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        scoreboardCanvas.SetActive(false);
    }

    public void ShowServer() {
        mainMenuCanvas.SetActive(false);
        serverCanvas.SetActive(true);
        registerCanvas.SetActive(false);
        waitingCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        scoreboardCanvas.SetActive(false);
    }

    public void ShowRegister() {
        mainMenuCanvas.SetActive(false);
        serverCanvas.SetActive(false);
        registerCanvas.SetActive(true);
        waitingCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        scoreboardCanvas.SetActive(false);
    }

    public void ShowGame() {
        mainMenuCanvas.SetActive(false);
        serverCanvas.SetActive(false);
        registerCanvas.SetActive(false);
        waitingCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        scoreboardCanvas.SetActive(false);
    }

    public void ShowScoreboard() {
        mainMenuCanvas.SetActive(false);
        serverCanvas.SetActive(false);
        registerCanvas.SetActive(false);
        waitingCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        scoreboardCanvas.SetActive(true);
    }

    public void BackToGame() {
        mainMenuCanvas.SetActive(false);
        serverCanvas.SetActive(false);
        registerCanvas.SetActive(false);
        waitingCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        scoreboardCanvas.SetActive(false);
    }

    public void PlayGame() {
        ShowServer();
    }

    public void ExitGame() {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
