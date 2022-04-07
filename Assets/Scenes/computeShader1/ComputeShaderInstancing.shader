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

                float cx = cos(angles.x * Deg2Rad); float sx = sin(angles.x * Deg2Rad);
                float cy = cos(angles.z * Deg2Rad); float sy = sin(angles.z * Deg2Rad);
                float cz = cos(angles.y * Deg2Rad); float sz = sin(angles.y * Deg2Rad);

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
                float s = sin(angle.x);
                float c = cos(angle.x);
                float2x2 rotX = float2x2(c, -s, s, c);

                float size = 10.0;
                float centX = v.vertex.x + size * ( p.position.x - 0.5);
                float centZ = v.vertex.z + size * ( p.position.y - 0.5);
                float3 center = float3(centX, 0., centX);

                v.vertex *= float4(p.scale,1);
                //v.vertex.xyz -= center;
                v.vertex = mul(eulerAnglesToRottationMatrix(angle),  v.vertex);
                //v.vertex.xyz += center;

                o.vertex = UnityObjectToClipPos(v.vertex + p.position );
                //o.vertex = UnityObjectToClipPos(v.vertex + float3(mul(rotX, p.position.xz), p.position.y).xzy );
                //o.vertex = UnityObjectToClipPos(v.vertex + float3(mul(eulerAnglesToRottationMatrix(angle), p.position).xyz ));
                
                
                
                
                //o.vertex = float4(mul(rotX, o.vertex.xz), o.vertex.yw).xzyw;
                //o.vertex = float4(mul(rotX, o.vertex.yz), o.vertex.xw).xyzw;
               //o.vertex.xz = mul(rotX, o.vertex.xz);
                
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