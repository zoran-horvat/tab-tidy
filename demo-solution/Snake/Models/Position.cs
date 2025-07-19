namespace Demo.Models;

public readonly record struct Position(int X, int Y)
{
    public Position Move(Direction direction)
    {
        return direction switch
        {
            Direction.Up => new Position(X, Y - 1),
            Direction.Down => new Position(X, Y + 1),
            Direction.Left => new Position(X - 1, Y),
            Direction.Right => new Position(X + 1, Y),
            _ => this
        };
    }
}
