using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ColorItem : IColorItem
{
    [Dependency]
    public IColorHistory ColorHistory { get; set; }

    public Color Color { get; set; }

    public void AddToHistory()
    {
        this.ColorHistory.AddColor(this);
    }
}
