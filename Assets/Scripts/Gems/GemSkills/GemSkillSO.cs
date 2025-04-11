using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillType", menuName = "Match3/Skill Type")]
public class GemSkillSO : ScriptableObject
{
    public Sprite icon;
    public MatchType matchType;
    public string title;
    [TextArea] public string description;
}
