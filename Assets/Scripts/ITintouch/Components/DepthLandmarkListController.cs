using System;
using System.Collections.Generic;
using Mediapipe;
using Mediapipe.Unity.PoseTracking;
using UnityEngine;

namespace ITintouch.Components
{
    public class DepthLandmarkListController : MonoBehaviour
    {
        private const float VisibilityThreshold = 0.01f;

        [SerializeField] private PoseTrackingSolution poseTrackingSolution;
        [SerializeField] private CVCameraCapture cvCameraCapture;
        [SerializeField] private DepthCameraCapture depthCameraCapture;
        [SerializeField] private GameObject landmarkPrefab;
        [SerializeField] private Vector2 depthCoordsScale = new Vector2(1f, 1f);
        [SerializeField] private float depthValueScale = 4f;
        [SerializeField] private float depthOffset = 1f;
        [SerializeField] private float offsetScale = 0.5f;
        [SerializeField] private float depthSmoothing = 1f;

        private Vector3 offset;
        private NormalizedLandmarkList poseLandmarks;
        private List<Renderer> landmarkRenderers = new();

        private float[] depthBuffer = Array.Empty<float>();
        private float lastTimer;

        private void OnEnable()
        {
            poseTrackingSolution.receivedNormalizedLandmarks += SetPoseLandmarks;
        }

        private void OnDisable()
        {
            poseTrackingSolution.receivedNormalizedLandmarks -= SetPoseLandmarks;
        }
        
        private void SetPoseLandmarks(NormalizedLandmarkList value)
        {
            poseLandmarks = value;
        }

        private void LateUpdate()
        {
            // TODO: this is a quick and dirty approximation that doesn't account for camera intrinsics / distortion
            if (poseLandmarks == null || poseLandmarks.Landmark == null) return;
            if (depthCameraCapture == null || cvCameraCapture == null) return;

            RenderLandmarks(poseLandmarks.Landmark);
        }

        private void RenderLandmarks(IList<NormalizedLandmark> landmarks)
        {
            Debug.Log("CV Camera FOV: " + cvCameraCapture.Intrinsics.FOV);
            
            var dt = Time.time - lastTimer;
            lastTimer = Time.time;
            
            AdjustChildCount(landmarks.Count, landmarkPrefab);
            if (landmarks.Count != depthBuffer.Length)
            {
                Array.Resize(ref depthBuffer, landmarks.Count);
            }

            var cameraTransform = cvCameraCapture.CameraTransform;
            transform.position = cameraTransform.GetPosition();
            transform.rotation = cameraTransform.rotation;

            for (int i = 0; i < landmarks.Count; i++)
            {
                var landmark = landmarks[i];
                var child = landmarkRenderers[i];

                depthBuffer[i] = Render(child, landmark, depthBuffer[i], dt);
            }
        }

        private float Render(Renderer child, NormalizedLandmark landmark, float depth, float dt)
        {
            if (landmark.Visibility < VisibilityThreshold)
            {
                child.enabled = false;
                return depth;
            }

            child.enabled = true;

            var x = landmark.X - 0.5f;
            var y = (landmark.Y - 0.5f) * cvCameraCapture.CaptureHeight / cvCameraCapture.CaptureWidth;

            var currentDepth = GetDepth(
                depthCameraCapture.ImageTexture,
                Mathf.RoundToInt(x * depthCameraCapture.CaptureWidth * depthCoordsScale.x) +
                depthCameraCapture.CaptureWidth / 2,
                Mathf.RoundToInt(y * depthCameraCapture.CaptureHeight * depthCoordsScale.y) +
                depthCameraCapture.CaptureHeight / 2
            ) * depthValueScale + depthOffset;

            var s = Mathf.Abs(dt * 1000f * depthSmoothing) + 1f;
            var newDepth = Mathf.Lerp(depth, currentDepth, Mathf.Clamp01(1f / s));

            // calculate offset
            var fov = Application.isEditor ? 60 : cvCameraCapture.Intrinsics.FOV;
            var frustumHeight = 2.0f * newDepth * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
            var scale = frustumHeight * offsetScale;
            offset = new Vector3(x * scale, -y * scale, newDepth);

            // set transform
            child.transform.localPosition = offset;

            return newDepth;
        }

        private void AdjustChildCount(int count, GameObject prefab)
        {
            for (var i = landmarkRenderers.Count; i < count; i++)
            {
                var go = Instantiate(prefab, transform);
                landmarkRenderers.Add(go.GetComponent<Renderer>());
            }

            for (var i = landmarkRenderers.Count; i > count; i--)
            {
                landmarkRenderers.RemoveAt(i - 1);

                var child = transform.GetChild(i - 1);
                if (child == null) continue;

                Destroy(child.gameObject);
            }
        }

        private int Wrap(int value, int size)
        {
            var ret = value % size;
            return ret < 0 ? size + ret : size;
        }

        private float GetDepth(Texture2D texture, int x, int y)
        {
            if (texture == null) return 0;

            return texture.GetPixel(Wrap(x, texture.width), Wrap(texture.height - 1 - y, texture.height)).r;
        }
    }
}