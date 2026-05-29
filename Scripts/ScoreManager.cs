using Godot;
using System;

public partial class ScoreManager : Node
{
	
	
	private int score;
	private int highScore;
	
	public void Awake()
	{
		score = 0;
	}
	public void EnemySquish()
	{
		score += 1;
		
	}
}
