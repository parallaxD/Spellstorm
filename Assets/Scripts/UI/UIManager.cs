using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    public void ShowMainMenu()
    {
        SetAllPanelsInactive();
        mainMenuPanel.SetActive(true);
    }

    public void ShowSettings()
    {
        SetAllPanelsInactive();
        settingsPanel.SetActive(true);
    }

    private void SetAllPanelsInactive()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }
}
