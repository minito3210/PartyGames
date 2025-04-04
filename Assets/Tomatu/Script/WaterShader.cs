using UnityEngine;

[ExecuteInEditMode]
public class WaterShader : MonoBehaviour
{
    public Material waterMaterial;
    public float waveSpeed = 1.0f;
    public float waveHeight = 0.1f;
    public float waveFrequency = 1.0f;
    public Texture2D normalMap;
    public Cubemap skybox;

    private void Update()
    {
        if (waterMaterial != null)
        {
            // 波の動きを制御する
            float time = Time.time * waveSpeed;
            waterMaterial.SetFloat("_WaveTime", time);
            waterMaterial.SetFloat("_WaveHeight", waveHeight);
            waterMaterial.SetFloat("_WaveFrequency", waveFrequency);

            // 水面の法線マップを設定する
            waterMaterial.SetTexture("_NormalMap", normalMap);
            // スカイボックスをマテリアルに設定
            waterMaterial.SetTexture("_Skybox", skybox);
        }
    }
}

