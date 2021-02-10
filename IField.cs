
namespace MinesweeperEngine
{
	public enum FieldType
	{
		Empty, // empty field
		Indicator, // mine count field
		Mine // mine
	}

	public interface IField
	{
		bool IsExplored { get; }

		bool IsFlagged { get; }

		FieldType Type { get; }
		
		// return true if blow up
		bool OnDefuse(uint x, uint y, IField activator);
		
		// return current flagged state (IsFlagged)
		bool OnFlag(uint x, uint y);
	}
}
