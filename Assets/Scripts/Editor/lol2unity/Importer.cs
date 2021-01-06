namespace Lol2Unity
{
	public abstract class Importer
	{
		protected uint StringToHash(char[] s)
		{
			uint hash = 0;

			foreach (char c in s)
			{
				char low = c.ToString().ToLower()[0];
				
				hash = (hash << 4) + low;
				
				uint temp = hash & 0xf0000000;

				if (temp != 0)
				{
					hash ^= (temp >> 24);
					hash ^= temp;
				}
			}

			return hash;
		}
	}
}