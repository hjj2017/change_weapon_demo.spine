using System.Collections;

using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools; // XXX 注意: 这里有个引用

using UnityEngine;

public class DemoScene : MonoBehaviour
{
    /// <summary>
    /// Start
    /// </summary>
    /// <returns>枚举迭代器</returns>
    IEnumerator Start()
    {
        yield return 1;

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

        // 等待 2 秒
        yield return new WaitForSeconds(2);

        // 加载武器图片
        Sprite spriteWeapon = Resources.Load<Sprite>("Weaponz/Weapon_1002_");

        Material materialTemp = new Material(Shader.Find("Spine/Skeleton"));
        materialTemp.mainTexture = spriteWeapon.texture;
        rWeapon = spriteWeapon.ToRegionAttachment(materialTemp);

        spAnim.Skeleton.FindSlot("Slot_RWeapon").Attachment = rWeapon;

        yield return new WaitForSeconds(2);

        Skin weaponeSkin = new Skin("NewWeapon");
        weaponeSkin.SetAttachment(2048, "X", rWeapon);

        Skin defaultSkin = spAnim.Skeleton.Data.FindSkin("default");

        Skin newSkin = new Skin("Mixed");
        newSkin.AddSkin(weaponeSkin);
        newSkin.AddSkin(defaultSkin);

        // 注意: 这里是一步优化操作, 将用到的图片打包到一个新的图片合集中.
        // 这样可以降低 DrawCall 次数
        Spine.Skin repackedSkin = newSkin.GetRepackedSkin(
            "RepackedSkin",
            spAnim.SkeletonDataAsset.atlasAssets[0].PrimaryMaterial,
            out var _,
            out var _
        );

        spAnim.Skeleton.SetSkin(repackedSkin);
        spAnim.skeleton.UpdateCache();
        spAnim.skeleton.SetSlotsToSetupPose();

        yield return new WaitForSeconds(2);

        spAnim.Skeleton.FindSlot("Slot_RWeapon").Attachment = repackedSkin.GetAttachment(2048, "X");
    }
}
