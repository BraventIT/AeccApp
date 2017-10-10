using Microsoft.Bot.Connector;
using System.Threading.Tasks;

namespace AeccBot.CommandHandling
{
    public interface IBotCommandHandler
    {
        /// <summary>
        /// Handles the direct commands to the bot.
        /// </summary>
        /// <param name="activity">The activity containing a possible command.</param>
        /// <returns>True, if a command was detected and handled. False otherwise.</returns>
        Task<bool> HandleCommandAsync(Activity activity);
    }
}