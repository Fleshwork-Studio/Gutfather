using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
public enum MatchType
    {
        Null,
        Three3,
        Four4,
        Five5,
        Six6,
        Cross
    }
public class MatchComboController : MonoBehaviour
{
    public static MatchComboController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<Vector2Int> FindMatches(GridSystem<GridObject<Gem>> grid, int height, int width)
    {
        HashSet<Vector2Int> matches = new();

        // Check horizontal and vertical matches
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                CheckMatch(grid, matches, x, y, 1, 0, width);  // Horizontal check
                CheckMatch(grid, matches, x, y, 0, 1, height); // Vertical check
            }
        }

        return new List<Vector2Int>(matches);
    }

    // Checks for matches in a given direction (dx, dy).
    private void CheckMatch(GridSystem<GridObject<Gem>> grid, HashSet<Vector2Int> matches, int x, int y, int dx, int dy, int limit)
    {
        List<Vector2Int> matchPositions = new();

        // Find basic linear matches
        for (int i = 0; i < 6; i++)
        {
            int newX = x + i * dx;
            int newY = y + i * dy;
            Vector2Int pos = new(newX, newY);

            if (!IsValidPosition(pos)) break;

            var gem = grid.GetValue(pos);

            if (!IsValidGem(gem) || (i > 0 && gem.GetValue().GetGemType() != grid.GetValue(matchPositions[0]).GetValue().GetGemType()))
                break;

            matchPositions.Add(pos);
        }

        if (matchPositions.Count < 3 || matchPositions.All(matches.Contains)) return;

        MatchType matchType = matchPositions.Count switch
        {
            >= 6 => MatchType.Six6,
            5 => MatchType.Five5,
            4 => MatchType.Four4,
            _ => MatchType.Three3
        };

        // Check for T or L shaped match
        if (IsCrossShape(grid, matches, matchPositions, dx, dy))
        {
            matchType = MatchType.Cross;
        }

        // Adds all matches to the matches HashSet
        foreach (var pos in matchPositions)
            matches.Add(pos);

        var gemTypeSO = grid.GetValue(x, y).GetValue().GetGemType();

        // Triggers events
        ComboMatchEvent eventData = new ComboMatchEvent { matchType = matchType, gemType = gemTypeSO, gemPositions = matchPositions };
        Bus.Publish(eventData);
    }

    // Finds T and L shaped matches
    private bool IsCrossShape(GridSystem<GridObject<Gem>> grid, HashSet<Vector2Int> matches, List<Vector2Int> baseMatch, int dx, int dy)
    {
        int verticalDir = dx; // Direction to check
        int horizontalDir = dy;

        int checkRange = 2; // How far to chechk to the sides

        bool isCross = false;

        GemTypeSO gemType = grid.GetValue(baseMatch[0]).GetValue().GetGemType(); // Main gem type

        for (int x = 0; x < baseMatch.Count; x++)
        {
            List<Vector2Int> additionalGems = new List<Vector2Int>();
            HashSet<int> wrongDirections = new HashSet<int>();

            for (int i = 1; i <= checkRange; i++)
            {
                Vector2Int offset = new Vector2Int(horizontalDir, verticalDir) * i; // Offset to check

                // Goes throught 1 and -1 mult, to check different sides of baseMatch
                for (int mult = 1; mult > -2; mult -= 2)
                {
                    // Continue, if there are a missing gem type on the way of checking
                    if (wrongDirections.Contains(mult)) continue;

                    Vector2Int position = baseMatch[x] + offset * mult;

                    if (!IsValidPosition(position)) continue;

                    var gem = grid.GetValue(position);

                    if (IsValidGem(gem) && gem.GetValue().GetGemType() == gemType)
                    {
                        additionalGems.Add(position);
                    }
                    else
                    {
                        // Adds direction to the wrong list if there are wrong gem on the way of checking
                        wrongDirections.Add(mult);
                    }
                }
            }

            // Check if there are 2 or more additional gems around gem in base match
            if (additionalGems.Count >= 2)
            {
                // Continue if additional gems are already in matches HashSet
                if (additionalGems.All(matches.Contains)) continue;

                foreach (Vector2Int position in additionalGems)
                {
                    matches.Add(position);
                    baseMatch.Add(position);
                }

                isCross = true;
            }
        }

        return isCross;
    }

    // Checks if a gem is valid (not null and not void).
    private bool IsValidGem(GridObject<Gem> gem) => gem != null && !gem.IsVoid();

    // Check if position are in bounds
    private bool IsValidPosition(Vector2Int position) => position.x < Match3.WIDTH && position.y < Match3.HEIGHT;
}
