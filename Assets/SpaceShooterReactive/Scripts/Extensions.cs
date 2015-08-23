using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class UnityUIExtensions
{
    public static void Visible(this Button button, bool visible)
    {
        if (!visible)
        {
            button.enabled = false;
            button.GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
            button.GetComponentInChildren<Text>().color = Color.clear;
        }
        else
        {
            button.enabled = true;
            button.GetComponentInChildren<CanvasRenderer>().SetAlpha(1);
            button.GetComponentInChildren<Text>().color = Color.black;
        }
    }
}