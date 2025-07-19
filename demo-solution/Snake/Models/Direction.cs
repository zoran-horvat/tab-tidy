namespace Demo.Models;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public static class DirectionExtensions
{
    public static Direction ToOpposite(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => direction
        };
    }

    public static bool IsOpposite(this Direction direction, Direction other)
    {
        return direction.ToOpposite() == other;
    }
}
