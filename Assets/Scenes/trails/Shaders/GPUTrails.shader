Shader "GPUTrails/GPUTrails" {

Properties {
	_Width("Width", Float) = 0.1
	_StartColor("StartColor", Color) = (1,1,1,1)
	_EndColor("EndColor", Color) = (0,0,0,1)
}
   
SubShader {
Pass{
	Cull Off Fog { Mode Off } ZWrite Off 
	Blend One One

	CGPROGRAM
	#pragma target 5.0
	#pragma vertex vert
	#pragma geometry geom
	#pragma fragment frag

	#include "UnityCG.cginc"
	#include "../cginc/GPUTrails.cginc"

	float _Width;
	float _Life;
	float4 _StartColor;
	float4 _EndColor;
	StructuredBuffer<Trail> _TrailBuffer;
	StructuredBuffer<Node> _NodeBuffer;

	Node GetNode(int trailIdx, int nodeIdx)
	{
		return _NodeBuffer[ToNodeBufIdx(trailIdx, nodeIdx)];
	}

	struct vs_out {
		float4 pos : POSITION0;
		float3 dir : TANGENT0;
		float4 col : COLOR0;
		float4 posNext: POSITION1;
		float3 dirNext : TANGENT1;
		float4 colNext : COLOR1;
	};

	struct gs_out {
		float4 pos : SV_POSITION;
		float4 col : COLOR;
	};

	vs_out vert (uint id : SV_VertexID, uint instanceId : SV_InstanceID)
	{
		vs_out Out;
		Trail trail = _TrailBuffer[instanceId];
		int currentNodeIdx = trail.currentNodeIdx;

		// 現在のnodeとその次のnodeの情報
		Node node0 = GetNode(instanceId, id-1);
		Node node1 = GetNode(instanceId, id); // current
		Node node2 = GetNode(instanceId, id+1);
		Node node3 = GetNode(instanceId, id+2);

		bool isLastNode = (currentNodeIdx == (int)id);

		// 末端nodeか未入力の場合
		if ( isLastNode || !IsValid(node1))
		{
			// node0~node3をコピーとして扱う
			node0 = node1 = node2 = node3 = GetNode(instanceId, currentNodeIdx);
		}

		// それぞれのnodeのpositionを取り出す
		float3 pos1 = node1.position;
		float3 pos0 = IsValid(node0) ? node0.position : pos1;
		float3 pos2 = IsValid(node2) ? node2.position : pos1;
		float3 pos3 = IsValid(node3) ? node3.position : pos2;

		// 出力として値を入れる
		Out.pos = float4(pos1, 1);
		Out.posNext = float4(pos2, 1);

		// pos1の接線
		Out.dir = normalize(pos2 - pos0);
		// pos2の接線
		Out.dirNext = normalize(pos3 - pos1);

		// 書き込み時間と現在の時間を比較
		float ageRate = saturate((_Time.y - node1.time) / _Life);
		float ageRateNext = saturate((_Time.y - node2.time) / _Life);
		// leapで線形補間して色を決める
		Out.col = lerp(_StartColor, _EndColor, ageRate);
		Out.colNext = lerp(_StartColor, _EndColor, ageRateNext);

		return Out;
	}

	[maxvertexcount(4)]
	void geom (point vs_out input[1], inout TriangleStream<gs_out> outStream) // TriangleStream ストリーム出力オブジェクト
	{
		gs_out output0, output1, output2, output3;
		float3 pos = input[0].pos; 
		float3 dir = input[0].dir;
		float3 posNext = input[0].posNext; 
		float3 dirNext = input[0].dirNext;

		// ワールド座標系のカメラの位置
		float3 camPos = _WorldSpaceCameraPos;
		// カメラへの方向ベクトル
		float3 toCamDir = normalize(camPos - pos);
		// 接線との外積
		float3 sideDir = normalize(cross(toCamDir, dir));

		// next要素でも同様にもとめる
		float3 toCamDirNext = normalize(camPos - posNext);
		float3 sideDirNext = normalize(cross(toCamDirNext, dirNext));
		float width = _Width * 0.5;

		// 正負でsideDir方向に0.5width伸ばす
		output0.pos = UnityWorldToClipPos(pos + (sideDir * width));
		output1.pos = UnityWorldToClipPos(pos - (sideDir * width));
		// nextも同様
		output2.pos = UnityWorldToClipPos(posNext + (sideDirNext * width));
		output3.pos = UnityWorldToClipPos(posNext - (sideDirNext * width));

		// 各頂点に色を渡す
		output0.col =
		output1.col = input[0].col;
		output2.col =
		output3.col = input[0].colNext;

		// 
		outStream.Append (output0);
		outStream.Append (output1);
		outStream.Append (output2);
		outStream.Append (output3);
	
		outStream.RestartStrip();
	}

	fixed4 frag (gs_out In) : COLOR
	{
		return In.col;
	}

	ENDCG
   
   }
}
}


