using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target; // プレイヤーのTransform
    public float distance = 3.0f; // プレイヤーとの距離
    public float minDistance = 2.0f;
    public float maxDistance = 6.0f;

    public float mouseSensitivity = 100.0f;
    public float zoomSpeed = 2.0f;

    private float yaw = 0.0f; // 横の角度
    private float pitch = 30.0f;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("プレイヤーのTransformを設定してください！");
        }

        Cursor.lockState = CursorLockMode.Locked; // カーソルを固定
    }

    void Update()
    {
        RotateCamera();
        ZoomCamera();
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        yaw += mouseX;

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0); // 縦方向は動かさない
        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        // カメラの高さをプレイヤーの高さに固定
        Vector3 targetPosition = target.position;
        targetPosition.y += 1.5f; // 高さを適宜調整（プレイヤーの頭上あたり）

        transform.position = targetPosition + offset;
        transform.LookAt(targetPosition);
    }

    void ZoomCamera()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }
}
