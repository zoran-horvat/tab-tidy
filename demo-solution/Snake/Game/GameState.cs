namespace Demo.Game;

public enum GameState
{
    Starting,
    Playing,
    Paused,
    GameOver
}

public class GameStats
{
    public int Score { get; set; }
    public int FoodEaten { get; set; }
    public TimeSpan GameTime { get; set; }
    public int HighScore { get; set; }

    public void Reset()
    {
        Score = 0;
        FoodEaten = 0;
        GameTime = TimeSpan.Zero;
    }

    public void AddScore(int points)
    {
        Score += points;
        if (Score > HighScore)
            HighScore = Score;
    }
}
