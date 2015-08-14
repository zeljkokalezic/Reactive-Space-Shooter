using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using System.Collections;

public class ColorHistory : MonoBehaviour, IColorHistory
{

    private readonly List<IColorItem> colorHistory = new List<IColorItem>();

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10,50, 100, 300));

        GUILayout.Label("Color History:");

        foreach (var colorItem in this.colorHistory)
        {
            GUILayout.Label(GetColorName(colorItem.Color));
        }
        
        GUILayout.EndArea();
    }

    private static string GetColorName(Color color)
    {
        if (color.r == 1) return "red";
        if (color.g == 1) return "green";
        if (color.b == 1) return "blue";

        return null;
    }

    #region IColorHistory Members

    public void AddColor(IColorItem item)
    {
        if (this.colorHistory.Count > 10)
        {
            this.colorHistory.RemoveAt(0);
        }

        colorHistory.Add(item);
    }

    #endregion
}
