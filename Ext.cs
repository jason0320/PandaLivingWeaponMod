using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using static HotItemLayout;

namespace PandaLivingWeaponMod
{
    public static class Ext
    {
        public static List<Dropdown.OptionData> ToDropdownOptions(this List<string> list)
        {
            return list.Select((string x) => new Dropdown.OptionData(x)).ToList();
        }

        public static string _(this string ja, string en = "")
        {
            if (en == null)
            {
                return ja;
            }
            if (!Lang.isJP)
            {
                return en;
            }
            return ja;
        }

        public static void DestroyAllChildren(this Transform parent)
        {
            foreach (Transform item in parent)
            {
                item.gameObject.SetActive(value: false);
                UnityEngine.Object.Destroy(item.gameObject);
                item.SetActive(enable: false);
                UnityEngine.Object.Destroy(item);
            }
        }

        public static T2 ReplaceComponent<T, T2>(this T original) where T : MonoBehaviour where T2 : MonoBehaviour
        {
            GameObject gameObject = original.gameObject;
            for (int i = 0; i < original.transform.childCount; i++)
            {
                original.transform.GetChild(i).gameObject.SetActive(value: false);
            }
            UnityEngine.Object.Destroy(original);
            gameObject.name = typeof(T2).Name;
            T2 val = gameObject.AddComponent<T2>();
            try
            {
                FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (FieldInfo fieldInfo in fields)
                {
                    fieldInfo.SetValue(val, fieldInfo.GetValue(original));
                }
            }
            catch (Exception)
            {
            }
            return val;
        }

        public static LayoutElement LayoutElement(this Component component)
        {
            return component.GetOrCreate<LayoutElement>();
        }

        public static void DestroyObject(this Component component)
        {
            UnityEngine.Object.Destroy(component.gameObject);
        }

        public static T ReplaceLayerComponent<T>(this Layer original) where T : MonoBehaviour
        {
            GameObject gameObject = original.gameObject;
            T val = gameObject.AddComponent<T>();
            try
            {
                FieldInfo[] fields = typeof(Layer).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (FieldInfo fieldInfo in fields)
                {
                    fieldInfo.SetValue(val, fieldInfo.GetValue(original));
                }
                UnityEngine.Object.Destroy(original);
                gameObject.name = typeof(T).Name;
            }
            catch (Exception)
            {
            }
            return val;
        }

        public static Window AddWindow(this Layer layer, Window.Setting setting)
        {
            if (setting.tabs == null)
            {
                setting.tabs = new List<Window.Setting.Tab>();
            }
            LayerList obj = (LayerList)Layer.Create(typeof(LayerList).Name);
            Window window = obj.windows.First();
            window.transform.SetParent(layer.transform);
            layer.windows.Add(window);
            UnityEngine.Object.Destroy(obj.gameObject);
            window.setting = setting;
            window.RectTransform.sizeDelta = setting.bound.size;
            window.RectTransform.position = setting.bound.position;
            window.Find("Content View").DestroyAllChildren();
            return window;
        }

        public static UIInputText WithPlaceholder(this UIInputText input, string text)
        {
            Transform transform = input.Find("Placeholder");
            Text component = transform.GetComponent<Text>();
            transform.SetActive(enable: true);
            component.text = text;
            return input;
        }

        public static T WithName<T>(this T component, string text) where T : Component
        {
            component.gameObject.name = text;
            return component;
        }

        public static T WithWidth<T>(this T component, int size) where T : Component
        {
            component.GetOrCreate<LayoutElement>().preferredWidth = size;
            return component;
        }

        public static T WithMinWidth<T>(this T component, int size) where T : Component
        {
            component.GetOrCreate<LayoutElement>().minWidth = size;
            return component;
        }

        public static T WithHeight<T>(this T component, int size) where T : Component
        {
            component.GetOrCreate<LayoutElement>().preferredHeight = size;
            return component;
        }

        public static T WithMinHeight<T>(this T component, int size) where T : Component
        {
            component.GetOrCreate<LayoutElement>().minHeight = size;
            return component;
        }

        public static T WithPivot<T>(this T component, float x, float y) where T : Component
        {
            component.Rect().pivot = new Vector2(x, y);
            return component;
        }

        public static T WithLayerParent<T>(this T component) where T : Component
        {
            try
            {
                component.transform.SetParent(component.transform.GetComponentInParent<ELayer>().transform);
            }
            catch
            {
            }
            component.SetActive(enable: false);
            return component;
        }
    }
}
