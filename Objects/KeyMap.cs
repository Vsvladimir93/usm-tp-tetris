using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TetrisApp
{
	class KeyMap
	{
		private Dictionary<ConsoleKey, string> keys;

		public KeyMap()
		{
			InitKeyData();
		}

		private void InitKeyData()
		{
			keys = new Dictionary<ConsoleKey, string>();
			keys.Add(ConsoleKey.S, "Press 's' to start.");
			keys.Add(ConsoleKey.P, "Press 'p' to pause.");
			keys.Add(ConsoleKey.UpArrow, "Press '↑' to rotate figure.");
			keys.Add(ConsoleKey.DownArrow, "Press '↓' to speed up the fall of the figure.");
			keys.Add(ConsoleKey.LeftArrow, "Press '←' to move figure left.");
			keys.Add(ConsoleKey.RightArrow, "Press '→' to move figure right.");
		}

		public KeyValuePair<ConsoleKey, string> GetKeyData(ConsoleKey key)
		{
			KeyValuePair<ConsoleKey, string> kv = keys.FirstOrDefault(k => k.Key.Equals(key));

			return kv.Equals(default(KeyValuePair<ConsoleKey, string>)) ? KeyValuePair.Create(ConsoleKey.NoName, "") : kv;
		}


	}

}
