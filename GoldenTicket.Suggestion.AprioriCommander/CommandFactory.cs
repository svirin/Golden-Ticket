﻿using GoldenTicket.Command.Interfaces;
using GoldenTicket.Model;

namespace GoldenTicket.Suggestion.AprioriCommander
{
    public class CommandFactory : ICommandFactory<UserRecientBlock>
    {
        public ICommand<UserRecientBlock> CreateCommand()
        {
            return new Command();
        }
    }
}