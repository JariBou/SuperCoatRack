using System;
using System.Collections;
using _project.Scripts.Managers.Inputs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

namespace _project.Scripts.Managers
{
    public class ScanManager : MonoBehaviour
    {
        [SerializeField]
        private AspectRatioFitter _aspectRatioFitter;
        [SerializeField]
        private TextMeshProUGUI _textOut;
        [SerializeField]
        private RectTransform _scanZone;
        [SerializeField]
        private RawImage _rawImageBackground;
        
        [SerializeField, Range(0, 144)]
        private int _updatesPerSecond = 10;
        
        [SerializeField]
        private InputManager _inputManager;

        private bool _isCamAvaible;
        private WebCamTexture _cameraTexture;
        
        void Start()
        {
            SetUpCamera();
            StartCoroutine(ScanCoroutine());
        }

        // Update is called once per frame
        void Update()
        {
            UpdateCameraRender();
            // Scan();
        }

        private IEnumerator ScanCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f / _updatesPerSecond);
                if (Scan())
                {
                    yield return new WaitForSeconds(1.5f);
                }
            }
        }


        private void SetUpCamera()
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            if (devices.Length == 0)
            {
                Debug.LogError("❌ Aucune caméra détectée !");
                _isCamAvaible = false;
                return;
            }

            WebCamDevice selectedDevice = devices[0];
            foreach (var device in devices)
            {
                if (device.isFrontFacing)
                {
                    selectedDevice = device;
                    break;
                }
            }

            _cameraTexture = new WebCamTexture(selectedDevice.name, (int)_scanZone.rect.width, (int)_scanZone.rect.height);
            _cameraTexture.Play();

            _rawImageBackground.texture = _cameraTexture;
            _isCamAvaible = true;
            Debug.Log($"✅ Caméra utilisée : {selectedDevice.name}");
        }

        private void UpdateCameraRender()
        {
            if (_isCamAvaible == false)
            {
                return;
            }
            float ratio = (float)_cameraTexture.width / (float)_cameraTexture.height;
            _aspectRatioFitter.aspectRatio = ratio;

            _rawImageBackground.rectTransform.localEulerAngles =
                new Vector3(0, 0, -_cameraTexture.videoRotationAngle);
        }
        public void OnClickScan()
        {
            Scan();
        }
        private bool Scan()
        {
            if (_cameraTexture.width < 100)
                return false; // trop petit => pas encore prête
            Debug.Log("Scan happens");
            try
            {
                IBarcodeReader barcodeReader = new BarcodeReader();
                Result result = barcodeReader.Decode(_cameraTexture.GetPixels32(), _cameraTexture.width, _cameraTexture.height);
                if (result != null)
                {
                    _inputManager.OnScan(result.Text);
                    _textOut.text = result.Text;
                    return true;
                }
                _textOut.text = "Failed to Read QR CODE";
            }
            catch
            {
                _textOut.text = "FAILED IN TRY";
                return false;
            }
            return false;
        }

        private void OnDisable()
        {
            if (_cameraTexture != null)
            {
                _cameraTexture.Stop();
            }
        }
    }
}