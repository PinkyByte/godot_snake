using Godot;
using System;

public partial class OkButton : Button
{
	public override void _Pressed()
    {
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://Scenes/menu.tscn");
    }
}
