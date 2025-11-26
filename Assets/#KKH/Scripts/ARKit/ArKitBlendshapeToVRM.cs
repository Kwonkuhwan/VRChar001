using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;
using UniVRM10;
using VRM;

public class ARKitBlendshapeToVRM : MonoBehaviour
{
    [Header("AR Foundation")]
    public ARFaceManager faceManager;
    public ARFace arFace;

    [Header("VRM10")]
    public Vrm10Instance vrm10Instance;


    // VRM10 Expression Keys
    ExpressionKey _blinkL = ExpressionKey.BlinkLeft;
    ExpressionKey _blinkR = ExpressionKey.BlinkRight;
    ExpressionKey _aa = ExpressionKey.Aa;
    ExpressionKey _happy = ExpressionKey.Happy;

    void Update()
    {
#if UNITY_IOS && !UNITY_EDITOR
if (arFace == null || vrm10Instance == null || faceManager == null) return;

        var subsystem = faceManager.subsystem as ARKitFaceSubsystem;
        if(subsystem == null) return;

        using(var coeffs = subsystem.GetBlendShapeCoefficients(arFace.trackableId, Allocator.Temp))
        {
        var exp = vrm10Instance.Runtime.Expression;
            float blinkL = 0f;
            float blinkR = 0f;
            float jawOpen = 0f;
            float smileL = 0f;
            float smileR = 0f;

            foreach (var c in coeffs)
            {
                switch (c.blendShapeLocation)
                {
                    case ARKitBlendShapeLocation.EyeBlinkLeft:
                        blinkL = c.coefficient;
                        break;

                    case ARKitBlendShapeLocation.EyeBlinkRight:
                        blinkR = c.coefficient;
                        break;

                    case ARKitBlendShapeLocation.JawOpen:
                        jawOpen = c.coefficient;
                        break;

                    case ARKitBlendShapeLocation.MouthSmileLeft:
                        smileL = c.coefficient;
                        break;

                    case ARKitBlendShapeLocation.MouthSmileRight:
                        smileR = c.coefficient;
                        break;
                }
            }
            exp.SetWeight(_blinkL, blinkL);
            exp.SetWeight(_blinkR, blinkR);
            exp.SetWeight(_aa, jawOpen);
            exp.SetWeight(_happy, (smileL + smileR) * 0.5f);
        }
#endif
    }
}
