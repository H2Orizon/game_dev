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
            statusText.text = "�� �������� ID ������.";
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
            statusText.text = "������� ��������! ������� ����� �������...";
            StartCoroutine(WaitForAllSubmissions());
        } else {
            statusText.text = "������� �����������!";
        }
    }


    IEnumerator WaitForAllSubmissions(){
        while (true){
            UnityWebRequest request = UnityWebRequest.Get(checkUrl);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success){
                string json = request.downloadHandler.text;
                  Debug.Log("��������� ����� JSON: " + json);
                if (json.Contains("\"status\":\"cleared\"")){
                    statusText.text = "�� ������ ��������. �������� ����������!";
                    yield return new WaitForSeconds(1f);

                    FindObjectOfType<ResultFetcher>().FetchResults();
                    break;
                } else {
                    statusText.text = "������� ����� �������...";
                }
            } else {
                statusText.text = "������� �������� �������.";
            }

            yield return new WaitForSeconds(2f);
        }
    }
}
