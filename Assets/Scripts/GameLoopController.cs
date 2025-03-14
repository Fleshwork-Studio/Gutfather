using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopController : MonoBehaviour
{
    [SerializeField] InputReader inputReader;

    Match3 match3;
    MatchComboController matchComboController;

    List<Vector2Int> matches = new List<Vector2Int>();

    private bool isPlayerTurn = true;

    private void Start()
    {
        match3 = Match3.Instance;
        matchComboController = MatchComboController.Instance;

        inputReader.Fire += OnSelectGem;
    }

    private void OnDestroy()
    {
        inputReader.Fire -= OnSelectGem;
    }

    private void OnSelectGem()
    {
        if (!isPlayerTurn) return;

        // Get click position
        var gridPos = match3.GetGrid().GetXY(Camera.main.ScreenToWorldPoint(inputReader.Selected));

        // Check if gem selection can result in Gem Swap
        bool succesfull = match3.OnGemSelect(gridPos);

        // Run gameloop of player's match 3 if 'succesfull' is true
        if (succesfull) StartCoroutine(RunGameLoop(match3.GetSelectedGemPos(), gridPos));
    }

    private IEnumerator RunGameLoop(Vector2Int gridPosA, Vector2Int gridPosB)
    {
        isPlayerTurn = false;

        // PART 1: gem swapping
        yield return StartCoroutine(match3.SwapGems(gridPosA, gridPosB));

        do {
            // Matches
            matches = matchComboController.FindMatches(match3.GetGrid(), Match3.HEIGHT, Match3.WIDTH);

            // Swap back if no matches were made
            if (matches.Count == 0 && match3.IsGemSelected()) StartCoroutine(match3.SwapGems(gridPosA, gridPosB));

            // Make gems explode
            yield return StartCoroutine(match3.ExplodeGems(matches));
            // Make gems fall
            yield return StartCoroutine(match3.MakeGemsFall());
            // Fill empty spots
            yield return StartCoroutine(match3.FillEmptySpots());

            match3.DeselectGem();
        } while (matches.Count > 0); // Return unitl there are no mathces on the board

        // PART 1 END

        // TODO: Part 2 with doing all player's collected abilities
        // TODO: Part 3 - enemy's turn

        isPlayerTurn = true;
    }
}
