using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Map : TileMapLayer
{
	private List<Vector2I> snake;
	private Vector2I apple;
	private Random random;
    private Timer timer;
    private Direction dir;
    private int length;
    [Signal] public delegate void ScoreIncreasedEventHandler(int length);
    [Signal] public delegate void GameOverEventHandler();
    [Signal] public delegate void GameWonEventHandler();

    public override void _Ready()
    {
        length = 1;

        random = new Random();

        snake = new List<Vector2I>();
        snake.Add(new Vector2I(0, 0));
        
        GenerateApple();

        dir = Direction.Up;

        UpdateMap();

        timer = new Timer();
        timer.WaitTime = 0.3;
        timer.Timeout += MoveSnake;
        AddChild(timer);
        timer.Start();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("ui_up") && dir != Direction.Down)
        {
            dir = Direction.Up;
        }
        else if (Input.IsActionPressed("ui_down") && dir != Direction.Up)
        {
            dir = Direction.Down;
        }
        else if (Input.IsActionPressed("ui_left") && dir != Direction.Right)
        {
            dir = Direction.Left;
        }
        else if (Input.IsActionPressed("ui_right") && dir != Direction.Left)
        {
            dir = Direction.Right;
        }
    }

    private void MoveSnake()
    {
        int nextX = snake[0].X;
        int nextY = snake[0].Y;
        switch (dir)
        {
            case Direction.Up:
                snake[0] = new Vector2I(nextX, nextY - 1);
                break;
            case Direction.Down:
                snake[0] = new Vector2I(nextX, nextY + 1);
                break;
            case Direction.Left:
                snake[0] = new Vector2I(nextX - 1, nextY);
                break;
            case Direction.Right:
                snake[0] = new Vector2I(nextX + 1, nextY);
                break;
        }

        if (snake[0].X == apple.X && snake[0].Y == apple.Y)
        {
            ++length;
            EmitSignal(SignalName.ScoreIncreased, length);
            GenerateApple();
            if (length % 10 == 0)
            {
                timer.WaitTime -= 0.03;
            }
            snake.Insert(1, new Vector2I(nextX, nextY));
        }
        else if (snake.Skip(1).Any(vector => vector == snake[0]) || snake[0].X > 9 || snake[0].X < -10 || snake[0].Y > 4 || snake[0].Y < -5)
        {
            EmitSignal(SignalName.GameOver);
        }
        else
        {
            for (int i = 1; i < snake.Count; i++)
            {
                int newX = nextX;
                int newY = nextY;
                nextX = snake[i].X;
                nextY = snake[i].Y;
                snake[i] = new Vector2I(newX, newY);
            }
        }

        if (length == 200)
        {
            EmitSignal(SignalName.GameWon);
        }

        UpdateMap();
    }

    private void UpdateMap()
    {
        for (int i = -10; i < 10; i++)
        {
            for (int j = -5; j < 5; j++)
            {   
                if (apple.X == i && apple.Y == j)
                {
                    SetCell(new Vector2I(i, j), 0, new Vector2I(1, 1));
                }
                else if (snake.Any(vector => vector.X == i && vector.Y == j))
                {
                    SetCell(new Vector2I(i, j), 0, new Vector2I(1, 0));
                }
                else
                {
                    SetCell(new Vector2I(i, j), 0, new Vector2I(0, 0));
                }
            }
        }
    }

    private void GenerateApple()
    {
        int x = random.Next(-10, 10);
        int y = random.Next(-5, 5);
        while (snake.Any(vector => vector.X == x && vector.Y == y))
        {
            x = random.Next(-10, 10);
            y = random.Next(-5, 5);
        }
        apple = new Vector2I(x, y);
    }
}
