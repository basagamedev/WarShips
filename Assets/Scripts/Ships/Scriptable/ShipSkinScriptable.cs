using UnityEngine;

[CreateAssetMenu(fileName = "ShipSkin", menuName = "ScriptableObjects/ShipSkin", order = 2)]
public class ShipSkinScriptable : ScriptableObject
{
    #region SERIALIZE_FIELDS
    [Header("Skin")]
    [SerializeField] private Sprite[] shipSkin = null;
    [SerializeField] private Sprite[] flagSkin = null;
    #endregion

    #region PROPERTIES
    public Sprite[] ShipSkin => shipSkin;
    public Sprite[] FlagSkin => flagSkin;
    #endregion
}
