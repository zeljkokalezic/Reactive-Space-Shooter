using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    public float Speed;
    public float JumpForce;
	void FixedUpdate () {

	    if (Input.GetButtonDown("Jump"))
	    {
	        this.GetComponent<Rigidbody>().AddForce(Vector3.up * this.JumpForce, ForceMode.Impulse);
	    }

        this.transform.position = new Vector3(this.transform.position.x + Input.GetAxis("Horizontal") *
            this.Speed * Time.deltaTime, this.transform.position.y, this.transform.position.z);
	}
}
