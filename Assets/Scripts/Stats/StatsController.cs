using System;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
