using Demo.Game;
using Demo.Models;

namespace Demo.UI;

public class GameRenderer
{
    private readonly int _boardOffsetX;
    private readonly int _boardOffsetY;
    private readonly char _snakeBodyChar = '■';
    private readonly char _snakeHeadChar = '●';
    private readonly char _foodChar = '♦';
    private readonly char _borderChar = '█';
    private readonly char _emptyChar = ' ';

    public GameRenderer()
    {
        _boardOffsetX = 2;
        _boardOffsetY = 3;
    }

    public void Initialize(GameBoard board)
    {
        Console.Clear();
        Console.CursorVisible = false;
        
        try
        {
            if (OperatingSystem.IsWindows())
            {
                var width = Math.Max(Console.WindowWidth, board.Width + 20);
                var height = Math.Max(Console.WindowHeight, board.Height + 10);
                Console.SetWindowSize(Math.Min(width, Console.LargestWindowWidth), 
                                     Math.Min(height, Console.LargestWindowHeight));
            }
        }
        catch
        {
        }

        DrawBorder(board);
        DrawInstructions();
    }

    public void RenderGame(SnakeGameEngine engine)
    {
        DrawBoard(engine.Board, engine.Snake, engine.Food);
        DrawStats(engine.Stats, engine.State);
        DrawGameState(engine.State);
    }

    public void RenderBoard(GameBoard board, Snake snake, Food food)
    {
        DrawBoard(board, snake, food);
    }

    public void RenderStats(GameStats stats, GameState state)
    {
        DrawStats(stats, state);
    }

    public void ShowGameOver(GameStats stats)
    {
        var centerX = _boardOffsetX + 20;
        var centerY = _boardOffsetY + 10;

        Console.SetCursorPosition(centerX - 10, centerY - 3);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("╔══════════════════╗");
        Console.SetCursorPosition(centerX - 10, centerY - 2);
        Console.WriteLine("║   GAME OVER!     ║");
        Console.SetCursorPosition(centerX - 10, centerY - 1);
        Console.WriteLine($"║   Score: {stats.Score,-8} ║");
        Console.SetCursorPosition(centerX - 10, centerY);
        Console.WriteLine($"║   Food: {stats.FoodEaten,-9} ║");
        Console.SetCursorPosition(centerX - 10, centerY + 1);
        Console.WriteLine("║   R - Restart    ║");
        Console.SetCursorPosition(centerX - 10, centerY + 2);
        Console.WriteLine("║   Q - Quit       ║");
        Console.SetCursorPosition(centerX - 10, centerY + 3);
        Console.WriteLine("╚══════════════════╝");
        Console.ResetColor();
    }

    public void ShowPaused()
    {
        var centerX = _boardOffsetX + 20;
        var centerY = _boardOffsetY + 10;

        Console.SetCursorPosition(centerX - 8, centerY);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("PAUSED - Press SPACE to continue");
        Console.ResetColor();
    }

    private void DrawBorder(GameBoard board)
    {
        Console.ForegroundColor = ConsoleColor.White;
        
        Console.SetCursorPosition(_boardOffsetX - 1, _boardOffsetY - 1);
        Console.Write(_borderChar);
        for (int x = 0; x < board.Width; x++)
        {
            Console.Write(_borderChar);
        }
        Console.WriteLine(_borderChar);

        for (int y = 0; y < board.Height; y++)
        {
            Console.SetCursorPosition(_boardOffsetX - 1, _boardOffsetY + y);
            Console.Write(_borderChar);
            Console.SetCursorPosition(_boardOffsetX + board.Width, _boardOffsetY + y);
            Console.Write(_borderChar);
        }

        Console.SetCursorPosition(_boardOffsetX - 1, _boardOffsetY + board.Height);
        Console.Write(_borderChar);
        for (int x = 0; x < board.Width; x++)
        {
            Console.Write(_borderChar);
        }
        Console.WriteLine(_borderChar);

        Console.ResetColor();
    }

    private void DrawBoard(GameBoard board, Snake snake, Food food)
    {
        for (int y = 0; y < board.Height; y++)
        {
            Console.SetCursorPosition(_boardOffsetX, _boardOffsetY + y);
            for (int x = 0; x < board.Width; x++)
            {
                Console.Write(_emptyChar);
            }
        }

        Console.SetCursorPosition(_boardOffsetX + food.Position.X, _boardOffsetY + food.Position.Y);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(_foodChar);

        Console.ForegroundColor = ConsoleColor.Green;
        foreach (var segment in snake.Body.Skip(1))
        {
            Console.SetCursorPosition(_boardOffsetX + segment.X, _boardOffsetY + segment.Y);
            Console.Write(_snakeBodyChar);
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.SetCursorPosition(_boardOffsetX + snake.Head.X, _boardOffsetY + snake.Head.Y);
        Console.Write(_snakeHeadChar);

        Console.ResetColor();
    }

    private void DrawStats(GameStats stats, GameState state)
    {
        var statsX = _boardOffsetX + 45;
        var statsY = _boardOffsetY;

        Console.ForegroundColor = ConsoleColor.Cyan;
        
        Console.SetCursorPosition(statsX, statsY);
        Console.WriteLine("SNAKE GAME");
        
        Console.SetCursorPosition(statsX, statsY + 2);
        Console.WriteLine($"Score: {stats.Score}");
        
        Console.SetCursorPosition(statsX, statsY + 3);
        Console.WriteLine($"Food Eaten: {stats.FoodEaten}");
        
        Console.SetCursorPosition(statsX, statsY + 4);
        Console.WriteLine($"High Score: {stats.HighScore}");
        
        Console.SetCursorPosition(statsX, statsY + 5);
        Console.WriteLine($"Time: {stats.GameTime:mm\\:ss}");

        Console.ResetColor();
    }

    private void DrawGameState(GameState state)
    {
        var statsX = _boardOffsetX + 45;
        var statsY = _boardOffsetY + 7;

        Console.SetCursorPosition(statsX, statsY);
        Console.ForegroundColor = state switch
        {
            GameState.Playing => ConsoleColor.Green,
            GameState.Paused => ConsoleColor.Yellow,
            GameState.GameOver => ConsoleColor.Red,
            _ => ConsoleColor.White
        };
        
        Console.WriteLine($"Status: {state}");
        Console.ResetColor();
    }

    private void DrawInstructions()
    {
        var instructionsX = _boardOffsetX;
        var instructionsY = _boardOffsetY + 25;

        Console.SetCursorPosition(instructionsX, instructionsY);
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("Controls:");
        Console.WriteLine("  Arrow Keys or WASD - Move");
        Console.WriteLine("  SPACE or P - Pause/Resume");
        Console.WriteLine("  R - Restart (when game over)");
        Console.WriteLine("  Q or ESC - Quit");
        Console.ResetColor();
    }
}
