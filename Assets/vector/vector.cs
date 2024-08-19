using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CustomMotionVectorVisualizationFeature : ScriptableRendererFeature
{
    class CustomMotionVectorVisualizationPass : ScriptableRenderPass
    {
        private Material material;
        private RenderTargetHandle motionVectorTexture;

        public CustomMotionVectorVisualizationPass(Material material)
        {
            this.material = material;
            motionVectorTexture.Init("_MotionVectorTexture");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Motion Vector Visualization");

            RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
            descriptor.depthBufferBits = 0;

            cmd.GetTemporaryRT(motionVectorTexture.id, descriptor, FilterMode.Bilinear);

            // Copy the motion vector texture
            Blit(cmd, renderingData.cameraData.renderer.cameraColorTarget, motionVectorTexture.Identifier());

            // Set the motion vector texture as a global texture
            cmd.SetGlobalTexture("_MotionVectors", motionVectorTexture.Identifier());

            // Blit the motion vectors to the camera target
            Blit(cmd, motionVectorTexture.Identifier(), renderingData.cameraData.renderer.cameraColorTarget, material);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(motionVectorTexture.id);
        }
    }

    [System.Serializable]
    public class CustomMotionVectorVisualizationSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        public Material visualizationMaterial = null;
    }

    public CustomMotionVectorVisualizationSettings settings = new CustomMotionVectorVisualizationSettings();
    private CustomMotionVectorVisualizationPass pass;

    public override void Create()
    {
        pass = new CustomMotionVectorVisualizationPass(settings.visualizationMaterial)
        {
            renderPassEvent = settings.renderPassEvent
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.visualizationMaterial != null)
        {
            renderer.EnqueuePass(pass);
        }
    }
}
