using System;
using System.Threading;
using System.Threading.Tasks;

namespace TetrisApp
{
	class KeyInputListener
	{
		public event Action<ConsoleKeyInfo> OnKeyPressed;

		private CancellationTokenSource tokenSource;
		private CancellationToken token;

		private Task workerThread;

		public void Listen()
		{
			if (workerThread == null)
			{
				tokenSource = new CancellationTokenSource();
				token = tokenSource.Token;

				workerThread = new Task(
				() =>
				{
					try
					{
						while (true)
						{

							if (token.IsCancellationRequested)
							{
								token.ThrowIfCancellationRequested();
							}
							OnKeyPressed.Invoke(Console.ReadKey(true));

						}
					}
					catch (OperationCanceledException e)
					{
						tokenSource.Dispose();
					}
				}, token);
				workerThread.Start();
			}
		}

		public void Finish()
		{
			if (tokenSource != null)
			{
				tokenSource.Cancel();
				workerThread = null;
			}
		}

	}
}
