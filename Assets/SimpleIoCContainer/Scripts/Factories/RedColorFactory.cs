using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RedColorFactory : IColorFactory
{
    #region IColorFactory Members

    public GameObject GetColorObject()
    {
        var gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        gameObject.name = "red";
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        gameObject.AddComponent<Collectable>();
        gameObject.AddComponent<Rigidbody>();
        return gameObject;
    }

    #endregion
}
