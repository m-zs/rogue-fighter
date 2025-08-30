using System;
using UnityEngine;

public enum CreatureAction
{
    Move,
    Attack
}

public class Creature : MonoBehaviour
{
    /*public bool CanPerformAction(CreatureAction action)
    {
        if (isDead) return false;

        return action switch
        {
            CreatureAction.Move => movementSpeed > 0,
            CreatureAction.Attack => true,
            _ => true
        };
    }*/
}
