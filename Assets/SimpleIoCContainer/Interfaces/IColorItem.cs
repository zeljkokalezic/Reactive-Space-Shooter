using UnityEngine;

public interface IColorItem
{
    Color Color { get; set; }
    void AddToHistory();
}