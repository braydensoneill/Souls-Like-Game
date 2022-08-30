using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BON
{
    public class ColourCodes : MonoBehaviour
    {
        private static string item_Poor = "#9d9d9d";        // Grey
        private static string item_Common = "#ffffff";      // White
        private static string item_Uncommon = "#1eff00";    // Green
        private static string item_Rare = "#0070dd";        // Blue
        private static string item_Epic = "#a335ee";        // Purple
        private static string item_Legendary = "#ff8000";   // Orange

        public string GetItemPoor()
        {
            return item_Poor;
        }

        public string GetItemCommon()
        {
            return item_Common;
        }

        public string GetItemUncommon()
        {
            return item_Uncommon;
        }

        public string GetItemRare()
        {
            return item_Rare;
        }

        public string GetItemEpic()
        {
            return item_Epic;
        }

        public string GetItemLegenary()
        {
            return item_Legendary;
        }
    }
}
