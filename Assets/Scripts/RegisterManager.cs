using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class RegisterManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField usernameInput;
    public Button registerButton;
    public TextMeshProUGUI statusText;

    [Header("Canvas Panels")]
    public GameObject registerCanvas;
    public GameObject mainCanvas;

    private string registerUrl = "http://localhost:80/drone_game/register.php";

    void Start()
    {
        if (registerButton != null)
        {
            registerButton.onClick.AddListener(OnRegisterClicked);
        }
        else
        {
            Debug.LogError("Register Button is not assigned!");
        }
    }

    void OnRegisterClicked()
    {
        if (usernameInput == null || statusText == null)
        {
            Debug.LogError("usernameInput або statusText не призначено в інспекторі!");
            return;
        }

        string username = usernameInput.text.Trim();

        if (string.IsNullOrEmpty(username))
        {
            statusText.text = "Введіть ім'я!";
            return;
        }

        StartCoroutine(RegisterPlayer(username));
    }

    IEnumerator RegisterPlayer(string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);

        UnityWebRequest request = UnityWebRequest.Post(registerUrl, form);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            statusText.text = $"Помилка: {request.error}";
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Отримано відповідь: " + jsonResponse);

            PlayerResponse response = JsonUtility.FromJson<PlayerResponse>(jsonResponse);
            int playerId = response.player_id;

            statusText.text = $"Зареєстровано! Ваш ID: {playerId}";
            PlayerPrefs.SetInt("player_id", playerId);

            OnRegisterSuccess();
        }
    }

    [System.Serializable]
    public class PlayerResponse
    {
        public int player_id;
    }

    public void OnRegisterSuccess()
    {
        if (registerCanvas != null) registerCanvas.SetActive(false);
        if (mainCanvas != null) mainCanvas.SetActive(true);
    }
}
