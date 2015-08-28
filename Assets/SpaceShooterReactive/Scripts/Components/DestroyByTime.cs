using UnityEngine;
using System.Collections;
using Zenject;

public class DestroyByTime : MonoBehaviour
{
    [Inject]
	public float lifetime;

	void Start ()
	{
		Destroy (gameObject, lifetime);
	}
}
