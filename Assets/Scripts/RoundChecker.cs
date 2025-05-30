using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class RoundChecker : MonoBehaviour{
    public string checkUrl = "http://localhost/drone_game/check_round.php";
    public float checkInterval = 2.0f;
    public UnityEvent onRoundCleared;
    public UnityEvent onWaiting;
    private bool checking = false;

    void Start(){
        StartChecking();
    }

    public void StartChecking(){
        if (!checking){
            StartCoroutine(CheckLoop());
        }
    }

    IEnumerator CheckLoop(){
        checking = true;

        while (true){
            using (UnityWebRequest www = UnityWebRequest.Get(checkUrl)){
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError){
                    Debug.LogError("Check failed: " + www.error);
                }
                else{
                    var json = www.downloadHandler.text;
                    Debug.Log("Server response: " + json);
                    RoundStatus status = JsonUtility.FromJson<RoundStatus>(json);
                    if (status.status == "cleared"){
                        if (onRoundCleared != null)
                            onRoundCleared.Invoke();
                        yield break;
                    }
                    else if (status.status == "waiting" && onWaiting != null){
                        onWaiting.Invoke();
                    }
                }
            }
            yield return new WaitForSeconds(checkInterval);
        }
    }

    [System.Serializable]
    public class RoundStatus
    {
        public string status;
        public int submitted;
        public int total;
    }
}
