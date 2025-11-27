using UnityEngine;
using UniVRM10;

[RequireComponent(typeof(Vrm10Instance))]
public class Vrm10VmcFaceDriver : MonoBehaviour
{
    public VmcReceiver vmc; // VmcReceiver 오브젝트 드래그

    Vrm10Instance _instance;

    ExpressionKey _blinkLKey, _blinkRKey, _blinkKey;
    ExpressionKey _aaKey, _ihKey, _ouKey, _eeKey, _ohKey;
    ExpressionKey _neutralKey, _happyKey, _angryKey, _funKey, _sorrowKey, _surprisedKey;
    ExpressionKey _lookUpKey, _lookDownKey, _lookLeftKey, _lookRightKey;
       
    void Awake()
    {
        _instance = GetComponent<Vrm10Instance>();
        _blinkLKey = ExpressionKey.BlinkLeft;
        _blinkRKey = ExpressionKey.BlinkRight;
        _blinkKey = ExpressionKey.Blink;

        _aaKey = ExpressionKey.Aa;
        _ihKey = ExpressionKey.Ih;
        _ouKey = ExpressionKey.Ou;
        _eeKey = ExpressionKey.Ee;
        _ohKey = ExpressionKey.Oh;

        _neutralKey = ExpressionKey.Neutral;
        _happyKey = ExpressionKey.Happy;
        _angryKey = ExpressionKey.Angry;
        _sorrowKey = ExpressionKey.Sad;
        _funKey = ExpressionKey.Relaxed;
        _surprisedKey = ExpressionKey.Surprised;

        _lookUpKey = ExpressionKey.LookUp;
        _lookDownKey = ExpressionKey.LookDown;
        _lookLeftKey = ExpressionKey.LookLeft;
        _lookRightKey = ExpressionKey.LookRight;
    }

    private void ResetWeight()
    {
        // 기본 0
        var exp = _instance.Runtime.Expression;

        exp.SetWeight(_blinkLKey, 0);
        exp.SetWeight(_blinkRKey, 0);
        exp.SetWeight(_blinkKey, 0);

        exp.SetWeight(_aaKey, 0);
        exp.SetWeight(_ihKey, 0);
        exp.SetWeight(_ouKey, 0);
        exp.SetWeight(_eeKey, 0);

        exp.SetWeight(_neutralKey, 0);
        exp.SetWeight(_happyKey, 0);
        exp.SetWeight(_angryKey, 0);
        exp.SetWeight(_sorrowKey, 0);
        exp.SetWeight(_funKey, 0);
        exp.SetWeight(_surprisedKey, 0);

        exp.SetWeight(_lookUpKey, 0);
        exp.SetWeight(_lookDownKey, 0);
        exp.SetWeight(_lookLeftKey, 0);
        exp.SetWeight(_lookRightKey, 0);
    }

    private void SetWeigh()
    {
        ResetWeight();

        var exp = _instance.Runtime.Expression;

        exp.SetWeight(_blinkLKey, vmc.GetBlend("Blink_L"));     // 왼쪽 눈 깜빡임
        exp.SetWeight(_blinkRKey, vmc.GetBlend("Blink_R"));     // 오른쪽 눈 깜빡임
        //exp.SetWeight(_blinkKey, Mathf.Max(vmc.GetBlend("Blink_L"), vmc.GetBlend("Blink_R")));  // 양쪽 눈 깜빡임

        exp.SetWeight(_aaKey, vmc.GetBlend("A"));               // 입 "아"
        exp.SetWeight(_ihKey, vmc.GetBlend("I"));               // 입 "이"
        exp.SetWeight(_ouKey, vmc.GetBlend("U"));               // 입 "우"
        exp.SetWeight(_eeKey, vmc.GetBlend("E"));               // 입 "에"
        exp.SetWeight(_ohKey, vmc.GetBlend("O"));               // 입 "오"

        exp.SetWeight(_neutralKey, vmc.GetBlend("Neutral"));    // 기본
        exp.SetWeight(_happyKey, vmc.GetBlend("Joy"));          // 행복
        exp.SetWeight(_angryKey, vmc.GetBlend("Angry"));        // 화남
        exp.SetWeight(_sorrowKey, vmc.GetBlend("Sorrow"));      // 슬픔
        exp.SetWeight(_funKey, vmc.GetBlend("Fun"));         // 즐거움
        exp.SetWeight(_surprisedKey, vmc.GetBlend("Surprised"));         // 놀람

        exp.SetWeight(_lookUpKey, vmc.GetBlend("Look_Up"));         // 시선 위
        exp.SetWeight(_lookDownKey, vmc.GetBlend("Look_Down"));     // 시선 아래
        exp.SetWeight(_lookLeftKey, vmc.GetBlend("Look_Left"));     // 시선 왼쪽
        exp.SetWeight(_lookRightKey, vmc.GetBlend("Look_Right"));   // 시선 오른쪽
    }

    void Update()
    {
        if (_instance == null || _instance.Runtime == null || vmc == null) return;
        
        SetWeigh();
    }
}
