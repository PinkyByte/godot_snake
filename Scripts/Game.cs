using Godot;
using System;

public partial class Game : Node2D
{
	private Map map;
	private Control control;

    public override void _Ready()
    {
        map = GetNode<Map>("Map");
		map.ScoreIncreased += ChangeScoreText;
		map.GameOver += GameOverScreen;
		map.GameWon += GameWonScreen;

		control = GetNode<Control>("Control");
		control.ProcessMode = ProcessModeEnum.Always;
    }

	private void ChangeScoreText(int length)
	{
		control.GetNode<Label>("Score").Text = $"Score: {length}";
	}

	private void GameOverScreen()
	{
		control.GetNode<Label>("Label").Text = "Game over!";
		control.GetNode<Label>("Label").Visible = true;
		control.GetNode<Button>("Ok").Visible = true;
		GetTree().Paused = true;
	}

	private void GameWonScreen()
	{
		control.GetNode<Label>("Label").Text = "Game won!";
		control.GetNode<Label>("Label").Visible = true;
		control.GetNode<Button>("Ok").Visible = true;
		GetTree().Paused = true;
	}
}
