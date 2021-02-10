
namespace MinesweeperEngine
{
	public class FieldEmpty : Field
	{
		public override FieldType Type => FieldType.Empty;

		public override bool OnDefuse(uint x, uint y, IField activator)
		{
			base.OnDefuse(x, y, activator);

			Engine.Instance.EnumerateInRange(x, y, 1, (field, fx, fy) =>
			{
				if (field == activator || field == this || field.Type == FieldType.Mine || field.IsExplored)
					return false;

				field.OnDefuse(fx, fy, this);
				return false;
			});
			
			return false;
		}
	}
}
