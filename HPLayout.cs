using System;
using UnityEngine;
using UnityEngine.UI;
using PandaLivingWeaponMod;

public class HPLayout<T>
{
    public class GridLayout
    {
        public UIContent ui;

        public RectTransform transform;

        public GridLayoutGroup group;

        public UIItemList items;

        public GridLayout(Transform parent)
        {
            ui = HPUI.Create<UIContent>(parent);
            transform = ui.GetComponent<RectTransform>();
            transform.SetAnchor(RectPosition.TopLEFT);
            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
            group = ui.gameObject.AddComponent<GridLayoutGroup>();
            group.padding = new RectOffset(0, 0, 0, 15);
            items = ui.gameObject.AddComponent<UIItemList>();
            items.gridLayout.cellSize = new Vector2(180f, 50f);
            items.gridLayout.spacing = new Vector2(2f, 2f);
            items.gridLayout.constraint = (GridLayoutGroup.Constraint)1;
            items.gridLayout.constraintCount = 3;
        }
    }

    public class LayoutGroup
    {
        public UIContent ui;

        public RectTransform transform;

        public HorizontalLayoutGroup group;

        public UIItemList items;

        public LayoutElement element;

        public LayoutGroup(Transform parent)
        {
            ui = HPUI.Create<UIContent>(parent);
            element = ui.gameObject.AddComponent<LayoutElement>();
            element.preferredHeight = 36f;
            transform = ui.GetComponent<RectTransform>();
            transform.SetAnchor(RectPosition.TopLEFT);
            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 52f);
            group = ui.gameObject.AddComponent<HorizontalLayoutGroup>();
            ((HorizontalOrVerticalLayoutGroup)group).childControlHeight = false;
            ((HorizontalOrVerticalLayoutGroup)group).childForceExpandHeight = false;
            group.childAlignment = (TextAnchor)3;
            items = ui.gameObject.AddComponent<UIItemList>();
            items.layoutItems.padding = new RectOffset(2, 2, 2, 2);
        }
    }

    public class InputTextField
    {
        public RectTransform transform;

        public UIInputText input;

        public RectTransform inputTransform;

        public Transform placeholder;

        public Text placeholderText;

        public LayoutElement element;

        public InputTextField(Transform parent)
        {
            Transform transform = Util.Instantiate("UI/Element/Input/InputText", parent);
            this.transform = transform.gameObject.GetComponent<RectTransform>();
            element = transform.gameObject.GetComponent<LayoutElement>();
            element.preferredWidth = 80f;
            input = transform.Find("InputField").GetComponent<UIInputText>();
            input.gameObject.GetComponent<CanvasRenderer>().SetColor(new Color(0.99f, 0.99f, 0.99f, 0.3f));
            inputTransform = input.gameObject.GetComponent<RectTransform>();
            inputTransform.anchoredPosition = new Vector2(0f, 27f);
            inputTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 37f);
            ((Graphic)input.Find("Text").gameObject.GetComponent<Text>()).color = new Color(0.2896f, 0.2255f, 0.1293f, 1f);
            transform.Find("text invalid (1)").SetActive(enable: false);
            placeholder = input.Find("Placeholder");
            placeholderText = placeholder.GetComponent<Text>();
            ((Graphic)placeholderText).color = new Color(0.2896f, 0.2255f, 0.1293f, 1f);
            placeholder.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 16f);
            input.Find("InputField Input Caret");
            input.Find("Image").SetActive(enable: false);
            this.transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 55f);
        }
    }

    public class ScrollLayout
    {
        public ScrollRect scroll;

        public RectTransform rect;

        public RectTransform headerRect;

        public UIText header;

        public VerticalLayoutGroup layout;

        public RectTransform root;

        public LayoutElement element;

        public ScrollLayout(Transform parent)
        {
            scroll = HPUI.GetResource<ScrollRect>();
            rect = ((Component)(object)scroll).Rect();
            rect.SetParent(parent);
            element = ((Component)(object)scroll).gameObject.GetComponent<LayoutElement>();
            element.flexibleHeight = 10f;
            headerRect = (RectTransform)rect.Find("Header Top Parchment");
            header = headerRect.Find("UIText").gameObject.GetComponent<UIText>();
            RectTransform rectTransform = (RectTransform)rect.Find("Viewport");
            rectTransform.anchoredPosition = new Vector2(0f, 0f);
            root = (RectTransform)rectTransform.Find("Content");
            root.DestroyChildren();
            root.DestroyAllChildren();
            layout = root.gameObject.GetComponent<VerticalLayoutGroup>();
            ((HorizontalOrVerticalLayoutGroup)layout).childControlHeight = true;
        }
    }

    public Window window;

    public Transform parent;

    public UIContent root;

    public VerticalLayoutGroup layout;

    public RectTransform rect;

    public virtual void OnCreate(T data)
    {
    }

    public UIItem AddTopic(string text, string value = "", Transform? parent = null)
    {
        UIItem uIItem = root.AddTopic(text, value);
        uIItem.transform.SetParent(parent ?? ((Component)(object)layout).transform);
        return uIItem;
    }

    public UIItem AddText(string text, Transform? parent = null)
    {
        UIItem uIItem = root.AddText(text);
        uIItem.transform.SetParent(parent ?? ((Component)(object)layout).transform);
        LayoutElement component = uIItem.gameObject.GetComponent<LayoutElement>();
        UIText component2 = uIItem.gameObject.GetComponent<UIText>();
        component.minWidth = ((((Text)component2).preferredWidth < 80f) ? 80f : ((Text)component2).preferredWidth);
        uIItem.gameObject.GetComponent<ContentSizeFitter>().horizontalFit = (ContentSizeFitter.FitMode)2;
        return uIItem;
    }

    public UIButton AddToggle(string text, bool isOn, Action<bool> action, Transform? parent = null)
    {
        UIButton uIButton = root.AddToggle(text, isOn, action);
        ((Component)(object)uIButton).transform.SetParent(parent ?? ((Component)(object)layout).transform);
        ((Component)(object)uIButton).gameObject.AddComponent<LayoutElement>();
        return uIButton;
    }

    public UIDropdown AddDropdown(Transform? parent = null)
    {
        Transform transform = Util.Instantiate("UI/Element/Input/DropdownDefault", parent ?? ((Component)(object)layout).transform);
        Text component = transform.Find("Label").GetComponent<Text>();
        component.horizontalOverflow = (HorizontalWrapMode)0;
        component.verticalOverflow = (VerticalWrapMode)0;
        return transform.GetComponent<UIDropdown>();
    }

    public UIButton AddButton(string text, Action action, Transform? parent = null)
    {
        UIButton uIButton = root.AddButton(text, action);
        ((Component)(object)uIButton).transform.SetParent(parent ?? ((Component)(object)layout).transform);
        ((Component)(object)uIButton).gameObject.AddComponent<LayoutElement>();
        return uIButton;
    }

    public InputTextField AddInputText(Transform? parent = null)
    {
        return new InputTextField(parent ?? ((Component)(object)layout).transform);
    }

    public TLayout AddItem<TLayout>(Transform? parent = null) where TLayout : MonoBehaviour
    {
        GameObject gameObject = new GameObject(typeof(TLayout).Name, typeof(RectTransform));
        gameObject.transform.SetParent(root.gameObject.transform);
        TLayout val = gameObject.AddComponent<TLayout>();
        val.transform.SetParent(parent ?? ((Component)(object)layout).transform);
        return val;
    }

    public RectTransform AddSpace(int sizeY = 0, int sizeX = 1, Transform? parent = null)
    {
        RectTransform rectTransform = Util.Instantiate<Transform>("UI/Element/Deco/Space", parent ?? ((Component)(object)layout).transform).Rect();
        rectTransform.sizeDelta = new Vector2(sizeX, sizeY);
        if (sizeY != 1)
        {
            rectTransform.GetComponent<LayoutElement>().preferredHeight = sizeY;
        }
        if (sizeX != 1)
        {
            rectTransform.GetComponent<LayoutElement>().preferredWidth = sizeX;
        }
        return rectTransform;
    }

    public GridLayout AddGridLaout(Transform? parent)
    {
        return new GridLayout(parent ?? ((Component)(object)layout).transform);
    }

    public LayoutGroup AddLaoutGroup(Transform? parent)
    {
        return new LayoutGroup(parent ?? ((Component)(object)layout).transform);
    }

    public ScrollLayout AddScrollLayout(Transform parent)
    {
        return new ScrollLayout(parent);
    }

}
