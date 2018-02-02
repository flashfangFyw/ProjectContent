using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

/* 
    Author:     fyw 
    CreateDate: 20170209 
    Desc:       PNG导出
                不要挂在摄像机上
*/
namespace ffDevelopmentSpace
{
    public class PngExporter : MonoBehaviour
    {

        #region public property
        // Default folder name where you want the animations to be output  
        public string folder = "PNG_Animations";
        public string fileNameEx = "pic";

        // Framerate at which you want to play the animation  
     
        public int frameRate = 25;                  // export frame rate 导出帧率，设置Time.captureFramerate会忽略真实时间，直接使用此帧率  
        public float frameCount = 100;              // export frame count 导出帧的数目，100帧则相当于导出5秒钟的光效时间。由于导出每一帧的时间很长，所以导出时间会远远长于直观的光效播放时间  
        public int screenWidth = 960;               // not use 暂时没用，希望可以直接设置屏幕的大小（即光效画布的大小）  
        public int screenHeight = 640;
        public Camera renderCamera;
        public Vector3 cameraPosition = Vector3.zero;
        public Vector3 cameraRotation = Vector3.zero;
        #endregion
        #region private property
        private string realFolder = ""; // real folder where the output files will be  
        private float originaltimescaleTime; // track the original time scale so we can freeze the animation between frames  
        private float currentTime = 0;
        private bool over = false;
        private int currentIndex = 0;
        private Camera exportCamera;    // camera for export 导出光效的摄像机，使用RenderTexture  
        private bool renderFlag = false;
        #endregion

        #region unity function
        void Start()
        {
            // set frame rate  
            Time.captureFramerate = frameRate;

            // Create a folder that doesn't exist yet. Append number if necessary.  
            realFolder = Path.Combine(folder, fileNameEx);



            // Create the folder  
            if (!Directory.Exists(realFolder))
            {
                Directory.CreateDirectory(realFolder);
            }

            originaltimescaleTime = Time.timeScale;

            if(renderCamera==null)
            {
                GameObject goCamera = Camera.main.gameObject;
                if (cameraPosition != Vector3.zero)
                {
                    goCamera.transform.position = cameraPosition;
                }

                if (cameraRotation != Vector3.zero)
                {
                    goCamera.transform.rotation = Quaternion.Euler(cameraRotation);
                }

                GameObject go = Instantiate(goCamera) as GameObject;
                exportCamera = go.GetComponent<Camera>();
            }
            else
            {
                exportCamera = renderCamera;
            }

       
            currentTime = 0;


        }
        void Update()
        {
            if (!renderFlag) return;
            if (over) return;
            currentTime += Time.deltaTime;
            
            if (!over && currentIndex >= frameCount)
            {
                over = true;
                Cleanup();
                Debug.Log("Finish");
                return;
            }
            // 每帧截屏  
            StartCoroutine(CaptureFrame());
        }
        #endregion

        #region public function
        #endregion
        public void StartRender()
        {
            renderFlag = true;
        }
        public void StopRender()
        {
            renderFlag = false;
        }
        #region private function
        private void Cleanup()
        {
            if (renderCamera == exportCamera) return;
            DestroyImmediate(exportCamera);
            DestroyImmediate(gameObject);
        }

        IEnumerator CaptureFrame()
        {
            // Stop time  
            Time.timeScale = 0;
            // Yield to next frame and then start the rendering  
            // this is important, otherwise will have error  
            yield return new WaitForEndOfFrame();

            string filename = String.Format("{0}/{1}{2:D04}.png", realFolder, fileNameEx,++currentIndex);
            Debug.Log(filename);

          
            //Debug.Log("width0=" + screenWidth + "   height0=" + screenWidth);
            int width = Screen.width;
            int height = Screen.height;
            //Screen.width = Screen.width;
            Debug.Log("width="+width+ "   height="+ height);
            Debug.Log(Screen.currentResolution);
            //width = screenWidth;
            //height = screenHeight;

            //Initialize and render textures  
            RenderTexture blackCamRenderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            RenderTexture whiteCamRenderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);

            exportCamera.targetTexture = blackCamRenderTexture;
            exportCamera.backgroundColor = Color.black;
            exportCamera.Render();
            RenderTexture.active = blackCamRenderTexture;
            Texture2D texb = GetTex2D();

            //Now do it for Alpha Camera  
            exportCamera.targetTexture = whiteCamRenderTexture;
            exportCamera.backgroundColor = Color.white;
            exportCamera.Render();
            RenderTexture.active = whiteCamRenderTexture;
            Texture2D texw = GetTex2D();

            // If we have both textures then create final output texture  
            if (texw && texb)
            {
                Texture2D outputtex = new Texture2D(width, height, TextureFormat.ARGB32, false);

                // we need to check alpha ourselves,because particle use additive shader  
                // Create Alpha from the difference between black and white camera renders  
                for (int y = 0; y < outputtex.height; ++y)
                { // each row  
                    for (int x = 0; x < outputtex.width; ++x)
                    { // each column  
                        float alpha;
                        alpha = texw.GetPixel(x, y).r - texb.GetPixel(x, y).r;
                        alpha = 1.0f - alpha;
                        Color color;
                        if (alpha == 0)
                        {
                            color = Color.clear;
                        }
                        else
                        {
                            color = texb.GetPixel(x, y);
                        }
                        color.a = alpha;
                        outputtex.SetPixel(x, y, color);
                    }
                }
                outputtex = ScaleTexture(outputtex, screenWidth, screenHeight);

                // Encode the resulting output texture to a byte array then write to the file  
                byte[] pngShot = outputtex.EncodeToPNG();
                File.WriteAllBytes(filename, pngShot);

                // cleanup, otherwise will memory leak  
                pngShot = null;
                RenderTexture.active = null;
                exportCamera.targetTexture = null;
                DestroyImmediate(outputtex);
                outputtex = null;
                DestroyImmediate(blackCamRenderTexture);
                blackCamRenderTexture = null;
                DestroyImmediate(whiteCamRenderTexture);
                whiteCamRenderTexture = null;
                DestroyImmediate(texb);
                texb = null;
                DestroyImmediate(texw);
                texb = null;

                System.GC.Collect();

                // Reset the time scale, then move on to the next frame.  
                Time.timeScale = originaltimescaleTime;
            }
        }

        // Get the texture from the screen, render all or only half of the camera  
        private Texture2D GetTex2D()
        {
            // Create a texture the size of the screen, RGB24 format  
            int width = Screen.width;
            int height = Screen.height;
            Texture2D tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
            // Read screen contents into the texture  
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();
            return tex;
        }
        private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);

            float incX = (1.0f / (float)targetWidth);
            float incY = (1.0f / (float)targetHeight);

            for (int i = 0; i < result.height; ++i)
            {
                for (int j = 0; j < result.width; ++j)
                {
                    Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                    result.SetPixel(j, i, newColor);
                }
            }
            result.Apply();
            return result;
        }
        #endregion

        #region event function
        #endregion
    }
}
