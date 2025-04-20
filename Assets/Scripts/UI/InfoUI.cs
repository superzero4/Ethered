using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] protected IconTextUI _descriptionText;
        private Pool<IconTextUI> _pool;

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
                _descriptionText.SetInfo(new IIcon.IconText());
                _pool = new(_descriptionText, 3, _descriptionText.transform.parent);
                _pool.Reset();
            }

            AfterAwake();
        }

        protected virtual void AfterAwake()
        {
        }

        public void SetInfo(VisualInformations? infoo, IEnumerable<IIcon.IconText> additionalInformations)
        {
            var info = infoo ?? VisualInformations.Default;
            if (!infoo.HasValue)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            Assert.IsTrue(_image != null || info.Sprite == null);
            Assert.IsTrue(_nameText != null || string.IsNullOrEmpty(info.Name));
            Assert.IsTrue(_descriptionText != null || string.IsNullOrEmpty(info.Description));
            _image.sprite = info.Sprite;
            _image.color = info.Color;
            _nameText.text = info.Name;
            if (string.IsNullOrEmpty(info.Description))
                additionalInformations.Prepend(new IIcon.IconText(info.Description));
            int i = 0;
            if (_pool != null)
                _pool.SetElements(additionalInformations,
                    (iconText, text) =>
                    {
                        text.gameObject.SetActive(true);
                        text.rectTransform.anchorMin = new Vector2(0, -.2f * (i + 1) + .05f);
                        text.rectTransform.anchorMax = new Vector2(1, -.2f * (i));
                        text.SetInfo(iconText);
                        i++;
                    });
        }

        public void SetInfo(IIcon iconProvider)
        {
            (this as IVisualInformationUI).SetIcon(iconProvider);
        }
    }
}