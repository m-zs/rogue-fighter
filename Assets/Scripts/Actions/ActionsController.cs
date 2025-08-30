using System;
using UnityEngine;

public enum CharacterAction
{
    Move,
    Jump,
    Dash,
    Attack,
    Cast,
    Use,
    DealDamage,
    TakeDamage,
    Die
}

public class ActionsController : MonoBehaviour
{
    public event Action<CharacterAction> OnActionPerformed;
    public event Action<CharacterAction> OnActionFailed;

    private StatsController statsController;
    private StatusController statusController;

    private void Awake()
    {
        statsController = GetComponent<StatsController>();
        statusController = GetComponent<StatusController>();
    }

    private void Start()
    {
        statsController.health.OnDeathEvent += () =>
        {
            InvokeActionPerformed(CharacterAction.Die);
        };
    }

    public bool TryToPerformAction(CharacterAction action)
    {
        if (CanPerformAction(action)) return true;
        OnActionFailed?.Invoke(action);
        return false;
    }

    public void InvokeActionPerformed(CharacterAction action)
    {
        OnActionPerformed?.Invoke(action);
    }

    private bool CanPerformAction(CharacterAction action)
    {
        if (statsController.health.IsDead) return false;

        return action switch
        {
            CharacterAction.Attack => !statsController.health.IsDead && !statusController.HasStatusType(StatusType.Stun),
            CharacterAction.Cast => !statsController.health.IsDead && !statusController.HasStatusType(StatusType.Stun),
            CharacterAction.DealDamage => !statsController.health.IsDead,
            CharacterAction.TakeDamage => !statsController.health.IsDead,
            CharacterAction.Use => !statsController.health.IsDead && !statusController.HasStatusType(StatusType.Stun),
            CharacterAction.Jump => statsController.stats.GetStat(Stat.MovementSpeed) > 0 && !statusController.HasStatusType(StatusType.Stun),
            CharacterAction.Dash => statsController.stats.GetStat(Stat.MovementSpeed) > 0 && !statusController.HasStatusType(StatusType.Stun),
            CharacterAction.Move => statsController.stats.GetStat(Stat.MovementSpeed) > 0 && !statusController.HasStatusType(StatusType.Stun),
            _ => true
        };
    }
}
