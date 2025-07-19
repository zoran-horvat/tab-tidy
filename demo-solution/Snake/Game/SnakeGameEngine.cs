using Demo.Models;

namespace Demo.Game;

public class GameEventArgs : EventArgs
{
    public GameStats Stats { get; }
    public GameState State { get; }

    public GameEventArgs(GameStats stats, GameState state)
    {
        Stats = stats;
        State = state;
    }
}

public class SnakeGameEngine
{
    private readonly GameBoard _board;
    private readonly Random _random;
    private readonly Timer _gameTimer;
    private Snake _snake = null!;
    private Food _food = null!;
    private GameState _gameState;
    private readonly GameStats _stats;
    private DateTime _gameStartTime;

    public event EventHandler<GameEventArgs>? GameStateChanged;
    public event EventHandler<GameEventArgs>? ScoreChanged;
    public event EventHandler<GameEventArgs>? GameOver;

    public GameBoard Board => _board;
    public Snake Snake => _snake;
    public Food Food => _food;
    public GameState State => _gameState;
    public GameStats Stats => _stats;

    public SnakeGameEngine(int boardWidth = 40, int boardHeight = 20)
    {
        _board = new GameBoard(boardWidth, boardHeight);
        _random = new Random();
        _stats = new GameStats();
        _gameTimer = new Timer(GameTick, null, Timeout.Infinite, Timeout.Infinite);
        
        InitializeGame();
    }

    public void StartNewGame()
    {
        InitializeGame();
        _gameState = GameState.Playing;
        _gameStartTime = DateTime.Now;
        _gameTimer.Change(200, 200);
        OnGameStateChanged();
    }

    public void TogglePause()
    {
        if (_gameState == GameState.Playing)
        {
            _gameState = GameState.Paused;
            _gameTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }
        else if (_gameState == GameState.Paused)
        {
            _gameState = GameState.Playing;
            var interval = CalculateGameSpeed();
            _gameTimer.Change(interval, interval);
        }
        OnGameStateChanged();
    }

    public bool HandleInput(Direction direction)
    {
        if (_gameState != GameState.Playing)
            return false;

        return _snake.ChangeDirection(direction);
    }

    public void StopGame()
    {
        _gameTimer.Change(Timeout.Infinite, Timeout.Infinite);
        _gameState = GameState.GameOver;
        OnGameOver();
    }

    private void InitializeGame()
    {
        _stats.Reset();
        
        var centerX = _board.Width / 2;
        var centerY = _board.Height / 2;
        _snake = new Snake(new Position(centerX, centerY), Direction.Right);
        
        _food = new Food(GenerateFoodPosition());
        _gameState = GameState.Starting;
    }

    private void GameTick(object? state)
    {
        if (_gameState != GameState.Playing)
            return;

        _stats.GameTime = DateTime.Now - _gameStartTime;

        bool foodEaten = _snake.IsAtPosition(_food.Position);
        var newHead = _snake.Move(foodEaten);

        if (!_board.IsWithinBounds(newHead) || _snake.CollidesWithSelf())
        {
            StopGame();
            return;
        }

        if (foodEaten)
        {
            _stats.FoodEaten++;
            _stats.AddScore(10 + _stats.FoodEaten);
            _food = _food with { Position = GenerateFoodPosition() };
            OnScoreChanged();
            
            var newInterval = CalculateGameSpeed();
            _gameTimer.Change(newInterval, newInterval);
        }
    }

    private Position GenerateFoodPosition()
    {
        Position newPosition;
        do
        {
            var x = _random.Next(0, _board.Width);
            var y = _random.Next(0, _board.Height);
            newPosition = new Position(x, y);
        } while (_snake.OccupiesPosition(newPosition));

        return newPosition;
    }

    private int CalculateGameSpeed()
    {
        var speed = Math.Max(50, 200 - (_stats.FoodEaten * 5));
        return speed;
    }

    private void OnGameStateChanged()
    {
        GameStateChanged?.Invoke(this, new GameEventArgs(_stats, _gameState));
    }

    private void OnScoreChanged()
    {
        ScoreChanged?.Invoke(this, new GameEventArgs(_stats, _gameState));
    }

    private void OnGameOver()
    {
        GameOver?.Invoke(this, new GameEventArgs(_stats, _gameState));
    }

    public void Dispose()
    {
        _gameTimer?.Dispose();
    }
}
