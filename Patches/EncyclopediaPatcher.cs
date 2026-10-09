using HarmonyLib;

namespace EncyclopediaPlus.Patches;


[HarmonyPatch(typeof(Encyclopedia), nameof(EncyclopediaPage.RefreshUI))]
internal class EncyclopediaPatcher
{
    public static void Postfix(Encyclopedia __instance)
    {
        int totalFishCaught = 0;
        __instance.currentFishList.ForEach(f =>
        {
            totalFishCaught += GameManager.Instance.SaveData.GetCaughtCountById(f.id);
        });

        UnityEngine.Transform discoveryCounter = __instance.transform.Find("DiscoveryCounter");

        // append total caught to discovery count text
        discoveryCounter.Find("DiscoveryCountText").GetComponent<TMPro.TextMeshProUGUI>().text += $"     Total Caught: {totalFishCaught}";

        // resize + reposition discovery count background image
        UnityEngine.RectTransform rect = discoveryCounter.GetComponent<UnityEngine.UI.Image>().rectTransform;
        rect.sizeDelta = new UnityEngine.Vector2(750, 60);
        rect.transform.SetLocalPositionX(20);
    }   
}
