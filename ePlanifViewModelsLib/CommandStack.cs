using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib
{
	public class CommandStack : IEnumerable<Command>
	{
		private List<Command> commands;
		
		public string FirstCommandDescription
		{
			get
			{
				if (commands.Count == 0) return null;
				else return commands[commands.Count - 1].Description;
			}
		}

		private int maxCapacity;
		public int MaxCapacity
		{
			get { return maxCapacity; }
		}

		public int Count
		{
			get { return commands.Count; }
		}

		public CommandStack(int MaxCapacity)
		{
			this.maxCapacity = MaxCapacity;

			commands = new List<Command>();
		}


		public Command Peek()
		{
			if (commands.Count == 0) return null;
			Command command = commands[commands.Count - 1];

			return command;
		}


		public Command Pop()
		{
			if (commands.Count == 0) return null;
			Command command = commands[commands.Count - 1];
			commands.RemoveAt(commands.Count - 1);

			return command;
		}

		public void Push(Command Command)
		{
			commands.Add(Command);
			if ((commands.Count > maxCapacity) && (maxCapacity > 0))
			{
				Command removedCommand = commands[0];
				commands.RemoveAt(0);
				removedCommand.Dispose();
			}
		}

		public void Clear()
		{
			foreach (Command command in commands)
			{
				command.Dispose();
			}
			commands.Clear();
		}



		public IEnumerator<Command> GetEnumerator()
		{
			return commands.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return commands.GetEnumerator();
		}



	}

}
