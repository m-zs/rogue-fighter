using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private InputManager inputManager;

    void Start()
    {
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveCharacter(float movementSpeed)
    {
        rb.linearVelocity = inputManager.movementInput * movementSpeed;
    }
}
