using UnityEngine;
using UniVRM10;

public class ExpressionController : MonoBehaviour
{
    private Vrm10Instance _instance;
    private ExpressionKey _neutralKey;
    private ExpressionKey _happyKey;
    private ExpressionKey _angryKey;
    private ExpressionKey _sadKey;
    private ExpressionKey _blinkKey;
    private ExpressionKey _blink_L_Key;
    private ExpressionKey _blink_R_Key;

    private ExpressionKey _aaKey;

    private ExpressionKey _testExpKey;


    void Awake()
    {
        _instance = GetComponent<Vrm10Instance>();
        if (_instance == null)
        {
            Debug.LogError("Vrm10Instance 못 찾음");
            return;
        }

        _neutralKey = ExpressionKey.Neutral;
        _happyKey = ExpressionKey.Happy;
        _angryKey = ExpressionKey.Angry;
        _sadKey = ExpressionKey.Sad;
        _blinkKey = ExpressionKey.Blink;
        _blink_L_Key = ExpressionKey.BlinkLeft;
        _blink_R_Key = ExpressionKey.BlinkRight;

        _aaKey = ExpressionKey.Aa;

        _testExpKey = ExpressionKey.CreateCustom("TestExp");
    }

    void Update()
    {
        if (_instance == null || _instance.Runtime == null) return;

        var exp = _instance.Runtime.Expression;

        // 기본값
        exp.SetWeight(_neutralKey, 1.0f);
        exp.SetWeight(_happyKey, 0.0f);
        exp.SetWeight(_angryKey, 0.0f);
        exp.SetWeight(_sadKey, 0.0f);
        exp.SetWeight(_blinkKey, 0.0f);
        exp.SetWeight(_blink_L_Key, 0.0f);
        exp.SetWeight(_blink_R_Key, 0.0f);
        exp.SetWeight(_aaKey, 0.0f);

        exp.SetWeight(_testExpKey, 0.0f);


        // 1키: 웃는 표정
        if (Input.GetKey(KeyCode.Alpha1))
        {
            exp.SetWeight(_happyKey, 1.0f);
        }

        // 2키: 화난 표정
        if (Input.GetKey(KeyCode.Alpha2))
        {
            exp.SetWeight(_angryKey, 1.0f);
        }

        // 3키: 슬픈 표정
        if (Input.GetKey(KeyCode.Alpha3))
        {
            exp.SetWeight(_sadKey, 1.0f);
        }

        // 스페이스바 : 눈 깜빡임
        if (Input.GetKey(KeyCode.Space))
        {
            exp.SetWeight(_blinkKey, 1.0f);
        }

        // 마우스 왼쪽 버튼 : 왼쪽 눈 깜빡임
        if (Input.GetMouseButton(0))
        {
            exp.SetWeight(_blink_L_Key, 1.0f);
        }

        // 마우스 오른쪽 버튼 : 오른쪽 눈 깜빡임
        if (Input.GetMouseButton(1))
        {
            exp.SetWeight(_blink_R_Key, 1.0f);
        }

        // A : 입 "아"
        if (Input.GetKey(KeyCode.A))
        {            
            exp.SetWeight(_aaKey, 1.0f);
        }

        if (Input.GetKey(KeyCode.T))
        {
            exp.SetWeight(_testExpKey, 1.0f);
        }
    }
}
