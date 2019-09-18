using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOperation : MonoBehaviour {

    Vector3 RotateGap;
    float rotLeftRight;
    float mouseSensitivity = 2f;
    float verticalRotation;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        CameraMove();
        CameraRotate();
        CameraZoom();
	}

    void CameraMove()    // W,A,S,D 키 입력으로 카메라 상하좌우 움직임
    {
        if(Input.GetKey(KeyCode.D))
        {
            Camera.main.transform.Translate(Vector3.right * 0.5f);
        }
        if(Input.GetKey(KeyCode.A))
        {
            Camera.main.transform.Translate(Vector3.left * 0.5f);
        }
        if(Input.GetKey(KeyCode.W))
        {
            Camera.main.transform.Translate(Vector3.up * 0.5f);
        }
        if(Input.GetKey(KeyCode.S))
        {
            Camera.main.transform.Translate(Vector3.down * 0.5f);
        }

    }

    void CameraRotate()    // 마우스 드래그로 카메라 방향 전환
    {
        if(Input.GetMouseButton(1))
        {
            float yRot = Input.GetAxis("Mouse X") * mouseSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * mouseSensitivity;

            //오브젝트(기준이 되는 축을 유지해야 됨)와 카메라 회전을 분리해야 됨
            //쿼터니안은 곱해야 누적됨
            Camera.main.transform.localRotation *= Quaternion.Euler(0, yRot, 0);
            Camera.main.transform.localRotation *= Quaternion.Euler(-xRot, 0, 0);
            Camera.main.transform.localEulerAngles = new Vector3(Camera.main.transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, 0);


        }

    }

    void CameraZoom()    // 마우스휠로 카메라 줌 인,아웃
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Camera.main.transform.Translate(Vector3.back * 0.5f);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Camera.main.transform.Translate(Vector3.forward * 0.5f);
        }
    }
}
