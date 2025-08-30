using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private InputManager inputManager;
    private StatsController statsController;
    
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        inputManager = GetComponent<InputManager>();
        statsController = GetComponent<StatsController>();
    }

    private void Update()
    {
        playerMovement.MoveCharacter(statsController.stats.GetStat(Stat.MovementSpeed));
    }
}