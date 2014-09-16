﻿namespace ENode.Commanding
{
    public class ProcessingCommand
    {
        public string AggregateRootId { get; private set; }
        public string ProcessId { get; private set; }
        public ICommand Command { get; private set; }
        public ICommandExecuteContext CommandExecuteContext { get; private set; }
        public int RetriedCount { get; private set; }

        public ProcessingCommand(ICommand command, ICommandExecuteContext commandExecuteContext)
        {
            Command = command;
            CommandExecuteContext = commandExecuteContext;

            if (command is IAggregateCommand)
            {
                AggregateRootId = ((IAggregateCommand)command).AggregateRootId;
                if (string.IsNullOrEmpty(AggregateRootId) && (!(command is ICreatingAggregateCommand)))
                {
                    throw new CommandAggregateRootIdMissingException(command);
                }
            }
            if (command is IStartProcessCommand)
            {
                ProcessId = ((IStartProcessCommand)command).ProcessId;
                if (string.IsNullOrEmpty(ProcessId))
                {
                    throw new CommandProcessIdMissingException(command);
                }
            }
            else if (command.Items.ContainsKey("ProcessId"))
            {
                ProcessId = command.Items["ProcessId"];
                if (string.IsNullOrEmpty(ProcessId))
                {
                    throw new CommandProcessIdMissingException(command);
                }
            }
        }

        public void IncreaseRetriedCount()
        {
            RetriedCount++;
        }
    }
}
