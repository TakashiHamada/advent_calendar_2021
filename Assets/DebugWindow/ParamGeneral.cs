using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Param/ParamGeneral")]
public class ParamGeneral : ScriptableObject
{
    [DisplayAsString]
    public string param;
    [DisplayAsString]
    public string param2;
}
