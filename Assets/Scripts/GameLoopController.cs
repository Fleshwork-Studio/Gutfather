using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopController : MonoBehaviour
{
    enum GamePhase
    {
        PlayerTurn,
        GemDestroying,
        PlayerSkills,
        EnemiesTurn,
        EndOfBattle
    }
    [SerializeField] InputReader inputReader;

    Match3 match3;
    MatchComboController matchComboController;
    FightField fightField;
    PlayerSkillsController playerSkillsController;

    List<Vector2Int> matches = new List<Vector2Int>();

    private GamePhase gamePhase;

    private Vector2Int selectedGem;

    private void Start()
    {
        match3 = Match3.Instance;
        matchComboController = MatchComboController.Instance;
        fightField = FightField.Instance;
        playerSkillsController = PlayerSkillsController.Instance;

        inputReader.Fire += OnSelectGem;

        gamePhase = GamePhase.PlayerTurn;
    }


    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(ChangeGamePhaseCoroutine(GamePhase.GemDestroying));
        yield return StartCoroutine(ChangeGamePhaseCoroutine(GamePhase.PlayerSkills));
        yield return StartCoroutine(ChangeGamePhaseCoroutine(GamePhase.EnemiesTurn));
        yield return StartCoroutine(ChangeGamePhaseCoroutine(GamePhase.PlayerTurn));
    }


    private IEnumerator ChangeGamePhaseCoroutine(GamePhase newPhase)
    {
        gamePhase = newPhase;
        Debug.Log($"Current game phase: {gamePhase}");

        switch (gamePhase)
        {
            case GamePhase.PlayerTurn:
                yield break;

            case GamePhase.GemDestroying:
                yield return StartCoroutine(SwapGems(match3.GetSelectedGemPos(), selectedGem));
                yield break;

            case GamePhase.PlayerSkills:
                yield return StartCoroutine(playerSkillsController.UseSkills());
                yield break;

            case GamePhase.EnemiesTurn:
                yield return StartCoroutine(fightField.PerformEnemiesTurn());
                yield break;

            case GamePhase.EndOfBattle:
                yield break;
        }
    }
    #region GEM_SWAP
    private void OnSelectGem()
    {
        if (gamePhase != GamePhase.PlayerTurn) return;

        // Get click position
        selectedGem = match3.GetGrid().GetXY(Camera.main.ScreenToWorldPoint(inputReader.Selected));

        // Check if gem selection can result in Gem Swap
        bool succesfull = match3.OnGemSelect(selectedGem);

        // Run gameloop of player's match 3 if 'succesfull' is true
        if (succesfull)
            StartCoroutine(GameLoop());
    }

    private IEnumerator SwapGems(Vector2Int gridPosA, Vector2Int gridPosB)
    {
        yield return StartCoroutine(match3.SwapGems(gridPosA, gridPosB));

        do
        {
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
    }
    #endregion
    private void OnDestroy()
    {
        inputReader.Fire -= OnSelectGem;
    }
}
