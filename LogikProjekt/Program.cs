using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogikProjekt
{
    class Program
    {
        public struct GameCoords
        {
            int[][] savedGuesses;
            int[][] evaluation;
            
        }
        static void Main(string[] args)
        {
            ShowInitGreetings(); //first screen player see

            bool mistake = false;
            bool end = false;
            string choice;
            while (!end)
            {
                Console.Clear();
                if (mistake == true) Console.WriteLine("\t\tInput invalid. Please try again.");
                choice = ShowMainMenu(); //shows main menu with choices
                mistake = DoAskedChoice(choice, ref end); //does what plyer chose
            }

            Console.WriteLine("exiting program");
            Console.Read();

        }
         static void ShowInitGreetings()
        {
            string[] lines = new string[3];
            lines[0] = "Logik the game";
            lines[1] = "by Vladimir jansa";
            lines[2] = "Press any key for continue.";
            int consoleMid = 0; // pro vypocet stredu konzole

            Console.WriteLine(); //pro lepsi vyhled jedno odradkovani

            for(int i = 0; i<3; i++)
            {
                if (i == 2) Console.Write("\n\n");

                consoleMid = (Console.WindowWidth - lines[i].Length) / 2;
                Console.SetCursorPosition(consoleMid, Console.CursorTop);
                Console.WriteLine(lines[i]);
            }
            Console.ReadLine();
            Console.Clear();



        }
        static string ShowMainMenu()
        {
            string ret;
            Console.WriteLine(); //pro lepsi vyhled jedno odradkovani
            Console.WriteLine("\n\n\t\t1. Start new game");
            Console.WriteLine("\t\t2. Load"); // nahraje ze slozky posledni ulozenou hru
            Console.WriteLine("\t\t3. Scoreboard"); // ukaze 10 nejlepsich !ulozenych! her
            Console.WriteLine("\t\t4. Game rules"); // vysvetli hru
            Console.WriteLine("\t\t5. End program");
            //Console.Write("\n\n");
            Console.WriteLine("\n\n\t\tType number (1-4) or the name of your choice.");
            ret = Console.ReadLine();
            return ret;
        }
        static bool DoAskedChoice(string choice, ref bool end)
        {

            if (choice == "1" || choice == "Start new game" || choice == "start new game") 
            {
                Console.Clear();
                StartNewGame();
                return false;
            }
            if (choice == "2" || choice == "Load" || choice == "load")
            {
                /*LoadGame*/
                return false;
            }
            if (choice == "3" || choice == "Scoreboard" || choice == "scoreboard")
                /*ShowScoreboard*/;
            if (choice == "4" || choice == "Game rules" || choice == "game rules")
                /*ShowRules*/;
            if (choice == "5" || choice == "End" || choice == "end")
            {
                end = true;
                return false;
            }

                return true;
        }
        static void StartNewGame()
        {
            int round = 1;
            char[,] savedGuesses = new char[10,5];
            char[,] evaluation = new char[10,5];
            FillArrays(ref savedGuesses, ref evaluation);
            Random rd = new Random();
            int[] code = new int[5]; //the 5 numbers player is guessing
            int[] guess = new int[5];
            for (int i = 0; i < 5; i++)
            {
                code[i] = rd.Next(1, 7);
            }
            for(int i = 0; i < 1; i++) //10 kol a zaverecny nahled
            {
                ShowGrid(ref guess, ref savedGuesses, ref evaluation);
                Console.ReadLine();
            }
        }
        static void FillArrays(ref char[,] savedGuesses, ref char[,] evaluation)
        {
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    savedGuesses[i, j] = 'x';
                    evaluation[i, j] = 'x';
                }
            }
        }
        static void ShowGrid(ref int[] guess, ref char[,] savedGuesses, ref char[,] evaluation)
        {
            /*
            for(int i = 0; i < 17; i++)
            {
                Console.Write("_"); // horni ohraniceni hraciho pole
            }
            */
            Console.WriteLine();
            for (int i = 0; i < 10; i++) //10 radku se hrou
            {
                Console.Write("{0, 2}.| ", i + 1);
                for (int j = 0; j < 5; j++)
                {
                    Console.Write(evaluation[i, j]);
                }
                Console.Write(" | ");
                for (int j = 0; j < 5; j++)
                {
                    Console.Write(savedGuesses[i, j]);
                }
                Console.Write(" |");
                Console.WriteLine();
            }
            /*
            for (int i = 0; i < 17; i++)
            {
                Console.Write("_"); // dolni ohraniceni hraciho pole
            }
            */
        }
    }
}
