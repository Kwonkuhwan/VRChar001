using UnityEngine;
using UniVRM10;

[RequireComponent(typeof(Vrm10Instance))]
public class Vrm10VmcFaceDriver : MonoBehaviour
{
    public VmcReceiver vmc; // VmcReceiver 오브젝트 드래그

    Vrm10Instance _instance;
    ExpressionKey _blinkLKey, _blinkRKey, _blinkKey;
    ExpressionKey _aaKey, _happyKey, _angryKey, _funKey, _sorrowKey;

    void Awake()
    {
        _instance = GetComponent<Vrm10Instance>();
        _blinkLKey = ExpressionKey.Blink;
        _blinkRKey = ExpressionKey.BlinkRight;
        _blinkKey = ExpressionKey.Blink;
        _aaKey = ExpressionKey.Aa;
        _happyKey = ExpressionKey.Happy;
        _angryKey = ExpressionKey.Angry;
        _sorrowKey = ExpressionKey.Sad;
    }

    void Update()
    {
        if (_instance == null || _instance.Runtime == null || vmc == null) return;

        var exp = _instance.Runtime.Expression;

        // 기본 0
        exp.SetWeight(_blinkLKey, 0);
        exp.SetWeight(_blinkRKey, 0);
        exp.SetWeight(_blinkKey, 0);
        exp.SetWeight(_aaKey, 0);
        exp.SetWeight(_happyKey, 0);
        exp.SetWeight(_angryKey, 0);
        exp.SetWeight(_sorrowKey, 0);

        // VSeeFace 값 → VRM 표정
        float bl = vmc.GetBlend("blink_l");
        float br = vmc.GetBlend("blink_r");
        float a = vmc.GetBlend("A");    // 입 "아"
        float j = vmc.GetBlend("Joy");  // 웃음
        float angry = vmc.GetBlend("Angry"); // 화남
        float sad = vmc.GetBlend("Sorrow"); // 슬픔

        exp.SetWeight(_blinkLKey, bl);
        exp.SetWeight(_blinkRKey, br);
        exp.SetWeight(_blinkKey, Mathf.Max(bl, br));

        exp.SetWeight(_aaKey, a);
        exp.SetWeight(_happyKey, j);
        exp.SetWeight(_angryKey, angry);
        exp.SetWeight(_sorrowKey, sad);
        // Apply는 Vrm10Instance가 LateUpdate에서 알아서 처리
    }
}
