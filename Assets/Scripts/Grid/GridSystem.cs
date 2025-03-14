using UnityEngine;

public class GridSystem <T>
{
    int width;
    int height;
    float cellSize;
    Vector3 origin;
    T[,] gridArray;

    CoordinateConverter coordinateConverter;

    public static GridSystem<T> VerticalGrid(int width, int height, float cellSize, Vector3 origin)
    {
        return new GridSystem<T>(width, height, cellSize, origin, new CoordinateConverter());
    }

    public GridSystem(int width, int height, float cellSize, Vector3 origin, CoordinateConverter coordinateConverter)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;
        this.coordinateConverter = coordinateConverter ?? new CoordinateConverter();

        gridArray = new T[width, height];
    }

    public class CoordinateConverter
    {
        public Vector3 GridToWorld(int x, int y, float cellSize, Vector3 origin)
        {
            return new Vector3(x, y, 0) * cellSize + origin;
        }

        public Vector3 GridToWorldCenter(int x, int y, float cellSize, Vector3 origin)
        {
            return new Vector3(x * cellSize + cellSize * 0.5f, y * cellSize + cellSize * 0.5f, 0) + origin;
        }

        /// <summary>
        /// Returns grid position from world position
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <param name="cellSize"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public Vector2Int WorldToGrid(Vector3 worldPosition, float cellSize, Vector3 origin)
        {
            Vector3 gridPosition = (worldPosition - origin) / cellSize;
            var x = Mathf.FloorToInt(gridPosition.x);
            var y = Mathf.FloorToInt(gridPosition.y);
            return new Vector2Int(x, y);
        }

        public Vector3 Forward => Vector3.forward;
    }

    // Set a value from a grid position
    public void SetValue(Vector3 worldPosition, T value)
    {
        Vector2Int pos = coordinateConverter.WorldToGrid(worldPosition, cellSize, origin);
        SetValue(pos.x, pos.y, value);
    }

    public void SetValue(int x, int y, T value)
    {
        if (IsValid(x, y))
        {
            gridArray[x, y] = value;
        }
    }

    // Get a value from a grid position
    public T GetValue(Vector3 worldPosition)
    {
        Vector2Int pos = GetXY(worldPosition);
        return GetValue(pos.x, pos.y);
    }

    public T GetValue(int x, int y)
    {
        return IsValid(x, y) ? gridArray[x, y] : default;
    }
    public T GetValue(Vector2Int position)
    {
        return GetValue(position.x, position.y);
    }

    bool IsValid(int x, int y) => x >= 0 && y >= 0 && x < width && y < height;

    public Vector2Int GetXY(Vector3 worldPosition) => coordinateConverter.WorldToGrid(worldPosition, cellSize, origin);

    public Vector3 GetWorldPositionCenter(int x, int y) => coordinateConverter.GridToWorldCenter(x, y, cellSize, origin);

    Vector3 GetWorldPosition(int x, int y) => coordinateConverter.GridToWorld(x, y, cellSize, origin);
}

