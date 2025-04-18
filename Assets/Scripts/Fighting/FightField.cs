using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FightField : MonoBehaviour
{
    public static FightField Instance;
    [SerializeField] private List<FightSlot> fightSlots = new List<FightSlot>();

    private FightSlot selectedSlot;
    void Awake()
    {
        Instance = this;

        InitializeSlots();
    }
    private void InitializeSlots()
    {
        for (int i = 0; i < fightSlots.Count; i++)
        {
            fightSlots[i].SetIndex(i);
            fightSlots[i].OnSelected += SelectSlot;
        }

        SelectSlot(0);
    }
    public IEnumerator PerformEnemiesTurn()
    {
        foreach (FightSlot slot in fightSlots)
            yield return StartCoroutine(slot.RunEnemyTurn());

        yield return null;
    }
    private void SelectSlot(int index)
    {
        Debug.Log("Selected slot " + index);

        selectedSlot = fightSlots[index];
    }
    public Enemy GetSelectedEnemy() => selectedSlot.GetEnemy();
    public List<Enemy> GetAllAliveEnemies()
    {
        List<Enemy> enemiesList = new List<Enemy>();

        foreach (FightSlot slot in fightSlots)
        {
            Enemy enemy = slot.GetEnemy();
            if (enemy.IsAlive()) enemiesList.Add(enemy);
        }

        return enemiesList;
    }
}
