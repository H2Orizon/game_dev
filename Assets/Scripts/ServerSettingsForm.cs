using UnityEngine;
using TMPro;

public class ServerSettingsForm : MonoBehaviour
{
    public TMP_InputField hostInput;
    public UIManager uiManager;

    public void ApplySettings(){
        string host = hostInput.text.Trim();

        ServerConfig.SetServer(host);
        
        if (uiManager != null){
            uiManager.ShowRegister();
        }
        else{
            Debug.LogError("UIManager не призначено в ServerSettingsForm!");
        }
    }
}
