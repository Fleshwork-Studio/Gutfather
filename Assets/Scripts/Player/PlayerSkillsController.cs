using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkillsController : MonoBehaviour
{
    public static PlayerSkillsController Instance;
    [SerializeField] private PlayerSkillsHolder skillsHolder;
    [SerializeField] private Transform skillPrefab;
    [SerializeField] private GemSkillsEncyclopediaSO skillsEncyclopedia;

    private void Awake()
    {
        Instance = this;

        Bus.Subscribe<ComboMatchEvent>(OnMatch);
    }
    private void OnMatch(ComboMatchEvent e)
    {
        Transform newSkill = Instantiate(skillPrefab, skillsHolder.transform);

        var skillScript = newSkill.GetComponent<SkillUnit>();
        GemSkillSO gemSkillSO = skillsEncyclopedia.GetGemList(e.gemType)?.GetSkill(e.matchType);
        skillScript.SetParametres(e.matchType, e.gemType, gemSkillSO);

        skillsHolder.AddSkill(skillScript);
    }
    public IEnumerator UseSkills()
    {
        yield return StartCoroutine(skillsHolder.UseSkills());
    }
}
