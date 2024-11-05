using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.PostProcessing.PostProcessResources;

public class MapRendererFeature : ScriptableRendererFeature
{
    public class MapRenderePass : ScriptableRenderPass
    {
        public enum Passes
        {
            Contrast,
            HSV,
            GrayToon
        }

        Material m_material;
        public MapRenderePass(Shader shader)
        {
            //シェーダーからマテリアルの作成
            m_material = CoreUtils.CreateEngineMaterial(shader);
        }

        /// <summary>
        /// レンダラーパス順のセット
        /// </summary>
        /// <param name="_renderPass"></param>
        public void Setup(RenderPassEvent _renderPass)
        {
            renderPassEvent = _renderPass;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (m_material == null) return;

            //Volumeを取得
            var volumeStack = VolumeManager.instance.stack;
            var volum = volumeStack.GetComponent<MapPostProcessVolume>();

            //コマンドリスト取得(コマンドリストをレンタルする)
            var cmd = CommandBufferPool.Get("Map Post Prosess");
            //中身一応クリア
            cmd.Clear();

            //現在のRTの情報を取得
            var tempTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            tempTargetDescriptor.depthBufferBits = 0;

            //ソース画像
            int tempRTID = Shader.PropertyToID("_TempRT");
            cmd.GetTemporaryRT(tempRTID, tempTargetDescriptor);

            int contrastRTID = Shader.PropertyToID("_ContrastRT");
            cmd.GetTemporaryRT(contrastRTID, tempTargetDescriptor);

            int toonRTID = Shader.PropertyToID("_ToonRT");
            cmd.GetTemporaryRT(toonRTID, tempTargetDescriptor);



            //============================================================================
            //①現在の画面をRTへコピー
            //===========================================================================

            //RTにカメラ画像をコピー
            cmd.Blit
                (
                 renderingData.cameraData.renderer.cameraColorTargetHandle,
                 tempRTID);

            //======================================================================
            //②加工画像作成
            //======================================================================  

            m_material.SetFloat("_ContrastPower", volum.ContrastPower.value);
            //描画(コントラスト)
            cmd.Blit
                (
                tempRTID,//描画元画像
                contrastRTID,//描画先画像
                m_material, ((int)Passes.Contrast)
                );

            // 描画（HSV）
            m_material.SetFloat("_Hue", volum.HueShift.value);
            m_material.SetFloat("_Saturation", volum.Saturation.value);
            m_material.SetFloat("_Value", volum.Value.value);
            cmd.Blit
                (
                contrastRTID,//描画元画像
                tempRTID,//描画先画像
                m_material,((int)Passes.HSV)
                );

            // トゥーンやるかどうか
            if(volum.ToonAble.value)
            {
                //描画（GrayToon） 画面へ
                m_material.SetColor("_WhiteColor", Color.white);
                m_material.SetColor("_GrayColor", Color.red);
                m_material.SetColor("_DarkColor", Color.black);
                cmd.Blit
                    (
                    tempRTID,//描画元画像
                    toonRTID,//描画先画像
                    m_material, ((int)Passes.GrayToon)
                    );
               cmd.Blit
                    (
                    toonRTID,//描画元画像
                    tempRTID//描画先画像
                    );
            }

            //　画面
            cmd.Blit
                (
                tempRTID,//描画元画像
                renderingData.cameraData.renderer.cameraColorTargetHandle//描画先画像
                );


            //======================================================================
            //後片付け
            //======================================================================

            //RT解放
            cmd.ReleaseTemporaryRT(tempRTID);
            cmd.ReleaseTemporaryRT(contrastRTID);

            //コマンドバッファをGPUに転送
            context.ExecuteCommandBuffer(cmd);
            //返却
            CommandBufferPool.Release(cmd);

        }
    }



    [SerializeField] Shader _shader;
    [SerializeField] RenderPassEvent _passEvent = RenderPassEvent.AfterRenderingPostProcessing;
    MapRenderePass _pass;

    public override void Create()
    {
        //作成は必ずCreateに作る
        _pass = new MapRenderePass(_shader);
    }

    /// <summary>
    /// パスを追加される必要がある時に実行される
    /// </summary>
    /// <param name="renderer"></param>
    /// <param name="renderingData"></param>
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        //描画として動かせるようになった
        renderer.EnqueuePass(_pass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        _pass.Setup(_passEvent);
    }

}
