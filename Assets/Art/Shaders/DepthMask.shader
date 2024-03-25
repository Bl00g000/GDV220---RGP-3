Shader "Custom/DepthMask"
{
    Properties
    {
        
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry-1" "RenderPipeline" = "UniversalPipeline"}

        Pass 
        {
            Blend Zero One
            ZWrite Off

        }
    }
}
