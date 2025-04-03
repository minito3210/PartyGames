using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float mouseSensitivity = 100f;  // �}�E�X���x
    public Transform playerBody;           // �v���C���[�̖{��

    private float xRotation = 0f;          // �㉺��]�̊p�x

    void Start()
    {
        // �J�[�\�������b�N�i�Q�[����ʓ��ŌŒ�j
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // �}�E�X�̓��͎擾
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // �㉺�̉�]�iPitch�j
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // �㉺�̐���

        // ���E�̉�]�iYaw�j
        playerBody.Rotate(Vector3.up * mouseX);

        // �J�����̉�]��K�p
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
