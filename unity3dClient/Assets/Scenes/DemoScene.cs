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
        SkeletonAnimation spAnim = GameObject.Find("/Character_X_").GetComponent<SkeletonAnimation>();
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
        weaponeSkin.SetAttachment(1, "X", rWeapon);

        Skin defaultSkin = spAnim.Skeleton.Data.FindSkin("_Default");

        Skin newSkin = new Skin("Mixed");
        newSkin.AddSkin(defaultSkin);
        newSkin.AddSkin(weaponeSkin);

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

        spAnim.Skeleton.FindSlot("Slot_RWeapon").Attachment = repackedSkin.GetAttachment(1, "X");

        yield return new WaitForSeconds(2);

        ReplaceBothLeg();
    }

    private void ReplaceBothLeg()
    {
        // 只加载 Spine 的合图资源,
        // 武器没有导入骨骼动画
        SpineAtlasAsset spAtlasAsset = Resources.Load<SpineAtlasAsset>("Characterz/Character_10002_/Spine/Character_10002__Atlas");
        Atlas spAtlas = spAtlasAsset.GetAtlas();

        AtlasRegion atlasReg1 = spAtlas.FindRegion("LLeg_1_ZhiLi");
        AtlasRegion atlasReg2 = spAtlas.FindRegion("LLeg_2_PaoBu");

        RegionAttachment raLLeg1 = atlasReg1.ToRegionAttachment(atlasReg1.name);
        RegionAttachment raLLeg2 = atlasReg1.ToRegionAttachment(atlasReg2.name);

        SkeletonAnimation spAnim = GameObject.Find("/Character_X_").GetComponent<SkeletonAnimation>();
        SlotData slotLLeg = spAnim.Skeleton.Data.FindSlot("Slot_LLeg");

        Skin skinLeg = new Skin("NewLeg");
        skinLeg.SetAttachment(slotLLeg.Index, slotLLeg.AttachmentName, raLLeg1);
        //skinLeg.SetAttachment(slotLLeg.Index, "LLeg_2", raLLeg2);

        // 获取主角的 Spine 骨骼动画
        Skin skinDefault = spAnim.Skeleton.Data.FindSkin("_Default");
        var oldReg = (skinDefault.GetAttachment(slotLLeg.Index, slotLLeg.AttachmentName) as RegionAttachment);
        skinDefault.SetAttachment(slotLLeg.Index, slotLLeg.AttachmentName, raLLeg1);
        raLLeg1.SetPositionOffset(new Vector2(oldReg.X, oldReg.Y));
        raLLeg1.SetRotation(oldReg.Rotation);
        raLLeg1.UpdateRegion();

        Skin skinMixed = new Skin("Mixed");
        skinMixed.AddSkin(skinDefault);

        Spine.Skin skinRepacked = skinMixed.GetRepackedSkin(
            "RepackedSkin2",
            spAnim.SkeletonDataAsset.atlasAssets[0].PrimaryMaterial,
            out var _,
            out var _
        );

        spAnim.Skeleton.SetSkin(skinRepacked);
        spAnim.skeleton.UpdateCache();
        spAnim.skeleton.SetSlotsToSetupPose();

        //spAnim.Skeleton.FindSlot("Slot_LLeg").Skeleton.SetSkin(skinLeg);
    }
}
