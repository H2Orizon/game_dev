using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TeamComparer : MonoBehaviour {
    private int[,] teams;
    private int[] teamPoints;
    private int winnerIndex = -1;

    private List<TextMeshProUGUI> teamTexts = new List<TextMeshProUGUI>();

    private Color baseColor = Color.white;
    private Color glowColor = Color.yellow;
    private float glowSpeed = 2f;

    public void SetTeams(int[,] teamsData) {
        teams = teamsData;
        teamPoints = new int[teams.GetLength(0)];
        CalculateResults();
    }

    public void SetTeamTexts(List<TextMeshProUGUI> texts) {
        teamTexts = texts;
    }

    private void CalculateResults() {
        int numTeams = teams.GetLength(0);
        int numPlanets = teams.GetLength(1);

        for (int i = 0; i < numTeams - 1; i++) {
            for (int j = i + 1; j < numTeams; j++) {
                int winI = 0, winJ = 0;

                for (int p = 0; p < numPlanets; p++) {
                    if (teams[i, p] > teams[j, p]) winI++;
                    else if (teams[i, p] < teams[j, p]) winJ++;
                }
                if (winI > winJ) teamPoints[i] += 2;
                else if (winI == winJ) {
                    teamPoints[i] += 1;
                    teamPoints[j] += 1;
                } else {
                    teamPoints[j] += 2;
                }
            }
        }

        int maxPoints = -1;
        for (int t = 0; t < numTeams; t++) {
            if (teamTexts != null && teamTexts.Count > t && teamTexts[t] != null) {
                teamTexts[t].text += $" | Очки: {teamPoints[t]}";
            }
            if (teamPoints[t] > maxPoints) {
                maxPoints = teamPoints[t];
                winnerIndex = t;
            }
        }

        if (winnerIndex != -1) {
            Debug.Log($"Команда {winnerIndex + 1} перемогла з {teamPoints[winnerIndex]} очками.");
        }
    }

    void Update() {
        if (winnerIndex != -1 && teamTexts != null && winnerIndex < teamTexts.Count) {
            float glow = (Mathf.Sin(Time.time * glowSpeed) + 1f) / 2f;
            Color lerped = Color.Lerp(baseColor, glowColor, glow);
            teamTexts[winnerIndex].color = lerped;
        }
    }
}
