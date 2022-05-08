using System.Runtime.InteropServices;
using UnityEngine;

public class ComputeShaderInstancing2 : MonoBehaviour
{
    public struct Particle
    {
        public Vector3 position;
        public Vector3 velocity;
        public Color color;
        public Vector3 dir;
        public Vector3 scale;
    }

    public ComputeShader computeShader;
    public Mesh mesh;
    public Material material;
    public int count = 10000;

    ComputeBuffer particleBuffer;
    Particle[] particles;

    int kernelIndex;
    Vector3Int kernelThreads;

    void Start()
    {
        kernelIndex = computeShader.FindKernel("UpdateParticles");

        uint x, y, z;

        computeShader.GetKernelThreadGroupSizes(kernelIndex, out x, out y, out z);

        kernelThreads = new Vector3Int((int)x, (int)y, (int)z);

        particles = new Particle[count];
        float sphereRadius = 10;

        for (int i = 0; i < count; i++)
        {
            particles[i] = new Particle()
            {
                position = Random.onUnitSphere * sphereRadius,
                velocity = new Vector3(Random.Range(-3f, 4f),
                                       Random.Range(-3f, 6f),
                                       Random.Range(-3f, 2f)),
                color = new Color(Random.value,
                                       Random.value,
                                       Random.value),
                dir = new Vector3(Random.Range(-3f, 4f),
                                       Random.Range(-3f, 6f),
                                       Random.Range(-3f, 2f)),
                // scale = new Vector3(Random.Range(2f, 4f),
                //                        Random.Range(0.1f, 1f),
                //                        Random.Range(0.1f, 1f)),
                scale = new Vector3(1f, 1f, 1f),
            };
        }

        particleBuffer = new ComputeBuffer(count, Marshal.SizeOf(typeof(Particle)));
        particleBuffer.SetData(particles);

        computeShader.SetBuffer(kernelIndex, "_ParticleBuffer", particleBuffer);
        material.SetBuffer("_ParticleBuffer", particleBuffer);
    }

    void Update()
    {
        computeShader.SetFloat("_DeltaTime", Time.deltaTime);
        computeShader.SetFloat("_Time", Time.time);

        computeShader.Dispatch(kernelIndex, particleBuffer.count / kernelThreads.x, 1, 1);

        Graphics.DrawMeshInstancedProcedural
            (mesh, 0, material, new Bounds(Vector3.zero, Vector3.one * 100f), count);
    }

    void OnDestroy()
    {
        particleBuffer.Release();
    }
}