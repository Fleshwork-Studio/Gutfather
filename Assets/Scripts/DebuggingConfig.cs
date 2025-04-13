using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggingConfig : MonoBehaviour
{
    public static bool GameState;
    public static bool SkillUnitCreated;
    public static bool UsingPlayerSkills;

    [SerializeField] private bool gameState;
    [SerializeField] private bool skillUnitCreated;
    [SerializeField] private bool usingPlayerSkills;

    void Awake()
    {
        GameState = gameState;
        SkillUnitCreated = skillUnitCreated;
        UsingPlayerSkills = usingPlayerSkills;
    }
}
