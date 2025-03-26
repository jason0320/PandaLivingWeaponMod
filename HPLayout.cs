using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static HotItemLayout;

namespace PandaLivingWeaponMod
{
    public class HPLayout : UIContent
    {
        public virtual void OnLayout()
        {
        }

        public RectTransform Spacer(int height, int width = 1)
        {
            Transform obj = Util.Instantiate<Transform>("UI/Element/Deco/Space", layout);
            obj.SetParent(base.transform);
            RectTransform rectTransform = obj.Rect();
            rectTransform.sizeDelta = new Vector2(width, height);
            if (height != 1)
            {
                rectTransform.LayoutElement().preferredHeight = height;
            }
            if (width != 1)
            {
                rectTransform.LayoutElement().preferredWidth = width;
            }
            return rectTransform;
        }

        public UIItem Header(string text, Sprite? sprite = null)
        {
            UIItem uIItem = AddHeader(text, sprite);
            uIItem.transform.SetParent(base.transform);
            return uIItem;
        }

        public UIItem HeaderCard(string text, Sprite? sprite = null)
        {
            UIItem uIItem = AddHeaderCard(text, sprite);
            uIItem.transform.SetParent(base.transform);
            return uIItem;
        }

        public UIItem HeaderSmall(string text, Sprite? sprite = null)
        {
            UIItem uIItem = AddHeader("HeaderNoteSmall", text, sprite);
            uIItem.transform.SetParent(base.transform);
            return uIItem;
        }

        public UIText Text(string text, FontColor color = FontColor.DontChange)
        {
            UIItem uIItem = AddText(text, color);
            uIItem.transform.SetParent(base.transform);
            uIItem.text1.horizontalOverflow = HorizontalWrapMode.Wrap;
            uIItem.GetOrCreate<LayoutElement>().minWidth = 80f;
            return uIItem.GetComponent<UIText>();
        }

        public UIText TextLong(string text, FontColor color = FontColor.DontChange)
        {
            UIItem uIItem = AddText("NoteText_long", text, color);
            uIItem.transform.SetParent(base.transform);
            uIItem.text1.horizontalOverflow = HorizontalWrapMode.Wrap;
            uIItem.GetOrCreate<LayoutElement>().minWidth = 80f;
            return uIItem.GetComponent<UIText>();
        }

        public UIText TextMedium(string text, FontColor color = FontColor.DontChange)
        {
            UIItem uIItem = AddText("NoteText_medium", text, color);
            uIItem.transform.SetParent(base.transform);
            uIItem.text1.horizontalOverflow = HorizontalWrapMode.Wrap;
            uIItem.GetOrCreate<LayoutElement>().minWidth = 80f;
            return uIItem.GetComponent<UIText>();
        }

        public UIText TextSmall(string text, FontColor color = FontColor.DontChange)
        {
            UIItem uIItem = AddText("NoteText_small", text, color);
            uIItem.transform.SetParent(base.transform);
            uIItem.text1.horizontalOverflow = HorizontalWrapMode.Wrap;
            uIItem.GetOrCreate<LayoutElement>().minWidth = 80f;
            return uIItem.GetComponent<UIText>();
        }

        public UIText TextFlavor(string text, FontColor color = FontColor.DontChange)
        {
            UIItem uIItem = AddText("NoteText_flavor", text, color);
            uIItem.transform.SetParent(base.transform);
            uIItem.GetOrCreate<LayoutElement>().minWidth = 80f;
            return uIItem.GetComponent<UIText>();
        }

        public UIItem Topic(string text, string? value = null)
        {
            UIItem uIItem = AddTopic("TopicDefault", text, value);
            uIItem.transform.SetParent(base.transform);
            return uIItem;
        }

        public UIItem TopicAttribute(string text, string? value = null)
        {
            UIItem uIItem = AddTopic("TopicAttribute", text, value);
            uIItem.transform.SetParent(base.transform);
            return uIItem;
        }

        public UIItem TopicDomain(string text, string? value = null)
        {
            UIItem uIItem = AddTopic("TopicDomain", text, value);
            uIItem.transform.SetParent(base.transform);
            return uIItem;
        }

        public UIItem TopicLeft(string text, string? value = null)
        {
            UIItem uIItem = AddTopic("TopicLeft", text, value);
            uIItem.transform.SetParent(base.transform);
            return uIItem;
        }

        public UIItem TopicPair(string text, string? value = null)
        {
            UIItem uIItem = AddTopic("TopicPair", text, value);
            uIItem.transform.SetParent(base.transform);
            return uIItem;
        }

        public UIButton Button(string text, Action action)
        {
            UIButton uIButton = AddButton(text, delegate
            {
                SE.ClickGeneral();
                action();
            });
            uIButton.transform.SetParent(base.transform);
            uIButton.GetOrCreate<LayoutElement>().minWidth = 80f;
            return uIButton;
        }

        public UIButton Toggle(string text, bool isOn = false, Action<bool>? onClick = null)
        {
            UIButton uIButton = AddToggle(text, isOn, onClick);
            uIButton.transform.SetParent(base.transform);
            return uIButton;
        }

        public UISlider Slider<TValue>(int index, IList<TValue> list, Action<int, TValue> onChange, Func<TValue, string>? getInfo = null)
        {
            if (!HP.UIObjects.ContainsKey(typeof(UISlider)))
            {
                LayerEditPCC layerEditPCC = Layer.Create<LayerEditPCC>();
                HP.UIObjects.Add(typeof(UISlider), UnityEngine.Object.Instantiate(layerEditPCC.sliderPortrait));
                UnityEngine.Object.Destroy(layerEditPCC.gameObject);
            }
            UISlider obj = (UISlider)UnityEngine.Object.Instantiate(HP.UIObjects[typeof(UISlider)]);
            obj.Rect().SetParent(base.transform);
            obj.SetList(index, list, onChange, getInfo);
            obj.textMain.text = "";
            obj.textInfo.text = "";
            return obj;
        }

        public Slider Slider(float value, Action<float> setvalue, float min, float max, Func<float, string>? labelfunc = null)
        {
            if (!HP.UIObjects.ContainsKey(typeof(Slider)))
            {
                LayerConfig layerConfig = Layer.Create<LayerConfig>();
                HP.UIObjects.Add(typeof(Slider), UnityEngine.Object.Instantiate(layerConfig.sliderBGM));
                UnityEngine.Object.Destroy(layerConfig.gameObject);
            }
            Slider obj = (Slider)UnityEngine.Object.Instantiate(HP.UIObjects[typeof(Slider)]);
            obj.Rect().SetParent(base.transform);
            Func<float, string> labelfunc2 = labelfunc;
            obj.SetSlider(value, delegate (float v)
            {
                string result = ((labelfunc2 != null) ? labelfunc2(v) : null) ?? v.ToString();
                setvalue(v);
                return result;
            }, (int)min, (int)max);
            return obj;
        }
        public HPScroll Scroll()
        {
            HPScroll HPScroll = HP.Create<HPScroll>(base.transform);
            HPScroll.OnLayout();
            return HPScroll;
        }

        public T Create<T>() where T : HPLayout
        {
            T val = HP.Create<T>(base.transform);
            val.OnLayout();
            return val;
        }
    }

    public class HPLayout<T> : HPLayout
    {
        protected HPLayer<T>? _layer;

        public HPLayer<T> Layer
        {
            get
            {
                return _layer;
            }
            set
            {
                _layer = value;
            }
        }

        public override void OnSwitchContent(int idTab)
        {
            Build();
        }
    }

}
