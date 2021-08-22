using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;

using UnityEngine;

/// <summary>
/// 演示场景
/// </summary>
public class DemoScene : MonoBehaviour
{
    /// <summary>
    /// Start
    /// </summary>
    void Start()
    {
        SpineAtlasAsset spAtlasAsset = Resources.Load<SpineAtlasAsset>("Weaponz/Spine/Weapon_1xxx__Atlas");
        Atlas spAtlas = spAtlasAsset.GetAtlas();

        AtlasRegion atlasReg = spAtlas.FindRegion("img/Weapon_1001_");
        RegionAttachment rWeapon = atlasReg.ToRegionAttachment(atlasReg.name);

        // 获取 Spine 骨骼
        SkeletonAnimation spAnim = GameObject.Find("/Character_1002_").GetComponent<SkeletonAnimation>();
        spAnim.Skeleton.FindSlot("Slot_RWeapon").Attachment = rWeapon;
    }
}
