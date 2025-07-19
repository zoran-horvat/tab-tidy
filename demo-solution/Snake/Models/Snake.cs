namespace Demo.Models;

public class Snake
{
    private readonly LinkedList<Position> _body;
    private Direction _direction;

    public IEnumerable<Position> Body => _body;
    public Position Head => _body.First?.Value ?? throw new InvalidOperationException("Snake has no body");
    public Direction Direction => _direction;
    public int Length => _body.Count;

    public Snake(Position startPosition, Direction initialDirection)
    {
        _body = new LinkedList<Position>();
        _body.AddFirst(startPosition);
        _direction = initialDirection;
    }

    public bool ChangeDirection(Direction newDirection)
    {
        if (_direction.IsOpposite(newDirection))
            return false;

        _direction = newDirection;
        return true;
    }

    public Position Move(bool grow = false)
    {
        var newHead = Head.Move(_direction);
        _body.AddFirst(newHead);

        if (!grow)
        {
            _body.RemoveLast();
        }

        return newHead;
    }

    public bool CollidesWithSelf()
    {
        return _body.Skip(1).Contains(Head);
    }

    public bool IsAtPosition(Position position)
    {
        return Head == position;
    }

    public bool OccupiesPosition(Position position)
    {
        return _body.Contains(position);
    }
}
