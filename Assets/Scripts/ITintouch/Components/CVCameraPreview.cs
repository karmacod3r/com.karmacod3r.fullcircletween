using System;
using UnityEngine;
using UnityEngine.UI;

namespace ITintouch.Components
{
    [RequireComponent(typeof(RawImage))]
    public class CVCameraPreview : MonoBehaviour
    {
        private RawImage rawImage;
        
        [SerializeField] 
        private CVCameraCapture cvCameraCapture;

        private void Awake()
        {
            rawImage = GetComponent<RawImage>();
        }

        private void Update()
        {
            rawImage.texture = cvCameraCapture.RenderTexture;
        }
    }
}
