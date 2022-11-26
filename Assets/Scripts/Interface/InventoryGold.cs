using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BON
{
    public class InventoryGold : MonoBehaviour
    {
        public TextMeshProUGUI goldCountText;
        private PlayerStats playerStats;

        private void Awake()
        {
            playerStats = FindObjectOfType<PlayerStats>();
        }
        public void Update()
        {
            SetGoldCountText(playerStats.gold_Current);
        }

        public void SetGoldCountText(int _goldCount)
        {
            goldCountText.text = _goldCount.ToString();
        }
    }
}