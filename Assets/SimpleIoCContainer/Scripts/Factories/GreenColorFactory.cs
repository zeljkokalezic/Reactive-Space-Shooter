using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GreenColorFactory : IColorFactory
{
    #region IColorFactory Members

    public GameObject GetColorObject()
    {
        var gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        gameObject.name = "green";
        gameObject.GetComponent<Renderer>().material.color = Color.green;
        gameObject.AddComponent<Collectable>();
        gameObject.AddComponent<Rigidbody>();
        return gameObject;
    }

    #endregion
}
