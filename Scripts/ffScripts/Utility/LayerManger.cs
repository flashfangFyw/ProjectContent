using UnityEngine;
using System.Collections;

public class LayerManger
{
    public static string IGNORE_RAYCAST = "Ignore Raycast";
    #region static funtion
    public static void setLayer(GameObject targer,string layerName)
    {
        targer.layer = LayerMask.NameToLayer(layerName);
    }
    #endregion
}
