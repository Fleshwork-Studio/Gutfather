using UnityEngine;
using DG.Tweening;
using TMPro;

public class VFXMatchPopup : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private TextMeshPro text;

    MatchPopupController matchPopupController;

    Sequence sequence;

    private void Start()
    {
        matchPopupController = MatchPopupController.Instance;
    }

    public void Enable(ComboMatchEvent e)
    {
        gameObject.SetActive(true);

        Vector2Int averagePosition = e.gemPositions[e.gemPositions.Count / 2];
        transform.position = (Vector2)averagePosition + Vector2.one * 0.5f;

        SetText(e.matchType);

        SetSprite(e.gemType);

        PlayAnim();
    }
    private void SetText(MatchComboController.MatchType matchType)
    {
        string textString = matchType switch
        {

            MatchComboController.MatchType.Cross => "X",
            MatchComboController.MatchType.Six6 => "6",
            MatchComboController.MatchType.Five5 => "5",
            MatchComboController.MatchType.Four4 => "4",
            _ => "3",
        };

        text.SetText(textString);
    }

    private void SetSprite(GemTypeSO gem) => sprite.sprite = gem.sprite;

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private void PlayAnim()
    {
        // Moves popup by Y axis
        var endPos = transform.position.y + 1.5f;
        transform.DOMoveY(endPos, 1).SetEase(Ease.OutQuad);

        // Scale modification of popup
        if (sequence != null || sequence.IsActive()) sequence.Kill();

        sequence = DOTween.Sequence();
        transform.localScale = Vector3.one;

        sequence.Append(transform.DOPunchScale(Vector3.one * 0.6f, 2f, 2, 0));
        sequence.Append(transform.DOScale(Vector3.zero, 0.4f));
        sequence.OnComplete(() => matchPopupController.ReturnObject(this));

        sequence.Play();
    }
}


