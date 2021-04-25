using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogikProjekt
{
    class Program
    {

        static void Main(string[] args)
        {
            ShowInitGreetings(); //first screen player see

            bool mistake = false;
            bool end = false;
            string choice;
            
            while (!end)
            {
                Console.Clear();
                if (mistake == true) Console.WriteLine("\t\tInput invalid. Please try again."); //says when player has wrong input
                choice = ShowMainMenu(); //shows main menu with choices
                mistake = DoAskedChoice(choice, ref end); //does what player choses
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
        static bool DoAskedChoice(string choice, ref bool end) //zjisti vstup od hrace v menu
        {
            if (choice == "1" || choice.Equals("start new game", StringComparison.InvariantCultureIgnoreCase)) 
            {
                Console.Clear();
                StartNewGame();
                return false;
            }
            if (choice == "2" || choice.Equals("load", StringComparison.InvariantCultureIgnoreCase))
            {
                /*LoadGame*/
                return false;
            }
            if (choice == "3" || choice.Equals("scoreboard", StringComparison.InvariantCultureIgnoreCase))
                /*ShowScoreboard*/
            if (choice == "4" || choice.Equals("game rules", StringComparison.InvariantCultureIgnoreCase))
                /*ShowRules*/
            if (choice == "5" || choice.Equals("end", StringComparison.InvariantCultureIgnoreCase))
            {
                end = true;
                return false;
            }

                return true;
        }
        static void StartNewGame()
        {
            char[,] savedGuesses = new char[10,5];
            char[,] evaluation = new char[10,5];
            FillArrays(ref savedGuesses, ref evaluation);

            Random rd = new Random();
            int[] code = new int[5]; //5 čísel z random generátoru pro hráče na uhodnutí
            char[] colours = new char[5]; //převod na char zde aby se později v programu nepřičítá 48 (kvůli ascii)
            string playerInput = ""; //vstup od hráče

            bool wrongInput = false;
            bool gameVictory = false;

            for (int i = 0; i < 5; i++) //tento cyklus se stará o to aby se vygenerované čísla neopakovala
            {
                code[i] = rd.Next(1, 8); //random číslo mezi 1 a 8 (znázorňuje jednu z 8 barev ve hře)
                for(int j = i - 1; j >= 0; j--) //následuje kontrola jestli číslo (barva) už v kódu není
                {
                    if (i == 0) break;
                    if(code[i] == code[j]) //číslo je duplikát což být nesmí
                    {
                        i--;
                        break;
                    }
                }
                colours[i] = Convert.ToChar(code[i] + 48); // +48 z ascii hodnot
            }

            for(int round = 0; round < 10; round++) //10 kol a +1 zaverecny nahled
            {
                Console.Clear();
                Console.WriteLine();
                ShowColours(round, gameVictory, colours); //pokud hra skončila tak odhalí kód (barvy) jinak je kód skrytý
                ShowGrid(savedGuesses, evaluation);

                if (wrongInput) Console.WriteLine("Wrong Input. Please enter 5 digits in range of 1 to 8");
                if (gameVictory && round != 0) Console.WriteLine("You won! It took you %d turns");
                if (gameVictory && round == 0) Console.WriteLine("You just won a 1 to 6720 lottery... Could you try again so my programing isn't skipped by your luck? ...Thanks"); //i to se může stát

                playerInput = Console.ReadLine();

                if (PlayerInputIsCorrect(playerInput))
                {
                    RoundEvaluation(round, playerInput, ref savedGuesses, ref evaluation, colours);
                }
                else
                {
                    wrongInput = true;
                    round--;
                }
            }
        }
        static void FillArrays(ref char[,] savedGuesses, ref char[,] evaluation) //vyplneni pole 'x' znakem
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
        static void ShowGrid(char[,] savedGuesses, char[,] evaluation) //ukaze hrací plochu v realnem čase
        {
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
                Console.Write(" |\n");
            }
            Console.Write("\n\n");
        }

        static bool PlayerInputIsCorrect(string playerInput)
        {
            int number;

            if (playerInput.Length != 5) return false;
            if(Int32.TryParse(playerInput, out number))
            {
                for(int i = 0; i < 5; i++) //for cyklus pro převod jednotlivých čísel ze stringu do pole guess
                {
                    if (playerInput[i] < '1' || playerInput[i] > '8') return false; //ve hře jsou jen čísla 1 - 8 (barvy)
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        static void RoundEvaluation(int round, string playerInput, ref char[,] savedGuesses, ref char[,] evaluation, char[] colours)
        /* tato funkce vyhodnotí hráčovi čísla a následně řekne s jakými čísly ("barvami") se hráč strefil
           obě tyto informace dále zapíše do hracího pole (Grid)*/
        {
            int blackPin = 0; //na uhádnutí barvy na správném místě
            int whitePin = 0; //uhádnutí barvy ale se špatným umístěním
            for(int i = 0; i < 5; i++)
            {
                savedGuesses[round, i] = playerInput[i]; //převede číselnou hodnotu na char pro výpis
                for(int j = 0; j < 5; j++)
                {
                    if (playerInput[i] == colours[j])
                    {
                        if(i == j)
                        {
                            blackPin++;
                        }
                        else
                        {
                            whitePin++;
                        }
                    }
                }
            }
            for (int i = 4; i >= 0; i--)
            {
                if (blackPin > 0)
                {
                    blackPin--;
                    evaluation[round, i] = 'B';
                    continue;
                }
                if(whitePin > 0)
                {
                    whitePin--;
                    evaluation[round, i] = 'W';
                    continue;
                }
                evaluation[round, i] = '0';
            }
            round++;

        }

        static void ShowColours(int round, bool gameVictory, char[] colours)
        {
            if (!gameVictory || round == 9)
            {
                Console.Write("{0, 18}", "?????");
            }
            else
            {
                Console.Write("{0, 18}", colours);
            }
        }
    }
}
