using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SpawnTimeRange
{
   [Header("最小生成間隔"), SerializeField]
   public float minTime;
   [Header("最大生成間隔"), SerializeField]
   public float maxTime;
   [Header("適用するプレイ時間の閾値"), SerializeField]
   public float threshold;
}

public class CreateManager : MonoBehaviour
{
   [Header("ゲームマネージャー"), SerializeField]
   private GameObject m_gameManager;
   [Header("生成するレーザー"), SerializeField]
   private GameObject m_laser;
   [Header("レーザーの最小速度"), SerializeField]
   private float laserMinSpeed = 1f;
   [Header("レーザーの最大速度"), SerializeField]
   private float laserMaxSpeed = 3f;

   private float m_minSpawnTime = 1.0f; // 最短1秒
   private float m_maxSpawnTime = 3.0f; // 最長3秒
   [Header("時間ごとの生成間隔リスト"), SerializeField]
   private List<SpawnTimeRange> m_spawnTimeRanges = new List<SpawnTimeRange>();

   [Header("生成座標"), SerializeField]
   private Transform m_spawnPoint;

   private int m_currentRangeIndex = -1; // 現在適用中の範囲のインデックス
   // Start is called before the first frame update
   void Start()
   {
      if (m_spawnTimeRanges.Count > 0)
      {
         // 初期値として最初の範囲を適用
         m_minSpawnTime = m_spawnTimeRanges[0].minTime;
         m_maxSpawnTime = m_spawnTimeRanges[0].maxTime;
      }

      // 各オブジェクトが異なるタイミングでコルーチンを開始するようにランダムな遅延を設定
      float randomStartDelay = Random.Range(0f, 3f); // 0から1秒の間でランダムに遅延
      StartCoroutine(SpawnLaserLoop(randomStartDelay)); // 一定時間ごとにレーザーを生成
   }

   private IEnumerator SpawnLaserLoop(float delay)
   {
      yield return new WaitForSeconds(delay); // ランダムな遅延を待つ

      while (true) // 無限ループでレーザーを作成
      {
         UpdateSpawnInterval(); // 時間に応じて生成間隔を更新
         CreateLaser();
         float waitTime = Random.Range(m_minSpawnTime, m_maxSpawnTime); // ランダムな待機時間
         yield return new WaitForSeconds(waitTime);
      }
   }

   private void CreateLaser()
   {
      if (m_laser != null && m_spawnPoint != null)
      {
         GameObject laserInstance = Instantiate(m_laser, m_spawnPoint.position, m_spawnPoint.rotation);

         // LaserMove スクリプトがアタッチされているか確認
         LaserMove laserMove = laserInstance.GetComponent<LaserMove>();

         if (laserMove == null)
         {
            laserMove = laserInstance.AddComponent<LaserMove>();
         }

         // 生成時の回転方向を渡す
         laserMove.SetDirection(m_spawnPoint.forward);
         // ランダムな速度を設定
         float randomSpeed = Random.Range(laserMinSpeed, laserMaxSpeed);
         laserMove.SetSpeed(randomSpeed);
      }
   }

   private void UpdateSpawnInterval()
   {
      if (m_gameManager == null) return;

      float playTime = m_gameManager.GetComponent<LaserGameManager>().m_timer; // プレイ時間を取得

      // 現在のプレイ時間に適用する範囲を見つける
      for (int i = 0; i < m_spawnTimeRanges.Count; i++)
      {
         if (playTime >= m_spawnTimeRanges[i].threshold)
         {
            if (m_currentRangeIndex != i) // すでに設定されている場合は変更しない
            {
               m_minSpawnTime = m_spawnTimeRanges[i].minTime;
               m_maxSpawnTime = m_spawnTimeRanges[i].maxTime;
               m_currentRangeIndex = i; // 更新
               Debug.Log($"プレイ時間: {playTime}秒 → 生成間隔変更: {m_minSpawnTime}秒 - {m_maxSpawnTime}秒");
            }
         }
         else
         {
            break; // 閾値を超えていない場合はループを終了
         }
      }
   }
}
