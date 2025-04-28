using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class WaitingRoom : MonoBehaviour {
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI statusText;
    public GameObject waitingCanvas;
    public GameObject gameCanvas;

    private float totalTime = 180f;
    private string startGameUrl = "http://localhost:80/drone_game/start_game.php";

    void Start(){
        StartCoroutine(WaitingCountdown());
        StartCoroutine(CheckPlayersPeriodically());
    }

    IEnumerator WaitingCountdown(){
        while (totalTime > 0f) {
            totalTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(totalTime / 60);
            int seconds = Mathf.FloorToInt(totalTime % 60);
            timerText.text = $"Очікування: {minutes:D2}:{seconds:D2}";
            yield return null;
        }

        yield return StartCoroutine(CheckPlayers(true));
    }

    IEnumerator CheckPlayersPeriodically(){
        while (totalTime > 0f) {
            yield return StartCoroutine(CheckPlayers(false));
            yield return new WaitForSeconds(5f);
        }
    }

    IEnumerator CheckPlayers(bool finalCheck){
        UnityWebRequest request = UnityWebRequest.Get(startGameUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success){
            int playerCount = int.Parse(request.downloadHandler.text);

            if (playerCount >= 2){
                StartGame();
                yield break;
            } else if (finalCheck){
                statusText.text = "Недостатньо гравців для початку гри.";
            }
        }else{
            statusText.text = "Помилка при перевірці гравців.";
        }
    }

    void StartGame(){
        waitingCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        Debug.Log("Гра почалась!");
    }
}
