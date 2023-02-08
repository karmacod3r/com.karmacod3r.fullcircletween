using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using Mediapipe;
using UnityEngine;

namespace ITintouch.Components
{
    public class PoseWorldLandmarksPositioning : MonoBehaviour
    {
        private const int LeftHipIndex = 23;
        private const int RightHipIndex = 24;

        [SerializeField]
        private CVCameraCapture cvCameraCapture;

        [SerializeField]
        private DepthCameraCapture depthCameraCapture;

        [SerializeField]
        private Transform targetTransform;

        [SerializeField]
        private Transform childTransform;

        [SerializeField]
        private Vector2 depthCoordsScale = new Vector2(1f, 1f);

        [SerializeField]
        private float depthValueScale = 4f;

        [SerializeField]
        private float depthOffset = 1f;

        private Vector3 offset;

        public void SetPoseLandmarks(NormalizedLandmarkList poseLandmarks)
        {
            // TODO: this is a quick and dirty approximation that doesn't account for camera intrinsics / distortion 

            var hipCenter = CalculateHipCenter(poseLandmarks.Landmark);

            var depth = GetDepth(
                depthCameraCapture.ImageBuffer,
                Mathf.RoundToInt(hipCenter.x * cvCameraCapture.CaptureWidth * depthCoordsScale.x),
                Mathf.RoundToInt(hipCenter.y * cvCameraCapture.CaptureHeight * depthCoordsScale.y),
                depthCameraCapture.CaptureWidth,
                depthCameraCapture.CaptureHeight
            ) / 255f * depthValueScale + depthOffset;
            
            // calculate offset
            // var frustumHeight = 2.0f * depth * Mathf.Tan(cvCameraCapture.Intrinsics.FOV * 0.5f * Mathf.Deg2Rad);
            // var scale = frustumHeight;
            // offset = new Vector3(hipCenter.x * scale, hipCenter.y * scale, depth);
            offset = new Vector3(0, 0, depth);
        }

        private void LateUpdate()
        {
            var cameraTransform = cvCameraCapture.CameraTransform;
            targetTransform.position = cameraTransform.GetPosition();
            targetTransform.rotation = cameraTransform.rotation;

            childTransform.localPosition = offset;
        }

        private int Wrap(int value, int size)
        {
            var ret = value % size;
            return ret < 0 ? size + ret : size;
        }

        private byte GetDepth(byte[] imageBuffer, int x, int y, int bufferWidth, int bufferHeight)
        {
            return imageBuffer[Wrap(x, bufferWidth) + (bufferHeight - 1 - Wrap(y, bufferHeight)) * bufferWidth];
        }

        /// <summary>
        /// calculates the 2d center position of the hip in a range of [-0.5, 0.5]
        /// </summary>
        /// <param name="landmarks"></param>
        /// <returns></returns>
        private Vector2 CalculateHipCenter(IList<NormalizedLandmark> landmarks)
        {
            return new Vector2(
                (landmarks[LeftHipIndex].X + landmarks[RightHipIndex].X) * 0.5f - 0.5f,
                (landmarks[LeftHipIndex].Y + landmarks[RightHipIndex].Y) * 0.5f - 0.5f
            );
        }
    }
}