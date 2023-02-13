// %BANNER_BEGIN%
// ---------------------------------------------------------------------
// %COPYRIGHT_BEGIN%
// Copyright (c) 2022 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Software License Agreement, located here: https://www.magicleap.com/software-license-agreement-ml2
// Terms and conditions applicable to third-party materials accompanying this distribution may also be found in the top-level NOTICE file appearing herein.
// %COPYRIGHT_END%
// ---------------------------------------------------------------------
// %BANNER_END%

Shader "Magic Leap/Depth Camera Sample"
{
    Properties
    {
        [HideInInspector] _MainTex("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent" "RenderQueue" = "Transparent"
        }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZTest Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _MinDepth;
            float _MaxDepth;

            float4 HueShift(in float3 color, in float shift)
            {
                const float m = 0.55735;
                const float3 midPoint = float3(m, m, m);
                float3 P = midPoint * dot(midPoint, color);

                float3 U = color - P;

                float3 V = cross(midPoint, U);

                color = U * cos(shift * 6.2832) + V * sin(shift * 6.2832) + P;

                return float4(color, 1.0);
            }

            fixed3 GetColorVisualization(float x)
            {
                return HueShift(float3(1.0, .0, .0), x).rgb;
            }

            float3 GetConfidenceColor(float conf)
            {
                return float3(conf, 1.0 - conf, 0.0);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float depthValue = i.color.r;
                fixed4 depth_color = fixed4(GetColorVisualization(depthValue), 1.0);

                // Values outside of range mapped to black.
                if (depthValue < 0.001f || depthValue > 0.999)
                {
                    depth_color.rgb *= 0.0;
                }

                return fixed4(depth_color.rgb, tex2D(_MainTex, i.uv).a);
            }
            ENDCG
        }
    }
}