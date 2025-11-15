using UnityEngine;
using UnityEngine.InputSystem;

public class MovementAbilityHandler : MonoBehaviour
{
    MovementAbility _currentAbility;

    private void Start()
    {
        _currentAbility = new Teleport();

        InputHandler.Instance.MovementAbility.performed += OnActionPerformed;
    }

    private void SetNewAbility(MovementAbility movementAbility)
    {
        _currentAbility = movementAbility;
    }

    private void OnActionPerformed(InputAction.CallbackContext context)
    {
        _currentAbility?.Action(Constants.PlayerTransform);
    }

    private void OnDestroy()
    {
        if (InputHandler.Instance != null)
        {
            InputHandler.Instance.MovementAbility.performed -= OnActionPerformed;
        }
    }

}
