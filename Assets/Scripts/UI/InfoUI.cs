using TMPro;
using UI.Battle;
using UnityEngine;
using UnityEngine.UI;

public class InfoUI : MonoBehaviour
{
    [SerializeField] private Image _spriteRenderer;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    public void SetInfo(VisualInformations info)
    {
        _spriteRenderer.sprite = info.Sprite;
        _spriteRenderer.color = info.Color;
        _nameText.text = info.Name;
        _descriptionText.text = info.Description;
    }

    public void SetInfo(IIcon iconProvider)
    {
        SetInfo(iconProvider.VisualInformations);
    }
}