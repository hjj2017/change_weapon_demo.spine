using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools; // XXX 注意: 这里有个引用

using UnityEngine;

public class DemoScene : MonoBehaviour
{
    void Start()
    {
        // 只加载 Spine 的合图资源,
        // 武器没有导入骨骼动画
        SpineAtlasAsset spAtlasAsset = Resources.Load<SpineAtlasAsset>("Weaponz/Spine/Weapon_1xxx__Atlas");
        Atlas spAtlas = spAtlasAsset.GetAtlas();

        // 查找编号 = 1001 这个武器在合图中的区域位置
        AtlasRegion atlasReg = spAtlas.FindRegion("img/Weapon_1001_");

        // 创建附件,
        // XXX 注意: 要成功调用 ToRegionAttachment 函数,
        // 必须先引用 Spine.Unity.AttachmentTools
        RegionAttachment rWeapon = atlasReg.ToRegionAttachment(atlasReg.name);

        // 获取主角的 Spine 骨骼动画
        SkeletonAnimation spAnim = GameObject.Find("/Character_1002_").GetComponent<SkeletonAnimation>();
        // 查找右手武器插槽, 并替换其中的附件
        spAnim.Skeleton.FindSlot("Slot_RWeapon").Attachment = rWeapon;
    }
}
