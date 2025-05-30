using UnityEngine;

public static class ServerConfig{
    public static string Host = "localhost:80"; // localhost for ngrok

    public static string BaseUrl => $"http://{Host}/drone_game";

    public static void SetServer(string newHost){
        if (!string.IsNullOrWhiteSpace(newHost))
            Host = newHost;

        Debug.Log($"Сервер оновлено: {BaseUrl}");
    }
}
