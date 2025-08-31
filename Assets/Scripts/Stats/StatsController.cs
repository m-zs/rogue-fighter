using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(ManaComponent))]
[RequireComponent(typeof(StatsComponent))]
public class StatsController : MonoBehaviour
{
    public HealthComponent health;
    public ManaComponent mana;
    public StatsComponent stats;
    private ActionsController actionsController;

    private void Awake()
    {
        health = GetComponent<HealthComponent>();
        mana = GetComponent<ManaComponent>();
        stats = GetComponent<StatsComponent>();
        actionsController = GetComponent<ActionsController>();
    }

    private void Start()
    {
        health.SetMaxHealth(health.BaseMaxHealth + stats.GetStat(Stat.Endurance), true);
        mana.SetMaxMana(mana.BaseMaxMana + stats.GetStat(Stat.Intelligence), true);
    }
}
