using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FightField : MonoBehaviour
{
    public static FightField Instance;
    [SerializeField] private List<FightSlot> fightSlots = new List<FightSlot>();

    void Awake()
    {
        Instance = this;
    }
    public IEnumerator PerformEnemiesTurn()
    {
        foreach (FightSlot slot in fightSlots)
            yield return StartCoroutine(slot.RunEnemyTurn());

        yield return null;
    }
}
