using UnityEngine;
using TMPro;

public class WinnerHighlighter : MonoBehaviour {
    public TextMeshProUGUI[] scoreLabels;
    public GameObject[] planetEffects;

    public void HighlightWinner(int[] scores) {
        int maxScore = Mathf.Max(scores);
        for (int i = 0; i < scores.Length; i++) {
            if (scores[i] == maxScore) {
                scoreLabels[i].color = Color.yellow;
                planetEffects[i].SetActive(true);
            }
        }
    }
}
