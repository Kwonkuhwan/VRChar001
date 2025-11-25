using UnityEngine;
using UniVRM10;

public class FaceTrackingToVrm : MonoBehaviour
{
    public float eyeBlinkL;  // 0~1
    public float eyeBlinkR;
    public float mouthOpen;  // 0~1
    public float smile;      // 0~1
    public float lookDown;   // 0~1

    Vrm10Instance _instance;
    Vrm10RuntimeExpression expression;
    ExpressionKey _blinkKey, _blinkLKey, _blinkRKey;
    ExpressionKey _aaKey, _happyKey, _lookDownKey;

    void Awake()
    {
        _instance = GetComponent<Vrm10Instance>();

        if (_instance != null)
        {
            expression = _instance.Runtime.Expression;
        }

        if (expression != null)
        {
            _blinkKey = ExpressionKey.Blink;
            _blinkLKey = ExpressionKey.BlinkLeft;
            _blinkRKey = ExpressionKey.BlinkRight;
            _aaKey = ExpressionKey.Aa;
            _happyKey = ExpressionKey.Happy;
            _lookDownKey = ExpressionKey.LookDown;
        }
    }

    void Update()
    {
        if (_instance == null || _instance.Runtime == null) return;

        var exp = _instance.Runtime.Expression;

        // 눈 깜빡임
        exp.SetWeight(_blinkLKey, eyeBlinkL);
        exp.SetWeight(_blinkRKey, eyeBlinkR);
        // 필요하면 중앙 Blink도 같이
        exp.SetWeight(_blinkKey, Mathf.Max(eyeBlinkL, eyeBlinkR));

        // 입 "아" (간단 버전: mouthOpen 그대로 사용)
        exp.SetWeight(_aaKey, mouthOpen);

        // 웃음
        exp.SetWeight(_happyKey, smile);

        // 눈만 아래로 내리기 = LookDown 프리셋 사용
        exp.SetWeight(_lookDownKey, lookDown);

        // Apply는 Runtime.Process가 알아서 함 (UpdateType LateUpdate)
    }
}
