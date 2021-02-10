using System;

namespace MinesweeperEngine
{
	public class FieldIndicator : Field
	{
		private uint mineCount;

		public FieldIndicator(uint mineCount)
		{
			this.mineCount = mineCount >= 1 && mineCount <= 8 ? mineCount : throw new OverflowException("mine count must be in range [1,8]");
		}

		public uint MineCount => mineCount;

		public override FieldType Type => FieldType.Indicator;

		public override bool OnDefuse(uint x, uint y, IField activator) => base.OnDefuse(x, y, activator);
	}
}
