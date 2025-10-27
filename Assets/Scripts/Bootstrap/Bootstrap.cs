using UnityEngine;

public class Bootstrap : MonoBehaviour
{

    private void Awake()
    {
        InitializeInputHandler();
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

}