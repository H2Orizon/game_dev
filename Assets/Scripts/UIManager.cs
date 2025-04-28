using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject registerCanvas;
    public GameObject gameCanvas;

    void Start()
    {
        ShowRegister();
    }

    public void ShowRegister()
    {
        registerCanvas.SetActive(true);
        gameCanvas.SetActive(false);
    }

    public void ShowGame()
    {
        registerCanvas.SetActive(false);
        gameCanvas.SetActive(true);
    }
}
