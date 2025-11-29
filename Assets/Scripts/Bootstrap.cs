using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private void Awake()
    {
        InitializeInputHandler();
        InitializeTileManager();
    }

    private void InitializeInputHandler()
    {
        if (InputHandler.IsInitialized)
        {
            return;
        }

        GameObject inputHandlerGO = new GameObject("InputHandler");
        InputHandler inputHandler = inputHandlerGO.AddComponent<InputHandler>();
        inputHandler.Initialize();
        DontDestroyOnLoad(inputHandlerGO);
    }

    public void InitializeTileManager()
    {
        var tileManager = FindFirstObjectByType<TileManager>();

        if (tileManager != null)
        {
            tileManager.Initialize();
        }
        else
        {
            Debug.LogError("TileManager not found in the scene! (bootstrap)");
        }
    }
}