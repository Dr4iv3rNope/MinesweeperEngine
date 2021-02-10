using System;

namespace MinesweeperEngine
{
	public class Engine : IEngine
	{
		public static Engine Instance = new Engine();

		public Engine()
		{
			Log("Initialize");

			width = 0;
			height = 0;
			defusedMineCount = 0;
			mineCount = 0;

			fields = null;

			state = EngineState.Idle;
		}

		private Random random = new Random();

		public event Action<EngineState> OnStateChange;

		public event Action<string> OnLog;

		private void Log(string message) => OnLog?.Invoke(message);

		private uint width;
		private uint height;

		private uint mineCount;

		private uint defusedMineCount;

		private IField[,] fields;

		private EngineState state;

		public IField[,] Fields => fields;

		public EngineState State => state;

		private void SetState(EngineState newState)
		{
			Log($"State: {newState}");
			OnStateChange?.Invoke(newState);
			state = newState;
		}

		public uint Width => width;

		public uint Height => height;

		public uint MineCount => mineCount;

		public uint DefusedMineCount => defusedMineCount;

		public IField GetField(uint x, uint y) => fields[x, y];

		public bool Defuse(uint x, uint y)
		{
			var field = GetField(x, y);
			
			if (field.OnDefuse(x, y, field))
			{
				Log($"We blew up on mine at x:{x} y:{y}!");

				SetState(EngineState.Lose);
				return true;
			}

			return false;
		}

		public bool Flag(uint x, uint y)
		{
			var field = GetField(x, y);
			
			if (field.Type == FieldType.Mine)
			{
				if (field.OnFlag(x, y))
					defusedMineCount++;
				else
					defusedMineCount--;

				if (defusedMineCount == mineCount)
				{
					Log("We won!");

					SetState(EngineState.Win);
				}

				return true;
			}

			return false;
		}

		public delegate bool EnumerateCallback(IField field, uint x, uint y);

		public void EnumerateAll(EnumerateCallback callback)
		{
			for (uint i = 0; i < width; i++)
				for (uint j = 0; j < height; j++)
					if (callback(GetField(i, j), i, j))
						break;
		}

		public void EnumerateInRange(uint x, uint y, uint range, EnumerateCallback callback)
		{
			for (uint i = (uint)Math.Max((int)x - (int)range, 0); i <= Math.Min(x + range, width - 1); i++)
				for (uint j = (uint)Math.Max((int)y - (int)range, 0); j <= Math.Min(y + range, height - 1); j++)
					if (callback(GetField(i, j), i, j))
						break;
		}

		private void FillFields()
		{
			Log("Generating mines");
			{
				uint generatedMineCount = 0;

				while (generatedMineCount != mineCount)
				{
					var x = (uint)random.Next(0, (int)width - 1);
					var y = (uint)random.Next(0, (int)width - 1);
					
					var field = GetField(x, y);

					if (field == null)
						fields[x, y] = new FieldMine();
					else
					{
						if (field.Type == FieldType.Mine)
							continue;
					}

					generatedMineCount++;
				}

				Log("Generating indicators");
				{
					EnumerateAll((field, x, y) =>
					{
						if (field == null)
						{
							uint mineCount = 0;

							EnumerateInRange(x, y, 1, (mine, mx, my) =>
							{
								if (mine != null && mine.Type == FieldType.Mine)
									mineCount++;

								return false;
							});

							if (mineCount > 0)
								fields[x, y] = new FieldIndicator(mineCount);
						}

						return false;
					});
				}

				Log("Fill empty fields");
				{
					EnumerateAll((field, x, y) =>
					{
						if (field == null)
							fields[x, y] = new FieldEmpty();

						return false;
					});
				}
			}
		}

		public void NewGame(uint width, uint height, uint mineCount)
		{
			Log($"Starting new game (w:{width}, h:{height}, mc:{mineCount})");

			this.width = width > 1 ? width : throw new Exception("width is so small");
			this.height = height;

			this.mineCount = mineCount;

			Log("Allocating fields");

			this.fields = new IField[width, height];

			Log("Fields successfully allocated!");

			SetState(EngineState.Idle);
			SetState(EngineState.Generating);
			FillFields();
			SetState(EngineState.Playing);
		}
	}
}
