using UnityEngine;

public class CanvasTemp : MonoBehaviour
{
    public GameObject mainCanvas;
    public GameObject Image;

    public ComputeShader computeShader;

    // RenderTexture renderTexture_A;
    // RenderTexture renderTexture_B;

    // static public RenderTexture outputTexture; 

    // int kernelIndex_KernelFunction_A;
    // int kernelIndex_KernelFunction_B;

    // int kernelIndex;

    // struct ThreadSize
    // {
    //     public int x;
    //     public int y;
    //     public int z;

    //     public ThreadSize(uint x, uint y, uint z)
    //     {
    //         this.x = (int)x;
    //         this.y = (int)y;
    //         this.z = (int)z;
    //     }
    // }

    // ThreadSize kernelThreadSize_KernelFunction_A;
    // ThreadSize kernelThreadSize_KernelFunction_B;
    //     ThreadSize kernelThreadSize;


    // void Start()
    // {
    //     // enableRandomWrite を有効にして、
    //     // 書き込み可能な状態のテクスチャを生成することが重要です。

    //     this.renderTexture_A = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGB32);
    //     this.renderTexture_B = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGB32);

    //     this.renderTexture_A.enableRandomWrite = true;
    //     this.renderTexture_B.enableRandomWrite = true;

    //     this.renderTexture_A.Create();
    //     this.renderTexture_B.Create();

    //     initTexture();
    //     initCanvas();

        

    //     // カーネルのインデックスと、スレッドのサイズを保存します。

    //     this.kernelIndex_KernelFunction_A
    //         = this.computeShader.FindKernel("KernelFunction_A");
    //     this.kernelIndex_KernelFunction_B
    //         = this.computeShader.FindKernel("KernelFunction_B");

    //     uint threadSizeX, threadSizeY, threadSizeZ;

    //     this.computeShader.GetKernelThreadGroupSizes
    //         (this.kernelIndex_KernelFunction_A,
    //          out threadSizeX, out threadSizeY, out threadSizeZ);

    //     this.kernelThreadSize_KernelFunction_A
    //         = new ThreadSize(threadSizeX, threadSizeY, threadSizeZ);

    //     this.computeShader.GetKernelThreadGroupSizes
    //         (this.kernelIndex_KernelFunction_B,
    //          out threadSizeX, out threadSizeY, out threadSizeZ);

    //     this.kernelThreadSize_KernelFunction_B
    //         = new ThreadSize(threadSizeX, threadSizeY, threadSizeZ);

    //     // ComputeShader で計算した結果を保存するためのバッファとして、ここではテクスチャを設定します。

    //     this.computeShader.SetTexture
    //         (this.kernelIndex_KernelFunction_A, "textureBuffer", this.renderTexture_A);
    //     this.computeShader.SetTexture
    //         (this.kernelIndex_KernelFunction_B, "textureBuffer", this.renderTexture_B);


    //     // -----
    //     this.kernelIndex = this.computeShader.FindKernel("Kernel");
    //     this.computeShader.GetKernelThreadGroupSizes
    //         (this.kernelIndex,
    //          out threadSizeX, out threadSizeY, out threadSizeZ);

    //     this.kernelThreadSize
    //         = new ThreadSize(threadSizeX, threadSizeY, threadSizeZ);

    // }

    // // テクスチャの生成
    // static void initTexture(){
	// 	outputTexture = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGB32);
	// 	outputTexture.enableRandomWrite = true;
	// 	outputTexture.Create();
	// }

    // // キャンバスの設定
    // static public void initCanvas(){
	// 	mainCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
	// 	mainCanvas.GetComponent<Canvas>().worldCamera = Camera.main;
	// 	mainCanvas.GetComponent<UnityEngine.UI.CanvasScaler>().uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
	// 	mainCanvas.GetComponent<UnityEngine.UI.CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
	// 	mainCanvas.GetComponent<UnityEngine.UI.CanvasScaler>().matchWidthOrHeight = 1.0f;
	// 	outputImage = Image.GetComponent<UnityEngine.UI.Image>();
	// 	outputImage.material.mainTexture = outputTexture;
	// 	outputImage.type = UnityEngine.UI.Image.Type.Simple;
	// 	outputImage.GetComponent<RectTransform>().sizeDelta = new Vector2(1080, 1080);
	// }

    // void Update()
    // {
    //     // Dispatch メソッドで ComputeShader を実行します。
    //     // ここで、グループ数は、"解像度 / スレッド数" で算出します。
    //     // これですべてのピクセルが処理されます。
    //     // 
    //     // 例えば 512 / 8 = 64 です。各グループは 64 ピクセルずつ平行に処理します。
    //     // またグループスレッドは、64 / 8 = 8 ピクセルずつ並行に処理します。

    //     this.computeShader.Dispatch(this.kernelIndex_KernelFunction_A,
    //                                 this.renderTexture_A.width / this.kernelThreadSize_KernelFunction_A.x,
    //                                 this.renderTexture_A.height / this.kernelThreadSize_KernelFunction_A.y,
    //                                 this.kernelThreadSize_KernelFunction_A.z);

    //     this.computeShader.Dispatch(this.kernelIndex_KernelFunction_B,
    //                                 this.renderTexture_B.width / this.kernelThreadSize_KernelFunction_B.x,
    //                                 this.renderTexture_B.height / this.kernelThreadSize_KernelFunction_B.y,
    //                                 this.kernelThreadSize_KernelFunction_B.z);

    //     // 実行して得られた結果をテクスチャとして設定します。

    //     plane_A.GetComponent<Renderer>().material.mainTexture = this.renderTexture_A;
    //     plane_B.GetComponent<Renderer>().material.mainTexture = this.renderTexture_B;
    // }
}