Shader "UI/TwistEffectWithStencil_Transparent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TwistStrength ("Twist Strength", Float) = 5.0
        _Center ("Center", Vector) = (0.5, 0.5, 0, 0)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Name "FORWARD"
            Tags { "LightMode"="Always" }

            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            Lighting Off

            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _TwistStrength;
            float2 _Center;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                float2 delta = uv - _Center;
                float angle = length(delta) * _TwistStrength;
                float s = sin(angle);
                float c = cos(angle);

                float2 twistedUV;
                twistedUV.x = c * delta.x - s * delta.y;
                twistedUV.y = s * delta.x + c * delta.y;
                twistedUV = _Center + twistedUV;

                fixed4 col = tex2D(_MainTex, twistedUV);

                // Отбрасываем полностью прозрачные пиксели, чтобы stencil не писался в пустоте
                clip(col.a - 0.01);

                // Возвращаем цвет с альфа (чтобы не белело)
                return col;
            }
            ENDCG
        }
    }
}
