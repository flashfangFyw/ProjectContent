using UnityEngine;
using System.Collections;
namespace ffDevelopmentSpace
{
    public interface IActionLine : IAction
    {
        void addAction(IAction action);
    }
}
