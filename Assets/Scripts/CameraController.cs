using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //[SerializeField]
    //private CinemachineVirtualCamera virtualCamera;

    //[SerializeField]
    //private CinemachineFreeLook freeLookCamera;

    //[SerializeField]
    //private float mouseLookSensitivity = 0.5f;

    //private CinemachineComposer aim;

    //private void Awake()
    //{
    //    aim = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
    //}

    //private void FixedUpdate()
    //{
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        freeLookCamera.Priority = 100;
    //        freeLookCamera.m_RecenterToTargetHeading.m_enabled = false;
    //    }
    //    else if (Input.GetMouseButtonUp(1))
    //    {
    //        freeLookCamera.Priority = 0;
    //        freeLookCamera.m_RecenterToTargetHeading.m_enabled = true;
    //    }
    //    if (!Input.GetMouseButton(1))
    //    {
    //        var vertical = Input.GetAxis("Mouse Y") * mouseLookSensitivity;
    //        aim.m_TrackedObjectOffset.y += vertical;
    //        aim.m_TrackedObjectOffset.y = Mathf.Clamp(aim.m_TrackedObjectOffset.y, -4f, 8f);
    //    }
    //}
}