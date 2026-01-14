using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject gamePanel;
    public GameObject pausePanel;

    public GameObject burningWastelandsPanel;
    public GameObject floodedTemplesPanel;
    public GameObject crystalSteppesPanel;

    private GameObject[] panels;
    private GameObject lastPanel;

    public PauseManager pauseManager;
    public GenerationManager generationManager;

    void Awake()
    {
        panels = new GameObject[]
        {
            mainMenuPanel,
            gamePanel,
            settingsPanel,
            pausePanel,
            burningWastelandsPanel,
            floodedTemplesPanel,
            crystalSteppesPanel
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

    public void ShowResultPanel()
    {
        var location = generationManager.CurrentLocation;

        var panel = location switch
        {
            LocationType.BurningWastelands => burningWastelandsPanel,
            LocationType.FloodedTemples => floodedTemplesPanel,
            LocationType.CrystalSteppes => crystalSteppesPanel,
            _ => null
        };

        if (panel != null)
        {
            pauseManager.PauseGame();
            ShowPanel(panel);
        }
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
