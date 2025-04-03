using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float mouseSensitivity = 100f;  // マウス感度
    public Transform playerBody;           // プレイヤーの本体

    private float xRotation = 0f;          // 上下回転の角度

    void Start()
    {
        // カーソルをロック（ゲーム画面内で固定）
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // マウスの入力取得
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 上下の回転（Pitch）
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 上下の制限

        // 左右の回転（Yaw）
        playerBody.Rotate(Vector3.up * mouseX);

        // カメラの回転を適用
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
