using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Creature))]
public class PlayerManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private InputManager inputManager;
    private Creature creature;
    
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        inputManager = GetComponent<InputManager>();
        creature = GetComponent<Creature>();
    }

    private void Update()
    {
        /*if (creature.CanPerformAction(CreatureAction.Move))
        {
            playerMovement.MoveCharacter(creature.MovementSpeed);
        }*/
    }
}