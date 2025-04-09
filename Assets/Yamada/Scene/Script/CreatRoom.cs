using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class CreatRoom : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ホスト開始
    public void StartHost()
    {
      //接続承認コールバック
      NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
      //ホスト開始
      NetworkManager.Singleton.StartHost();
      //シーン切り替え
      NetworkManager.Singleton.SceneManager.LoadScene("PlayScene", LoadSceneMode.Single);
    }

    //クライアントがホストに接続
    public void StartClient()
    {
       //ホストに接続
       bool result = NetworkManager.Singleton.StartClient();
    }

    //接続承認関数
    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
      //追加の承認手順が必要な場合は、追加の手順が完了するまでこれをtrueに設定する
      //trueからfalseに遷移すると、接続応答が処理される
      response.Pending = true;

      //最大人数ンをチェックする
      if(NetworkManager.Singleton.ConnectedClients.Count >= 2)
      {
         response.Approved = false; //接続を許可しない
         response.Pending = false;
         return;
      }
      //ここからは接続成功クライアントに向けた処理
      response.Approved = true;     //接続許可

      //PlayerObjectを生成するかどうか
      response.CreatePlayerObject = true; 

      //生成するPrefabハッシュ値。nullの場合NetworkManagerに登録したプレハブが使用される
      response.PlayerPrefabHash = null;

      //PlayerObjectをスポーンする位置(nullの場合Vector3.zero)
      var position = new Vector3(0, 1, -8);
      position.x = -5 + 5 * (NetworkManager.Singleton.ConnectedClients.Count % 3);
      response.Position = position;

      //PlayerObjectをスポーン時の回転 (nullの場合Quaternion.identity)
      response.Rotation = Quaternion.identity;

      response.Pending = false;
   }
}
