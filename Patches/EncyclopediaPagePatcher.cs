using HarmonyLib;

namespace EncyclopediaPlus.Patches;


[HarmonyPatch(typeof(EncyclopediaPage), nameof(EncyclopediaPage.PopulatePage))]
internal class EncyclopediaPagePatcher
{
    private static string originalTrawlText = null;
    public static void Postfix(EncyclopediaPage __instance, FishItemData itemData)
    {
        AddTrawlDepths(__instance, itemData);
    }

    private static void AddTrawlDepths(EncyclopediaPage page, FishItemData itemData)
    {
        ToggleableTextWithBackplate text = page.catchTypeTexts[ItemSubtype.NET];
        if (originalTrawlText == null)
        {
            originalTrawlText = text.text.text;
        }
        else
        {
            text.text.text = originalTrawlText;
        }

        if (itemData.harvestableType == HarvestableType.CRAB) return;

        if (itemData.canBeCaughtByNet)
        {
            bool hasMin = itemData.hasMinDepth;
            bool hasMax = itemData.hasMaxDepth;

            if (hasMin || hasMax)
            {
                // temporarily fix DepthEnum = 6 to use GetDepthString where not intended.
                DepthEnum? prevMin = null;
                DepthEnum? prevMax = null;
                if (itemData.minDepth == (DepthEnum)6) { prevMin = itemData.minDepth; itemData.minDepth = DepthEnum.SHALLOW; }
                if (itemData.maxDepth == (DepthEnum)6) { prevMax = itemData.maxDepth; itemData.maxDepth = DepthEnum.SHALLOW; }

                string depthString = itemData.GetDepthString();

                if (prevMin != null) itemData.minDepth = (DepthEnum)prevMin;
                if (prevMax != null) itemData.maxDepth = (DepthEnum)prevMax;

                text.text.text += ": " + depthString;
            }
            else
            {
                text.text.text += ": Any Depth";
            }
        }
    }
}
