using UnityEngine;
using UniVRM10;

[RequireComponent(typeof(Vrm10Instance))]
public class Vrm10VmcMotionDriver : MonoBehaviour
{
    public VmcReceiver vmc;

    [Header("매핑할 본들")]
    public Transform rootTransform;  // hips/root
    public Transform headTransform;  // head

    public string rootBoneName = "Root"; // VMC에서 오는 이름
    public string headBoneName = "Head";

    Vrm10Instance _vrm;

    void Awake()
    {
        _vrm = GetComponent<Vrm10Instance>();
        if (rootTransform == null)
        {
            // 보통 hips를 root로 씀
            var animator = GetComponent<Animator>();
            if (animator != null)
            {
                rootTransform = animator.GetBoneTransform(HumanBodyBones.Hips);
                headTransform = animator.GetBoneTransform(HumanBodyBones.Head);
            }
        }
    }

    void LateUpdate()
    {
        if (vmc == null || _vrm == null || _vrm.Runtime == null) return;

        // 1. 루트 위치/회전
        if (vmc.TryGetRootPose(out var rootPose) && rootTransform != null)
        {
            // 좌표계 차이 때문에 실제 프로젝트에서는 축을 좀 바꿔줘야 할 수 있음
            rootTransform.position = rootPose.position;
            rootTransform.rotation = rootPose.rotation;
        }

        // 2. 헤드 회전만 따라가게 (위치는 루트/애니메이션에 맡기고)
        if (headTransform != null && vmc.TryGetBonePose(headBoneName, out var headPose))
        {
            headTransform.rotation = headPose.rotation;
        }
    }
}
