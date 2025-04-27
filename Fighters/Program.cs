using Fighters.GameEngine;
using Fighters.models.fighters;

namespace Fighters;

class Program
{
    static void Main()
    {
        List<IFighter> fighters = new();
        var gameManager = new GameManager();
        gameManager.StartGame( fighters );
    }
}