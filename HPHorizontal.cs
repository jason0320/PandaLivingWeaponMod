using UnityEngine;
using UnityEngine.UI;
using static HotItemLayout;

namespace PandaLivingWeaponMod
{
    public class HPHorizontal : HPLayout
    {
        protected HorizontalLayoutGroup? _layout;

        protected ContentSizeFitter? _fitter;

        public HorizontalLayoutGroup Layout => _layout;

        public ContentSizeFitter Fitter => _fitter;

        public override void OnLayout()
        {
            HorizontalLayoutGroup horizontalLayoutGroup = base.gameObject.AddComponent<HorizontalLayoutGroup>();
            horizontalLayoutGroup.childControlHeight = false;
            horizontalLayoutGroup.childForceExpandHeight = false;
            horizontalLayoutGroup.childControlWidth = true;
            horizontalLayoutGroup.childForceExpandWidth = false;
            horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
            _layout = horizontalLayoutGroup;
            ContentSizeFitter contentSizeFitter = base.gameObject.AddComponent<ContentSizeFitter>();
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.MinSize;
            _fitter = contentSizeFitter;
        }
    }
}
