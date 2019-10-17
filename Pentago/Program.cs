using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pentago
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Pentago!");
            Console.WriteLine("Select the number of players(type '1' or '2'):");
            char rk = Console.ReadKey().KeyChar;
            bool isP2Computer = rk == '1';
            Console.WriteLine(String.Empty);

            Pentago pentagoGame = new Pentago(6, isP2Computer);
            pentagoGame.start();

            Console.ReadKey();

        }
    }
}
