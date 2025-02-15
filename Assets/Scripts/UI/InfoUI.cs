using NUnit.Framework;
using TMPro;
using Common.Visuals;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class InfoUI : MonoBehaviour, IVisualInformationUI
    {
        [SerializeField] protected bool _startHidden = true;

        [FormerlySerializedAs("_spriteRenderer")] [SerializeField]
        protected Image _image;

        [SerializeField] protected TextMeshProUGUI _nameText;
        [SerializeField] protected TextMeshProUGUI _descriptionText;

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
                _descriptionText.text = string.Empty;
            AfterAwake();
        }

        protected virtual void AfterAwake()
        {
        }

        public void SetInfo(VisualInformations info)
        {
            Assert.IsTrue(_image != null || info.Sprite == null);
            Assert.IsTrue(_nameText != null || string.IsNullOrEmpty(info.Name));
            Assert.IsTrue(_descriptionText != null || string.IsNullOrEmpty(info.Description));
            _image.sprite = info.Sprite;
            _image.color = info.Color;
            _nameText.text = info.Name;
            _descriptionText.text = info.Description;
        }

        public void SetInfo(IIcon iconProvider)
        {
            (this as IVisualInformationUI).SetIcon(iconProvider);
        }
    }
}