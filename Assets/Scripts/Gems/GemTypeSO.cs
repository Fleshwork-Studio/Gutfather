using UnityEngine;

[CreateAssetMenu(fileName = "GemType", menuName = "Match3/Gem Type")]
public class GemTypeSO : ScriptableObject
{
    public GemTypeEnum gemType;
    public Sprite sprite;
}
public enum GemTypeEnum
{
    Eye, // Magic
    Bone, // Atack
    Tooth, // Debuff
    Meat, // Heal
    Skin, // Sheild
    Fat, // Buff
}

