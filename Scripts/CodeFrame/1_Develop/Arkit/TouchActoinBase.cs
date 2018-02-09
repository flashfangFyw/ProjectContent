using UnityEngine;
using System.Collections;
using ffDevelopmentSpace;


/* 
    Author:     fyw 
    CreateDate: 2018-02-08 17:29:18 
    Desc:       触碰动作基类
*/

namespace ffDevelopmentSpace
{

    public class TouchActoinBase : MonoBehaviour
    {

        #region protected property
        protected GameObject _targetGameObject;
        #endregion
        #region private property
        #endregion

        #region unity function
        void OnEnable()
        {
        }
        void Start()
        {
        }
        void Update()
        {
            UpdateTouchInput();
        }
        void OnDisable()
        {
        }
        void OnDestroy()
        {
        }
        #endregion

        #region public function
        public void SetTargetGameObject(GameObject obj)
        {
            _targetGameObject = obj;
        }
        #endregion
        protected virtual void UpdateTouchInput()
        {
        }

        #region event function
        #endregion
    }
}
