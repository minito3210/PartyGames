using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public GameObject bombPrefab;
    public Transform throwPoint;
    public Transform cameraTransform; // �J������ Transform ���擾

    private Rigidbody rb;
    private int maxBombs = 1;
    private int currentBombs = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        if (Input.GetButtonDown("Fire1"))
        {
            ThrowBomb();
        }
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal"); // A/D �܂��� ��/��
        float v = Input.GetAxis("Vertical");   // W/S �܂��� ��/��

        // �J������Y���̉�]�������l���iX���̌X���Ȃǂ͖����j
        Vector3 forward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        Vector3 right = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z).normalized;

        // �J�����̌�������Ɉړ�����������
        Vector3 moveDirection = forward * v + right * h;

        if (moveDirection.magnitude > 0.1f)
        {
            // �v���C���[�̈ړ�
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);

            // �v���C���[�̌������J�����̐��������ɍ��킹��
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(targetRotation);
        }
    }

    void ThrowBomb()
    {
        if (currentBombs < maxBombs)
        {
            GameObject bomb = Instantiate(bombPrefab, throwPoint.position, throwPoint.rotation);
            Rigidbody bombRb = bomb.GetComponent<Rigidbody>();
            bombRb.AddForce(transform.forward * 10.0f, ForceMode.Impulse);

            currentBombs++;
            StartCoroutine(BombCooldown());
        }
    }

    IEnumerator BombCooldown()
    {
        yield return new WaitForSeconds(3f);
        currentBombs--;
    }

    public void IncreaseBombCapacity()
    {
        maxBombs++;
        Debug.Log("�{���̍ő�ݒu��������: " + maxBombs);
    }

    public IEnumerator SpeedBoost(float duration)
    {
        moveSpeed *= 1.5f;
        Debug.Log("�X�s�[�h�A�b�v�I");
        yield return new WaitForSeconds(duration);
        moveSpeed /= 1.5f;
        Debug.Log("�X�s�[�h�����ɖ߂���");
    }
}
