using UnityEngine;
using System.Collections;
namespace ffDevelopmentSpace
{
    public interface IAction : IEventDispatcher
    {
        void onExecute();
        void onDispose();
        bool isEnd();
        //void update();
    }
}