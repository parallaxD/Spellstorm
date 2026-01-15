using UnityEngine;
using UnityEngine.InputSystem;

public class MovementAbilityHandler : MonoBehaviour
{
    private MovementAbility _currentAbility;
    private float _currentCooldown;
    private bool _isOnCooldown;

    private void Start()
    {
        _currentAbility = new Flip();
        InputHandler.Instance.MovementAbility.performed += OnActionPerformed;
    }

    private void Update()
    {
        if (_isOnCooldown)
        {
            _currentCooldown -= Time.deltaTime;
            if (_currentCooldown <= 0)
            {
                _isOnCooldown = false;
            }
        }
    }

    private void SetNewAbility(MovementAbility movementAbility)
    {
        _currentAbility = movementAbility;
    }

    private void OnActionPerformed(InputAction.CallbackContext context)
    {
        if (_isOnCooldown || _currentAbility == null) return;

        _currentAbility.Action(Constants.PlayerTransform);
        StartCooldown(1);
    }

    private void StartCooldown(float cooldownTime)
    {
        if (cooldownTime <= 0) return;

        _currentCooldown = cooldownTime;
        _isOnCooldown = true;
    }

    public bool IsOnCooldown() => _isOnCooldown;
    public float GetCooldownPercent() => _isOnCooldown ? _currentCooldown / 1 : 0f;
    public float GetRemainingCooldown() => _currentCooldown;

    private void OnDestroy()
    {
        if (InputHandler.Instance != null)
        {
            InputHandler.Instance.MovementAbility.performed -= OnActionPerformed;
        }
    }
}