using UnityEngine;
using Mirror;

public class BoardController : NetworkBehaviour
{
    [HideInInspector] public Vector3 CurrentMousePosition;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
		if (mainCamera == null)
		{
            mainCamera = Camera.main;
		}

        Ray ray = GetRay();
        RaycastHit[] hits = Physics.RaycastAll(ray);

        foreach (var hit in hits)
        {
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Table")) continue;
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.red);

            CurrentMousePosition = hit.point;

            break;
        }
    }

    public Ray GetRay()
	{
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        return ray;
	}
}