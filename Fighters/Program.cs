using Fighters.GameEngine;
using Fighters.models.fighters;

namespace Fighters;

class Program
{
    private static void Main()
    {
        List<IFighter> fighters = new();
        GameManager gameManager = new();
        gameManager.StartGame( fighters );
    }
}