using System;
using UnityEngine;
using UnityEngine.UI;

namespace ITintouch.Components
{
    [RequireComponent(typeof(RawImage))]
    public class DepthCameraPreview : MonoBehaviour
    {
        private RawImage rawImage;
        
        [SerializeField] 
        private DepthCameraCapture depthCameraCapture;

        private Texture2D texture;
        private Color[] pixels;

        private void Awake()
        {
            rawImage = GetComponent<RawImage>();
        }

        private void Update()
        {
            rawImage.texture = depthCameraCapture.ImageTexture;
        }

        private void RenderByteBufferToBufferTexture(ref Texture2D output)
        {
            if (output == null || output.width != depthCameraCapture.CaptureWidth || output.height != depthCameraCapture.CaptureHeight)
            {
                output = new Texture2D(depthCameraCapture.CaptureWidth, depthCameraCapture.CaptureHeight, TextureFormat.RGBA32, false);
                pixels = new Color[output.width * output.height];
            }
            
            var i = 0;
            for (var y = 0; y < output.height; y++)
            {
                for (var x = 0; x < output.width; x++)
                {
                    var c = pixels[i];
                    c.r = depthCameraCapture.GetDepth(x, y);
                    pixels[i] = c;
                    i++;
                }
            }
            
            output.SetPixels(pixels);
            output.Apply();
        }

        private void RenderImageTextureToBufferTexture(ref Texture2D output)
        {
            if (output == null || output.width != depthCameraCapture.CaptureWidth || output.height != depthCameraCapture.CaptureHeight)
            {
                output = new Texture2D(depthCameraCapture.CaptureWidth, depthCameraCapture.CaptureHeight, TextureFormat.RGBA32, false);
                pixels = new Color[output.width * output.height];
            }
            
            var i = 0;
            for (var y = 0; y < output.height; y++)
            {
                for (var x = 0; x < output.width; x++)
                {
                    var c = pixels[i];
                    c = depthCameraCapture.ImageTexture.GetPixel(x, y);
                    pixels[i] = c;
                    i++;
                }
            }
            
            output.SetPixels(pixels);
            output.Apply();
        }
    }
}
