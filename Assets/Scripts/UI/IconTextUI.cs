using Common.Visuals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class IconTextUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _image;

        public RectTransform rectTransform => transform as RectTransform;

        public void SetInfo(IIcon.IconText info)
        {
            gameObject.SetActive(!string.IsNullOrEmpty(info.text));
            _text.text = info.text;
            if (_image != null)
                _image.sprite = null;
        }
    }
}