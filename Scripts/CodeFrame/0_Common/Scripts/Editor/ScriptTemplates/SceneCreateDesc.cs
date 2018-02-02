using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEditor;

/* 
    Author:     fyw
    CreateDate: #CreateDate# 
    Desc:       C#文档模板
*/

public class SceneCreateDesc : UnityEditor.AssetModificationProcessor
{
    private static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        if (path.EndsWith(".cs"))
        {
            string strContent = File.ReadAllText(path);
            strContent = strContent.Replace("#AuthorName#", "fyw").Replace("#CreateDate#", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            File.WriteAllText(path, strContent);
            AssetDatabase.Refresh();
        }
    }
}
