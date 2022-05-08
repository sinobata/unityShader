using UnityEngine;

namespace GPUBasedTrails
{
    [RequireComponent(typeof(GPUTrails))]
    public class GPUTrailsRenderer : MonoBehaviour
    {
        public Material _material;
        GPUTrails _trails;


        private void Start()
        {
            _trails = GetComponent<GPUTrails>();
        }

        void OnRenderObject()
        {
            _material.SetInt(GPUTrails.CSPARAM.NODE_NUM_PER_TRAIL, _trails.nodeNum);
            _material.SetFloat(GPUTrails.CSPARAM.LIFE, _trails.life); // trailの表示時間
            _material.SetBuffer(GPUTrails.CSPARAM.TRAIL_BUFFER, _trails.trailBuffer);
            _material.SetBuffer(GPUTrails.CSPARAM.NODE_BUFFER, _trails.nodeBuffer);
            _material.SetPass(0);

            var nodeNum = _trails.nodeNum;
            var trailNum = _trails.trailNum;
            // DrawProceduralNow は GPU 上で、頂点やインデックスバッファなしで描画 呼び出しを行ないます
            Graphics.DrawProceduralNow(MeshTopology.Points, nodeNum, trailNum);
        }
    }
}
