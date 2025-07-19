namespace Demo.Models;

public record GameBoard(int Width, int Height)
{
    public static GameBoard Create(int width, int height) =>
        new GameBoard(
            width > 0 ? width : throw new ArgumentException("Width must be positive"),
            height > 0 ? height : throw new ArgumentException("Height must be positive"));
    
    public bool IsWithinBounds(Position position) =>
        position.X >= 0 && position.X < Width &&
        position.Y >= 0 && position.Y < Height;

    public IEnumerable<Position> GetAllPositions() =>
        from y in Enumerable.Range(0, Height)
        from x in Enumerable.Range(0, Width)
        select new Position(x, y);
}
