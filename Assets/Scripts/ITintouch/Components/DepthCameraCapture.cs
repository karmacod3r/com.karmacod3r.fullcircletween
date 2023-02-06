using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

public class DepthCameraCapture : MonoBehaviour
{
    private readonly MLPermissions.Callbacks permissionCallbacks = new();

    private bool permissionGranted;
    private MLDepthCamera depthCamera;
    private MLDepthCamera.Data lastData;

    [SerializeField, Tooltip("Timeout in milliseconds for data retrieval.")]
    private ulong timeout = 0;

    public Texture2D ImageTexture { get; private set; }

    private MLDepthCamera.Mode mode = MLDepthCamera.Mode.LongRange;
    private MLDepthCamera.CaptureFlags captureFlag = MLDepthCamera.CaptureFlags.DepthImage;

    void Awake()
    {
        permissionCallbacks.OnPermissionGranted += OnPermissionGranted;
        permissionCallbacks.OnPermissionDenied += OnPermissionDenied;
        permissionCallbacks.OnPermissionDeniedAndDontAskAgain += OnPermissionDenied;
    }

    void Start()
    {
        var settings = new MLDepthCamera.Settings()
        {
            Mode = mode,
            Flags = captureFlag
        };
        depthCamera = new MLDepthCamera(settings);

        MLPermissions.RequestPermission(MLPermission.DepthCamera, permissionCallbacks);
    }

    void Update()
    {
        if (!permissionGranted || depthCamera == null || !depthCamera.IsConnected)
        {
            return;
        }

        var result = depthCamera.GetLatestDepthData(timeout, out MLDepthCamera.Data data);
        if (result.IsOk)
        {
            lastData = data;
        }

        if (lastData.DepthImage != null)
        {
            CheckAndCreateTexture((int)lastData.DepthImage.Value.Width, (int)lastData.DepthImage.Value.Height);

            ImageTexture.LoadRawTextureData(lastData.DepthImage.Value.Data);
            ImageTexture.Apply();
        }
    }

    private void OnDestroy()
    {
        permissionCallbacks.OnPermissionGranted -= OnPermissionGranted;
        permissionCallbacks.OnPermissionDenied -= OnPermissionDenied;
        permissionCallbacks.OnPermissionDeniedAndDontAskAgain -= OnPermissionDenied;
        DisonnectCamera();
    }

    private void OnPermissionDenied(string permission)
    {
        if (permission == MLPermission.Camera)
        {
            Debug.LogError($"{permission} denied, example won't function.");
        }
        else if (permission == MLPermission.DepthCamera)
        {
            Debug.LogError($"{permission} denied, example won't function.");
        }
    }

    private void OnPermissionGranted(string permission)
    {
        Debug.Log($"Granted {permission}.");
        permissionGranted = true;
        ConnectCamera();
    }

    private void ConnectCamera()
    {
        var result = depthCamera.Connect();
        if (result.IsOk && depthCamera.IsConnected)
        {
            Debug.Log(
                $"Connected to new depth camera with mode = {depthCamera.CurrentSettings.Mode} and flags = {depthCamera.CurrentSettings.Flags}");
        }
        else
        {
            Debug.LogError($"Failed to connect to camera: {result.Result}");
        }
    }

    private void DisonnectCamera()
    {
        var result = depthCamera.Disconnect();
        if (result.IsOk && !depthCamera.IsConnected)
        {
            Debug.Log(
                $"Disconnected depth camera with mode = {depthCamera.CurrentSettings.Mode} and flags = {depthCamera.CurrentSettings.Flags}");
        }
        else
        {
            Debug.LogError($"Failed to disconnect to camera: {result.Result}");
        }
    }

    private void CheckAndCreateTexture(int width, int height)
    {
        if (ImageTexture == null || ImageTexture.width != width || ImageTexture.height != height)
        {
            ImageTexture = new Texture2D(width, height, TextureFormat.RFloat, false);
            ImageTexture.filterMode = FilterMode.Bilinear;
        }
    }
}