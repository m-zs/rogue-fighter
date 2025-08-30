using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(ManaComponent))]
[RequireComponent(typeof(StatsComponent))]
public class StatsController : MonoBehaviour
{
    public HealthComponent health;
    public ManaComponent mana;
    public StatsComponent stats;

    private void Awake()
    {
        health = GetComponent<HealthComponent>();
        mana = GetComponent<ManaComponent>();
        stats = GetComponent<StatsComponent>();
    }

    private void Start()
    {
        health.SetMaxHealth(health.BaseMaxHealth + stats.GetStat(Stat.Endurance), true);
        mana.SetMaxMana(mana.BaseMaxMana + stats.GetStat(Stat.Intelligence), true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
