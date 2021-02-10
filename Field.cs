
namespace MinesweeperEngine
{
	public abstract class Field : IField
	{
		protected bool isExplored;

		public bool IsExplored => isExplored;

		protected bool isFlagged;

		public bool IsFlagged => isFlagged;

		public abstract FieldType Type { get; }

		protected Field()
		{
			isExplored = false;
			isFlagged = false;
		}

		public virtual bool OnDefuse(uint x, uint y, IField activator)
		{
			isExplored = true;

			return false;
		}

		public bool OnFlag(uint x, uint y)
		{
			isFlagged = !isFlagged;

			return isFlagged;
		}
	}
}
