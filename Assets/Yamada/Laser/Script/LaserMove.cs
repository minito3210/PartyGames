using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMove : MonoBehaviour
{

   [Header("�����i�b�j"), SerializeField]
   private float lifetime = 3f; // ���[�U�[�̏��ł܂ł̎���

   private Vector3 moveDirection = Vector3.forward; // �f�t�H���g�͑O����
   private float m_speed;

   // ���������\�b�h�i�O�����������ݒ肷��j
   public void SetDirection(Vector3 direction)
   {
      moveDirection = direction.normalized;
   }
   public void SetSpeed(float speed)
   {
      m_speed = speed;
   }

   // Start is called before the first frame update
   void Start()
   {
      // ��莞�Ԍ�ɍ폜
      Destroy(gameObject, lifetime);
   }

   // Update is called once per frame
   void Update()
    {
      Move();
    }

   private void Move()
   {
      moveDirection.Normalize();
      transform.position += moveDirection * m_speed * Time.deltaTime;
   }
}
