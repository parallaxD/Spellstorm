using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    public void Awake()
    {
        InitializeInputHandler();
        InitializeTileManager();
        InitializeDecorationManager();
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

    private void InitializeTileManager()
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

    private void InitializeDecorationManager()
    {
        var decorationManager = FindFirstObjectByType<DecorationManager>();

        if (decorationManager != null)
        {
            decorationManager.Initialize();
        }
        else
        {
            Debug.LogError("DecorationManager not found in the scene! (bootstrap)");
        }
    }
}