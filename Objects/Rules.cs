namespace TetrisApp.Objects
{
	sealed class Rules
	{
		private static string[] rules;
		private Rules()
		{

		}

		static Rules()
		{
			rules = new string[]
				{
					"Control keys",
					"S : Start / Unpause",
					"P : Pause",
					"N : New Game",
					"----------------",
					"UpArrow : Rotate figure",
					"LeftArrow / RightArrow : Move figure",
					"DownArrow : Accelerate the fall of figure"
				};
		}

		public static string[] GetRuleset()
		{
			return rules;
		}

	}
}
