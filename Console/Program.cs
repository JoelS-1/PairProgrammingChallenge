using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonGame
{
    public class Program
    {
        static void Main(string[] args)
        {
            //program load happens here
            ProgramUI game = new ProgramUI();

            //the method that will be run
            game.Run();
        }
    }
}
