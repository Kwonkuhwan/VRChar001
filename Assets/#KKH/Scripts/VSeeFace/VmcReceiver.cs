using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

/// <summary>
/// VSeeFace VMC(OSC) 수신 + /VMC/Ext/Blend/Val 파싱 전용 리시버
/// </summary>
public class VmcReceiver : MonoBehaviour
{
    [Header("VSeeFace VMC 포트 (VSeeFace 설정과 동일)")]
    public int listenPort = 39539;

    private UdpClient _udp;
    private Thread _thread;
    private bool _running;

    // blendName -> value
    private readonly ConcurrentDictionary<string, float> _blendDict =
        new ConcurrentDictionary<string, float>();

    [Header("디버그용 값 (Inspector에서 확인용)")]
    public float blinkL;
    public float blinkR;
    public float mouthA;
    public float mouthI;
    public float mouthU;
    public float mouthE;
    [Space(10)]
    public float neutra;
    public float happy;
    public float angry;
    public float fun;
    public float sorrow;
    public float surprised;
    [Space(10)]
    public float lookUp;
    public float lookDown;
    public float lookLeft;
    public float lookRight;


    public struct PoseData
    {
        public Vector3 position;
        public Quaternion rotation;
    }

    [Header("루트 포즈 (Inspector에서 확인용)")]
    public PoseData _rootPose;
    private readonly ConcurrentDictionary<string, PoseData> _bonePoses =
        new ConcurrentDictionary<string, PoseData>();

    void Start()
    {
        try
        {
            _udp = new UdpClient(listenPort);
            _udp.Client.ReceiveTimeout = 1000;
            _running = true;

            _thread = new Thread(ReceiveLoop);
            _thread.IsBackground = true;
            _thread.Start();

            Debug.Log($"[VMC] Listening on port {listenPort}");
        }
        catch (Exception e)
        {
            Debug.LogError("[VMC] Udp start error: " + e);
        }
    }

    void Update()
    {
        // 일단 이렇게 Inspector에서 보면서 어떤 값이 오는지 확인
#if UNITY_EDITOR
        blinkL = GetBlend("Blink_L");     // 또는 나중에 EyeBlinkLeft 등으로 교체
        blinkR = GetBlend("Blink_R");

        mouthA = GetBlend("A");           // 입 "아"
        mouthI = GetBlend("I");           // 입 "이"
        mouthU = GetBlend("U");           // 입 "우"
        mouthE = GetBlend("E");           // 입 "에"

        neutra = GetBlend("Neutral");     // 기본
        happy = GetBlend("Joy");         // 웃음
        angry = GetBlend("Angry");       // 화남
        fun = GetBlend("Fun");           // 즐거움
        sorrow = GetBlend("Sorrow");     // 슬픔
        surprised = GetBlend("Surprised"); // 놀람

        lookUp = GetBlend("Look_Up");         // 시선 위
        lookDown = GetBlend("Look_Down");     // 시선 아래
        lookLeft = GetBlend("Look_Left");     // 시선 왼쪽
        lookRight = GetBlend("Look_Right");   // 시선 오른쪽
#endif
    }


    void OnDestroy()
    {
        _running = false;

        try { _udp?.Close(); } catch { }

        if (_thread != null && _thread.IsAlive)
        {
            try { _thread.Abort(); } catch { }
        }
    }

    private void ReceiveLoop()
    {
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, listenPort);

        while (_running)
        {
            try
            {
                var data = _udp.Receive(ref ep);
                int offset = 0;
                ParsePacket(data, ref offset, data.Length);
            }
            catch (SocketException)
            {
                // timeout → 무시
            }
            catch (ThreadAbortException)
            {
                return;
            }
            catch (Exception e)
            {
                Debug.LogWarning("[VMC] recv error: " + e.Message);
            }
        }
    }

    // 패킷이 번들이든 단일 메시지든 처리
    private void ParsePacket(byte[] data, ref int offset, int length)
    {
        int tmp = offset;
        string head = ReadOscString(data, ref tmp);

        if (head == "#bundle")
        {
            // 번들 모드
            offset = tmp;
            offset += 8; // timetag

            while (offset + 4 <= length)
            {
                int size = ReadInt32BE(data, ref offset);
                int end = Mathf.Min(offset + size, length);
                ParseMessage(data, ref offset, end);
                offset = end;
            }
        }
        else
        {
            // 단일 메시지 모드
            // 주소를 한 번 더 읽게 두고, offset은 처음으로
            offset = 0;
            ParseMessage(data, ref offset, length, null);
            // 또는: offset = tmp; ParseMessage(..., head); 위에서 설명한 방식 둘 중 하나 택
        }
    }


    private void ParseMessage(byte[] data, ref int offset, int end, string firstAddress = null)
    {
        if (offset >= end) return;

        string address = firstAddress ?? ReadOscString(data, ref offset);
        string typeTags = ReadOscString(data, ref offset); // 예: ",sf"

        if (address == "/VMC/Ext/Blend/Val")
        {
            // arg: 이름(string)
            string name = ReadOscString(data, ref offset);
            // arg: 값(float)
            float value = ReadOscFloat(data, ref offset);

            _blendDict[name] = value;
            //Debug.Log($"{name}");

//#if UNITY_EDITOR
//            // 처음 보는 키는 한 번 찍어보자 (어떤 이름이 오는지 확인용)
//            if (!_loggedNames.Contains(name))
//            {
//                _loggedNames.Add(name);
//            }
//            // Debug.Log($"[VMC Blend] {name} = {value}");
//#endif
        }
        else if (address == "/VMC/Ext/Root/Pos")
        {
            string rootName = ReadOscString(data, ref offset);
            float px = ReadOscFloat(data, ref offset);
            float py = ReadOscFloat(data, ref offset);
            float pz = ReadOscFloat(data, ref offset);
            float qx = ReadOscFloat(data, ref offset);
            float qy = ReadOscFloat(data, ref offset);
            float qz = ReadOscFloat(data, ref offset);
            float qw = ReadOscFloat(data, ref offset);

            _rootPose.position = new Vector3(px, py, pz);
            _rootPose.rotation = new Quaternion(qx, qy, qz, qw);

            //Debug.Log($"[VMC Root Pos] |  {data} : {_rootPose.position}");
            //Debug.Log($"[VMC Root Rot] |  {data} : {_rootPose.rotation}");
        }
        else if (address == "/VMC/Ext/Bone/Pos")
        {
            string boneName = ReadOscString(data, ref offset);

            float px = ReadOscFloat(data, ref offset);
            float py = ReadOscFloat(data, ref offset);
            float pz = ReadOscFloat(data, ref offset);
            float qx = ReadOscFloat(data, ref offset);
            float qy = ReadOscFloat(data, ref offset);
            float qz = ReadOscFloat(data, ref offset);
            float qw = ReadOscFloat(data, ref offset);

            var pose = new PoseData
            {
                position = new Vector3(px, py, pz),
                rotation = new Quaternion(qx, qy, qz, qw)
            };
            _bonePoses[boneName] = pose;

            //Debug.Log($"[VMC Bone Pos] | {boneName}");
            //Debug.Log($"[VMC Bone Rot]| {boneName}");
            //Debug.Log($"public string {boneName} = \"{boneName}\";");
            //if(boneName == "LeftEye" || boneName == "RightEye")
            //{
            //    Debug.Log($"[VMC Eye Pos] | {boneName} : {pose.position}");
            //    Debug.Log($"[VMC Eye Rot]| {boneName} : {pose.rotation}");
            //}
            //Debug.Log($"{boneName}");
        }
        else
        {
            // 우리가 안 쓰는 메시지면 인자 스킵
            SkipArguments(typeTags, data, ref offset, end);
        }
    }

    // ==== OSC 헬퍼들 ====

    private readonly System.Collections.Generic.HashSet<string> _loggedNames =
        new System.Collections.Generic.HashSet<string>();

    private string ReadOscString(byte[] data, ref int offset)
    {
        int start = offset;
        while (offset < data.Length && data[offset] != 0) offset++;

        int len = offset - start;
        string s = Encoding.UTF8.GetString(data, start, len);

        // null byte
        if (offset < data.Length) offset++;

        // 4바이트 align
        while (offset % 4 != 0 && offset < data.Length) offset++;

        return s;
    }

    private int ReadInt32BE(byte[] data, ref int offset)
    {
        if (offset + 4 > data.Length) return 0;
        int value =
            (data[offset] << 24) |
            (data[offset + 1] << 16) |
            (data[offset + 2] << 8) |
             data[offset + 3];
        offset += 4;
        return value;
    }

    private float ReadOscFloat(byte[] data, ref int offset)
    {
        if (offset + 4 > data.Length) return 0f;
        byte[] tmp = new byte[4];
        Array.Copy(data, offset, tmp, 0, 4);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(tmp);
        }
        float f = BitConverter.ToSingle(tmp, 0);
        offset += 4;
        return f;
    }

    private void SkipArguments(string typeTags, byte[] data, ref int offset, int end)
    {
        if (string.IsNullOrEmpty(typeTags) || typeTags[0] != ',') return;

        for (int i = 1; i < typeTags.Length && offset < end; i++)
        {
            char t = typeTags[i];
            switch (t)
            {
                case 'i': // int32
                case 'f': // float
                    offset += 4;
                    break;
                case 'h': // int64
                case 'd': // double
                    offset += 8;
                    break;
                case 's': // string
                case 'S':
                    ReadOscString(data, ref offset);
                    break;
                default:
                    break;
            }
        }
    }

    // ==== 포즈 관련 ====
    public bool TryGetRootPose(out PoseData pose)
    {
        pose = _rootPose;
        return true; // 루트는 항상 있다고 가정
    }

    public bool TryGetBonePose(string name, out PoseData pose)
    {
        return _bonePoses.TryGetValue(name, out pose);
    }

    // ==== Unity에서 쓰기 쉽게 꺼내기 ====

    public float GetBlend(string name)
    {
        if (_blendDict.TryGetValue(name, out var v))
            return v;
        return 0f;
    }
}