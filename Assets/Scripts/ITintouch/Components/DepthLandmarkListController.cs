using System.Collections.Generic;
using Mediapipe;
using UnityEngine;

namespace ITintouch.Components
{
    public class DepthLandmarkListController : MonoBehaviour
    {
        private const float VisibilityThreshold = 0.5f;

        [SerializeField]
        private CVCameraCapture cvCameraCapture;

        [SerializeField]
        private DepthCameraCapture depthCameraCapture;

        [SerializeField]
        private GameObject landmarkPrefab;

        [SerializeField]
        private Vector2 depthCoordsScale = new Vector2(1f, 1f);

        [SerializeField]
        private float depthValueScale = 4f;

        [SerializeField]
        private float depthOffset = 1f;

        [SerializeField]
        private float offsetScale = 0.5f;

        private Vector3 offset;
        private NormalizedLandmarkList poseLandmarks;
        private List<Renderer> landmarkRenderers = new();

        public void SetPoseLandmarks(NormalizedLandmarkList poseLandmarks)
        {
            this.poseLandmarks = poseLandmarks;
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
            AdjustChildCount(landmarks.Count, landmarkPrefab);

            var cameraTransform = cvCameraCapture.CameraTransform;
            transform.position = cameraTransform.GetPosition();
            transform.rotation = cameraTransform.rotation;

            for (int i = 0; i < landmarks.Count; i++)
            {
                var landmark = landmarks[i];
                var child = landmarkRenderers[i];

                Render(child, landmark);
            }
        }

        private void Render(Renderer child, NormalizedLandmark landmark)
        {
            if (landmark.Visibility < VisibilityThreshold)
            {
                child.enabled = false;
                return;
            }

            child.enabled = true;

            var x = landmark.X - 0.5f;
            var y = (landmark.Y - 0.5f) * cvCameraCapture.CaptureHeight / cvCameraCapture.CaptureWidth;

            var depth = GetDepth(
                depthCameraCapture.ImageTexture,
                Mathf.RoundToInt(x * depthCameraCapture.CaptureWidth * depthCoordsScale.x) +
                depthCameraCapture.CaptureWidth / 2,
                Mathf.RoundToInt(y * depthCameraCapture.CaptureHeight * depthCoordsScale.y) +
                depthCameraCapture.CaptureHeight / 2
            ) * depthValueScale + depthOffset;

            // calculate offset
            var fov = Application.isEditor ? 60 : cvCameraCapture.Intrinsics.FOV;
            var frustumHeight = 2.0f * depth * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
            var scale = frustumHeight * offsetScale;
            offset = new Vector3(x * scale, -y * scale, depth);

            // set transform
            child.transform.localPosition = offset;
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