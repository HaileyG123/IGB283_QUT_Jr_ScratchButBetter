Shader "blur/blur"
{
   Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NumBlurLayers ("Number of Blur Layers", Float) = 5.0
        _Sigma ("Sigma", Float) = 3.0
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float _NumBlurLayers;
            float _Sigma;
            sampler2D _MainTex;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 sum = fixed4(0.0, 0.0, 0.0, 0.0);
                float blurSize = 1.0 / _ScreenParams.z;

                float size = 2.0 * ceil(_Sigma * 3.0) + 1.0;

                for (float layer = 0.0; layer < _NumBlurLayers; layer++)
                {
                    float offset = layer * blurSize;
                    float weight = 1.0 / (size * size);

                    for (float x = -size; x <= size; x++)
                    {
                        for (float y = -size; y <= size; y++)
                        {
                            float2 offsetCoord = i.texcoord + float2(x * offset / _ScreenParams.x, y * offset / _ScreenParams.y);
                            sum += tex2D(_MainTex, offsetCoord) * weight;
                        }
                    }
                }

                return sum;
            }
            ENDCG
        }
    }
}