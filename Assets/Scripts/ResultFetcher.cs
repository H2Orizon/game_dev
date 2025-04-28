using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class ResultFetcher : MonoBehaviour {
    public TextMeshProUGUI resultsText;
    private string resultsUrl = "http://localhost:80/drone_game/get_results.php";

    public void FetchResults(){
        StartCoroutine(GetResults());
    }

    IEnumerator GetResults(){
        UnityWebRequest request = UnityWebRequest.Get(resultsUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success){
            resultsText.text = "Результати гри:\n" + request.downloadHandler.text;
        }else{
            resultsText.text = "Не вдалося отримати результати.";
        }
    }
}
