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
            //�V�F�[�_�[����}�e���A���̍쐬
            _material = CoreUtils.CreateEngineMaterial(shader);
            _material2 = CoreUtils.CreateEngineMaterial(shader);
        }

        public void Setup(RenderPassEvent _renderPass)
        {
            renderPassEvent = _renderPass;
        }

        /// <summary>
        /// �`����s
        /// </summary>
        /// <param name="context"></param>
        /// <param name="renderingData"></param>
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (_material == null) return;

            //Volume���擾
            var volumeStack = VolumeManager.instance.stack;
            var volum = volumeStack.GetComponent<DiffusionPostProcessVolume>();

            //if (volum.Strength.value <= 0.0f) return;

            //�R�}���h���X�g�擾(�R�}���h���X�g�������^������)
            var cmd = CommandBufferPool.Get("Diffusion Post Prosess");
            //���g�ꉞ�N���A
            cmd.Clear();

            //----------------------------------------------------------------------
            //�����_�[�^�[�Q�b�g�擾
            //---------------------------------------------------------------------

            //���݂�RT�̏����擾
            var tempTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            tempTargetDescriptor.depthBufferBits = 0;

            //�\�[�X�摜
            int tempRTID = Shader.PropertyToID("_TempRT");
            cmd.GetTemporaryRT(tempRTID, tempTargetDescriptor);

            //��Z�p�摜
            int tempMultiplyRTID = Shader.PropertyToID("_TempMultiplyRT");
            cmd.GetTemporaryRT(tempMultiplyRTID, tempTargetDescriptor);

            //��r(��)������̌���
            int tempComparisonRTID = Shader.PropertyToID("_TempComporisonRT");
            cmd.GetTemporaryRT(tempComparisonRTID, tempTargetDescriptor);

            //�u���[�p�摜
            int fullW = renderingData.cameraData.camera.scaledPixelWidth;//RT�̕�
            int fullH = renderingData.cameraData.camera.scaledPixelHeight;//RT�̍���
            int tempBlurXRTID = Shader.PropertyToID("_TempBlurXRT");
            int tempBlurYRTID = Shader.PropertyToID("_TempBlurYRT");

            cmd.GetTemporaryRT(tempBlurXRTID, fullW / 2, fullH / 2, 0,
                FilterMode.Bilinear, RenderTextureFormat.ARGB32);
            cmd.GetTemporaryRT(tempBlurYRTID, fullW / 2, fullH / 2, 0,
                FilterMode.Bilinear, RenderTextureFormat.ARGB32);


            //============================================================================
            //�@���݂̉�ʂ�RT�փR�s�[
            //===========================================================================


            //RT�ɃJ�����摜���R�s�[
            cmd.Blit
                (
                 renderingData.cameraData.renderer.cameraColorTargetHandle,
                 tempRTID);


            //======================================================================
            //�A��Z�摜�쐬
            //======================================================================  

            //_material.SetInt("_SampleCount", volum.SampleCount.value);
            //_material.SetFloat("_Strength", volum.Strength.value);


            //�`��(��Z)
            cmd.Blit
                (
                tempRTID,//�`�挳�摜
                tempMultiplyRTID,//�`���摜
                                 //renderingData.cameraData.renderer.cameraColorTargetHandle,//�`���摜
                _material, (int)Passes.SelfMultiply                                          //�`�悷��V�F�[�_�[(�ԍ�)
                );


            //=====================================================================
            //�B��Z�摜�Ƀu���[��������
            //====================================================================
            //�k���o�b�t�@�փR�s�[
            cmd.Blit(tempMultiplyRTID, tempBlurYRTID);

            //X�u���[
            _material.SetFloat("_Dispersion", volum.GaussDispersion.value);
            _material.SetInt("_SmaplingTexelAmount", volum.GaussSmaplingTexelAmount.value);//��ɂ���
            _material.SetVector("_Direction", new Vector4(1.0f / (fullW / 2), 0, 0, 0));
            cmd.Blit(tempBlurYRTID, tempBlurXRTID, _material, (int)Passes.Blur);

            //�R�}���h�o�b�t�@��GPU�ɓ]��
            context.ExecuteCommandBuffer(cmd);

            //Y�u���[
            _material2.SetFloat("_Dispersion", volum.GaussDispersion.value);
            _material2.SetInt("_SmaplingTexelAmount", volum.GaussSmaplingTexelAmount.value);//��ɂ���
            _material2.SetVector("_Direction", new Vector4(0.0f, 1.0f / (fullH / 2), 0, 0));
            cmd.Blit(tempBlurXRTID, tempBlurYRTID, _material2, (int)Passes.Blur);

            ////�e�X�g
            //cmd.Blit(tempBlurYRTID,
            //    renderingData.cameraData.renderer.cameraColorTargetHandle
            //    );


            //=================================================================
            //�C��r��
            //=================================================================
            //�e�N�X�`�����Z�b�g
            cmd.SetGlobalTexture("_BackTex", tempBlurYRTID);
            cmd.Blit(tempRTID, tempComparisonRTID, _material, (int)Passes.ComparisonBright);

            //�e�X�g
            //cmd.Blit(tempComparisonRTID,
            //    renderingData.cameraData.renderer.cameraColorTargetHandle
            //    );

            //=================================================================
            //�D�X�N���[������
            //================================================================
            _material.SetFloat("_Blend", volum.ScreenBlend.value);

            //�e�N�X�`�����Z�b�g
            cmd.SetGlobalTexture("_BackTex", tempComparisonRTID);
            cmd.Blit(tempMultiplyRTID,
                renderingData.cameraData.renderer.cameraColorTargetHandle
                , _material, (int)Passes.ComparisonBright);

            //-------------------------------------------------------------------



            //RT���
            cmd.ReleaseTemporaryRT(tempRTID);
            cmd.ReleaseTemporaryRT(tempMultiplyRTID);
            cmd.ReleaseTemporaryRT(tempComparisonRTID);
            cmd.ReleaseTemporaryRT(tempBlurXRTID);
            cmd.ReleaseTemporaryRT(tempBlurYRTID);


            //�R�}���h�o�b�t�@��GPU�ɓ]��
            context.ExecuteCommandBuffer(cmd);
            //�ԋp
            CommandBufferPool.Release(cmd);

        }
    }


    [SerializeField] Shader _shader;
    [SerializeField] RenderPassEvent _passEvent = RenderPassEvent.AfterRenderingPostProcessing;

    DiffusionRenderePass _pass;

    public override void Create()
    {
        //�쐬�͕K��Create�ɍ��
        _pass = new DiffusionRenderePass(_shader);
    }

    /// <summary>
    /// �p�X��ǉ������K�v�����鎞�Ɏ��s�����
    /// </summary>
    /// <param name="renderer"></param>
    /// <param name="renderingData"></param>
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        //�`��Ƃ��ē�������悤�ɂȂ���
        renderer.EnqueuePass(_pass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        _pass.Setup(_passEvent);
    }

}
