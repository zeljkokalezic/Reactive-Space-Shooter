using UnityEngine;
using System.Collections;
using Zenject;

public class Mover : MonoBehaviour
{
    public class Factory : ComponentFactory<float, Mover>
    {
    }

    [Inject]
	public float speed;

    [PostInject]
    void InitializeComponent()
	{
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}
}
