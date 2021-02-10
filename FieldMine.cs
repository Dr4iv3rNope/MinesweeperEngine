
namespace MinesweeperEngine
{
	public class FieldMine : Field
	{
		public override bool OnDefuse(uint x, uint y, IField activator)
		{
			base.OnDefuse(x, y, activator);
			return true;
		}

		public override FieldType Type => FieldType.Mine;
	}
}
