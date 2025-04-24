using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI winText;

    public void CheckWinCondition()
    {
        GameObject[] goals = GameObject.FindGameObjectsWithTag("Goal");
        GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");

        int boxesOnGoals = 0;

        foreach (GameObject goal in goals)
        {
            foreach (GameObject box in boxes)
            {
                float dist = Vector3.Distance(goal.transform.position, box.transform.position);
                if (dist < 0.1f) // Margen de error permitido
                {
                    boxesOnGoals++;
                    break;
                }
            }
        }

        if (boxesOnGoals == goals.Length)
        {
            Debug.Log("¡Ganaste!");
            if (winText != null)
            {
                winText.gameObject.SetActive(true);
                winText.text = "¡GANASTE!";
            }
        }
    }
}
