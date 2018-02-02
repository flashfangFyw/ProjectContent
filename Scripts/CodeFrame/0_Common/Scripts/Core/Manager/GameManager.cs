using UnityEngine;
using System.Collections;

/* 
    Author:     fyw 
    CreateDate: 2017-06-05 14:31:58 
    Desc:       注释 
*/
namespace ffDevelopmentSpace
{
    public class GameManager : SingletonMB<GameManager>
    {

        #region public property
        #endregion
        #region private property
        #endregion

        #region unity function
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        void OnEnable()
        {
        }
        void Start()
        {
        }
        void Update()
        {
        }
        void OnDisable()
        {
        }
        void OnDestroy()
        {
        }
        #endregion

        #region public function
        #endregion
        #region private function
        #endregion

        #region event function
        #endregion
    }
}
