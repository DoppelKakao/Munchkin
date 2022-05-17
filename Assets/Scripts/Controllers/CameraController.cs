using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private Transform playerCameraTransform;

	private void Start()
	{
		playerCameraTransform = gameObject.transform;
	}

	public void AttachCameraToCameraMountPoint(Transform cameraMountPoint)
	{
		playerCameraTransform.SetParent(cameraMountPoint);
		playerCameraTransform.localPosition = cameraMountPoint.transform.localPosition;
	}
}
