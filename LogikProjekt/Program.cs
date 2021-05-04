using System;
using System.IO;

namespace LogikProjekt
{
    class Program
    {

        static void Main(string[] args)
        {
            ShowInitGreetings(); //první obrazovka co hráč vidí

            bool mistake = false;
            bool end = false;
            string choice;
            
            while (!end) //cyklus, který ukažuje hlavní menu
            {
                Console.Clear();                
                choice = ShowMainMenu(mistake); //tato funkce ukáže menu
                mistake = DoAskedChoice(choice, ref end); //zjistí co si hráč z menu vybral, jinak vypíše chybovou hlášku
            }
            Console.WriteLine("exiting program");
            Console.Read();

        }
         static void ShowInitGreetings()
            /*zobrazí se jednou při zapnutí hry*/
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
            /*metoda zobrazí hlavní nabídku menu*/
        {
            string ret;
            Console.WriteLine(); //pro lepsi vyhled jedno odradkovani
            Console.WriteLine("\n\n\t\t1. New game");
            Console.WriteLine("\t\t2. Load"); // nahraje ze slozky posledni ulozenou hru
            Console.WriteLine("\t\t3. Scoreboard"); // ukaze 10 nejlepsich zapsanych her
            Console.WriteLine("\t\t4. Game rules"); // vysvetli hru
            Console.WriteLine("\t\t5. End program");
            Console.WriteLine("\n\n\t\tType number [1-5] or the name of your choice.");
            if (mistake == true) Console.WriteLine("\n\t\tInput invalid. Please try again."); //řekne hráči pokud má špatný vstup
            ret = Console.ReadLine();
            return ret;
        }
        static bool DoAskedChoice(string choice, ref bool end)
            /*zjisti výběr hráče v menu a spustí žádanou funkci*/
        {
            if (choice == "1" || choice.Equals("new game", StringComparison.InvariantCultureIgnoreCase)) 
            {
                Console.Clear();
                StartNewGame(); //začne novou hru
                return false;
            }
            if (choice == "2" || choice.Equals("load", StringComparison.InvariantCultureIgnoreCase))
            {
                loadFromFile(); //načte a následně začne načtenou hru
                return false;
            }
            if (choice == "3" || choice.Equals("scoreboard", StringComparison.InvariantCultureIgnoreCase))
            {
                showScoreboard(); //ukáže score board, pokud existuje
                return false;
            }
            if (choice == "4" || choice.Equals("game rules", StringComparison.InvariantCultureIgnoreCase))
            {
                ShowRules(); //ukáže pravidla hry mastermind
                return false;
            }
            if (choice == "5" || choice.Equals("end", StringComparison.InvariantCultureIgnoreCase))
            {
                end = true;
                return false;
            }

                return true;
        }
        static void ShowRules()
            /*slouží jen na zobrazení pravidel hry mastermind*/
        {
            Console.Clear();
            Console.WriteLine("In this version of Mastermind you are trying to guess number code instead of colours. In other words, colours are represented by a number.");
            Console.WriteLine("there are 8 numbers(colours) [1 - 8]. The numbers in code do not duplicate.");
            Console.WriteLine("There are 10 rounds. In each round you write your guess. The guess is 5 digit number. Order of numbers is important!");
            Console.WriteLine("After writing your guess the game evaluates your code with 0 or W or B. The evaluation is shown in the left side next to your guess.");
            Console.WriteLine("For each number you wrote in correct place there will be 'B' evaluation.");
            Console.WriteLine("For each number you wrote and is in the code, just not in the same place there will be evaluation 'W'.");
            Console.WriteLine("For each number you wrote but is not in the code at all, there will be evaluation '0'.");
            Console.WriteLine("After 10 rounds or after you make right guess the \"?????\" will show the right code instead of question marks.");
            Console.WriteLine();
            Console.WriteLine("During the game you can enter \"save\" instead of 5 digit nuber, which lets you save your game.");
            Console.WriteLine("There is only one save slot so saving overwrites any saved game.");
            Console.WriteLine();
            Console.WriteLine("If you manage to guess the code successefully and win. The game will ask if you wish to save your score (score means number of rounds).");
            Console.WriteLine("In scoreboard are 10 players with the best score. Even if you chose to save your score, you may not make it to scoreboard");
            Console.WriteLine("\npress enter to return to main menu");
            Console.ReadLine();
        }
        static void StartNewGame()
        /*začne novou hru mastermind
         používá ShowGrid, SaveToScoreboard, SaveGameToFile, PlayerInputIsCorrect, RoundEvaluation*/
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

            for(int round = 0; round < 11; round++) //10 kol + 1 na vyhodnocení
            {
                Console.Clear();
                Console.WriteLine();
                ShowGrid(gameEnd, code, savedGuesses, evaluation);
                if (wrongInput)
                {
                    Console.WriteLine("Wrong Input. Please enter 5 digits in range of 1 to 8");
                    wrongInput = false;
                }
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
                    SaveToScoreboard(round + 1);//zeptá se hráče, jestli chce uložit, za kolik kol kód uhádl
                    Console.WriteLine("\npress enter to return to main menu");
                    Console.ReadLine();
                    return;
                }
                if (round == 10)//ukončí herní cyklus, jelikož hráč využil všech 10 tahů
                {
                    break;
                }
                /*pro kontrolu kódu během hry
                 * pro zjištění jestli kód generuje a ohodnocuje správně*/
                Console.Write("psst, the code is: ");
                for(int k = 0; k < 5; k++)
                {
                    Console.Write(code[k]);
                }
                
                Console.WriteLine();
                playerInput = Console.ReadLine();

                if (PlayerInputIsCorrect(playerInput, ref save))
                {
                    if (save)
                    {
                        if (SaveGameToFile(round, code, savedGuesses, evaluation))//vrací true pokud hráč chce uložit
                        {
                            return;
                        }
                        else
                        {
                            round--;
                            save = false;
                            continue;
                        }
                    }
                    RoundEvaluation(ref gameEnd, round, playerInput, ref savedGuesses, ref evaluation, code);// ohodnocení, hlavní mechanika hry
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
            Console.WriteLine("You lost!\n");
            Console.WriteLine("press enter to exit to main menu");
            Console.ReadLine();

        }
        static void FillArrays(ref char[,] savedGuesses, ref char[,] evaluation) 
            /*vyplní pole na začátku StartNewGame*/
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
        static void ShowGrid(bool gameVictory, int[] code, char[,] savedGuesses, char[,] evaluation)
            /*funkce slouží k zobrazení hracího pole s čísly co hráč hádal a ohodnocením odhadů*/
        {
            Console.WriteLine();
            if (!gameVictory) //na konci hry zajistí aby se kód odhalil hráči
            {
                Console.Write("\t{0, 18}\n", "?????");
            }
            else
            {
                Console.Write("\t{0, 14}", code[0]);
                for (int i = 1; i < 5; i++)
                {
                    Console.Write(code[i]);
                }
                Console.WriteLine();
            }

            for (int i = 0; i < 10; i++) //10 radku se hrou
            {
                Console.Write("\t{0, 2}.| ", i + 1); //očíslování řádku
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
            int number;//jen pro out u TryParse, není využito
            if(playerInput == "save")
            {
                save = true;
                return true;
            }
            if (playerInput.Length != 5) return false;
            if(Int32.TryParse(playerInput, out number))
            {
                for(int i = 0; i < 5; i++) //for cyklus pro kontrolu jednotlivých čísel na vstupu
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
        /* tato funkce vyhodnotí hráčova čísla (která hádal) a následně řekne s jakými čísly ("barvami") se hráč strefil
           obě tyto informace dále zapíše do hracího pole (Grid)*/
        {
            char[] colours = new char[5]; //na převod code do charu
            int blackPin = 0; //na uhádnutí barvy na správném místě
            int whitePin = 0; //uhádnutí barvy ale se špatným umístěním
            for(int i = 0; i < 5; i++)//cyklus pro připočtení kolíčků (pinů)
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
            for (int i = 4; i >= 0; i--) //naplní pole s ohodnocením W/B/0
            {
                if (blackPin > 0)
                {
                    blackPin--;
                    evaluation[round, i] = 'B';//"trefa"
                    continue;
                }
                if(whitePin > 0)
                {
                    whitePin--;
                    evaluation[round, i] = 'W';//"vedle"
                    continue;
                }
                evaluation[round, i] = '0';//"číslo není v kódu"
            }

        }
        static bool SaveGameToFile(int round, int[] code, char[,]savedGuess, char[,] evaluation)
            /*stará se o zápis nebo přepis souboru s uloženou hrou
             metoda se StreamWriter*/
        {
            Console.WriteLine("Reminder: Save will overwrite any already saved game");
            Console.WriteLine("Do you wish to save current progress? write 'y' for yes, for no anything else.");
            if(Console.ReadLine().Equals("y", StringComparison.InvariantCultureIgnoreCase))
            {
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
        /*celá funkce si zakládá na tom, že v souboru nikdo neprováděl změny manuálně
         načte ze souboru uloženou hru. naplní herní proměnné ze souboru
        používá StartLoadedGame*/
        {
            if (!File.Exists("saveFile")) //pokud soubor s uložením neexistuje, není co načítat
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
            line = saveFile.ReadLine();//pro kolo
            round = Int16.Parse(line);
            line = saveFile.ReadLine();//pro kód na uhádnutí
            for (int i = 0; i < 5; i++)
            {
                code[i] = line[i] - 48; //-48 pro převod z int na char pomocí ascii tabulky
            }
            for (int i = 0; i < 10; i++)
            {
                line = saveFile.ReadLine();// pro hrací pole
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
            StartLoadedGame(round, code, savedGuesses, evaluation); //spustí hru s načtenými informacemi
            return;
        }
        static void StartLoadedGame(int turn, int[] code, char[,] savedGuesses, char[,] evaluation)
        /*v podstatě je to přebraná funkce StartNewGame, akorát nebude zapisovat do scoreboard. Vynechá počáteční deklarace, generaci kódu a možnost uložit výsledek do scoreboard
         používá ShowGrid, PlayerInputIsCorrect, SaveGameToFile, RoundEvaluation*/
        {
            string playerInput; //vstup od hráče
            
            bool wrongInput = false;
            bool gameEnd = false;
            bool save = false;

            for (int round = turn; round < 11; round++) //10 kol + 1 na kontrolu 10. kola
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
                        Console.WriteLine("Heh nice try, but this doesn't count\nsince you loaded saved game and might already know from previous tries."); //pro chytrálky co načtou hru a na prvním kole, přičemž zjistili kód z předešlích pokusů na uloženém souboru
                        Console.WriteLine("SHAME");

                    }
                    else
                    {
                        Console.WriteLine("You won! It took you {0} turns", round + 1);//round + 1 vzhledem k přístupu do pole
                    }
                    Console.WriteLine("\npress enter to return to main menu");
                    Console.ReadLine();
                    return;
                }
                if (round == 10)//ukončí herní cyklus, jelikož hráč využil všech 10 tahů
                {
                    break;
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

                if (PlayerInputIsCorrect(playerInput, ref save)) //vrátí true, pokud je vstup od hráče validní
                {
                    if (save)//pokud hráč zadal save
                    {
                        if (SaveGameToFile(round, code, savedGuesses, evaluation)) 
                        {
                            return; //hra uložena, ukončí hru
                        }
                        else //po rozmyšlení hráče pro neuložení
                        {
                            round--;
                            save = false;
                            continue;
                        }
                    }
                    RoundEvaluation(ref gameEnd, round, playerInput, ref savedGuesses, ref evaluation, code);//vyhodnotí hráčův tip
                }
                else
                {
                    wrongInput = true;
                    round--;//odečte 1 kolo, pokud byl vstup špatný, aby dal hráči další pokus
                }
            }
            Console.Clear();
            gameEnd = true;// skončilo 10 pokusů hráče
            ShowGrid(gameEnd, code, savedGuesses, evaluation);
            Console.WriteLine("You lost!\n");
            Console.WriteLine("press enter to exit to main menu");
            Console.ReadLine();

        }

        static void SaveToScoreboard(int round)
            /*metoda nepočítá s manuálním zásahem do souboru z vnější
             metoda s streamWriter a streamReader
            zapisuje jen hráče se score nejhůř stejným jako ve scoreboard jestliže je scoreboard plný*/
        {
            Console.WriteLine("\nDo you wish to save the number of rounds that took you to win the game to scoreboard? ({0})", round);
            Console.WriteLine("Write 'y' for yes, for no write anything else.");
            if (Console.ReadLine().Equals("y", StringComparison.InvariantCultureIgnoreCase))
            {
                bool wrongInput = false;
                string nickname;
                do //cyklus na správný vstup
                {
                    Console.Clear();
                    if (wrongInput)
                    {
                        Console.Clear();
                        Console.WriteLine("Wrong input detected. Choose other Nickname\n");
                    }
                    Console.WriteLine("Select a nickname, that will be saved alongside the number of rounds. nickname can have maximum of 10 characters");
                    Console.Write("Please enter your nickname: ");
                    nickname = Console.ReadLine();
                    if(nickname.Length > 10 || nickname == null) // kotrola vstupu od uživatele
                    {
                        wrongInput = true;
                    }
                    else
                    {
                        wrongInput = false;
                    }
                } while (wrongInput);

                if(!File.Exists("scoreboard"))//první zápis, takže vytvoření souboru
                {
                    StreamWriter scoreBoard = new StreamWriter("scoreboard", false);
                    scoreBoard.WriteLine(nickname);
                    scoreBoard.WriteLine(round);
                    scoreBoard.Flush();
                    scoreBoard.Close();
                    Console.WriteLine("\ngame saved to score board");
                    return;
                }

                int numOfNicknames = 0;
                StreamReader scoreBoardCheck = new StreamReader("scoreboard", false);
                while(scoreBoardCheck.ReadLine() != null)
                {
                    scoreBoardCheck.ReadLine();// jeden zápis zabere dva řádky
                    numOfNicknames++;
                }
                scoreBoardCheck.Close();
                if (numOfNicknames < 10) //změna čísla určí počet nejlepších score ve score board
                {
                    StreamWriter scoreBoard = new StreamWriter("scoreboard", true);
                    scoreBoard.WriteLine(nickname);
                    scoreBoard.WriteLine(round);
                    scoreBoard.Flush();
                    scoreBoard.Close();
                    Console.WriteLine("game saved to score board");
                    return;
                }
                else
                {
                    StreamReader scoreBoardCheck2 = new StreamReader("scoreboard", false);
                    int worstGame = 1;//nejmenší počet kola je 1
                    int worstLine = 0;//řádek s nejhorším score
                    string[] lines = new string[numOfNicknames * 2]; //pro práci se souborem
                    for (int i = 0; i < numOfNicknames * 2; i++)
                    {
                        lines[i] = scoreBoardCheck2.ReadLine();
                    }
                    for (int i = 1; i < numOfNicknames * 2; i += 2)//hledá čísla, která mají v poli liché indexy
                    {
                        if (worstGame < Int16.Parse(lines[i]))
                        {
                            worstGame = Int16.Parse(lines[i]);
                            worstLine = i - 1;//najde řádek s nickname hráče s nejhorším score
                        }
                    }
                    scoreBoardCheck2.Close();
                    if (round > worstGame)
                    {
                        Console.Clear();
                        Console.WriteLine("Sorry, you didn't make it to the scoreboard.");//ve score board jsou hry s lepšími score, takže se na něj hráč nedostal
                        return;
                    }
                    for (int i = worstLine; i < (numOfNicknames * 2) - 2; i += 2)//cyklus vyřadí hráče s nejstarším a nejhorším score a posune zbytek hráčů pro logiku nejstaršího score
                    {
                        lines[i] = lines[i + 2];
                        lines[i + 1] = lines[i + 3];
                    }
                    lines[(numOfNicknames * 2) - 2] = nickname;//poslední uvolněné místo připadne novému hráči
                    lines[(numOfNicknames * 2) - 1] = "" + round;//převede int round na string a zapíše do souboru

                    StreamWriter scoreBoard = new StreamWriter("scoreboard", false);
                    for(int i = 0; i < numOfNicknames * 2; i++)
                    {
                        scoreBoard.WriteLine(lines[i]);
                    }
                    scoreBoard.Flush();
                    scoreBoard.Close();
                }
            }
            return;
        }

        static void showScoreboard()
            /*metoda se stará o zobrazení scoreboard, jestli už nějaký hráč zapsal svoje score
             pouze zobrazuje
            metoda se StreamReader*/
        {
            Console.Clear();
            if (!File.Exists("scoreboard"))//kontrola existence souboru
            {
                Console.WriteLine("\t\tthere are no players in scoreboard yet\n");
                Console.WriteLine("\t\tpress enter to return to main menu");
                Console.ReadLine();
                return;
            }

            StreamReader scoreBoard = new StreamReader("scoreboard", false);// pro zjištění počtu řádků v souboru
            int numOfLines = 0;
            while(scoreBoard.ReadLine() != null)
            {
                numOfLines++;
            }
            scoreBoard.Close();

            StreamReader scoreBoard1 = new StreamReader("scoreboard", false);//pro přečtení řádků ze souboru a uložení do proměnné
            string[] lines = new string[numOfLines];
            for(int i = 0; i < numOfLines; i++)
            {
                lines[i] = scoreBoard1.ReadLine();
            }
            scoreBoard1.Close();

            string[] sortedLines = new string[numOfLines];//proměná pro seřazení řádků podle scóre
            int order = 0;
            for (int i = 1; i < 11; i++ )//seřadí hráče podle scóre
            {
                for (int j = 1; j < numOfLines; j += 2)//číslo na každém druhém řádku
                {
                    if(Int16.Parse(lines[j]) == i)//seřadí od nejlepšího scóre
                    {
                        sortedLines[order] = lines[j - 1];
                        sortedLines[order + 1] = lines[j];
                        order += 2;
                    }
                }

            }

            Console.Write("{0, 11} |{1, 7}\n", "nickname", "rounds");
            for(int i = 0; i < numOfLines; i++)//vypíše seřazenou tabulku scoreboard
            {
                Console.Write("{0, 11} |", sortedLines[i]);
                i++;
                Console.Write("{0, 2}\n", sortedLines[i]);
            }
            Console.WriteLine("\npress enter to return to main menu");
            Console.ReadLine();
        }
    }
}
