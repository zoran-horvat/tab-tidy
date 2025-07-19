# Snake Game

A classic Snake game implementation in C# for the console, featuring clean architecture and separation of concerns.

## Features

- **Classic Gameplay**: Navigate the snake to eat food and grow longer
- **Progressive Difficulty**: Game speed increases as you eat more food
- **Score System**: Earn points for each food eaten with increasing rewards
- **Game Statistics**: Track score, food eaten, time played, and high score
- **Pause/Resume**: Pause the game at any time
- **Clean UI**: Clear console-based interface with borders and status display

## Controls

- **Arrow Keys** or **WASD** - Move the snake
- **SPACE** or **P** - Pause/Resume game
- **R** - Restart game (when game over)
- **Q** or **ESC** - Quit game

## Architecture

The project follows clean architecture principles with clear separation of concerns:

### Models (`/Models`)
- **Position**: Represents coordinates on the game board
- **Direction**: Enum for snake movement directions with helper methods
- **Snake**: Snake entity with movement and collision logic
- **Food**: Food entity for snake consumption
- **GameBoard**: Game board boundaries and utilities

### Game Engine (`/Game`)
- **GameState**: Game state management and statistics
- **SnakeGameEngine**: Core game logic, timing, and event handling

### User Interface (`/UI`)
- **GameRenderer**: Console rendering and display logic
- **InputHandler**: Keyboard input processing

### Controller
- **GameController**: Main coordinator between engine and UI
- **Program**: Application entry point

## Key Design Principles

1. **Separation of Concerns**: Each class has a single responsibility
2. **Event-Driven Architecture**: Game engine publishes events for UI updates
3. **Immutable Value Types**: Position struct is immutable for thread safety
4. **Async/Await**: Proper async handling for game loop and input
5. **Error Handling**: Comprehensive exception handling and graceful degradation
6. **Cross-Platform**: Works on Windows, macOS, and Linux (with platform-specific optimizations)

## Building and Running

### Prerequisites
- .NET 9.0 or later

### Build
```bash
dotnet build
```

### Run
```bash
dotnet run --project Demo
```

Or use the VS Code task "Run Snake Game"

## Game Rules

1. Control the snake using arrow keys or WASD
2. Eat the red food (♦) to grow and increase your score
3. Avoid hitting the walls or your own body
4. The game gets progressively faster as you eat more food
5. Try to achieve the highest score possible!

## Scoring

- Each food eaten gives you base points (10) plus bonus points based on how much food you've eaten
- Score formula: `10 + food_count` points per food
- High score is automatically tracked across game sessions

## Technical Features

- **Real-time Input**: Responsive input handling with dedicated input thread
- **Smooth Animation**: 60 FPS rendering loop for smooth gameplay
- **Memory Efficient**: Minimal object allocations during gameplay
- **Thread Safe**: Proper synchronization between game logic and input handling
- **Extensible**: Easy to add new features like power-ups, different game modes, etc.

## Future Enhancements

The architecture supports easy addition of:
- Different difficulty levels
- Power-ups and special food types
- Multiple snake colors/themes
- Sound effects
- High score persistence
- Multiplayer support
- Different game board sizes
