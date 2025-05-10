using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewSkin", menuName = "Car Purchase/Skin")]
// This scriptable object is used to define the properties of a car that can be purchased in the game.
public class carpurchaseScriptableObject : ScriptableObject
{
    public string SkinName; // Name of the car
    public int price; // Price of the car
    public Image carImage; // Image of the car to be displayed in the UI
    public Material material;
};