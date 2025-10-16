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
            yield return new WaitForSeconds(1f/_updatesPerSecond);
            Scan();
            StartCoroutine(ScanCoroutine());
        }

        private void SetUpCamera()
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            if (devices.Length == 0)
            {
                _isCamAvaible = false;
                return;
            }
            for (int i = 0; i < devices.Length; i++)
            {
                if (devices[i].isFrontFacing == true)
                {
                    _cameraTexture = new WebCamTexture(devices[i].name, (int)_scanZone.rect.width, (int)_scanZone.rect.height);
                    break;
                }
            }
            _cameraTexture.Play();
            _isCamAvaible = true;
        }

        private void UpdateCameraRender()
        {
            if (_isCamAvaible == false)
            {
                return;
            }
            float ratio = (float)_cameraTexture.width / (float)_cameraTexture.height;
            _aspectRatioFitter.aspectRatio = ratio;

            int orientation = _cameraTexture.videoRotationAngle;
            orientation = orientation * 3;
        }
        public void OnClickScan()
        {
            Scan();
        }
        private void Scan()
        {
            try
            {
                IBarcodeReader barcodeReader = new BarcodeReader();
                Result result = barcodeReader.Decode(_cameraTexture.GetPixels32(), _cameraTexture.width, _cameraTexture.height);
                if (result != null)
                {
                    _inputManager.OnScan(result.Text);
                    _textOut.text = result.Text;
                }
                else {
                    _textOut.text = "Failed to Read QR CODE";
                }
            }
            catch
            {
                _textOut.text = "FAILED IN TRY";
            }
        }
    }
}