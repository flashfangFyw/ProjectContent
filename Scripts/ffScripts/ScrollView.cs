using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TouchMoveDir
{
    idle, left, right, up, down
}

public class ScrollView : MonoBehaviour
{
        public GameObject target;
        float minDis = 1;
        TouchMoveDir moveDir;

        // Use this for initialization
        void Start()
        {
            Input.multiTouchEnabled = true;
            Input.simulateMouseWithTouches = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (Input.GetTouch(0).deltaPosition.sqrMagnitude > minDis)
                {
                    Vector2 deltaDir = Input.GetTouch(0).deltaPosition;
                    if (Mathf.Abs(deltaDir.x) > Mathf.Abs(deltaDir.y))
                    {
                        moveDir = deltaDir.x > 0 ? TouchMoveDir.right : TouchMoveDir.left;
                    }
                    if (Mathf.Abs(deltaDir.y) > Mathf.Abs(deltaDir.x))
                    {
                        moveDir = deltaDir.y > 0 ? TouchMoveDir.up : TouchMoveDir.down;
                    }
                }
            }

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                moveDir = TouchMoveDir.idle;
            }

            if (moveDir == TouchMoveDir.right)
            {
                Debug.Log("right");
                target.transform.position += transform.right * 0.2f;
            }
            if (moveDir == TouchMoveDir.left)
            {
                Debug.Log("left");
                target.transform.position -= transform.right * 0.2f;
            }
            if (moveDir == TouchMoveDir.up)
            {
                target.transform.position += transform.up * 0.2f;
                target.transform.position += transform.up * 0.2f;
            }
            if (moveDir == TouchMoveDir.down)
            {
                target.transform.position -= transform.up * 0.2f;
            }
        }


        public void Mid()
        {
            target.transform.position += transform.up *500f;
        }
}


