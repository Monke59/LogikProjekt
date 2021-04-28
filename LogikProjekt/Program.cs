using System;
using System.IO;

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
                choice = ShowMainMenu(mistake); //shows main menu with choices
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
            lines[2] = "Press enter to continue.";
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
        static string ShowMainMenu(bool mistake)
        {
            string ret;
            Console.WriteLine(); //pro lepsi vyhled jedno odradkovani
            Console.WriteLine("\n\n\t\t1. New game");
            Console.WriteLine("\t\t2. Load"); // nahraje ze slozky posledni ulozenou hru
            Console.WriteLine("\t\t3. Scoreboard"); // ukaze 10 nejlepsich !ulozenych! her
            Console.WriteLine("\t\t4. Game rules"); // vysvetli hru
            Console.WriteLine("\t\t5. End program");
            //Console.Write("\n\n");
            Console.WriteLine("\n\n\t\tType number [1-4] or the name of your choice.");
            if (mistake == true) Console.WriteLine("\n\t\tInput invalid. Please try again."); //says when player has wrong input
            ret = Console.ReadLine();
            return ret;
        }
        static bool DoAskedChoice(string choice, ref bool end) //zjisti vstup od hrace v menu
        {
            if (choice == "1" || choice.Equals("new game", StringComparison.InvariantCultureIgnoreCase)) 
            {
                Console.Clear();
                StartNewGame();
                return false;
            }
            if (choice == "2" || choice.Equals("load", StringComparison.InvariantCultureIgnoreCase))
            {
                loadFromFile();
                return false;
            }
            if (choice == "3" || choice.Equals("scoreboard", StringComparison.InvariantCultureIgnoreCase)) ;
                /*ShowScoreboard*/
            if (choice == "4" || choice.Equals("game rules", StringComparison.InvariantCultureIgnoreCase))
            {
                    ShowRules();
            }
            if (choice == "5" || choice.Equals("end", StringComparison.InvariantCultureIgnoreCase))
            {
                end = true;
                return false;
            }

                return true;
        }
        static void ShowRules()
        {
            Console.Clear();
            Console.WriteLine("In this version of Mastermind you are trying to guess number code instead of colours. In other words, colours are represented by a number");
            Console.WriteLine("there are 8 numbers(colours) [1 - 8]. The numbers in code do not duplicate.");
            Console.WriteLine("There are 10 rounds. In each round you write your guess. The guess is 5 digit number. Order of numbers is important!");
            Console.WriteLine("After writing your guess the game evaluates your code with 0 or W or B. The evaluation is shown in the left side next to your guess.");
            Console.WriteLine("For each number you wrote in correct place there will be 'B' evaluation.");
            Console.WriteLine("For each number you wrote and is in the code, just not in the same place there will be evaluation 'W'.");
            Console.WriteLine("For each number you wrote but is not in the code at all, there will be evaluation '0'.");
            Console.WriteLine("After 10 rounds or after you make right guess the \"?????\" will show the right code instead of question marks.");
            Console.WriteLine("during the game you can enter \"save\" instead of 5 digit nuber which saves your game");
            Console.WriteLine("There is only one save slot so be careful.");
            Console.WriteLine("\npress enter to return to main menu");
            Console.ReadLine();
        }
        static void StartNewGame()
        {
            char[,] savedGuesses = new char[10,5];
            char[,] evaluation = new char[10,5];
            FillArrays(ref savedGuesses, ref evaluation);

            Random rd = new Random();
            int[] code = new int[5]; //5 čísel z random generátoru pro hráče na uhodnutí
            string playerInput = ""; //vstup od hráče

            bool wrongInput = false;
            bool gameEnd = false;
            bool save = false;

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
            }

            for(int round = 0; round < 10; round++) //10 kol
            {
                Console.Clear();
                Console.WriteLine();
                ShowGrid(gameEnd, code, savedGuesses, evaluation);

                if (wrongInput) Console.WriteLine("Wrong Input. Please enter 5 digits in range of 1 to 8");
                if (gameEnd)
                {
                    round--; //aby se round nastavilo na kolo kde hráč uhádl
                    if(round == 0) //i tose může stát
                    {
                        Console.WriteLine("You just won a 1 to 6720 lottery... Could you try again so my programing isn't skipped by your luck? ...Thanks");
                    }
                    else
                    {
                        Console.WriteLine("You won! It took you {0} turns", round + 1);//round + 1 vzhledem k přístupu do pole
                    }
                    Console.WriteLine("press enter to return to main menu");
                    Console.ReadLine();
                    return;
                }
                /*pro debugging hry nebo podvod
                Console.Write("psst, the code is:");
                for(int k = 0; k < 5; k++)
                {
                    Console.Write(code[k]);
                }
                */
                Console.WriteLine();
                playerInput = Console.ReadLine();

                if (PlayerInputIsCorrect(playerInput, ref save))
                {
                    if (save)
                    {
                        if (SaveGameToFile(round, code, savedGuesses, evaluation)) return;
                    }
                    RoundEvaluation(ref gameEnd, round, playerInput, ref savedGuesses, ref evaluation, code);
                }
                else
                {
                    wrongInput = true;
                    round--;
                }
            }
            Console.Clear();
            gameEnd = true;// skončilo 10 pokusů hráče
            ShowGrid(gameEnd, code, savedGuesses, evaluation);
            Console.WriteLine("press enter to exit to main menu");
            Console.ReadLine();

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
        static void ShowGrid(bool gameVictory, int[] code, char[,] savedGuesses, char[,] evaluation) //ukáže hrací plochu v reálnem čase
        {
            Console.WriteLine();
            if (!gameVictory)
            {
                Console.Write("{0, 18}\n", "?????");
            }
            else
            {
                Console.Write("{0, 14}", code[0]);
                for (int i = 1; i < 5; i++) //výpis pro zbytek kódu jelikož je to int array
                {
                    Console.Write(code[i]);
                }
                Console.WriteLine();
            }

            for (int i = 0; i < 10; i++) //10 radku se hrou
            {
                Console.Write("{0, 2}.| ", i + 1); //očíslování řádku
                for (int j = 0; j < 5; j++)
                {
                    Console.Write(evaluation[i, j]); //pole s W/B ohodnocením
                }
                Console.Write(" | ");
                for (int j = 0; j < 5; j++)
                {
                    Console.Write(savedGuesses[i, j]); //pole s kódy hráče
                }
                Console.Write(" |\n");
            }
            Console.Write("\n\n");
        }

        static bool PlayerInputIsCorrect(string playerInput, ref bool save)
        {
            int number;

            if(playerInput == "save")
            {
                save = true;
                return true;
            }

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

        static void RoundEvaluation(ref bool gameVictory, int round, string playerInput, ref char[,] savedGuesses, ref char[,] evaluation, int[] code)
        /* tato funkce vyhodnotí hráčovi čísla a následně řekne s jakými čísly ("barvami") se hráč strefil
           obě tyto informace dále zapíše do hracího pole (Grid)*/
        {
            char[] colours = new char[5]; //na převod code do charu
            int blackPin = 0; //na uhádnutí barvy na správném místě
            int whitePin = 0; //uhádnutí barvy ale se špatným umístěním
            for(int i = 0; i < 5; i++)
            {
                savedGuesses[round, i] = playerInput[i]; //převede číselnou hodnotu na char pro výpis
                for(int j = 0; j < 5; j++)// projede číslo od hráče a porovná s kódem
                {
                    colours[j] = Convert.ToChar(code[j] + 48);
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
            if(blackPin == 5) //5 černých znamená uhádnutí všech 5 čísel
            {
                gameVictory = true;
            }
            for (int i = 4; i >= 0; i--) //naplní pole s ohodnocením W/B
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
        static bool SaveGameToFile(int round, int[] code, char[,]savedGuess, char[,] evaluation)
        {
            Console.WriteLine("Reminder: Save will overwrite any already saved game");
            Console.WriteLine("Do you wish to save current progress? [y/n]");
            if(Console.ReadLine().Equals("y", StringComparison.InvariantCultureIgnoreCase)){
                StreamWriter saveFile = new StreamWriter("saveFile", false);

                saveFile.WriteLine(round);
                for(int i = 0; i < 5; i++)
                {
                    saveFile.Write(code[i]);
                }
                saveFile.WriteLine();
                for (int i = 0; i < 10; i++)
                {
                    for(int j = 0; j < 5; j++)
                    {
                        saveFile.Write(evaluation[i, j]);                        
                    }
                    for(int j = 0; j < 5; j++)
                    {
                        saveFile.Write(savedGuess[i, j]);
                    }
                    saveFile.Write("\n");
                }
                saveFile.Flush();
                saveFile.Close();
                return true;
            }
            return false;
        }
        static void loadFromFile()
        {
            if (!File.Exists("saveFile"))
            {
                Console.WriteLine("There is no saved game");
                Console.WriteLine("press enter to return to main menu");
                Console.ReadLine();
                return;
            }
            char[,] savedGuesses = new char[10, 5];
            char[,] evaluation = new char[10, 5];
            int[] code = new int[5];
            int round;
            string line;
            StreamReader saveFile = new StreamReader("saveFile");
            line = saveFile.ReadLine();
            round = Int16.Parse(line);
            line = saveFile.ReadLine();
            for (int i = 0; i < 5; i++)
            {
                code[i] = line[i] - 48; //-48 pro převod z int na char pomocí ascii tabulky
            }
            for (int i = 0; i < 10; i++)
            {
                line = saveFile.ReadLine();
                for (int j = 0; j < 5; j++)
                {
                    evaluation[i, j] = line[j];
                }
                for (int j = 0; j < 5; j++)
                {
                    savedGuesses[i, j] = line[j + 5];
                }
            }
            saveFile.Close();
            //ShowGrid(true, code, savedGuesses, evaluation);
            StartLoadedGame(round, code, savedGuesses, evaluation);
            return;
        }
        static void StartLoadedGame(int turn, int[] code, char[,] savedGuesses, char[,] evaluation)
        {
            string playerInput = ""; //vstup od hráče

            bool wrongInput = false;
            bool gameEnd = false;
            bool save = false;

            for (int round = turn; round < 10; round++) //10 kol
            {
                Console.Clear();
                Console.WriteLine();
                ShowGrid(gameEnd, code, savedGuesses, evaluation);

                if (wrongInput) Console.WriteLine("Wrong Input. Please enter 5 digits in range of 1 to 8");
                if (gameEnd)
                {
                    round--; //aby se round nastavilo na kolo kde hráč uhádl
                    if (round == 0) //i tose může stát
                    {
                        Console.WriteLine("Heh nice try, but this doesn't count\nsince you loaded saved game and might already know from previous tries.");
                        Console.WriteLine("SHAME");

                    }
                    else
                    {
                        Console.WriteLine("You won! It took you {0} turns", round + 1);//round + 1 vzhledem k přístupu do pole
                    }
                    Console.WriteLine("press enter to return to main menu");
                    Console.ReadLine();
                    return;
                }
                /* pro debugging hry nebo podvod
                Console.Write("psst, the code is:");
                for (int k = 0; k < 5; k++)
                {
                    Console.Write(code[k]);
                }
                */
                Console.WriteLine();
                playerInput = Console.ReadLine();

                if (PlayerInputIsCorrect(playerInput, ref save))
                {
                    if (save)
                    {
                        if (SaveGameToFile(round, code, savedGuesses, evaluation)) return;
                    }
                    RoundEvaluation(ref gameEnd, round, playerInput, ref savedGuesses, ref evaluation, code);
                }
                else
                {
                    wrongInput = true;
                    round--;
                }
            }
            Console.Clear();
            gameEnd = true;// skončilo 10 pokusů hráče
            ShowGrid(gameEnd, code, savedGuesses, evaluation);
            Console.WriteLine("press enter to exit to main menu");
            Console.ReadLine();

        }
    }
}
