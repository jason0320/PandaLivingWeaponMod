using UnityEngine;
using UnityEngine.UI;
using static HotItemLayout;

namespace PandaLivingWeaponMod
{
    public class HPScroll : HPLayout
    {
        protected ScrollRect? _scrollRect;

        protected RectTransform? _contentTransform;

        protected VerticalLayoutGroup? _layout;

        protected ContentSizeFitter? _fitter;

        public ScrollRect ScrollRect => _scrollRect;

        public RectTransform ContentTransform => _contentTransform;

        public VerticalLayoutGroup Layout => _layout;

        public ContentSizeFitter Fitter => _fitter;

        public override void OnLayout()
        {
            _scrollRect = HP.GetResource<ScrollRect>("Scrollview parchment with Header");
            RectTransform rectTransform = _scrollRect.Rect();
            rectTransform.SetParent(base.transform);
            _scrollRect.gameObject.GetComponent<LayoutElement>().flexibleHeight = 10f;
            ((RectTransform)rectTransform.Find("Header Top Parchment")).DestroyObject();
            RectTransform rectTransform2 = (RectTransform)((RectTransform)rectTransform.Find("Viewport")).Find("Content");
            rectTransform2.DestroyAllChildren();
            _contentTransform = rectTransform2;
            VerticalLayoutGroup component = rectTransform2.gameObject.GetComponent<VerticalLayoutGroup>();
            component.childControlHeight = false;
            component.childForceExpandHeight = false;
            component.childControlWidth = true;
            component.childForceExpandWidth = true;
            _layout = component;
            ContentSizeFitter contentSizeFitter = rectTransform2.gameObject.AddComponent<ContentSizeFitter>();
            contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
            _fitter = contentSizeFitter;
        }
    }
}
