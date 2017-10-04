using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib
{
	
	public class CommandManager 
	{

		private CommandStack undoCommands;
		private CommandStack redoCommands;

		private int undoLevel;
		public int UndoLevel
		{
			get { return undoLevel; }
		}

		public Command[] CommandHistory
		{
			get { return undoCommands.ToArray(); }
		}

		public string UndoDescription
		{
			get { return undoCommands.FirstCommandDescription; }
		}
		public string RedoDescription
		{
			get { return redoCommands.FirstCommandDescription; }
		}

		
		public bool CanUndo
		{
			get { return undoCommands.Count > 0; }
		}
		public bool CanRedo
		{
			get { return redoCommands.Count > 0; }
		}




		public CommandManager(int UndoLevel)
		{
			undoCommands = new CommandStack(UndoLevel);
			redoCommands = new CommandStack(UndoLevel);

			this.undoLevel = UndoLevel;
		}

		public void Dispose()
		{
			Clear();

			undoCommands = null;
			redoCommands = null;
		}


		public void Clear()
		{
			if (redoCommands != null) redoCommands.Clear();
			if (undoCommands != null) undoCommands.Clear();
		}




		public async Task<bool> ExecuteAsync(Command Command)
		{
			bool result = await Command.ExecuteAsync();
			if (result)
			{
				redoCommands.Clear();
				undoCommands.Push(Command);
			}
			return result;
		}


		public async Task<bool> UndoAsync()
		{
			if (undoCommands.Count == 0)  return false;


			Command command = undoCommands.Peek();
			bool result=await command.CancelAsync();
			if (result)
			{
				undoCommands.Pop();
				redoCommands.Push(command);
			}
			return result;
		}

	
		public async Task<bool> RedoAsync()
		{
			if (redoCommands.Count == 0) return false;

			Command command = redoCommands.Peek();
			bool result=await command.ExecuteAsync();
			if (result)
			{
				redoCommands.Pop();
				undoCommands.Push(command);
			}

			return result;
		}
		


	

	}

}
