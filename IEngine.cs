using System;

namespace MinesweeperEngine
{
	public enum EngineState
	{
		Idle,
		Generating,
		Playing,
		Win,
		Lose
	}

	public interface IEngine
	{
		event Action<EngineState> OnStateChange;

		IField[,] Fields { get; }

		EngineState State { get; }

		uint Width { get; }

		uint Height { get; }

		uint MineCount { get; }

		uint DefusedMineCount { get; }

		// return true if game is successfully started
		bool NewGame(uint width, uint height, uint mineCount);

		// return true if game is successfully stopped
		bool StopGame();

		IField GetField(uint x, uint y);

		// return true if blow up
		bool Defuse(uint x, uint y);

		// return true if defused a mine
		bool Flag(uint x, uint y);
	}
}
