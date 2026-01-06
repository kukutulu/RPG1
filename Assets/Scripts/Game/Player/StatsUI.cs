using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
   public GameObject[] statsSlots;
   public CanvasGroup canvasGroup;

   private void Start()
   {
    UpdateStatsUI();
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Tab))
      {
         UpdateStatsUI();
         canvasGroup.alpha = 1;
      } 

      if(Input.GetKeyUp(KeyCode.Tab))
      {
         canvasGroup.alpha = 0;
      }
   }

   public void UpdateStatsUI()
   {
    statsSlots[0].GetComponentInChildren<TMP_Text>().text = "Damage: " + PlayerStats.Instance.Damage.ToString();
    statsSlots[1].GetComponentInChildren<TMP_Text>().text = "Speed: " + PlayerStats.Instance.Speed.ToString();
    statsSlots[2].GetComponentInChildren<TMP_Text>().text = "Range: " + PlayerStats.Instance.WeaponRange.ToString();
    statsSlots[3].GetComponentInChildren<TMP_Text>().text = "Health: " + PlayerStats.Instance.Health.ToString();
   }
}
