using System.Collections;
using System.Collections.Generic;
using ARGame2;
using UnityEngine;

public class GenesisStatsUI : MonoBehaviour
{
    private HexagonalStatsUI _hexagonalStatsUI;

    public List<Stats> stats;

    private void Awake() 
    {
        _hexagonalStatsUI = GetComponent<HexagonalStatsUI>();    
    }

    private void OnEnable() 
    {
        GenesisInventory.OnGenesisInventoryChange += UpdateStatsByGenesis;
    }

    private void OnDisable()
    {
        GenesisInventory.OnGenesisInventoryChange -= UpdateStatsByGenesis;
    }

    public void UpdateStatsByGenesis(ARGenesisBall aRGenesisBall)
    {
        float[] floats = new float[6];

        floats[0] = (float)aRGenesisBall.attack / Constant.k_MaxAttack;
        floats[1] = (float)aRGenesisBall.hp / Constant.k_MaxGenesisHP * 1f;
        floats[2] = (float)aRGenesisBall.hpregen / Constant.k_MaxHPRegen * 1f;
        floats[3] = (float)aRGenesisBall.critAttack / Constant.k_MaxCriticalAttackHP * 1f;
        floats[4] = (float)aRGenesisBall.speed / Constant.k_MaxSpeed * 1f;
        floats[5] = (float)aRGenesisBall.defense / Constant.k_MaxDefense * 1f;

        _hexagonalStatsUI.SetFloat(floats);
    }
}

public class Stats
{
    public EStatsType eStatsType;
    public float value;
}

public enum EStatsType
{
    attack,
    hp,
    hpregen,
    critAttack,
    speed,
    defense,
}
