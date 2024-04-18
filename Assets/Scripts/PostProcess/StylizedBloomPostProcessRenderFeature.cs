using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class StylizedBloomPostProcessRenderFeature : ScriptableRendererFeature
{
    [SerializeField] private Shader m_bloomShader;
    [SerializeField] private Shader m_compositeShader;

    private Material m_bloomMaterial;
    private Material m_compositeMaterial;
        
    private StylizedBloomPostProcessPass m_stylizedBloomPass;
    
    public override void Create()
    {
        m_bloomMaterial = CoreUtils.CreateEngineMaterial(m_bloomShader);
        m_compositeMaterial = CoreUtils.CreateEngineMaterial(m_compositeShader);
        
        m_stylizedBloomPass = new StylizedBloomPostProcessPass(m_bloomMaterial, m_compositeMaterial);
    }
    
    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(m_bloomMaterial);
        CoreUtils.Destroy(m_compositeMaterial);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    { 
        renderer.EnqueuePass(m_stylizedBloomPass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            m_stylizedBloomPass.ConfigureInput(ScriptableRenderPassInput.Depth | ScriptableRenderPassInput.Color);
            m_stylizedBloomPass.SetTarget(renderer.cameraColorTargetHandle, renderer.cameraDepthTargetHandle);
        }
    }
}
