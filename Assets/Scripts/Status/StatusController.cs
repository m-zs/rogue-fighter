using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum StatusType
{
    Slow,
    Stun,
    Bleed,
    Curse,
    Poison,
}

public struct StatModifier
{
    public Stat stat;
    public float value;
}

public class StatusData : ScriptableObject
{
    public StatusType type;
    public float duration = 3;
    public float tickInterval = 1;
    public int maxStacks = 1;
    public int stacks = 1;
    public int damagePerTick = 0;
    public int stacksToLosePerTick = 0;
    public bool mergeStacks = false;
    public StatModifier[] statModifiers;
    public DamageType damageType = DamageType.Physical;
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
                // do stuff
                element.nextTickTime += element.data.tickInterval;
                if (element.data.stacksToLosePerTick > 0)
                {
                    element.data.stacks -= element.data.stacksToLosePerTick;
                }
                activeStatuses[i] = element;
            }
            if (Time.time >= element.endTime || element.data.stacks <= 0)
            {
                activeStatuses.RemoveAt(i);
            }
        }
    }
    
    public void RegisterStatus(StatusData statusData)
    {
        ActiveStatus newActiveStatus = new()
        {
            data = Instantiate(statusData),
            nextTickTime = 0,
            endTime = Time.time + statusData.duration
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
                        newActiveStatus.data.stacks = Mathf.Clamp(statusData.stacks, 0, newActiveStatus.data.maxStacks);
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
