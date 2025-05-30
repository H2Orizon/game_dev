using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class RegisterManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public Button registerButton;
    public TextMeshProUGUI statusText;

    [Header("Canvas Panels")]
    public GameObject registerCanvas;
    public GameObject mainCanvas;

    [Header("References")]
    public PlayerPinger playerPinger;

    private string registerUrl = ServerConfig.BaseUrl + "/register.php";

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
        string username = usernameInput.text.Trim();
        string email = emailInput.text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
        {
            statusText.text = "Введіть ім’я та email!";
            return;
        }

        StartCoroutine(RegisterPlayer(username, email));
    }

    IEnumerator RegisterPlayer(string username, string email)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("email", email);

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
            if (response.player_id > 0)
            {
                PlayerPrefs.SetInt("player_id", response.player_id);
                statusText.text = $"Успіх: {response.message}, ID: {response.player_id}";
                if (playerPinger != null)
                    playerPinger.StartPinging(response.player_id);

                OnRegisterSuccess();
            }
            else
            {
                statusText.text = $"Помилка: {response.message}";
            }
        }
    }

    [System.Serializable]
    public class PlayerResponse
    {
        public int player_id;
        public string message;
    }

    public void OnRegisterSuccess()
    {
        if (registerCanvas != null) registerCanvas.SetActive(false);
        if (mainCanvas != null) mainCanvas.SetActive(true);
    }
}
