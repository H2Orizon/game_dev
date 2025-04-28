using UnityEngine;
using TMPro;

public class TeamComparer : MonoBehaviour{
    int[,] teams ={
/*        {350, 200, 200, 150, 100},
        {400, 300, 200, 100, 0},
        {300, 300, 200, 150, 50},
        {380, 250, 200, 100, 70},
        {370, 220, 210, 100, 100} */
    };
    
    int[] teamPoints = new int[5];

    public TextMeshProUGUI[] teamTexts;
    private int winnerIndex = -1;

    private Color baseColor = Color.white;
    private Color glowColor = Color.yellow;
    private float glowSpeed = 2f;

    void Start(){
        int numTeams = teams.GetLength(0);
        int numps = teams.GetLength(1);

        for (int i = 0; i < numTeams - 1; i++){
            for (int j = i + 1; j < numTeams; j++)
            {
                int winI = 0;
                int winJ = 0;

                for (int p = 0; p < numps; p++){
                    if (teams[i, p] > teams[j, p]) winI++;
                    else if (teams[i, p] < teams[j, p]) winJ++;
                }

                if (winI > winJ) teamPoints[i] += 2;
                else if (winI == winJ){
                    teamPoints[i] += 1;
                    teamPoints[j] += 1;
                }
                else teamPoints[j] += 2;
            }
        }

        int maxPoints = -1;
        for (int t = 0; t < numTeams; t++){
            Debug.Log($"Команда {t + 1} отримала {teamPoints[t]} балів.");

            if (teamPoints[t] > maxPoints){
                maxPoints = teamPoints[t];
                winnerIndex = t;
            }

            if (teamTexts != null && teamTexts.Length > t && teamTexts[t] != null){
                teamTexts[t].text = $"Команда {t + 1}: {teamPoints[t]} балів";
            }
        }

        if (winnerIndex != -1)
            Debug.Log($"Команда {winnerIndex + 1} перемогла, набравши {teamPoints[winnerIndex]} балів.");
    }

    void Update(){
        if (winnerIndex != -1 && teamTexts != null && winnerIndex < teamTexts.Length){
            float glow = (Mathf.Sin(Time.time * glowSpeed) + 1f) / 2f;
            Color lerped = Color.Lerp(baseColor, glowColor, glow);
            teamTexts[winnerIndex].color = lerped;
        }
    }
}
