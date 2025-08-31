using System.Collections.Generic;
using UnityEngine;

public enum StatusType
{
    Stun,
    Dot,
    Curse,
}

public class StatusData
{
    public StatusType type;
    public float duration = 3;
    public float tickInterval = 1;
    public int maxStacks = 1;
    public int stacks = 1;
    public int damagePerTick = 0;
    public int stacksToLosePerTick = 0;
    public bool mergeStacks = false;
    public List<StatModifier> statModifiers;
    public DamageType damageType = DamageType.Physical;
    public DamageController source;
}

public struct ActiveStatus
{
    public StatusData data;
    public float nextTickTime;
    public float endTime;
}

public class StatusController : MonoBehaviour
{
    private List<ActiveStatus> activeStatuses = new List<ActiveStatus>();
    private StatsController statsController;
    private ActionsController actionsController;
    private DamageController damageController;

    private void Start()
    {
        statsController = GetComponent<StatsController>();
        actionsController = GetComponent<ActionsController>();
        damageController = GetComponent<DamageController>();
    }

    private void Update()
    {
        UpdateActiveStatuses();
    }

    private void UpdateActiveStatuses()
    {
        for (var i = activeStatuses.Count - 1; i >= 0; i--)
        {
            var element = activeStatuses[i];
            if (Time.time >= activeStatuses[i].nextTickTime)
            {
                HandleModsAddition(element);
                HandleDamage(element);
                element.nextTickTime += element.data.tickInterval;
                element.data.stacks = CalculateStacks(element);
                activeStatuses[i] = element;
            }
            if (Time.time >= element.endTime || element.data.stacks <= 0)
            {
                HandleModsRemoval(element);
                activeStatuses.RemoveAt(i);
            }
        }
    }

    private int CalculateStacks(ActiveStatus element)
    {
        if (element.data.stacksToLosePerTick > 0)
        {
            element.data.stacks -= element.data.stacksToLosePerTick;
        }
        return element.data.stacks;
    }

    private void HandleModsAddition(ActiveStatus element)
    {
        if (element.data.statModifiers is { Count: > 0 } && actionsController.TryToPerformAction(CharacterAction.GetModifier))
        {
            statsController.stats.AddModifiers(element.data.statModifiers);
        }
    }

    private void HandleModsRemoval(ActiveStatus element)
    {
        if (element.data.statModifiers is { Count: > 0 } && actionsController.TryToPerformAction(CharacterAction.GetModifier))
        {
            statsController.stats.RemoveModifiers(element.data.statModifiers);
        }
    }

    private void HandleDamage(ActiveStatus element)
    {
        if (element.data.damagePerTick <= 0) return;
        DamagePayload payload = new()
        {
            Damage = element.data.damagePerTick * element.data.damagePerTick,
            Type = element.data.damageType
        };
        damageController.TakeDamage(payload, element.data.source);
    }
    
    public void RegisterStatus(StatusData statusData)
    {
        ActiveStatus newActiveStatus = new()
        {
            data = statusData,
            nextTickTime = 0,
            endTime = Time.time + statusData.duration,
        };
        var isMerged = false;

        if (newActiveStatus.data.mergeStacks)
        {
            for (var i = activeStatuses.Count - 1; i >= 0; i--)
            {
                if (activeStatuses[i].data.type == newActiveStatus.data.type)
                {
                    if (!isMerged)
                    {
                        isMerged = true;
                    }
                    activeStatuses.RemoveAt(i);
                }
            }
        }
        
        activeStatuses.Add(newActiveStatus);
    }

    public bool HasStatusType(StatusType statusType)
    {
        return activeStatuses.FindIndex(el => el.data.type == statusType) >= 0;
    }
}
