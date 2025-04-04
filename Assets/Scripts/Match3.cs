using DG.Tweening;
using Gutfather.Assets.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Match3 : MonoBehaviour
{
    public static int HEIGHT { get; } = 10;
    public static int WIDTH { get; } = 10;

    public static Match3 Instance;

    [SerializeField] private int cellSize;
    [SerializeField] Vector3 originPosition = Vector3.zero;

    [SerializeField] Gem gemPrefab;
    [SerializeField] GemTypeSO[] gemTypes;
    [SerializeField] Ease ease = Ease.InQuad;

    [SerializeField] GridTemplate gridTemplate;
    GridSystem<GridObject<Gem>> grid;

    Vector2Int selectedGem;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializeGrid();

        DeselectGem();
    }

    void InitializeGrid()
    {
        grid = GridSystem<GridObject<Gem>>.VerticalGrid(WIDTH, HEIGHT, cellSize, originPosition);


        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                if (gridTemplate.IsUnitPlayable(x, y)) // Makes grid empty if it's disabled
                {
                    CreateGem(x, y);
                }
                else
                {
                    var gridObject = new GridObject<Gem>(grid, x, y);
                    gridObject.SetVoid();
                    grid.SetValue(x, y, gridObject);
                }
            }
        }
    }

    void CreateGem(int x, int y)
    {
        var gem = Instantiate(gemPrefab, grid.GetWorldPositionCenter(x, y), Quaternion.identity, transform);
        // Create chances of each gem to spawn
        gem.SetType(gemTypes[UnityEngine.Random.Range(0, gemTypes.Length)]);
        var gridObject = new GridObject<Gem>(grid, x, y);
        gridObject.SetValue(gem);
        grid.SetValue(x, y, gridObject);
    }

    // Returns true if gems selected for swap succesfully
    public bool OnGemSelect(Vector2Int gridPos)
    {
        if (!IsValidPosition(gridPos) || IsEmptyPosition(gridPos)) return false;

        if (grid.GetValue(gridPos.x, gridPos.y).IsVoid()) return false; // Returns if selected grid is empty

        else if (selectedGem == gridPos) // Deselects if clicking on already selected gem
        {
            DeselectGem();
        }
        else if (selectedGem == Vector2.one * -1) // If there are no gems selected
        {
            SelectGem(gridPos);

            var gemSelected = new NewGemSelected { position = gridPos };
            Bus.Publish(gemSelected);
        }
        else // If there are another gem selected - return true
        {
            return true;
        }

        return false;
    }

    public IEnumerator SwapGems(Vector2Int gridPosA, Vector2Int gridPosB)
    {
        var gridObjectA = grid.GetValue(gridPosA.x, gridPosA.y);
        var gridObjectB = grid.GetValue(gridPosB.x, gridPosB.y);

        // See README for a link to the DOTween asset
        gridObjectA.GetValue().transform
            .DOLocalMove(grid.GetWorldPositionCenter(gridPosB.x, gridPosB.y), 0.5f)
            .SetEase(ease);
        gridObjectB.GetValue().transform
            .DOLocalMove(grid.GetWorldPositionCenter(gridPosA.x, gridPosA.y), 0.5f)
            .SetEase(ease);

        grid.SetValue(gridPosA.x, gridPosA.y, gridObjectB);
        grid.SetValue(gridPosB.x, gridPosB.y, gridObjectA);

        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator FillEmptySpots()
    {
        for (var x = 0; x < WIDTH; x++)
        {
            for (var y = 0; y < HEIGHT; y++)
            {
                if (grid.GetValue(x, y) == null)
                {
                    CreateGem(x, y);
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    public IEnumerator MakeGemsFall()
    {
        // TODO: Make this more efficient
        for (var x = 0; x < WIDTH; x++)
        {
            for (var y = 0; y < HEIGHT; y++)
            {
                if (grid.GetValue(x, y) == null)
                {
                    for (var i = y + 1; i < HEIGHT; i++)
                    {
                        var gridUnit = grid.GetValue(x, i);
                        if (gridUnit != null && !gridUnit.IsVoid())
                        {
                            var gem = grid.GetValue(x, i).GetValue();
                            grid.SetValue(x, y, grid.GetValue(x, i));
                            grid.SetValue(x, i, null);
                            gem.transform
                                .DOLocalMove(grid.GetWorldPositionCenter(x, y), 0.2f)
                                .SetEase(ease);

                            break;
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(0.3f);
    }

    public IEnumerator ExplodeGems(List<Vector2Int> matches)
    {
        foreach (var match in matches)
        {
            var gem = grid.GetValue(match.x, match.y).GetValue();
            grid.SetValue(match.x, match.y, null);

            gem.transform.DOPunchScale(Vector3.one * 0.6f, 0.5f, 5, 1f).OnComplete(() => gem.DestroyGem());

            // gem.DestroyGem();
        }

        yield return new WaitForSeconds(1f);
    }

    bool IsEmptyPosition(Vector2Int gridPosition) => grid.GetValue(gridPosition.x, gridPosition.y) == null;

    bool IsValidPosition(Vector2 gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.x < WIDTH && gridPosition.y >= 0 && gridPosition.y < HEIGHT;
    }

    public void DeselectGem() => selectedGem = new Vector2Int(-1, -1);
    void SelectGem(Vector2Int gridPos) => selectedGem = gridPos;

    public Vector2Int GetSelectedGemPos() => selectedGem;
    public bool IsGemSelected() => selectedGem != new Vector2Int(-1, -1);

    public GridSystem<GridObject<Gem>> GetGrid() => grid;
}


