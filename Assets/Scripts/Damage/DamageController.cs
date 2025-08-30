using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum DamageType
{
    Physical,
    Elemental
}

public struct DamagePayload
{
    public DamageType Type;
    public float Damage;
}

[RequireComponent(typeof(ActionsController))]
public class DamageController : MonoBehaviour
{
    private StatsController statsController;
    private ActionsController actionsController;

    private void Start()
    {
        statsController = GetComponent<StatsController>();
        actionsController = GetComponent<ActionsController>();
    }

    public void TakeDamage(DamagePayload damagePayload, DamageController damageDealerDamageController)
    {
        if (!actionsController.TryToPerformAction(CharacterAction.TakeDamage)) return;

        var damage = ProcessDamage(damagePayload, damageDealerDamageController);
        actionsController.InvokeActionPerformed(CharacterAction.TakeDamage);
        statsController.health.TakeDamage(damage);
    }

    private float ProcessDamage(DamagePayload damagePayload, DamageController damageDealerDamageController)
    {
        var totalDamage = damagePayload.Damage;
        var initiatorStats = damageDealerDamageController.statsController;
            
        switch (damagePayload.Type)
        {
            case DamageType.Elemental:
                totalDamage += initiatorStats.stats.GetStat(Stat.Intelligence);
                totalDamage -= ((totalDamage / 100) * statsController.stats.GetStat(Stat.ElementalDefence));
                break;
            case DamageType.Physical:
                totalDamage += initiatorStats.stats.GetStat(Stat.Strength);
                totalDamage -= ((totalDamage / 100) * statsController.stats.GetStat(Stat.PhysicalDefence));
                break;
        }

        var isCrit = (Random.value * 100) <= initiatorStats.stats.GetStat(Stat.Dexterity);
        if (isCrit) totalDamage *= 2;

        return totalDamage;
    }
}
