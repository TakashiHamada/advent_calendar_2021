using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GeneralParameter")]
public class GeneralParameter : ScriptableObject
{
    [DisplayAsString]
    public string param_0;
    [DisplayAsString]
    public string param_1;
}
