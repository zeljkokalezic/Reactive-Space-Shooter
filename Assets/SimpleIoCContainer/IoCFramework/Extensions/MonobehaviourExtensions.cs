using UnityEngine;

public static class MonobehaviourExtensions
{
    private static IDependencyInjector injector;

    public static void SetDependencyInjector(IDependencyInjector dependencyInjector)
    {
        injector = dependencyInjector;
    }

    public static void Inject<T>(this T monoBehaviour) where T : MonoBehaviour
    {
        injector.Inject<T>(monoBehaviour);
    }
}