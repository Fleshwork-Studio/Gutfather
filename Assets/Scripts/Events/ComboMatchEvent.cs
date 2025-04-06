using System;
using System.Collections.Generic;
using UnityEngine;

public struct ComboMatchEvent
{
    public MatchComboController.MatchType matchType; // Type of the match
    public GemTypeSO gemType;
    public List<Vector2Int> gemPositions; // List of all gems in this match
}