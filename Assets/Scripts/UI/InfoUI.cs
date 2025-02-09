using NUnit.Framework;
using TMPro;
using UI.Battle;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class InfoUI : MonoBehaviour
    {
        [FormerlySerializedAs("_spriteRenderer")] [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        private void Awake()
        {
            Assert.IsTrue(_image != null);
            Assert.IsTrue(_nameText != null);
            Assert.IsTrue(_descriptionText != null);
            _image.preserveAspect = true;
            _image.sprite = null;
            _image.color = new Color(0, 0, 0, 0);
            _nameText.text = string.Empty;
            _descriptionText.text = string.Empty;
        }
        public void SetInfo(VisualInformations info)
        {
            _image.sprite = info.Sprite;
            _image.color = info.Color;
            _nameText.text = info.Name;
            _descriptionText.text = info.Description;
        }

        public void SetInfo(IIcon iconProvider)
        {
            SetInfo(iconProvider?.VisualInformations ?? VisualInformations.Default);
        }
    }
}