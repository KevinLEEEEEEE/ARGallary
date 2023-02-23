using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class LightEstimate : MonoBehaviour
{
    public ARCameraManager arcamman;
    private Light our_light;

    void OnEnable()
    {
        arcamman.frameReceived += Getlight;
    }

    void OnDisable()
    {
        arcamman.frameReceived -= Getlight;
    }

    void Start()
    {
        our_light = transform.GetComponent<Light>();
    }

    private void Getlight(ARCameraFrameEventArgs args)
    {
        if (args.lightEstimation.mainLightColor.HasValue)
        {
            our_light.color = args.lightEstimation.mainLightColor.Value;
        }
    }

}