using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class DiffusionRendererFeature : ScriptableRendererFeature
{

    public class DiffusionRenderePass : ScriptableRenderPass
    {
        Material _material;
        Material _material2;


        public enum Passes
        {
            Blur,
            BlendScreen,
            ComparisonBright,
            SelfMultiply,
        }


        int _sampleCount;
        float _strength;

        public DiffusionRenderePass(Shader shader)
        {
            //シェーダーからマテリアルの作成
            _material = CoreUtils.CreateEngineMaterial(shader);
            _material2 = CoreUtils.CreateEngineMaterial(shader);
        }

        public void Setup(RenderPassEvent _renderPass)
        {
            renderPassEvent = _renderPass;
        }

        /// <summary>
        /// 描画実行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="renderingData"></param>
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (_material == null) return;

            //Volumeを取得
            var volumeStack = VolumeManager.instance.stack;
            var volum = volumeStack.GetComponent<DiffusionPostProcessVolume>();

            //if (volum.Strength.value <= 0.0f) return;

            //コマンドリスト取得(コマンドリストをレンタルする)
            var cmd = CommandBufferPool.Get("Diffusion Post Prosess");
            //中身一応クリア
            cmd.Clear();

            //----------------------------------------------------------------------
            //レンダーターゲット取得
            //---------------------------------------------------------------------

            //現在のRTの情報を取得
            var tempTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            tempTargetDescriptor.depthBufferBits = 0;

            //ソース画像
            int tempRTID = Shader.PropertyToID("_TempRT");
            cmd.GetTemporaryRT(tempRTID, tempTargetDescriptor);

            //乗算用画像
            int tempMultiplyRTID = Shader.PropertyToID("_TempMultiplyRT");
            cmd.GetTemporaryRT(tempMultiplyRTID, tempTargetDescriptor);

            //比較(明)した後の結果
            int tempComparisonRTID = Shader.PropertyToID("_TempComporisonRT");
            cmd.GetTemporaryRT(tempComparisonRTID, tempTargetDescriptor);

            //ブラー用画像
            int fullW = renderingData.cameraData.camera.scaledPixelWidth;//RTの幅
            int fullH = renderingData.cameraData.camera.scaledPixelHeight;//RTの高さ
            int tempBlurXRTID = Shader.PropertyToID("_TempBlurXRT");
            int tempBlurYRTID = Shader.PropertyToID("_TempBlurYRT");

            cmd.GetTemporaryRT(tempBlurXRTID, fullW / 2, fullH / 2, 0,
                FilterMode.Bilinear, RenderTextureFormat.ARGB32);
            cmd.GetTemporaryRT(tempBlurYRTID, fullW / 2, fullH / 2, 0,
                FilterMode.Bilinear, RenderTextureFormat.ARGB32);


            //============================================================================
            //①現在の画面をRTへコピー
            //===========================================================================


            //RTにカメラ画像をコピー
            cmd.Blit
                (
                 renderingData.cameraData.renderer.cameraColorTargetHandle,
                 tempRTID);


            //======================================================================
            //②乗算画像作成
            //======================================================================  

            //_material.SetInt("_SampleCount", volum.SampleCount.value);
            //_material.SetFloat("_Strength", volum.Strength.value);


            //描画(乗算)
            cmd.Blit
                (
                tempRTID,//描画元画像
                tempMultiplyRTID,//描画先画像
                                 //renderingData.cameraData.renderer.cameraColorTargetHandle,//描画先画像
                _material, (int)Passes.SelfMultiply                                          //描画するシェーダー(番号)
                );


            //=====================================================================
            //③乗算画像にブラーをかける
            //====================================================================
            //縮小バッファへコピー
            cmd.Blit(tempMultiplyRTID, tempBlurYRTID);

            //Xブラー
            _material.SetFloat("_Dispersion", volum.GaussDispersion.value);
            _material.SetInt("_SmaplingTexelAmount", volum.GaussSmaplingTexelAmount.value);//奇数にする
            _material.SetVector("_Direction", new Vector4(1.0f / (fullW / 2), 0, 0, 0));
            cmd.Blit(tempBlurYRTID, tempBlurXRTID, _material, (int)Passes.Blur);

            //コマンドバッファをGPUに転送
            context.ExecuteCommandBuffer(cmd);

            //Yブラー
            _material2.SetFloat("_Dispersion", volum.GaussDispersion.value);
            _material2.SetInt("_SmaplingTexelAmount", volum.GaussSmaplingTexelAmount.value);//奇数にする
            _material2.SetVector("_Direction", new Vector4(0.0f, 1.0f / (fullH / 2), 0, 0));
            cmd.Blit(tempBlurXRTID, tempBlurYRTID, _material2, (int)Passes.Blur);

            ////テスト
            //cmd.Blit(tempBlurYRTID,
            //    renderingData.cameraData.renderer.cameraColorTargetHandle
            //    );


            //=================================================================
            //④比較明
            //=================================================================
            //テクスチャをセット
            cmd.SetGlobalTexture("_BackTex", tempBlurYRTID);
            cmd.Blit(tempRTID, tempComparisonRTID, _material, (int)Passes.ComparisonBright);

            //テスト
            //cmd.Blit(tempComparisonRTID,
            //    renderingData.cameraData.renderer.cameraColorTargetHandle
            //    );

            //=================================================================
            //⑤スクリーン合成
            //================================================================
            _material.SetFloat("_Blend", volum.ScreenBlend.value);

            //テクスチャをセット
            cmd.SetGlobalTexture("_BackTex", tempComparisonRTID);
            cmd.Blit(tempMultiplyRTID,
                renderingData.cameraData.renderer.cameraColorTargetHandle
                , _material, (int)Passes.ComparisonBright);

            //-------------------------------------------------------------------



            //RT解放
            cmd.ReleaseTemporaryRT(tempRTID);
            cmd.ReleaseTemporaryRT(tempMultiplyRTID);
            cmd.ReleaseTemporaryRT(tempComparisonRTID);
            cmd.ReleaseTemporaryRT(tempBlurXRTID);
            cmd.ReleaseTemporaryRT(tempBlurYRTID);


            //コマンドバッファをGPUに転送
            context.ExecuteCommandBuffer(cmd);
            //返却
            CommandBufferPool.Release(cmd);

        }
    }


    [SerializeField] Shader _shader;
    [SerializeField] RenderPassEvent _passEvent = RenderPassEvent.AfterRenderingPostProcessing;

    DiffusionRenderePass _pass;

    public override void Create()
    {
        //作成は必ずCreateに作る
        _pass = new DiffusionRenderePass(_shader);
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
