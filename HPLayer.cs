using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace PandaLivingWeaponMod
{
    public abstract class HPLayer<T> : ELayer
    {
        protected T? _data;

        public virtual string Title { get; } = "ウィンドウ"._("Window");

        public virtual Rect Bound { get; } = new Rect(0f, 0f, 640f, 480f);

        public Window Window => windows[0];

        public T Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        public override bool blockWidgetClick => false;

        public virtual void OnLayout()
        {
        }

        public L CreateTab<L>(string idLang, string id) where L : HPLayout<T>
        {
            Transform transform = Window.Find("Content View");
            transform.gameObject.GetComponent<RectTransform>();
            if (!HP.UIObjects.ContainsKey(typeof(ScrollRect)))
            {
                Resources.Load("UI/Layer/LayerAnnounce");
            }
            ScrollRect resource = HP.GetResource<ScrollRect>("Scrollview parchment with Header");
            resource.gameObject.name = id;
            RectTransform rectTransform = resource.Rect();
            rectTransform.SetParent(transform);
            UIContent orCreate = resource.GetOrCreate<UIContent>();
            ((RectTransform)rectTransform.Find("Header Top Parchment")).SetActive(enable: false);
            RectTransform obj = (RectTransform)((RectTransform)rectTransform.Find("Viewport")).Find("Content");
            obj.DestroyAllChildren();
            VerticalLayoutGroup component = obj.gameObject.GetComponent<VerticalLayoutGroup>();
            component.childControlHeight = true;
            component.padding = new RectOffset(5, 5, 5, 5);
            L val = HP.Create<L>(obj);
            val.gameObject.name = id;
            val.GetComponent<RectTransform>();
            VerticalLayoutGroup verticalLayoutGroup = val.gameObject.AddComponent<VerticalLayoutGroup>();
            verticalLayoutGroup.childControlHeight = false;
            verticalLayoutGroup.childForceExpandHeight = false;
            verticalLayoutGroup.padding = new RectOffset(10, 10, 0, 10);
            val.Layer = this;
            val.OnLayout();
            val.RebuildLayout(recursive: true);
            Window.AddTab(idLang, orCreate);
            return val;
        }

        public override void OnBeforeAddLayer()
        {
            option.rebuildLayout = true;
        }

        public override void OnAfterAddLayer()
        {
            foreach (Window window in windows)
            {
                window.Rect();
                window.RectTransform.localPosition = new Vector3(window.setting.bound.x, window.setting.bound.y, 0f);
            }
        }
    }

}
