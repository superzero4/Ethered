using System.Collections.Generic;
using System.Linq;
using Common;
using TMPro;
using Common.Visuals;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Image = UnityEngine.UI.Image;

namespace UI
{
    public class InfoUI : MonoBehaviour, IVisualInformationUI, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected bool _startHidden = true;

        [FormerlySerializedAs("_spriteRenderer")] [SerializeField]
        protected Image _image;

        [SerializeField] protected TextMeshProUGUI _nameText;
        [SerializeField] protected IconTextUI _descriptionText;
        private IIcon.IconText _cachedDescription;
        private List<IIcon.IconText> _cachedInformations;
        private Pool<IconTextUI> _pool;

        [Header("In % of parent y")] [SerializeField, Range(0, 1f)]
        private float smallInterval = .2f;

        [SerializeField, Range(0, 1f)] private float bigInterval = .5f;

        [SerializeField, Range(0, 1f)] private float padding = .05f;

        private void Awake()
        {
            //Assert.IsTrue(_image != null);
            //Assert.IsTrue(_nameText != null);
            //Assert.IsTrue(_descriptionText != null);
            if (_startHidden)
            {
                gameObject.SetActive(false);
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
            _cachedDescription = new IIcon.IconText(IIcon.IconType.Text, info.Description);
            _cachedInformations = additionalInformations?.Where(v => !string.IsNullOrEmpty(v.text))?.ToList() ??
                                  new List<IIcon.IconText>();
            SetInfos(false);
        }


        private void SetInfos(bool includeDescription)
        {
            int i = 0;
            IEnumerable<IIcon.IconText> toSet = _cachedInformations ?? Enumerable.Empty<IIcon.IconText>();
            if (includeDescription && !string.IsNullOrEmpty(_cachedDescription.text))
                toSet = toSet.Append(_cachedDescription);
            int imax = _cachedInformations?.Count ?? 0;
            bool forceExpanded = imax < 3;
            bool isOdd = imax % 2 == 1;
            if (_pool != null)
                _pool.SetElements(toSet,
                    (iconText, text) =>
                    {
                        text.gameObject.SetActive(true);
                        // we check if it's corde informations or optional ones
                        Vector2 anchorMin;
                        Vector2 anchorMax;
                        int iHalf = forceExpanded ? i : i / 2;
                        if (i >= imax)
                        {
                            int aboveI = i - imax;
                            anchorMin = new Vector2(0,
                                1 - (smallInterval * (iHalf + (isOdd ? 1 : 0)) + bigInterval * (aboveI + 1)));
                            anchorMax = new Vector2(1,
                                1 - (smallInterval * (iHalf + (isOdd ? 1 : 0)) + padding + bigInterval * aboveI));
                        }
                        else
                        {
                            bool expand = (isOdd && i == imax - 1);
                            float yMin = 1 - smallInterval / (expand || forceExpanded || true ? 1 : 2) * (iHalf + 1);
                            float yMax = 1 - smallInterval / (expand || forceExpanded || true ? 1 : 2) * (iHalf) -
                                         padding;
                            //On each row, 2 side by side expect for the last one if is odd
                            anchorMin = new Vector2(forceExpanded ? 0 : expand ? .245f : (i % 2 == 0 ? 0 : .51f), yMin);
                            anchorMax = new Vector2(forceExpanded ? 1 : expand ? .755f : (i % 2 == 1 ? 1 : .49f), yMax);
                        }

                        text.rectTransform.anchorMin = anchorMin;
                        text.rectTransform.anchorMax = anchorMax;


                        text.SetInfo(iconText);
                        i++;
                    });
        }

        public void SetInfo(IIcon iconProvider)
        {
            (this as IVisualInformationUI).SetIcon(iconProvider);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SetInfos(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetInfos(false);
        }
    }
}