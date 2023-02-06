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

        private void Awake()
        {
            rawImage = GetComponent<RawImage>();
        }

        private void Update()
        {
            rawImage.texture = depthCameraCapture.ImageTexture;
        }
    }
}
