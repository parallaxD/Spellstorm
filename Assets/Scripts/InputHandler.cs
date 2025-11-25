using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private static InputHandler _instance;
    public static InputHandler Instance
    {
        get
        {
            return _instance;
        }
        private set => _instance = value;
    }

    public static bool IsInitialized => _instance != null;

    private PlayerController _inputActions;
    private PlayerController.PlayerActions _playerActions;

    public void Initialize()
    {
        if (_instance != null && _instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        _inputActions = new PlayerController();
        _playerActions = _inputActions.Player;
        _inputActions.Enable();

    }

    private void OnEnable() => _inputActions?.Enable();
    private void OnDisable() => _inputActions?.Disable();

    private void OnDestroy()
    {
        _inputActions?.Disable();
        if (_instance == this)
        {
            _instance = null;
        }
    }

    public InputAction MovementAbility => _playerActions.MovementAbility;
    public InputAction Move => _playerActions.Move;
}