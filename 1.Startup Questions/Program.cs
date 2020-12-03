using System;
using System.IO;

namespace WordSearch_Assignment
{
    struct WordSearch
    {
        public int Rows;
        public int Cols;
        public int NumWords;

        public string[] Words;
        public int[] Fcoord;
        public int[] Scoord;
        public string[] direction;
    }
    class Program
    {
        /// <summary>
        /// Method takes in a question and outputs the options as a menu.
        /// </summary>
        /// <param name="pQuestion"></param>
        /// <param name="pOptions"></param>
        /// <returns></returns>
        static int GetSelectionFromMenu(string pQuestion, string[] pOptions)
        {
            int selection;

            Console.WriteLine(pQuestion);

            for (int i = 0; i < pOptions.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + pOptions[i]);
            }

            selection = int.Parse(Console.ReadLine());

            return selection;
        }


        static void Main(string[] args)
        {
            WordSearch wordSearch;
            wordSearch.Rows = 0;
            wordSearch.Cols = 0;
            wordSearch.NumWords = 0;
            wordSearch.Words = null;
            wordSearch.Fcoord = null;
            wordSearch.Scoord = null;
            wordSearch.direction = null;

            bool loadSelectionContinue = false;
            bool fileSelectionContinue = false;
            int intLineValue = 0;
            int intLineValue2 = 0;

            //LOAD UP QUESTIONS

            Console.WriteLine("Welcome to WordSearch World!\n");

            while (loadSelectionContinue == false)
            {
                string[] loadOptions = new string[2];
                loadOptions[0] = "Default wordsearch";
                loadOptions[1] = "Load wordsearch file";

                int loadselection = GetSelectionFromMenu("Would you like to use the default wordsearch or load from a file?\n", loadOptions);

                if (loadselection == 1)
                {
                    Console.WriteLine("Default wordsearch selected");
                    wordSearch.Rows = 10;
                    wordSearch.Cols = 6;
                    wordSearch.NumWords = 2;

                    wordSearch.Words = new string[2];
                    wordSearch.direction = new string[2];
                    wordSearch.Fcoord = new int[2];
                    wordSearch.Scoord = new int[2];

                    wordSearch.Words[0] = "algorithm";
                    wordSearch.Words[1] = "virus";
                    wordSearch.direction[0] = "right";
                    wordSearch.direction[1] = "left";
                    wordSearch.Fcoord[0] = 1;
                    wordSearch.Fcoord[1] = 4;
                    wordSearch.Scoord[0] = 0;
                    wordSearch.Scoord[1] = 5;

                    loadSelectionContinue = true;
                }
                else if (loadselection == 3)
                {
                    //load file (create a 2nd struct and copy 1st struct into 2nd)
                }
                else if (loadselection == 2)
                {
                    string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt");

                    while (fileSelectionContinue == false)
                    {
                        Console.Clear();
                        int fileSelected = GetSelectionFromMenu("Select from the available WordSearches:\n", files);

                        if (fileSelected <= files.Length)
                        {
                            StreamReader reader = new StreamReader(files[fileSelected - 1]);

                            int numlines = 0;
                            while (!reader.EndOfStream)
                            {
                                reader.ReadLine();
                                numlines++;
                            }
                            WordSearch[] filecontent = new WordSearch[numlines];

                            reader.Close();
                            reader = new StreamReader(files[fileSelected - 1]);

                            string line1 = reader.ReadLine();
                            string[] line1Values = line1.Split(',');
                            int[] intLine1Values = { '0', '0', '0' };

                            for (int i = 0; i < line1Values.Length; i++)
                            {
                                int intLine1Value = int.Parse(line1Values[i]);
                                intLine1Values[i] = intLine1Value;
                            }

                            wordSearch.Rows = intLine1Values[0] + 1;
                            wordSearch.Cols = intLine1Values[1] + 1;
                            wordSearch.NumWords = intLine1Values[2];

                            wordSearch.Scoord = new int[numlines - 1];
                            wordSearch.Fcoord = new int[numlines - 1];
                            wordSearch.Words = new string[numlines - 1];
                            wordSearch.direction = new string[numlines - 1];


                            for (int i = 1; i < numlines; i++)
                            {
                                string line = reader.ReadLine();
                                string[] values = line.Split(',');

                                wordSearch.Words[i - 1] = values[0];
                                intLineValue2 = int.Parse(values[1]);
                                wordSearch.Scoord[i - 1] = intLineValue2;
                                intLineValue = int.Parse(values[2]);
                                wordSearch.Fcoord[i - 1] = intLineValue;
                                wordSearch.direction[i - 1] = values[3];

                            }

                            reader.Close();
                            loadSelectionContinue = true;
                            fileSelectionContinue = true;

                        }
                        else { Console.Clear(); }
                    }
                }
                else { Console.Clear(); }

            }

            //WORDSEARCH GENERATOR


            char[,] wordsearchGrid = new char[wordSearch.Cols + 1, wordSearch.Rows + 1];

            Random rng = new Random();
            string weightedalphabet = "aabcdeefghiijkllmnooopqrsstttuvwxyz";


            for (int rows = 0; rows < wordsearchGrid.GetLength(1); rows++)
            {
                for (int columns = 0; columns < wordsearchGrid.GetLength(0); columns++)
                {
                    wordsearchGrid[columns, rows] = weightedalphabet[rng.Next(26)];
                }
            }

            //add grid labels

            char colNum = '0';
            char rowNum = '0';

            for (int colLabel = 0; colLabel < wordSearch.Cols; colLabel++)
            {
                wordsearchGrid[0, 0] = ' ';

                if (colLabel != 0)
                {
                    wordsearchGrid[colLabel, 0] = colNum;
                    colNum++;
                }
            }

            Console.WriteLine(wordSearch.Rows);

            for (int rowLabel = 0; rowLabel < wordSearch.Rows; rowLabel++)
            {
                wordsearchGrid[0, 0] = ' ';

                if (rowLabel != 0)
                {
                    wordsearchGrid[0, rowLabel] = rowNum;
                    rowNum++;
                }
            }

            Console.Clear();

            //find word lengths

            int highestwordlength = 0;
            int[] wordLengths = new int[wordSearch.NumWords + 1];
            int followingLetterCoordsLength = 0;
            wordLengths[0] = 0;

            for (int wordIteration = 1; wordIteration < wordSearch.NumWords; wordIteration++)
            {
                char[] characters = wordSearch.Words[wordIteration].ToCharArray();
                int wordLength = characters.Length;
                wordLengths[wordIteration] = wordLength;                                    // index out of bounds

                if (wordLength > highestwordlength)
                {
                    highestwordlength = wordLength;
                }
                followingLetterCoordsLength = followingLetterCoordsLength + wordLength;
            }


            //generate word coords

            int[] lastcoords = new int[wordSearch.NumWords * 4];
            int coordAdd = 0;

            for (int wordIteration = 0; wordIteration < wordSearch.NumWords; wordIteration++)
            {
                char[] characters = wordSearch.Words[wordIteration].ToCharArray();


                for (int letterIteration = 0; letterIteration < characters.Length; letterIteration++) //TO DO look at paramatizing later
                {
                    if (wordSearch.direction[wordIteration] == "right")
                    {
                        int coordChange = wordSearch.Scoord[wordIteration] + letterIteration;
                        wordsearchGrid[wordSearch.Fcoord[wordIteration] + 1, coordChange + 1] = characters[letterIteration];
                        if (letterIteration == characters.Length - 1)
                        {
                            lastcoords[wordIteration + coordAdd] = wordSearch.Fcoord[wordIteration];
                            lastcoords[wordIteration + 1 + coordAdd] = coordChange;
                        }
                    }
                    else if (wordSearch.direction[wordIteration] == "left")
                    {
                        int coordChange = wordSearch.Scoord[wordIteration] - letterIteration;
                        wordsearchGrid[wordSearch.Fcoord[wordIteration] + 1, coordChange + 1] = characters[letterIteration];
                        if (letterIteration == characters.Length - 1)
                        {
                            lastcoords[wordIteration + coordAdd] = wordSearch.Fcoord[wordIteration];
                            lastcoords[wordIteration + 1 + coordAdd] = coordChange;
                        }
                    }
                    else if (wordSearch.direction[wordIteration] == "up")
                    {
                        int coordChange = wordSearch.Fcoord[wordIteration] - letterIteration;
                        wordsearchGrid[coordChange + 1, wordSearch.Scoord[wordIteration] + 1] = characters[letterIteration];
                        if (letterIteration == characters.Length - 1)
                        {
                            Console.WriteLine(coordAdd);
                            Console.WriteLine(wordIteration);
                            lastcoords[wordIteration + coordAdd] = coordChange;
                            lastcoords[wordIteration + 1 + coordAdd] = wordSearch.Fcoord[wordIteration];
                        }
                    }
                    else if (wordSearch.direction[wordIteration] == "down")
                    {
                        int coordChange = wordSearch.Fcoord[wordIteration] + letterIteration;
                        wordsearchGrid[coordChange + 1, wordSearch.Scoord[wordIteration] + 1] = characters[letterIteration];
                        if (letterIteration == characters.Length - 1)
                        {
                            lastcoords[wordIteration + coordAdd] = coordChange;
                            lastcoords[wordIteration + 1 + coordAdd] = wordSearch.Fcoord[wordIteration];
                        }
                    }
                    else if (wordSearch.direction[wordIteration] == "leftup")
                    {
                        int coordChange = wordSearch.Fcoord[wordIteration] - letterIteration;
                        int coordChange2 = wordSearch.Scoord[wordIteration] - letterIteration;
                        wordsearchGrid[coordChange + 1, coordChange2 + 1] = characters[letterIteration];
                        if (letterIteration == characters.Length - 1)
                        {
                            lastcoords[wordIteration + coordAdd] = coordChange;
                            lastcoords[wordIteration + 1 + coordAdd] = coordChange2;
                        }
                    }
                    else if (wordSearch.direction[wordIteration] == "rightdown")
                    {
                        int coordChange = wordSearch.Fcoord[wordIteration] + letterIteration;
                        int coordChange2 = wordSearch.Scoord[wordIteration] + letterIteration;
                        wordsearchGrid[coordChange + 1, coordChange2 + 1] = characters[letterIteration];
                        if (letterIteration == characters.Length - 1)
                        {
                            lastcoords[wordIteration + coordAdd] = coordChange;
                            lastcoords[wordIteration + 1 + coordAdd] = coordChange2;
                        }
                    }
                    else if (wordSearch.direction[wordIteration] == "leftdown")
                    {
                        int coordChange = wordSearch.Fcoord[wordIteration] + letterIteration;
                        int coordChange2 = wordSearch.Scoord[wordIteration] - letterIteration;
                        wordsearchGrid[coordChange + 1, coordChange2 + 1] = characters[letterIteration];
                        if (letterIteration == characters.Length - 1)
                        {
                            lastcoords[wordIteration + coordAdd] = coordChange;
                            lastcoords[wordIteration + 1 + coordAdd] = coordChange2;
                        }
                    }
                    else if (wordSearch.direction[wordIteration] == "rightup")
                    {
                        int coordChange = wordSearch.Fcoord[wordIteration] - letterIteration;
                        int coordChange2 = wordSearch.Scoord[wordIteration] + letterIteration;
                        wordsearchGrid[coordChange + 1, coordChange2 + 1] = characters[letterIteration];
                        if (letterIteration == characters.Length - 1)
                        {
                            lastcoords[wordIteration + coordAdd] = coordChange;
                            lastcoords[wordIteration + 1 + coordAdd] = coordChange2;
                        }
                    }
                }
                coordAdd = coordAdd + 2;
            }

            //SOLVING WORDSEARCH

            int correctWords = 0;
            char[,] referenceGrid = new char[wordSearch.Cols + 1,wordSearch.Rows + 1];

            for (int rows = 0; rows < referenceGrid.GetLength(1); rows++)
            {
                for (int columns = 0; columns < referenceGrid.GetLength(0); columns++)
                {
                    referenceGrid[columns, rows] = weightedalphabet[rng.Next(26)];
                }
            }

            int[] foundwords = new int[wordSearch.NumWords];

            while (correctWords != wordSearch.NumWords) //
            {
                //display grid

                Console.Clear();

                for (int k = 0; k < wordSearch.Cols; k++)
                {
                    for (int j = 0; j < wordSearch.Rows; j++)
                    {
                        if (j == 0 || k == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(wordsearchGrid[k, j]);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (referenceGrid[k, j] == '*')
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(wordsearchGrid[k, j]);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (referenceGrid[k, j] == '!')
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(wordsearchGrid[k, j]);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.Write(wordsearchGrid[k, j]);
                        }
                    }
                    Console.WriteLine();
                }


                //display words to find

                Console.WriteLine("\nWords to find:");

                for (int wordIteration = 0; wordIteration < wordSearch.NumWords; wordIteration++)
                {
                    if (foundwords[wordIteration] == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(wordSearch.Words[wordIteration]);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.WriteLine(wordSearch.Words[wordIteration]);
                    }
                }

                //choose to save

                string[] loadOptions2 = new string[2];
                loadOptions2[0] = "Continue";
                loadOptions2[1] = "Save for later";

                int loadselection2 = GetSelectionFromMenu("\nWould you like to continue or save your progress?\n", loadOptions2);

                if (loadselection2 == 2)
                {
                    // save progress
                }
                else if (loadselection2 == 1)
                {
                    //test
                    //test

                    //enter coordinates

                    Console.WriteLine("\nPlease input a coord for the row of the 1st letter of the word.");
                    string FLetRow = Console.ReadLine();
                    int intFirstletterYcoord = int.Parse(FLetRow) + 1;

                    Console.WriteLine("\nPlease input a coord for the column of the 1st letter of the word.");
                    string FLetCol = Console.ReadLine();
                    int intFirstletterXcoord = int.Parse(FLetCol);

                    Console.WriteLine("\nPlease input a coord for the row of the last letter of the word.");
                    string LLetRow = Console.ReadLine();
                    int intLastLetterYcoord = int.Parse(LLetRow) + 1;

                    Console.WriteLine("\nPlease input a coord for the column of the last letter of the word.");
                    string LLetCol = Console.ReadLine();
                    int intLastLetterXcoord = int.Parse(LLetCol);

                    //check words

                    for (int checkiteration = 0; checkiteration <= wordSearch.NumWords - 1; checkiteration++) 
                    {
                        if (intFirstletterYcoord == wordSearch.Fcoord[checkiteration] - 1 && intFirstletterXcoord == wordSearch.Scoord[checkiteration])
                        {
                            int addcoord = 0;
                            for (int doublecheckiteration = 0; doublecheckiteration < lastcoords.Length - 1; doublecheckiteration++)
                            {

                                if (intLastLetterYcoord == lastcoords[addcoord + checkiteration] && intLastLetterXcoord == lastcoords[addcoord + 1 + checkiteration])
                                {

                                    for (int greenletters = 0; greenletters < wordSearch.Words[checkiteration].Length; greenletters++)
                                    {
                                        if (wordSearch.direction[checkiteration] == "right")
                                        {
                                            referenceGrid[wordSearch.Fcoord[checkiteration] + 1, wordSearch.Fcoord[checkiteration] + greenletters] = '*';
                                        }
                                        else if (wordSearch.direction[checkiteration] == "left")
                                        {
                                            referenceGrid[wordSearch.Fcoord[checkiteration] + 1, wordSearch.Fcoord[checkiteration] + greenletters - 2] = '*';
                                        }
                                        //NEED TO DO OTHER DIRECTIONS
                                    }
                                    foundwords[checkiteration] = 1;
                                    correctWords = correctWords + 1;
                                }
                                addcoord = addcoord + 1;
                            }
                        }
                        else
                        {
                            //clear reds
                            for (int k = 0; k < wordSearch.Cols; k++)
                            {
                                for (int j = 0; j < wordSearch.Rows; j++)
                                {
                                    if (referenceGrid[k, j] == '!')
                                    {
                                        referenceGrid[k, j] = weightedalphabet[rng.Next(26)]; ;
                                    }
                                }
                                Console.WriteLine();
                            }

                            //add new reds

                            int XcoordDifference = intFirstletterXcoord - intLastLetterXcoord;
                            int YcoordDifference = intFirstletterYcoord - intLastLetterYcoord;

                            int currentLetterXcoord = intFirstletterXcoord;
                            int currentLetterYcoord = intFirstletterYcoord;


                            // this loop work for down - you **could** (but don't have to) parameterize the difference in row and column
                            // then add that on each time - for each direction the difference between the row and col of subsequent letter in and
                            // incorrect guess will be -1, 0 or 1

                            //right
                            if (XcoordDifference < 1 && YcoordDifference == 0) //difference is 1 rather than 0 as had to +1 earlier to allign coords because of labels
                            {
                                for (int redletterplaced = 0; redletterplaced < (XcoordDifference * -1) + 1; redletterplaced++, currentLetterXcoord++)// +1 as one for label and one for when ==
                                {
                                    referenceGrid[currentLetterYcoord, currentLetterXcoord + 1] = '!';
                                }
                            }
                            //left
                            else if (XcoordDifference > 1 && YcoordDifference == 0)
                            {
                                for (int redletterplaced = 0; redletterplaced < XcoordDifference + 1; redletterplaced++, currentLetterXcoord--)
                                {
                                    referenceGrid[currentLetterYcoord, currentLetterXcoord + 1] = '!';
                                }
                            }
                            //up
                            else if (XcoordDifference == 0 && YcoordDifference > 0)
                            {
                                for (int redletterplaced = 0; redletterplaced < YcoordDifference + 1; redletterplaced++, currentLetterYcoord--)
                                {
                                    referenceGrid[currentLetterYcoord, currentLetterXcoord + 1] = '!';
                                }
                            }
                            //down
                            else if (XcoordDifference == 0 && YcoordDifference < 0)
                            {
                                for (int redletterplaced = 0; redletterplaced < (YcoordDifference * -1) + 1; redletterplaced++, currentLetterYcoord++)
                                {
                                    referenceGrid[currentLetterYcoord, currentLetterXcoord + 1] = '!';
                                }
                            }
                            else if (XcoordDifference == YcoordDifference)
                            {
                                //left up
                                if (XcoordDifference > 1 && YcoordDifference > 0)
                                {
                                    for (int redletterplaced = 0; redletterplaced < XcoordDifference + 1; redletterplaced++, currentLetterXcoord--, currentLetterYcoord--)
                                    {
                                        referenceGrid[currentLetterYcoord, currentLetterXcoord + 1] = '!';
                                    }
                                }
                                //left down
                                else if (XcoordDifference > 1 && YcoordDifference < 0)
                                {
                                    for (int redletterplaced = 0; redletterplaced < XcoordDifference + 1; redletterplaced++, currentLetterXcoord--, currentLetterYcoord++)
                                    {
                                        referenceGrid[currentLetterYcoord, currentLetterXcoord + 1] = '!';
                                    }
                                }
                                //right up
                                else if (XcoordDifference < 1 && YcoordDifference > 0)
                                {
                                    for (int redletterplaced = 0; redletterplaced < (XcoordDifference * -1) + 1; redletterplaced++, currentLetterXcoord++, currentLetterYcoord--)
                                    {
                                        referenceGrid[currentLetterYcoord, currentLetterXcoord + 1] = '!';
                                    }
                                }
                                //right down
                                else if (XcoordDifference < 1 && YcoordDifference < 0)
                                {
                                    for (int redletterplaced = 0; redletterplaced < (XcoordDifference * -1) + 1; redletterplaced++, currentLetterXcoord++, currentLetterYcoord++)
                                    {
                                        referenceGrid[currentLetterYcoord, currentLetterXcoord + 1] = '!';
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nThis input does not create a straight line.\nPress enter to conitnue.");
                                Console.ReadLine();
                                checkiteration = wordSearch.NumWords;
                            }
                        }
                    }

                }


            }
            Console.WriteLine("Congratz");

        }
    }
}




    //play again

