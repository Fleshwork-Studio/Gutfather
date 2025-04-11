using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GemSkillsEncyclopedia", menuName = "Match3/Gem Skills Encyclopedia")]
public class GemSkillsEncyclopediaSO : ScriptableObject
{
    public List<GemSkillsListSO> list = new List<GemSkillsListSO>();

    public GemSkillsListSO GetGemList(GemTypeSO gemType)
    {
        foreach (GemSkillsListSO gemList in list)
        {
            if (gemList.gemType == gemType)
                return gemList;
        }

        return null;
    }
}
