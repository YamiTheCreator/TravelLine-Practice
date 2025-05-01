using Fighters.GameEngine;
using Fighters.Models.Fighters;

namespace Fighters
{
    internal static class Program
    {
        private static void Main()
        {
            List<IFighter> fighters = [ ];
            GameManager.GameController( fighters );
        }
    }
}