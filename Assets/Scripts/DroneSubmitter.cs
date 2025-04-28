using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class DroneSubmitter : MonoBehaviour{
    public TMP_InputField[] droneInputs;
    public TextMeshProUGUI submitStatusText;
    private string submitUrl = "http://localhost:80/drone_game/submit_move.php";

    public void OnSubmitClicked(){
        StartCoroutine(SendDroneData());
    }

    IEnumerator SendDroneData(){
        WWWForm form = new WWWForm();
        int playerId = PlayerPrefs.GetInt("player_id", -1);
        if (playerId == -1) {
            submitStatusText.text = "Не знайдено ID гравця.";
            yield break;
        }

        form.AddField("player_id", playerId);

        for (int i = 0; i < droneInputs.Length; i++){
            string value = droneInputs[i].text;
            if (string.IsNullOrEmpty(value)) value = "0";
            form.AddField($"planet_{i + 1}", value);
        }

        UnityWebRequest request = UnityWebRequest.Post(submitUrl, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success){
            submitStatusText.text = "Розподіл надіслано!";
        }else{
            submitStatusText.text = "Помилка відправлення!";
        }
    }
}
