using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveTracker : MonoBehaviour
{
    public int TalliedObjectives = 0;
    public int TotalObjectives = 0;

    private SceneChanger SceneChanger;
    // Start is called before the first frame update
    void Start()
    {
        SceneChanger = GetComponent<SceneChanger>();
    }

    public void TallyObjective()
    {
        TalliedObjectives += 1;
        if (AllObjectivesMet(TalliedObjectives))
        {
            Debug.Log("Objectives complete");
            SceneChanger.FadeToScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private bool AllObjectivesMet(int TalliedAmount)
    {
        return TalliedAmount >= TotalObjectives;
    }
}
