Shader "Custom/PixelOutline"
{
    Properties
    {
        _MainTex("Sprite", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineSize("Outline Size", Float) = 1
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
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
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_TexelSize;
                fixed4 _OutlineColor;
                float _OutlineSize;

                v2f vert(appdata_t IN)
                {
                    v2f OUT;
                    OUT.vertex = UnityObjectToClipPos(IN.vertex);
                    OUT.uv = IN.uv;
                    return OUT;
                }

                fixed4 frag(v2f IN) : SV_Target
                {
                    float2 offset = _MainTex_TexelSize.xy * _OutlineSize;
                    fixed4 original = tex2D(_MainTex, IN.uv);

                    float outlineAlpha = (
                        tex2D(_MainTex, IN.uv + float2(offset.x, 0)).a +
                        tex2D(_MainTex, IN.uv + float2(-offset.x, 0)).a +
                        tex2D(_MainTex, IN.uv + float2(0, offset.y)).a +
                        tex2D(_MainTex, IN.uv + float2(0, -offset.y)).a
                    ) > 0 ? 1 : 0; 

                    if (original.a > 0)
                        return original; 

                    return fixed4(_OutlineColor.rgb, outlineAlpha); 
                }
                ENDCG
            }
        }
}
