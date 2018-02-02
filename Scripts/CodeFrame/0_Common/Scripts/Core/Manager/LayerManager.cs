using UnityEngine;
using System.Collections;
namespace ffDevelopmentSpace
{
    public class LayerManager
    {
        public static string IGNORE_RAYCAST = "Ignore Raycast";
        #region static funtion
        public static void SetLayer(GameObject targer, string layerName)
        {
            targer.layer = LayerMask.NameToLayer(layerName);
        }
        #endregion
    }
}
