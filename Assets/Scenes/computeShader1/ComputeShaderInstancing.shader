Shader "Sample/ComputeShaderInstancing"
{
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }
        Pass
        {
            CGPROGRAM

            #pragma vertex   vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color  : TEXCOORD1;
            };

            struct particle
            {
                float3 position;
                float3 velocity;
                float4 color;
                float3 dir;
                float3 scale;
            };

            float4x4 eulerAnglesToRottationMatrix(float3 angles) {
                float Deg2Rad = 0.0174532924;
                angles *= Deg2Rad;

                float cx = cos(angles.x); float sx = sin(angles.x);
                float cy = cos(angles.z); float sy = sin(angles.z);
                float cz = cos(angles.y); float sz = sin(angles.y);

                return float4x4(
                    cz*cy + sz*sx*sy, -cz*sy + sz*sx*cy, sz*cx, 0,
                    sy*cx, cy*cx, -sx, 0,
                    -sz*cy + cz*sx*sy, sy*sz + cz*sx*cy, cz*cx, 0,
                    0, 0, 0, 1);

            }

            StructuredBuffer<particle> _ParticleBuffer;

            v2f vert (appdata v, uint instanceID : SV_InstanceID)
            {
                particle p = _ParticleBuffer[instanceID];

                v2f o;
                float3 angle = float3(p.dir);

                v.vertex *= float4(p.scale,1);
                v.vertex = mul(eulerAnglesToRottationMatrix(angle),  v.vertex);

                o.vertex = UnityObjectToClipPos(v.vertex + p.position );
                o.color  = p.color;
                

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return i.color;
            }

            ENDCG
        }
    }
}