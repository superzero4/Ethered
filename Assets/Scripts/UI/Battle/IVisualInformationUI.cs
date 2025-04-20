using System.Collections.Generic;
using System.Linq;
using Common.Visuals;

namespace UI
{
    public interface IVisualInformationUI
    {
        public void SetInfo(VisualInformations info, params IIcon.IconText[] additionalInformations);

        public void SetIcon(IIcon iconProvider)
        {
            IEnumerable<IIcon.IconText> iconTexts;
            if (iconProvider == null)
                SetInfo(VisualInformations.Default);
            else if ((iconTexts = iconProvider.IconTexts) == null || !iconTexts.Any())
                SetInfo(iconProvider.VisualInformations);
            else
                SetInfo(iconProvider.VisualInformations,
                    iconTexts.Where(i => !string.IsNullOrEmpty(i.text)).ToArray());
        }
    }
}