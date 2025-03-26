using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PandaLivingWeaponMod
{
    public static class HP
    {
        public static Dictionary<Type, UnityEngine.Object> UIObjects = new Dictionary<Type, UnityEngine.Object>();

        public static T GetResource<T>(string hint) where T : Component
        {
            if (!HP.UIObjects.TryGetValue(typeof(T), out UnityEngine.Object value))
            {
                value = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault((T x) => x.name == hint);
                HP.UIObjects.Add(typeof(T), value);
                if (value == null)
                {
                    Debug.Log("not instantiate resource: " + hint);
                }
            }
            return (T)UnityEngine.Object.Instantiate(value);
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

        public static T CreateLayer<T, T2>(T2 arg) where T : HPLayer<T2>
        {
            T val = EMono.ui.layers.Find((Layer o) => o.GetType() == typeof(T)) as T;
            if (val != null)
            {
                val.SetActive(enable: true);
                return val;
            }
            T val2 = Create<T>();
            T val3 = UnityEngine.Object.Instantiate(val2);
            val3.gameObject.name = typeof(T).Name;
            val2.DestroyObject();
            val3.Data = arg;
            val3.AddWindow(new Window.Setting
            {
                textCaption = val3.Title,
                bound = val3.Bound,
                allowMove = true,
                transparent = false,
                openLastTab = false
            });
            val3.OnLayout();
            EMono.ui.AddLayer(val3);
            return val3;
        }

        public static T CreateLayer<T>() where T : HPLayer<object>
        {
            return HP.CreateLayer<T, object>(0);
        }

    }
}
