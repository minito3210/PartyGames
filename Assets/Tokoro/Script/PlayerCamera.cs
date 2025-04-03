using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target; // �v���C���[��Transform
    public float distance = 3.0f; // �v���C���[�Ƃ̋���
    public float minDistance = 2.0f;
    public float maxDistance = 6.0f;

    public float mouseSensitivity = 100.0f;
    public float zoomSpeed = 2.0f;

    private float yaw = 0.0f; // ���̊p�x
    private float pitch = 30.0f;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("�v���C���[��Transform��ݒ肵�Ă��������I");
        }

        Cursor.lockState = CursorLockMode.Locked; // �J�[�\�����Œ�
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

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0); // �c�����͓������Ȃ�
        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        // �J�����̍������v���C���[�̍����ɌŒ�
        Vector3 targetPosition = target.position;
        targetPosition.y += 1.5f; // ������K�X�����i�v���C���[�̓��゠����j

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
