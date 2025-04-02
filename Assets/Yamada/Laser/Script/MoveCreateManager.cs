using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCreateManager : MonoBehaviour
{
   [Header("�X�e�[�W�I�u�W�F�N�g"), SerializeField]
   private GameObject m_stage;

   [Header("��]���x"), SerializeField]
   private float rotationSpeed = 30f; // ��]���x�i�x/�b�j

   // Start is called before the first frame update
   void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if (m_stage != null)
      {
         // ���S�I�u�W�F�N�g�̎������]
         transform.RotateAround(m_stage.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
      }
   }
}
