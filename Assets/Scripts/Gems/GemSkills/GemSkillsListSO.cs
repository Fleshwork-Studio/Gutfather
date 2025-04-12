using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GemSkillsList", menuName = "Match3/Gem Skills List")]
public class GemSkillsListSO : ScriptableObject
{
    public List<GemSkillSO> list = new List<GemSkillSO>();

    public GemSkillSO GetSkill(MatchType matchType)
    {
        foreach (GemSkillSO skill in list)
        {
            if (skill.matchType == matchType)
                return skill;
        }

        return null;
    }
}
