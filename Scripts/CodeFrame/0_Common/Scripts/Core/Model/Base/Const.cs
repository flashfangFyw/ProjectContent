using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
namespace ffDevelopmentSpace
{
    public class Const
    {
        public static string UserId = string.Empty;                 //用户ID
        public static string AppName = "client";           //应用程序名称
        public static string ExtName = ".u3d";                  //素材扩展名
        public static string AssetDirname = "StreamingAssets";         //素材目录 

        public static string WebUrl = "https://war.m.jiuwan.com/warship/android_test/asset/";  //测试更新地址
        public static string WebUrl_IOS = "https://war.m.jiuwan.com/warship/ios_test/asset/";  //测试更新地址
        public static string VersionFile = "version.txt";    //版本配置文件
        public static string AssetDetailed = "files.txt";    //资源配置文件
        public static int Platform = 0;                      //平台类型
        public static bool WebMode = true;                  //true下载，false不下载
        public static string ClientVersion = "2.0";         //客户端版本号

        public static int SocketPort;                           //Socket服务器端口
        public static string SocketAddress = "b36.goodplay.com";          //Socket服务器地址 58.246.63.157

    }
}
