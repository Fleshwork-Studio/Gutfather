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
            fightSlots[i].OnEnemyDeath += ReselectSlot;
        }

        SelectSlot(0);
    }
    public IEnumerator PerformEnemiesTurn()
    {
        foreach (FightSlot slot in fightSlots)
            yield return StartCoroutine(slot.RunEnemyTurn());

        yield return null;
    }
    private void ReselectSlot() // Reselects slot if enemy is dead on that slot
    {
        int index = 0;

        while (selectedSlot.GetEnemy() == null || !selectedSlot.GetEnemy().IsAlive())
        {
            if (index >= fightSlots.Count) break;
            SelectSlot(index);

            index++;
        }
    }
    private void SelectSlot(int index)
    {
        FightSlot newSlot = fightSlots[index];
        if (newSlot.GetEnemy() == null || !newSlot.GetEnemy().IsAlive()) return;

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
