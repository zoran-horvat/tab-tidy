using Demo.Models;

namespace Demo.UI;

public class InputHandler
{
    private readonly Dictionary<ConsoleKey, Direction> _keyMappings;

    public InputHandler()
    {
        _keyMappings = new Dictionary<ConsoleKey, Direction>
        {
            { ConsoleKey.UpArrow, Direction.Up },
            { ConsoleKey.W, Direction.Up },
            { ConsoleKey.DownArrow, Direction.Down },
            { ConsoleKey.S, Direction.Down },
            { ConsoleKey.LeftArrow, Direction.Left },
            { ConsoleKey.A, Direction.Left },
            { ConsoleKey.RightArrow, Direction.Right },
            { ConsoleKey.D, Direction.Right }
        };
    }

    public bool TryGetDirection(ConsoleKey key, out Direction direction)
    {
        return _keyMappings.TryGetValue(key, out direction);
    }

    public bool IsPauseKey(ConsoleKey key)
    {
        return key == ConsoleKey.Spacebar || key == ConsoleKey.P;
    }

    public bool IsQuitKey(ConsoleKey key)
    {
        return key == ConsoleKey.Escape || key == ConsoleKey.Q;
    }

    public bool IsRestartKey(ConsoleKey key)
    {
        return key == ConsoleKey.R || key == ConsoleKey.Enter;
    }
}
