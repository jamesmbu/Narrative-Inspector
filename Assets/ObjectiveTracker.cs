using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveTracker : MonoBehaviour
{
    public int TalliedObjectives = 0;
    public int TotalObjectives = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TallyObjective()
    {
        TalliedObjectives += 1;
        if (AllObjectivesMet(TalliedObjectives))
        {
            Debug.Log("Objectives complete");
        }
    }

    private bool AllObjectivesMet(int TalliedAmount)
    {
        return TalliedAmount >= TotalObjectives;
    }
}
