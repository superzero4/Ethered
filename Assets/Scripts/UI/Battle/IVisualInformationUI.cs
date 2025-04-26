using System.Collections.Generic;
using System.Linq;
using Common.Visuals;

namespace UI
{
    public interface IVisualInformationUI
    {
        public void SetInfo(VisualInformations? info, IEnumerable<IIcon.IconText> additionalInformations);

        public void SetIcon(IIcon iconProvider)
        {
            IEnumerable<IIcon.IconText> iconTexts;
            if (iconProvider == null)
                SetInfo(null, new IIcon.IconText[]{});
            else if ((iconTexts = iconProvider.IconTexts) == null || !iconTexts.Any())
                SetInfo(iconProvider.VisualInformations, new IIcon.IconText[]{});
            else
                SetInfo(iconProvider.VisualInformations,
                    iconTexts.Where(i => !string.IsNullOrEmpty(i.text)).ToArray());
        }
    }
}