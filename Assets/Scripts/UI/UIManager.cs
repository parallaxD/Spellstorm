using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject gamePanel;
    public GameObject pausePanel;

    private GameObject[] panels;
    private GameObject lastPanel;

    public PauseManager pauseManager;

    void Awake()
    {

        panels = new GameObject[]
        {
            mainMenuPanel,
            gamePanel,
            settingsPanel,
            pausePanel
        };

        ShowMainMenu();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPause();
        }
    }

    public void ShowMainMenu()
    {
        pauseManager.PauseGame();
        ShowPanel(mainMenuPanel);
    }

    public void ShowGame()
    {
        pauseManager.ResumeGame();
        ShowPanel(gamePanel);
    }

    public void ShowPause()
    {
        pauseManager.PauseGame();
        ShowPanel(pausePanel);
    }

    public void ShowSettings()
    {
        ShowPanel(settingsPanel);
    }

    public void CloseSettings()
    {
        ShowPanel(lastPanel);
    }

    private void ShowPanel(GameObject panel)
    {
        SetAllPanelsInactive();
        panel.SetActive(true);

        if (panel != settingsPanel) lastPanel = panel;
    }

    private void SetAllPanelsInactive()
    {
        foreach (var panel in panels)
        {
            panel.SetActive(false);
        }
    }
}
