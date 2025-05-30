using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerPinger : MonoBehaviour
{
    public int playerId;
    private Coroutine pingCoroutine;

    public void StartPinging(int id)
    {
        playerId = id;
        if (pingCoroutine == null)
            pingCoroutine = StartCoroutine(SendPingCoroutine());
    }

    private IEnumerator SendPingCoroutine()
    {
        while (true)
        {
            WWWForm form = new WWWForm();
            form.AddField("player_id", playerId);

            UnityWebRequest request = UnityWebRequest.Post("http://localhost/drone_game/ping.php", form);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
                Debug.LogWarning("Ping failed: " + request.error);

            yield return new WaitForSeconds(10f);
        }
    }
}
