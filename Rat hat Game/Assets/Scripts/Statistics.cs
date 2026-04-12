using System.Collections.Generic;
using UnityEngine;

// Keeps track of all saved variables
public static class Statistics
{ 

    // Current Level and Stage
    public static int CurrentLevel {get; set;} = 1;
    public static int CurrentStage {get; set;} = 0;

    public static int Levels {get; set;} = 6;

    public static float MoonScale = 1f;      // Persists across levels

}