using UnityEngine;
using System.Collections.Generic;
public class ChestData : MonoBehaviour
{
    public string chestName;
    public bool isOpen;
    public List<string> items;
    public void Initialize(string name, bool open, List<string> items)
{
    this.chestName = name;
    this.isOpen = open;
    this.items = items;
}
}
