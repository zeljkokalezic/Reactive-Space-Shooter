using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour
{
    // a new instance will be injected here because the color item was not registered as a singleton.
    // you could also use a property here, but its a script where you also need public fields
    // to access settings in the unity editor.To be consequent you could also use public fields for dependencies. -> your decision 
    [Dependency] public IColorItem ColorItem;

	void Start () {
	    this.Inject();
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.ColorItem.Color = this.GetComponent<Renderer>().material.color;
            this.ColorItem.AddToHistory();
        }

        Destroy(this.gameObject);
    }
}
