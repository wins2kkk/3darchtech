using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARGame2
{
    public class ARBall : MonoBehaviour, IPlayerCanTouchable
    {
        public void OnPlayerTouch()
        {
            Debug.Log("Touch On Ball");
            canTouch = true;
        }

            // Tốc độ xoay
        public float rotationSpeed = 0.1f;

        // Biến để lưu vị trí ngón tay trước đó
        private Vector2 lastTouchPosition;

        // Xác định xem ngón tay có đang chạm vào màn hình hay không
        private bool isTouching = false;
        private bool canTouch = false;

        void Update()
        {
            if(canTouch == false) return;
            // Kiểm tra xem có bao nhiêu ngón tay đang chạm vào màn hình
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0); // Lấy dữ liệu ngón tay đầu tiên

                if (touch.phase == TouchPhase.Began)
                {
                    // Khi bắt đầu chạm vào màn hình, lưu vị trí ngón tay
                    lastTouchPosition = touch.position;
                    isTouching = true;
                }
                else if (touch.phase == TouchPhase.Moved && isTouching)
                {
                    // Tính toán sự thay đổi vị trí ngón tay
                    Vector2 deltaPosition = touch.deltaPosition;

                    // Xoay đối tượng dựa trên sự thay đổi vị trí theo trục Y (ngang)
                    transform.Rotate(Vector3.up, -deltaPosition.x * rotationSpeed);
                    
                    // Cập nhật vị trí ngón tay hiện tại
                    lastTouchPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    // Khi ngón tay rời khỏi màn hình
                    isTouching = false;
                }
            }
        }
    }
}

