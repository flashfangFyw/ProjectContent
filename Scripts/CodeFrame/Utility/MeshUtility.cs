using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshUtility
{
    public static void GetMeshMinMaxWorldPoints(GameObject target, out Vector3 max, out Vector3 min)
    {
        max = Vector3.one*float.MinValue;
        min = Vector3.one * float.MaxValue;

        MeshFilter[] mf0s = target.GetComponents<MeshFilter>();
        MeshFilter[] mf1s = target.GetComponentsInChildren<MeshFilter>();

        List<MeshFilter> meshFilterList = new List<MeshFilter>();
        foreach (MeshFilter mf0 in mf0s)
        {
            if (mf0.mesh)
            {
                meshFilterList.Add(mf0);
            }
        }

        foreach (MeshFilter mf1 in mf1s)
        {
            if (mf1.mesh)
            {
                meshFilterList.Add(mf1);
            }
        }


        foreach(MeshFilter meshF in meshFilterList)
        {
            Vector3[] vs = meshF.mesh.vertices;

            foreach(Vector3 locV in vs)
            {
                Vector3 worldV = meshF.gameObject.transform.TransformPoint(locV);
                if(worldV.x <min.x)
                {
                    min.x = worldV.x;
                }
                else
                {
                    if (worldV.x > max.x)
                    {
                        max.x = worldV.x;
                    }
                }

                if (worldV.y < min.y)
                {
                    min.y = worldV.y;
                }
                else
                {
                    if (worldV.y > max.y)
                    {
                        max.y = worldV.y;
                    }
                }

                if (worldV.z < min.z)
                {
                    min.z = worldV.z;
                }
                else
                {
                    if (worldV.z > max.z)
                    {
                        max.z = worldV.z;
                    }
                }
            }
        }
    }
}
