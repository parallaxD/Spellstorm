using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    void Start()
    {
        InitializeResolutionDropdown();
    }

    private void InitializeResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        foreach(var item in resolutions)
        {
            Debug.Log(item);
        }
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = (float) Screen.currentResolution.refreshRateRatio.value;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resolutionOption = resolutions[i].width + " x " + resolutions[i].height;

            if (!ContainsResolution(resolutions[i]))
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();

        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + " x " +
                                     filteredResolutions[i].height;
 
            options.Add(resolutionOption);


            if (filteredResolutions[i].width == Screen.width &&
                filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private bool ContainsResolution(Resolution resolution)
    {
        foreach (var res in filteredResolutions)
        {
            if (res.width == resolution.width && res.height == resolution.height)
            {
                return true;
            }
        }
        return false;
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex < 0 || resolutionIndex >= filteredResolutions.Count)
            return;

        Resolution resolution = filteredResolutions[resolutionDropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        Debug.Log("Установлено разрешение: " + resolution.width + "x" + resolution.height);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }


    public void SetWindowed()
    {
        Screen.fullScreen = false;
    }

    public List<string> GetAllResolutions()
    {
        List<string> resList = new List<string>();
        foreach (var res in filteredResolutions)
        {
            resList.Add($"{res.width}x{res.height}");
        }
        return resList;
    }
}
