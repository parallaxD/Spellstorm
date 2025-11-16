using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject gamePanel;

    private GameObject[] panels;

    void Awake()
    {
        panels = new GameObject[]
        {
            mainMenuPanel,
            gamePanel,
            settingsPanel
        };

        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        SetAllPanelsInactive();
        mainMenuPanel.SetActive(true);
    }

    public void ShowGame()
    {
        SetAllPanelsInactive();
        gamePanel.SetActive(true);
    }

    public void ShowSettings()
    {
        SetAllPanelsInactive();
        settingsPanel.SetActive(true);
    }

    private void SetAllPanelsInactive()
    {
        foreach (var panel in panels)
        {
            panel.SetActive(false);
        }
    }
}
