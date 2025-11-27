using System;
using System.Collections.Generic;
using UnityEngine;
using UniVRM10;

[Serializable]
public class BoneBinding
{
    [Tooltip("VSeeFace / VMC에서 오는 본 이름")]
    public string vmcName;

    [Tooltip("VRM 캐릭터의 대응 Transform")]
    public Transform target;
    public Quaternion rot;
    public Vector3 rotvec;

    public bool applyPosition = false;  // 보통 루트/발만 ON
    public bool applyRotation = true;   // 대부분 회전만 따라가게
}

[RequireComponent(typeof(Vrm10Instance))]
public class Vrm10VmcMotionDriver : MonoBehaviour
{
    public VmcReceiver vmc;

    //[Header("매핑할 본들")]
    //public Transform rootTransform;  // hips/root
    //public Transform headTransform;  // head

    [Header("본 매핑 리스트")]
    public List<BoneBinding> boneBindings = new List<BoneBinding>();

    Vrm10Instance _vrm;

    void Awake()
    {
        _vrm = GetComponent<Vrm10Instance>();
        //if (rootTransform == null)
        //{
        //    // 보통 hips를 root로 씀
        //    var animator = GetComponent<Animator>();
        //    if (animator != null)
        //    {
        //        rootTransform = animator.GetBoneTransform(HumanBodyBones.Hips);
        //        headTransform = animator.GetBoneTransform(HumanBodyBones.Head);
        //    }
        //}

        var animator = GetComponent<Animator>();
        if (animator == null) return;

        #region boneBindings
        boneBindings = new List<BoneBinding>
    {
new BoneBinding {
            vmcName = "Hips",
            target = animator.GetBoneTransform(HumanBodyBones.Hips),
            applyPosition = true,
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftUpperLeg",
            target = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightUpperLeg",
            target = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftLowerLeg",
            target = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightLowerLeg",
            target = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftFoot",
            target = animator.GetBoneTransform(HumanBodyBones.LeftFoot),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightFoot",
            target = animator.GetBoneTransform(HumanBodyBones.RightFoot),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "Spine",
            target = animator.GetBoneTransform(HumanBodyBones.Spine),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "Chest",
            target = animator.GetBoneTransform(HumanBodyBones.Chest),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "Neck",
            target = animator.GetBoneTransform(HumanBodyBones.Neck),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "Head",
            target = animator.GetBoneTransform(HumanBodyBones.Head),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftShoulder",
            target = animator.GetBoneTransform(HumanBodyBones.LeftShoulder),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightShoulder",
            target = animator.GetBoneTransform(HumanBodyBones.RightShoulder),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftUpperArm",
            target = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightUpperArm",
            target = animator.GetBoneTransform(HumanBodyBones.RightUpperArm),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftLowerArm",
            target = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightLowerArm",
            target = animator.GetBoneTransform(HumanBodyBones.RightLowerArm),
            applyRotation = true
        },

new BoneBinding {
            vmcName = "LeftHand",
            target = animator.GetBoneTransform(HumanBodyBones.LeftHand),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightHand",
            target = animator.GetBoneTransform(HumanBodyBones.RightHand),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftToes",
            target = animator.GetBoneTransform(HumanBodyBones.LeftToes),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightToes",
            target = animator.GetBoneTransform(HumanBodyBones.RightToes),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftEye",
            target = animator.GetBoneTransform(HumanBodyBones.LeftEye),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightEye",
            target = animator.GetBoneTransform(HumanBodyBones.RightEye),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftThumbProximal",
            target = animator.GetBoneTransform(HumanBodyBones.LeftThumbProximal),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftThumbIntermediate",
            target = animator.GetBoneTransform(HumanBodyBones.LeftThumbIntermediate),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftThumbDistal",
            target = animator.GetBoneTransform(HumanBodyBones.LeftThumbDistal),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftIndexProximal",
            target = animator.GetBoneTransform(HumanBodyBones.LeftIndexProximal),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftIndexIntermediate",
            target = animator.GetBoneTransform(HumanBodyBones.LeftIndexIntermediate),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftIndexDistal",
            target = animator.GetBoneTransform(HumanBodyBones.LeftIndexDistal),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftMiddleProximal",
            target = animator.GetBoneTransform(HumanBodyBones.LeftMiddleProximal),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftMiddleIntermediate",
            target = animator.GetBoneTransform(HumanBodyBones.LeftMiddleIntermediate),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftMiddleDistal",
            target = animator.GetBoneTransform(HumanBodyBones.LeftMiddleDistal),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftRingProximal",
            target = animator.GetBoneTransform(HumanBodyBones.LeftRingProximal),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftRingIntermediate",
            target = animator.GetBoneTransform(HumanBodyBones.LeftRingIntermediate),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftRingDistal",
            target = animator.GetBoneTransform(HumanBodyBones.LeftRingDistal),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftLittleProximal",
            target = animator.GetBoneTransform(HumanBodyBones.LeftLittleProximal),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftLittleIntermediate",
            target = animator.GetBoneTransform(HumanBodyBones.LeftLittleIntermediate),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "LeftLittleDistal",
            target = animator.GetBoneTransform(HumanBodyBones.LeftLittleDistal),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightThumbProximal",
            target = animator.GetBoneTransform(HumanBodyBones.RightThumbProximal),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightThumbIntermediate",
            target = animator.GetBoneTransform(HumanBodyBones.RightThumbIntermediate),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightThumbDistal",
            target = animator.GetBoneTransform(HumanBodyBones.RightThumbDistal),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightIndexProximal",
            target = animator.GetBoneTransform(HumanBodyBones.RightIndexProximal),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightIndexIntermediate",
            target = animator.GetBoneTransform(HumanBodyBones.RightIndexIntermediate),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightIndexDistal",
            target = animator.GetBoneTransform(HumanBodyBones.RightIndexDistal),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightMiddleProximal",
            target = animator.GetBoneTransform(HumanBodyBones.RightMiddleProximal),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightMiddleIntermediate",
            target = animator.GetBoneTransform(HumanBodyBones.RightMiddleIntermediate),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightMiddleDistal",
            target = animator.GetBoneTransform(HumanBodyBones.RightMiddleDistal),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightRingProximal",
            target = animator.GetBoneTransform(HumanBodyBones.RightRingProximal),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightRingIntermediate",
            target = animator.GetBoneTransform(HumanBodyBones.RightRingIntermediate),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightRingDistal",
            target = animator.GetBoneTransform(HumanBodyBones.RightRingDistal),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightLittleProximal",
            target = animator.GetBoneTransform(HumanBodyBones.RightLittleProximal),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightLittleIntermediate",
            target = animator.GetBoneTransform(HumanBodyBones.RightLittleIntermediate),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "RightLittleDistal",
            target = animator.GetBoneTransform(HumanBodyBones.RightLittleDistal),
            applyRotation = true
        },
new BoneBinding {
            vmcName = "UpperChest",
            target = animator.GetBoneTransform(HumanBodyBones.UpperChest),
            applyRotation = true
        },
        // 팔/다리도 같은 방식으로 추가…
    };
        #endregion
    }

    void LateUpdate()
    {
        if (vmc == null || _vrm == null || _vrm.Runtime == null) return;

        //// 1. 루트 위치/회전
        //if (vmc.TryGetRootPose(out var rootPose) && rootTransform != null)
        //{
        //    // 좌표계 차이 때문에 실제 프로젝트에서는 축을 좀 바꿔줘야 할 수 있음
        //    rootTransform.position = rootPose.position;
        //    rootTransform.rotation = rootPose.rotation;
        //}

        //// 2. 헤드 회전만 따라가게 (위치는 루트/애니메이션에 맡기고)
        //if (headTransform != null && vmc.TryGetBonePose(headBoneName, out var headPose))
        //{
        //    headTransform.rotation = headPose.rotation;
        //}

        foreach (var b in boneBindings)
        {
            if (b.target == null || string.IsNullOrEmpty(b.vmcName))
                continue;

            if (!vmc.TryGetBonePose(b.vmcName, out var pose))
                continue;

            if (b.applyPosition)
                b.target.position = pose.position;

            if (b.applyRotation)
            {
                b.target.localRotation = pose.rotation;
            }
        }
    }
}
