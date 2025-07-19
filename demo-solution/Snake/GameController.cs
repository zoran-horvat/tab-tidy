using Demo.Game;
using Demo.Models;
using Demo.UI;

namespace Demo;

public class GameController
{
    private readonly SnakeGameEngine _gameEngine;
    private readonly GameRenderer _renderer;
    private readonly InputHandler _inputHandler;
    private volatile bool _isRunning;
    private Task? _inputTask;

    public GameController()
    {
        _gameEngine = new SnakeGameEngine(40, 20);
        _renderer = new GameRenderer();
        _inputHandler = new InputHandler();
        
        _gameEngine.GameStateChanged += OnGameStateChanged;
        _gameEngine.ScoreChanged += OnScoreChanged;
        _gameEngine.GameOver += OnGameOver;
    }

    public async Task RunAsync()
    {
        _isRunning = true;
        
        _renderer.Initialize(_gameEngine.Board);
        ShowWelcomeScreen();
        
        Console.ReadKey(true);
        
        _gameEngine.StartNewGame();
        _renderer.RenderGame(_gameEngine);
        
        _inputTask = Task.Run(HandleInputAsync);
        
        await GameLoopAsync();
        
        await CleanupAsync();
    }

    private void ShowWelcomeScreen()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                         SNAKE GAME                          ║");
        Console.WriteLine("╠══════════════════════════════════════════════════════════════╣");
        Console.WriteLine("║                                                              ║");
        Console.WriteLine("║  🐍 Welcome to the classic Snake Game!                      ║");
        Console.WriteLine("║                                                              ║");
        Console.WriteLine("║  How to play:                                               ║");
        Console.WriteLine("║  • Use arrow keys or WASD to move the snake                 ║");
        Console.WriteLine("║  • Eat the red food (♦) to grow and increase your score     ║");
        Console.WriteLine("║  • Avoid hitting the walls or your own body                 ║");
        Console.WriteLine("║  • The game gets faster as you eat more food                ║");
        Console.WriteLine("║                                                              ║");
        Console.WriteLine("║  Controls:                                                   ║");
        Console.WriteLine("║  • Arrow Keys / WASD - Move                                 ║");
        Console.WriteLine("║  • SPACE / P - Pause/Resume                                 ║");
        Console.WriteLine("║  • R - Restart (when game over)                             ║");
        Console.WriteLine("║  • Q / ESC - Quit                                           ║");
        Console.WriteLine("║                                                              ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
        Console.ResetColor();
        
        Console.WriteLine("\nPress any key to start playing...");
    }

    private async Task GameLoopAsync()
    {
        const int targetFps = 60;
        const int frameDelay = 1000 / targetFps;
        
        while (_isRunning)
        {
            try
            {
                if (_gameEngine.State == GameState.Playing)
                {
                    _renderer.RenderBoard(_gameEngine.Board, _gameEngine.Snake, _gameEngine.Food);
                    _renderer.RenderStats(_gameEngine.Stats, _gameEngine.State);
                }
                
                await Task.Delay(frameDelay);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in game loop: {ex.Message}");
                break;
            }
        }
    }

    private async Task HandleInputAsync()
    {
        while (_isRunning)
        {
            try
            {
                if (Console.KeyAvailable)
                {
                    var keyInfo = Console.ReadKey(true);
                    await ProcessInputAsync(keyInfo.Key);
                }
                
                await Task.Delay(10);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling input: {ex.Message}");
                break;
            }
        }
    }

    private async Task ProcessInputAsync(ConsoleKey key)
    {
        if (_inputHandler.IsQuitKey(key))
        {
            _isRunning = false;
            return;
        }

        if (_gameEngine.State == GameState.GameOver && _inputHandler.IsRestartKey(key))
        {
            _renderer.Initialize(_gameEngine.Board);
            _gameEngine.StartNewGame();
            _renderer.RenderGame(_gameEngine);
            return;
        }

        if (_inputHandler.IsPauseKey(key))
        {
            _gameEngine.TogglePause();
            
            if (_gameEngine.State == GameState.Paused)
            {
                _renderer.ShowPaused();
            }
            else if (_gameEngine.State == GameState.Playing)
            {
                _renderer.RenderGame(_gameEngine);
            }
            return;
        }

        if (_inputHandler.TryGetDirection(key, out Direction direction))
        {
            _gameEngine.HandleInput(direction);
        }
        
        await Task.CompletedTask;
    }

    private void OnGameStateChanged(object? sender, GameEventArgs e)
    {
        if (e.State == GameState.Playing)
        {
            _renderer.RenderGame(_gameEngine);
        }
    }

    private void OnScoreChanged(object? sender, GameEventArgs e)
    {
        _renderer.RenderStats(e.Stats, e.State);
    }

    private void OnGameOver(object? sender, GameEventArgs e)
    {
        _renderer.ShowGameOver(e.Stats);
    }

    private async Task CleanupAsync()
    {
        _gameEngine.Dispose();
        
        if (_inputTask != null)
        {
            await _inputTask;
        }
        
        Console.CursorVisible = true;
        Console.ResetColor();
    }
}
