using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ResultData {
    public string username;
    public int[] planets;
}

[System.Serializable]
public class ResultDataList {
    public ResultData[] results;
}

public class ResultFetcher : MonoBehaviour {
    public GameObject teamTextPrefab;
    public Transform container;

    [Header("UI")]
    public UIManager uiManager;
    public GameObject[] dronesToHide;

    [Header("Переможець: оформлення")]
    public Color winnerColor = new Color(1f, 0.9f, 0.3f);
    public Vector2 glowDistance = new Vector2(2f, -2f);

    private string resultsUrl = ServerConfig.BaseUrl + "/get_results.php";

    public void FetchResults() {
        StartCoroutine(GetResults());
    }

    IEnumerator GetResults() {
        UnityWebRequest request = UnityWebRequest.Get(resultsUrl);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success) {
            Debug.LogError("Не вдалося отримати результати: " + request.error);
            yield break;
        }

        string rawJson = request.downloadHandler.text;
        Debug.Log("Отриманий сирий JSON: " + rawJson);

        ResultDataList parsed = JsonUtility.FromJson<ResultDataList>(rawJson);
        if (parsed?.results == null) {
            Debug.LogError("Не вдалося десеріалізувати JSON або results == null");
            yield break;
        }

        foreach (Transform child in container) {
            Destroy(child.gameObject);
        }

        List<int> scores = new List<int>();
        foreach (var result in parsed.results) {
            int score = result.planets?.Sum() ?? 0;
            scores.Add(score);
        }

        int maxScore = scores.Max();

        for (int i = 0; i < parsed.results.Length; i++) {
            GameObject entry = Instantiate(teamTextPrefab, container);
            TextMeshProUGUI text = entry.GetComponent<TextMeshProUGUI>();

            int score = scores[i];
            text.text = $"{parsed.results[i].username}: {score} балів";

            if (score == maxScore) {
                text.color = winnerColor;
                text.fontMaterial.EnableKeyword("OUTLINE_ON");
                text.fontMaterial.SetFloat("_OutlineWidth", 0.2f);
                text.fontMaterial.SetColor("_OutlineColor", Color.yellow);
            }
        }

        GameObject[] drones = GameObject.FindGameObjectsWithTag("Drone");
        foreach (var drone in drones) {
            Destroy(drone);
        }

        if (uiManager != null) {
            uiManager.ShowScoreboard();
        }
    }
}
