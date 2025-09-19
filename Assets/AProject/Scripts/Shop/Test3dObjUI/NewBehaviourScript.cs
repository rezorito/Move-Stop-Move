using UnityEngine;

public class UITo3DObject : MonoBehaviour {
    public RectTransform uiButton;  // nút UI
    public Camera mainCamera;       // camera chính
    public GameObject object3D;     // object 3D muốn hiển thị
    public float zOffset = 5f;      // khoảng cách so với camera

    void Update() {
        if (uiButton == null || mainCamera == null || object3D == null) return;

        // Lấy vị trí screen từ UI
        Vector3 screenPos = uiButton.position;

        // Gán z để xác định khoảng cách từ camera
        screenPos.z = zOffset;

        // Convert sang World
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);

        // Đặt object
        object3D.transform.position = worldPos;
    }
}
