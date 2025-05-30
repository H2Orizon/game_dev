using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

[System.Serializable]
public class PlayerCountResponse {
    public int player_id;
}

public class WaitingRoom : MonoBehaviour {

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI statusText;
    public GameObject waitingCanvas;
    public GameObject gameCanvas;
    public Button readyButton;

    private float totalTime = 0f;
    private float maxTime = 120f;
    private string startGameUrl = "http://localhost:80/drone_game/start_game.php";
    private string setReadyUrl = "http://localhost:80/drone_game/set_ready.php?email=Test1@gmail.com";
    private bool gameStarted = false;
    private bool isReady = false;

    void Start() {
        string email = PlayerPrefs.GetString("player_email", "");
        Debug.LogError(email);
        if (string.IsNullOrEmpty(email)) {
            return;
        }

        setReadyUrl = $"http://localhost:80/drone_game/set_ready.php?email={UnityWebRequest.EscapeURL(email)}";

        readyButton.onClick.AddListener(OnReadyClicked);
        StartCoroutine(CheckPlayersPeriodically());
    }

    public void OnReadyClicked() {
        if (!isReady) {
            StartCoroutine(SetReadyStatus());
        }
    }

    IEnumerator SetReadyStatus() {
        UnityWebRequest request = UnityWebRequest.Get(setReadyUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success) {
            isReady = true;
            statusText.text = "Статус: готовий!";
            StartCoroutine(WaitingCountdown());
        } else {
            statusText.text = "Не вдалося встановити статус 'ready'.";
            Debug.LogError(request.error);
        }
    }

    IEnumerator WaitingCountdown() {
        while (!gameStarted) {
            if (totalTime > 0f) {
                totalTime -= Time.deltaTime;
                int minutes = Mathf.FloorToInt(totalTime / 60);
                int seconds = Mathf.FloorToInt(totalTime % 60);
                timerText.text = $"Очікування: {minutes:D2}:{seconds:D2}";
            } else {
                yield return StartCoroutine(CheckPlayers(true));
            }
            yield return null;
        }
    }

    IEnumerator CheckPlayersPeriodically() {
        while (!gameStarted) {
            yield return StartCoroutine(CheckPlayers(false));
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator CheckPlayers(bool finalCheck) {
        UnityWebRequest request = UnityWebRequest.Get(startGameUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success) {
            PlayerCountResponse response = JsonUtility.FromJson<PlayerCountResponse>(request.downloadHandler.text);
            int playerCount = response.player_id;
            Debug.Log("Гравців готових: " + playerCount);

            if (playerCount >= 2) {
                StartGame();
                yield break;
            } else {
                if (finalCheck) {
                    statusText.text = "Недостатньо гравців для початку гри.";
                    totalTime = maxTime;
                } else {
                    statusText.text = "Очікуємо більше готових гравців...";
                    totalTime = maxTime;
                }
            }
        } else {
            statusText.text = "Помилка при перевірці гравців.";
            Debug.LogWarning("Помилка запиту: " + request.error);
        }
    }

    void StartGame() {
        gameStarted = true;
        waitingCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        statusText.text = "Гра почалась!";
        Debug.Log("Гра почалась!");
    }
}
