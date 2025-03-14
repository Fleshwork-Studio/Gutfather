public class GridObject<T>
{
    GridSystem<GridObject<T>> grid;
    int x;
    int y;
    T gem;
    bool isVoid = false; // Cannot have any gems in it

    public GridObject(GridSystem<GridObject<T>> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void SetValue(T gem)
    {
        this.gem = gem;
    }

    public T GetValue() => gem;

    public void SetVoid() => isVoid = true;
    public bool IsVoid() => isVoid; 
}


