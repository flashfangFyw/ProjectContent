using UnityEngine;
using System.Collections;
namespace ffDevelopmentSpace
{
    public class faceToCamera : MonoBehaviour
    {
        public bool ifback = false;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //transform.rotation = Camera.main.transform.rotation;
            transform.LookAt(Camera.main.transform);
            if (ifback) transform.Rotate(0, 180, 0);
        }
    }
}
