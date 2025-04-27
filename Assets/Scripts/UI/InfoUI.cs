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

        [SerializeField, Range(1, 5)] private int _nbCol = 2;
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
            IEnumerable<IIcon.IconText> toSet = _cachedInformations ?? Enumerable.Empty<IIcon.IconText>();
            if (includeDescription && !string.IsNullOrEmpty(_cachedDescription.text))
                toSet = toSet.Append(_cachedDescription);
            int imax = _cachedInformations?.Count ?? 0;
            bool forceExpanded = imax < 3;
            bool isOdd = imax % 2 == 1;
            int row = 0;
            int col = 0;
            int i = 0;
            int above = 0;
            if (_pool != null)
                _pool.SetElements(toSet,
                    (iconText, text) =>
                    {
                        text.gameObject.SetActive(true);
                        // we check if it's corde informations or optional ones
                        Vector2 anchorMin;
                        Vector2 anchorMax;
                        float yMin = 1 - smallInterval * (row + 1);
                        float yMax = 1 - smallInterval * (row) - padding;
                        //Default full expanded
                        anchorMin = new Vector2(0, yMin);
                        anchorMax = new Vector2(1, yMax);
                        if (i >= imax)
                        {
                            anchorMin.y -= bigInterval * (above + 1) - smallInterval;
                            anchorMax.y -= bigInterval * above;
                            above++;
                            col = 0;
                        }
                        else
                        {
                            if (iconText.forceExpand || forceExpanded) //Big
                            {
                                row++;
                                col = 0;
                            }
                            else
                            {
                                if ((isOdd && row == imax - 1)) //Centered only
                                {
                                    anchorMin.x = .245f;
                                    anchorMax.y = .755f;
                                    row++;
                                    col = 0;
                                }
                                else //Row col normal case
                                {
                                    anchorMin.x = col * 1f / _nbCol + (col == 0 ? 0 : .01f);
                                    anchorMax.x = (col + 1f) / _nbCol - (col == _nbCol - 1 ? 0 : 0.01f);

                                    col++;
                                    if (col >= _nbCol)
                                    {
                                        row++;
                                        col = 0;
                                    }
                                }
                            }
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

        public void SetInfo(IIcon iconProvider, int distance)
        {
            var info = iconProvider.VisualInformations;
            var icon = iconProvider.IconTexts;
            icon = icon.Append(new(IIcon.IconType.Distance, distance.ToString()));
            (this as IVisualInformationUI).SetInfo(info, icon);
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