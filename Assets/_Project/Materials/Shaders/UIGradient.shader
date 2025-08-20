Shader "UI/Gradient"
{
    Properties
    {
        _ColorTop ("Top Color", Color) = (1,1,1,1)
        _ColorBottom ("Bottom Color", Color) = (0,0,0,1)
        _Alpha ("Alpha", Range(0,1)) = 1
        _Direction ("Direction (0=Vertical,1=Horizontal)", Float) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _ColorTop;
            fixed4 _ColorBottom;
            float _Alpha;
            float _Direction;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = (_Direction < 0.5) ? i.uv.y : i.uv.x;
                fixed4 col = lerp(_ColorBottom, _ColorTop, t);
                col.a *= _Alpha;
                return col;
            }
            ENDCG
        }
    }
}
