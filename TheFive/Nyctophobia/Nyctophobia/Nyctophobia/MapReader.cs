//Georgia Partyka

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nyctophobia

{
    /// <summary>
    /// holds methods for map reading. no constructors or properties. So far just one method.
    /// </summary>
    public class MapReader
    {
        public int[,] ReadMap(int puzNum, string fileName)
        {
            // create reader that reads from the file
            StreamReader input = null;
            // 2d array to be returned
            int[,] map = new int[1, 1];
            try
            {

                input = new StreamReader("../../../../NyctophobiaContent/" + fileName);
                // variable to hold the line
                string line = "";

                while ((line = input.ReadLine()) != null)
                {
                    // array to hold split lines
                    string[] lines;


                    // finds the puzzle number
                    if (line.Length == 1 || line.Length == 2)
                    {

                        int result;
                        int.TryParse(line, out result);

                        // tries the found number against the puzNum passed in
                        if (result == puzNum)
                        {
                            // read next line
                            line = input.ReadLine();

                            //create new array to hold dimensions
                            string[] dimensions = new string[2];

                            // split next line
                            dimensions = line.Split(',');

                            // create variabels to hold dimensions
                            int rows;
                            int col;

                            // parse dimensions to ints                            
                            int.TryParse(dimensions[0], out rows);
                            int.TryParse(dimensions[1], out col);

                            //Console.WriteLine("printing " + rows + " by " + col);

                            // initialize lines array with colo number
                            lines = new string[col];

                            // intialize map 2d array with rows and columns
                            map = new int[rows, col];

                            // loop to make sure just the next rows lines are read
                            for (int i = 0; i < rows; i++)
                            {
                                // read the next lines
                                if ((line = input.ReadLine()) != null)
                                {
                                    // split the line into an aray by commas
                                    lines = line.Split(',');
                                    for (int j = 0; j < col; j++)
                                    {
                                        //cast string to int
                                        int.TryParse(lines[j], out result);

                                        // put numbers into 2D array
                                        map[i, j] = result;
                                    }

                                }
                            }

                        }

                    }

                }

            }
            catch (Exception e)
            {
                string codeIsMadAtMe = e.Message;
                Console.WriteLine(codeIsMadAtMe);
            }
            finally
            {
                if (input != null)
                    input.Close();
            }

            return map;
           
        }

        /// <summary>
        /// converts map array into string, calls ReadMap
        /// </summary>
        /// <param name="puzNum">puzzle number to be passed into ReadMap</param>
        /// <param name="fileName">name of the file, just use the name puzzle, don't use "S_"</param>
        /// <param name="solution">if you want the sol'n: true, if you don't want the sol'n false</param>
        /// <returns>string of either the sol'n or the original puzzle</returns>
        public string MapString(int puzNum, string fileName, bool solution)
        {
            string puzzleString = "";
            int[,] puzzleArray;

            // appends "S_" to file name to read sol'n
            if (solution == true)
            {
                // reads file
                puzzleArray = this.ReadMap(puzNum, "S_" + fileName);

                foreach (int tileNum in puzzleArray)
                {
                    // converts the int to a string and adds it to returned string
                    puzzleString += tileNum.ToString();
                }
            }
            if (solution == false)
            {
                puzzleArray = this.ReadMap(puzNum, fileName);
                foreach (int tileNum in puzzleArray)
                {
                    puzzleString += tileNum.ToString();
                }

            }
            return puzzleString;
        }

        public string ReadStory(int index, string filename)
        {
            StreamReader input = null;
            string _story = "";
            try
            {

                input = new StreamReader(filename);
                string line;
                int result;


                while ((line = input.ReadLine()) != null)
                {
                    int.TryParse(line, out result);

                    if (result == index)
                    {
                        while ((line = input.ReadLine()) != null)
                        {
                            if (!int.TryParse(line, out result))
                            {
                                _story += line + "\n";
                            }

                            else
                            {
                                break;
                            }

                        }
                    }

                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                if (input != null)
                    input.Close();
            }

            return _story;
        }

        
     

    }
}
