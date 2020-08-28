namespace Identify
{
	public static class ID
	{
		static Worker _worker;

		public static long Generate()
		{
			if (_worker == null)
			{
				_worker = new Worker(0, 0);
			}

			return _worker.NextId();
		}
	}
}