using Common.Visuals;
namespace UI
{
    public interface IVisualInformationUI
    {
        void SetInfo(VisualInformations info);
        public void SetIcon(IIcon iconProvider)
        {
            SetInfo(iconProvider?.VisualInformations ?? VisualInformations.Default);
        }
    }
}