using System;
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

        public void Awake()
        {
            _image.preserveAspect = true;
        }

        public void SetInfo(IIcon.IconText info)
        {
            //gameObject.SetActive(!string.IsNullOrEmpty(info.text));
            _text.text = info.text;
            _image.sprite = info.icon;
            _image.color = info.color;
        }
    }
}