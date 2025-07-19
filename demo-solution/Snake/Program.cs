using Demo;

try
{
    var gameController = new GameController();
    await gameController.RunAsync();
}
catch (Exception ex)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"An error occurred: {ex.Message}");
    Console.ResetColor();
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}
