using UnityEngine;
using System.Collections;
using Zenject;

public class DestroyByTime : MonoBehaviour
{
    [Inject]
	public float lifetime;

    [PostInject]
    void InitializeComponent()
    {
		Destroy(gameObject, lifetime);
	}
}
