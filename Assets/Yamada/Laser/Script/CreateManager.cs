using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SpawnTimeRange
{
   [Header("�ŏ������Ԋu"), SerializeField]
   public float minTime;
   [Header("�ő吶���Ԋu"), SerializeField]
   public float maxTime;
   [Header("�K�p����v���C���Ԃ�臒l"), SerializeField]
   public float threshold;
}

public class CreateManager : MonoBehaviour
{
   [Header("�Q�[���}�l�[�W���["), SerializeField]
   private GameObject m_gameManager;
   [Header("�������郌�[�U�["), SerializeField]
   private GameObject m_laser;
   [Header("���[�U�[�̍ŏ����x"), SerializeField]
   private float laserMinSpeed = 1f;
   [Header("���[�U�[�̍ő呬�x"), SerializeField]
   private float laserMaxSpeed = 3f;

   private float m_minSpawnTime = 1.0f; // �ŒZ1�b
   private float m_maxSpawnTime = 3.0f; // �Œ�3�b
   [Header("���Ԃ��Ƃ̐����Ԋu���X�g"), SerializeField]
   private List<SpawnTimeRange> m_spawnTimeRanges = new List<SpawnTimeRange>();

   [Header("�������W"), SerializeField]
   private Transform m_spawnPoint;

   private int m_currentRangeIndex = -1; // ���ݓK�p���͈̔͂̃C���f�b�N�X
   // Start is called before the first frame update
   void Start()
   {
      if (m_spawnTimeRanges.Count > 0)
      {
         // �����l�Ƃ��čŏ��͈̔͂�K�p
         m_minSpawnTime = m_spawnTimeRanges[0].minTime;
         m_maxSpawnTime = m_spawnTimeRanges[0].maxTime;
      }

      // �e�I�u�W�F�N�g���قȂ�^�C�~���O�ŃR���[�`�����J�n����悤�Ƀ����_���Ȓx����ݒ�
      float randomStartDelay = Random.Range(0f, 3f); // 0����1�b�̊ԂŃ����_���ɒx��
      StartCoroutine(SpawnLaserLoop(randomStartDelay)); // ��莞�Ԃ��ƂɃ��[�U�[�𐶐�
   }

   private IEnumerator SpawnLaserLoop(float delay)
   {
      yield return new WaitForSeconds(delay); // �����_���Ȓx����҂�

      while (true) // �������[�v�Ń��[�U�[���쐬
      {
         UpdateSpawnInterval(); // ���Ԃɉ����Đ����Ԋu���X�V
         CreateLaser();
         float waitTime = Random.Range(m_minSpawnTime, m_maxSpawnTime); // �����_���ȑҋ@����
         yield return new WaitForSeconds(waitTime);
      }
   }

   private void CreateLaser()
   {
      if (m_laser != null && m_spawnPoint != null)
      {
         GameObject laserInstance = Instantiate(m_laser, m_spawnPoint.position, m_spawnPoint.rotation);

         // LaserMove �X�N���v�g���A�^�b�`����Ă��邩�m�F
         LaserMove laserMove = laserInstance.GetComponent<LaserMove>();

         if (laserMove == null)
         {
            laserMove = laserInstance.AddComponent<LaserMove>();
         }

         // �������̉�]������n��
         laserMove.SetDirection(m_spawnPoint.forward);
         // �����_���ȑ��x��ݒ�
         float randomSpeed = Random.Range(laserMinSpeed, laserMaxSpeed);
         laserMove.SetSpeed(randomSpeed);
      }
   }

   private void UpdateSpawnInterval()
   {
      if (m_gameManager == null) return;

      float playTime = m_gameManager.GetComponent<LaserGameManager>().m_timer; // �v���C���Ԃ��擾

      // ���݂̃v���C���ԂɓK�p����͈͂�������
      for (int i = 0; i < m_spawnTimeRanges.Count; i++)
      {
         if (playTime >= m_spawnTimeRanges[i].threshold)
         {
            if (m_currentRangeIndex != i) // ���łɐݒ肳��Ă���ꍇ�͕ύX���Ȃ�
            {
               m_minSpawnTime = m_spawnTimeRanges[i].minTime;
               m_maxSpawnTime = m_spawnTimeRanges[i].maxTime;
               m_currentRangeIndex = i; // �X�V
               Debug.Log($"�v���C����: {playTime}�b �� �����Ԋu�ύX: {m_minSpawnTime}�b - {m_maxSpawnTime}�b");
            }
         }
         else
         {
            break; // 臒l�𒴂��Ă��Ȃ��ꍇ�̓��[�v���I��
         }
      }
   }
}
