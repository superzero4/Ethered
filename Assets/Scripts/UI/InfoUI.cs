using Common;
using NUnit.Framework;
using TMPro;
using Common.Visuals;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Views.Battle.Selection;

namespace UI
{
    public class InfoUI : MonoBehaviour, IVisualInformationUI
    {
        [SerializeField] protected bool _startHidden = true;

        [FormerlySerializedAs("_spriteRenderer")] [SerializeField]
        protected Image _image;

        [SerializeField] protected TextMeshProUGUI _nameText;
        [SerializeField] protected TextMeshProUGUI _descriptionText;
        private Pool<TextMeshProUGUI> _pool;

        private void Awake()
        {
            //Assert.IsTrue(_image != null);
            //Assert.IsTrue(_nameText != null);
            //Assert.IsTrue(_descriptionText != null);
            _image.preserveAspect = true;
            if (_startHidden)
            {
                _image.sprite = null;
                _image.color = new Color(0, 0, 0, 0);
            }

            if (_nameText != null)
                _nameText.text = string.Empty;
            if (_descriptionText != null)
            {
                _descriptionText.text = string.Empty;
                _pool = new(_descriptionText, 3, transform);
                _pool.Reset();
            }

            AfterAwake();
        }

        protected virtual void AfterAwake()
        {
        }

        public void SetInfo(VisualInformations info, params IIcon.IconText[] additionalInformations)
        {
            Assert.IsTrue(_image != null || info.Sprite == null);
            Assert.IsTrue(_nameText != null || string.IsNullOrEmpty(info.Name));
            Assert.IsTrue(_descriptionText != null || string.IsNullOrEmpty(info.Description));
            _image.sprite = info.Sprite;
            _image.color = info.Color;
            _nameText.text = info.Name;
            _descriptionText.text = info.Description;
            int i = 0;
            if (_pool != null)
                _pool.SetElements(additionalInformations,
                    (iconText, text) =>
                    {
                        text.gameObject.SetActive(true);
                        text.rectTransform.anchorMin = new Vector2(0, -.2f * (i + 1));
                        text.rectTransform.anchorMax = new Vector2(1, -.2f * (i));
                        text.text = iconText.text;
                        i++;
                    });
        }

        public void SetInfo(IIcon iconProvider)
        {
            (this as IVisualInformationUI).SetIcon(iconProvider);
        }
    }
}