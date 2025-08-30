using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private InputManager inputManager;
    ActionsController actionsController;
    private Vector2 lastMovement;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody2D>();
        actionsController = GetComponent<ActionsController>();
        actionsController.OnActionPerformed += action =>
        {
            if (action == CharacterAction.Die)
            {
                rb.linearVelocity = Vector2.zero;
            }
        };
    }

    public void MoveCharacter(float movementSpeed)
    {
        if (lastMovement == inputManager.movementInput || !actionsController.TryToPerformAction(CharacterAction.Move)) return;
        lastMovement = inputManager.movementInput;
        rb.linearVelocity = inputManager.movementInput * movementSpeed;

        if (inputManager.movementInput == Vector2.zero) return;
        actionsController.InvokeActionPerformed(CharacterAction.Move);
    }
}
