using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class DroneSubmitAndWait : MonoBehaviour {
    public TMP_InputField[] droneInputs;
    public TextMeshProUGUI statusText;
    private string submitUrl = ServerConfig.BaseUrl + "/submit_move.php";
    private string checkUrl = ServerConfig.BaseUrl + "/clear_if_ready.php";

    public void OnSubmitClicked(string[] values){
        StartCoroutine(SendDroneData(values));
    }

    IEnumerator SendDroneData(string[] values){
        int playerId = PlayerPrefs.GetInt("player_id", -1);
        if (playerId == -1){
            statusText.text = "Не знайдено ID гравця.";
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("player_id", playerId);

        for (int i = 0; i < values.Length; i++){
            form.AddField($"planet_{i + 1}", values[i]);
        }

        UnityWebRequest request = UnityWebRequest.Post(submitUrl, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success){
            statusText.text = "Розподіл надіслано! Очікуємо інших гравців...";
            StartCoroutine(WaitForAllSubmissions());
        } else {
            statusText.text = "Помилка відправлення!";
        }
    }


    IEnumerator WaitForAllSubmissions(){
        while (true){
            UnityWebRequest request = UnityWebRequest.Get(checkUrl);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success){
                string json = request.downloadHandler.text;
                  Debug.Log("Отриманий сирий JSON: " + json);
                if (json.Contains("\"status\":\"cleared\"")){
                    statusText.text = "Всі гравці надіслали. Показуємо результати!";
                    yield return new WaitForSeconds(1f);

                    FindObjectOfType<ResultFetcher>().FetchResults();
                    break;
                } else {
                    statusText.text = "Очікуємо інших гравців...";
                }
            } else {
                statusText.text = "Помилка перевірки статусу.";
            }

            yield return new WaitForSeconds(2f);
        }
    }
}
