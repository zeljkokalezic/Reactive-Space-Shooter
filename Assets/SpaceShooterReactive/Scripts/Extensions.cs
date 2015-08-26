using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Zenject;
using ModestTree;
using System.Reflection;

public static class UnityUIExtensions
{
    public static void Visible(this Button button, bool visible)
    {
        if (!visible)
        {
            button.enabled = false;
            button.GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
            button.GetComponentInChildren<Text>().color = Color.clear;
        }
        else
        {
            button.enabled = true;
            button.GetComponentInChildren<CanvasRenderer>().SetAlpha(1);
            button.GetComponentInChildren<Text>().color = Color.black;
        }
    }
}

//namespace Zenject
//{
//    public static class GameObjectFactoryExtensions
//    {
//        public static TValue Create<TValue>(this GameObjectFactory<TValue> factory, GameObject prefab)
//        {
//            Assert.That(prefab != null,
//               "Null prefab given to factory create method when instantiating object with type '{0}'.", typeof(TValue));

//            FieldInfo _container = factory.GetType().GetField("_container", BindingFlags.NonPublic | BindingFlags.Instance);
//            return (TValue)((DiContainer)_container.GetValue(factory)).InstantiatePrefabForComponent(typeof(TValue), prefab);
//        }
//    }
//}