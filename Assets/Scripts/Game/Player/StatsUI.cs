using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
   public GameObject[] statsSlots;

   private void Start()
   {
    UpdateStatsUI();
   }

   public void UpdateStatsUI()
   {
    statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Damage: " + PlayerStats.Instance.Damage.ToString();
   }
}
