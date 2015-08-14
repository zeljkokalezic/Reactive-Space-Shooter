using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class ColorDropper : MonoBehaviour
{

    [Dependency("red")]     public IColorFactory RedColorFactory;
    [Dependency("blue")]    public IColorFactory BlueColorFactory;
    [Dependency("green")]   public IColorFactory GreenColorFactory;

    public float MinPosition = -3f;
    public float MaxPosition = 3f;
    public float Speed = 100f;
    private float direction = 1f;

	void Start () {
	    this.Inject();
	    this.StartCoroutine(this.DropColor());
	}

    void FixedUpdate()
    {
        if (this.transform.position.x > this.MaxPosition)
        {
            this.direction = -1;
        }
        else if (this.transform.position.x < this.MinPosition)
        {
            this.direction = 1;
        }
        
        this.GetComponent<Rigidbody>().velocity = new Vector3(this.direction, 0, 0);
    }

    IEnumerator DropColor()
    {
        while (true)
        {
            var random = Random.Range(0, 3);
            GameObject colorObject = null;
            switch (random)
            {
                case 0: colorObject = this.RedColorFactory.GetColorObject(); break;
                case 1: colorObject = this.GreenColorFactory.GetColorObject(); break;
                case 2: colorObject = this.BlueColorFactory.GetColorObject(); break;
            }

            colorObject.transform.position = this.transform.position - Vector3.up * 1.1f;
            colorObject.transform.parent = this.transform;

            yield return new WaitForSeconds(1);
        }
    }
}
