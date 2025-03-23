using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using PandaLivingWeaponMod;

public static class HPUI
{
    private static Window BaseWindow;

    private static Dictionary<Type, UnityEngine.Object> UIObjects;

    private static Dictionary<Type, string> UIElementsList;

    public static string[] categories;

    static HPUI()
    {
        categories = new string[20]
        {
        "food", "wall", "floor", "foundation", "door", "furniture", "storage", "spot", "mount", "facility",
        "tool", "deco", "mech", "light", "junk", "ext", "goods", "obj", "other", "area"
        };

        UIElementsList = new Dictionary<Type, string>
    {
        { typeof(Layer), "Layers(Float)" },
        { typeof(UIText), "text caption" },
        { typeof(UIButton), "ButtonBottom Parchment" },
        { typeof(UIScrollView), "Scrollview default" },
        { typeof(InputField), "InputField" },
        { typeof(ScrollRect), "Scrollview parchment with Header" }
    };

        UIObjects = UIElementsList.ToDictionary(
            kv => kv.Key,
            kv =>
            {
                var objs = Resources.FindObjectsOfTypeAll(kv.Key);
                // For ScrollRect, filter to get the one that has a child named "Header Top Parchment"
                if (kv.Key == typeof(ScrollRect))
                {
                    return objs.FirstOrDefault(x => x.name == kv.Value &&
                                                      ((ScrollRect)x).transform.Find("Header Top Parchment") != null);
                }
                return objs.FirstOrDefault(x => x.name == kv.Value);
            }
        );

        BaseWindow = Resources.FindObjectsOfTypeAll<Window>()
            .FirstOrDefault(x => x.name == "Window Parchment");
    }

    public static string __(string ja, string en = "")
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

    public static T Create<T>(Transform? parent = null) where T : MonoBehaviour
    {
        GameObject gameObject = new GameObject(typeof(T).Name, typeof(RectTransform));
        if (parent != null)
        {
            gameObject.transform.SetParent(parent);
        }
        return gameObject.AddComponent<T>();
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

    public static TDerived ReplaceComponent<TBase, TDerived>(this TBase original) where TBase : MonoBehaviour where TDerived : MonoBehaviour
    {
        GameObject gameObject = original.gameObject;
        TDerived val = gameObject.AddComponent<TDerived>();
        FieldInfo[] fields = typeof(TBase).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (FieldInfo fieldInfo in fields)
        {
            fieldInfo.SetValue(val, fieldInfo.GetValue(original));
        }
        UnityEngine.Object.Destroy(original);
        gameObject.name = typeof(TDerived).Name;
        gameObject.transform.DestroyChildren();
        return val;
    }

    public static T GetResource<T>() where T : UnityEngine.Object
    {
        return (T)UnityEngine.Object.Instantiate(UIObjects[typeof(T)]);
    }

    public static bool TryGetHeader(this Window window, out UIHeader? header)
    {
        header = null;
        if (!window.rectHeader)
        {
            return false;
        }
        header = window.rectHeader.GetComponentsInChildren<UIHeader>(includeInactive: true).FirstOrDefault();
        return true;
    }

    public static Transform FindMainContent(this Window window)
    {
        return window.Find("Content View").Find("Inner Simple Scroll").Find("Scrollview default")
            .Find("Viewport")
            .Find("Content");
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
        window.Init(layer);
        window.RectTransform.position = setting.bound.position;
        window.RectTransform.sizeDelta = setting.bound.size;
        window.GetType().GetMethod("RecalculatePositionCaches", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(window, null);
        window.RebuildLayout(recursive: true);
        if (window.TryGetHeader(out UIHeader header))
        {
            header.SetActive(enable: false);
        }
        ((Behaviour)(object)window.GetComponent<VerticalLayoutGroup>()).enabled = false;
        Transform transform = window.Find("Content View");
        transform.DestroyChildren();
        transform.DestroyAllChildren();
        transform.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        UnityEngine.Object.Destroy((UnityEngine.Object)(object)transform.GetComponent<HorizontalLayoutGroup>());
        return window;
    }

    public static T OpenWidget<T>() where T : HPWidget
    {
        T val = EMono.ui.layers.Find((Layer o) => o.GetType() == typeof(T)) as T;
        if (val != null)
        {
            val.SetActive(enable: true);
            return val;
        }
        T val2 = GetResource<Layer>().ReplaceComponent<Layer, T>();
        val2.Setup(0);
        EMono.ui.AddLayer(val2);
        return val2;
    }

    public static T OpenWidget<T>(object arg) where T : HPWidget
    {
        T val = EMono.ui.layers.Find((Layer o) => o.GetType() == typeof(T)) as T;
        if (val != null)
        {
            val.SetActive(enable: true);
            return val;
        }
        T val2 = GetResource<Layer>().ReplaceComponent<Layer, T>();
        val2.Setup(arg);
        EMono.ui.AddLayer(val2);
        return val2;
    }

    public static UIContent CreateBaseContent(Transform parent)
    {
        RectTransform component = parent.gameObject.GetComponent<RectTransform>();
        UIContent uIContent = Create<UIContent>(parent);
        RectTransform component2 = uIContent.GetComponent<RectTransform>();
        component2.SetAnchor(RectPosition.TopLEFT);
        component2.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, component.rect.width);
        component2.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, component.rect.height);
        VerticalLayoutGroup obj = uIContent.gameObject.AddComponent<VerticalLayoutGroup>();
        ((HorizontalOrVerticalLayoutGroup)obj).childControlHeight = false;
        ((HorizontalOrVerticalLayoutGroup)obj).childForceExpandHeight = false;
        ((LayoutGroup)obj).padding = new RectOffset(0, 0, 0, 0);
        ((HorizontalOrVerticalLayoutGroup)obj).spacing = 5f;
        return uIContent;
    }

    public static T CreatePage<T, TArg>(string id, Window window, TArg arg) where T : HPLayout<TArg>, new()
    {
        Transform parent = window.Find("Content View");
        UIContent uIContent = CreateBaseContent(parent);
        uIContent.gameObject.name = id;
        uIContent.gameObject.GetComponent<RectTransform>();
        VerticalLayoutGroup component = uIContent.gameObject.GetComponent<VerticalLayoutGroup>();
        ((LayoutGroup)component).padding = new RectOffset(10, 10, 40, 0);
        RectTransform component2 = ((Component)(object)component).gameObject.GetComponent<RectTransform>();
        T val = new T();
        val.window = window;
        val.parent = parent;
        val.root = uIContent;
        val.layout = component;
        val.rect = component2;
        val.OnCreate(arg);
        uIContent.RebuildLayout(recursive: true);
        return val;
    }

    public static T CreatePage<T>(string id, Window window) where T : HPLayout<object>, new()
    {
        return CreatePage<T, object>(id, window, 0);
    }
}
