
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinList", menuName = "Car Purchase/SkinList")]
// This scriptable object is used to define the properties of a car that can be purchased in the game.
public class carpurchaselistScriptableObject : ScriptableObject
{
    public List<carpurchaseScriptableObject> carPurchaseList = new List<carpurchaseScriptableObject>(); // List of all cars available for purchase
}
