using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class rendererFeature : ScriptableRendererFeature
{
    class HatchingMotionBlurPass : ScriptableRenderPass
    {
        private Material material;
        private RenderTargetIdentifier source;
        private RenderTargetHandle temporaryTexture;
        private RenderTargetIdentifier motionVectors;

        public HatchingMotionBlurPass(Material material)
        {
            this.material = material;
            temporaryTexture.Init("_TemporaryTexture");
        }

        public void Setup(RenderTargetIdentifier source, RenderTargetIdentifier motionVectors)
        {
            this.source = source;
            this.motionVectors = motionVectors;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("Hatching Motion Blur");

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            cmd.GetTemporaryRT(temporaryTexture.id, opaqueDesc);

            cmd.SetGlobalTexture("_MotionVectorsTex", motionVectors);
            Blit(cmd, source, temporaryTexture.Identifier(), material);
            Blit(cmd, temporaryTexture.Identifier(), source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(temporaryTexture.id);
        }
    }

    [System.Serializable]
    public class HatchingMotionBlurSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        public Material postProcessMaterial = null;
    }

    public HatchingMotionBlurSettings settings = new HatchingMotionBlurSettings();
    private HatchingMotionBlurPass hatchingMotionBlurPass;

    public override void Create()
    {
        if (settings.postProcessMaterial != null)
        {
            hatchingMotionBlurPass = new HatchingMotionBlurPass(settings.postProcessMaterial)
            {
                renderPassEvent = settings.renderPassEvent
            };
        }
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (hatchingMotionBlurPass != null)
        {
            hatchingMotionBlurPass.Setup(renderer.cameraColorTarget, new RenderTargetIdentifier("_CameraMotionVectorsTexture"));
            renderer.EnqueuePass(hatchingMotionBlurPass);
        }
    }
}
