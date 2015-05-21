using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MapCreator
{
    public partial class Creator : Form
    {
        //file info
        protected string fileName;
        protected int mapNum;
        
        //Button info
        protected const int BUTTON_WIDTH = 35;
        protected const int BUTTON_HEIGHT = 30;
        protected const int BIG_BUTTON_WIDTH = BUTTON_WIDTH * 2;
        protected const int BIG_BUTTON_HEIGHT = BUTTON_HEIGHT * 3 / 2;
        List<Button> uGrid = new List<Button>();
        List<Button> sGrid = new List<Button>();

        //Controls
        Label unsolved = new Label();
        Label solved = new Label();
        Label nameU = new Label();
        Label nameS = new Label();
        Button backBtn = new Button();
        Button clearBtn = new Button();
        Button unsolveBtn = new Button();
        Button createBtn = new Button();

        //Dictionary for the buttons


        //row and column info
        protected int r;
        protected int c;
        //Color[] colors = new Color[]{Color.Black, Color.Tan, Color.White, Color.Gray, Color.Yellow};

        public Creator(int row, int col, int map, string file)
        {
            r = row;
            c = col;
            mapNum = map;
            fileName = file;
            InitializeComponent();
            if (r == 0 || c == 0)
                this.ClientSize = new System.Drawing.Size(300, 200);
            else if (r < 4 || c < 2)
                this.ClientSize = new System.Drawing.Size((r + 2) * BUTTON_WIDTH, (c + 2) * BUTTON_HEIGHT + 250);
            else
                this.ClientSize = new System.Drawing.Size(2 * r * BUTTON_WIDTH + 9, 400 /*unsolved.Height + c * BUTTON_HEIGHT + nameU.Height + 2 * BIG_BUTTON_HEIGHT + 85*/);
            ButtonGrid();
        }

        public void ButtonGrid()
        {
            //Do the top labels
            unsolved.Text = "Unsolved";
            unsolved.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Controls.Add(unsolved);

            solved.Text = "Solved";
            solved.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Controls.Add(solved);
            #region{
            //Create the UNSOLVED grid
            for (int i = 0; i < c; i++)
            {
                for (int j = 0; j < r; j++)
                {
                    Button b = new Button();
                    b.Location = new Point(j * BUTTON_WIDTH, i * BUTTON_HEIGHT + unsolved.Height + 20);
                    b.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    b.Text = "T " + i + j;
                    //b.TabIndex = 100 + j + i * 10;
                    b.Size = new Size(BUTTON_WIDTH, BUTTON_HEIGHT);
                    b.Click += Button_Click;
                    b.BackColor = Color.Tan;
                    uGrid.Add(b);
                    this.Controls.Add(b);
                }
            }
            //Create the SOLVED grid
            for (int i = 0; i < c; i++)
            {
                for (int j = 0; j < r; j++)
                {
                    Button b = new Button();
                    b.Location = new Point(j * BUTTON_WIDTH + ClientSize.Width / 2 + 7, i * BUTTON_HEIGHT + unsolved.Height + 20);
                    b.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    b.Text = "T " + i + j;
                    b.Size = new Size(BUTTON_WIDTH, BUTTON_HEIGHT);
                    b.Click += Button_Click;
                    b.BackColor = Color.Tan;
                    sGrid.Add(b);
                    this.Controls.Add(b);
                }
            }
            #endregion


            //Make the label for file name on each side
            nameU.Text = "Name: " + fileName + ".txt";
            nameU.Width = (10 + fileName.Length) * 9;
            this.Controls.Add(nameU);

            nameS.Text = "Name: S_" + fileName + ".txt";
            nameS.Width = (12 + fileName.Length) * 9;
            this.Controls.Add(nameS);

            //Create the BACK, CLEAR, and CREATE buttons
            backBtn.Size = new Size(BIG_BUTTON_WIDTH, BIG_BUTTON_HEIGHT);
            backBtn.Text = "Back";
            backBtn.Click += backBtn_Click;
            this.Controls.Add(backBtn);

            clearBtn.Size = new Size(BIG_BUTTON_WIDTH, BIG_BUTTON_HEIGHT);
            clearBtn.Text = "Clear";
            clearBtn.Click += ResetColor;
            this.Controls.Add(clearBtn);

            unsolveBtn.Size = new Size(BIG_BUTTON_WIDTH, BIG_BUTTON_HEIGHT);
            unsolveBtn.Text = "Unsolve";
            unsolveBtn.Click += Unsolve;
            this.Controls.Add(unsolveBtn);

            createBtn.Size = new Size(BIG_BUTTON_WIDTH, BIG_BUTTON_HEIGHT);
            createBtn.Text = "Create";
            createBtn.Click += createBtn_Click;
            this.Controls.Add(createBtn);

            //Size the Client
            this.ClientSize = new System.Drawing.Size(2 * r * BUTTON_WIDTH + 9, unsolved.Height + c * BUTTON_HEIGHT + nameU.Height + 2 * BIG_BUTTON_HEIGHT + 85);
            int middle = this.ClientSize.Width / 2;
            //Place ALL the things
            unsolved.Location = new Point(ClientSize.Width / 4 - unsolved.Width / 2, 10);
            solved.Location = new Point(ClientSize.Width * 3 / 4 - solved.Width / 2, 10);
            nameU.Location = new Point(ClientSize.Width / 4 - nameU.Width / 2, r * BUTTON_HEIGHT + unsolved.Height + 50);
            nameS.Location = new Point(ClientSize.Width * 3 / 4 - nameS.Width / 2, r * BUTTON_HEIGHT + solved.Height + 50);
            unsolveBtn.Location = new Point(ClientSize.Width * 3 / 4 - BIG_BUTTON_WIDTH / 2, nameS.Location.Y + nameS.Height + 15);
            clearBtn.Location = new Point(ClientSize.Width / 4 - BIG_BUTTON_WIDTH / 2, nameU.Location.Y + nameU.Height + 15);
            backBtn.Location = new Point(ClientSize.Width / 4 - BIG_BUTTON_WIDTH / 2, clearBtn.Location.Y + BIG_BUTTON_HEIGHT + 10);
            createBtn.Location = new Point(ClientSize.Width * 3 / 4 - BIG_BUTTON_WIDTH / 2, unsolveBtn.Location.Y + BIG_BUTTON_HEIGHT + 10);

        }

        private void Unsolve(object sender, EventArgs e)
        {
            for(int i = 0; i < uGrid.Count; i++)
            {
                sGrid[i].BackColor = uGrid[i].BackColor;
                sGrid[i].ForeColor = uGrid[i].ForeColor;
            }
        }

        /// <summary>
        /// Closes the Window and goes back to the first window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WriteABit(string bit, TextWriter writer, bool isNewLine)
        {
            if (isNewLine)
                writer.WriteLine(bit);
            else
                writer.Write(bit);
        }

        private void WriteToFile(string name, List<Button> grid)
        {
            string n = name + ".txt";
            StreamWriter writer = null;
            try
            {
                using (writer = File.AppendText(n))
                {
                    //output = new StreamWriter(fileName);
                    WriteABit(mapNum.ToString(), writer, true);
                    WriteABit(r + "," + c, writer, true);
                    int i = 0;
                    foreach (Button b in grid)
                    {
                            //Black = Wall
                            //Tan = Regular
                            //White = Lit
                            //Gray = unlit
                            //Yellow = door
                            if (i >= c)
                            {
                                WriteABit("", writer, true);
                                i = 0;
                            }
                            switch (b.BackColor.Name)
                            {
                                case ("Black"):
                                    WriteABit("0,", writer, false);
                                    break;
                                case ("Tan"):
                                    WriteABit("2,", writer, false);
                                    break;
                                case ("Gray"):
                                    WriteABit("3,", writer, false);
                                    break;
                                case ("White"):
                                    WriteABit("4,", writer, false);
                                    break;
                                case ("Yellow"):
                                    WriteABit("1,", writer, false);
                                    break;
                                default:
                                    WriteABit("2,", writer, false);
                                    break;
                            }
                            i++;
                    }
                    WriteABit("", writer, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error writing file: " + ex.Message);
            }
            finally
            {
                //Make sure you close the file
                if (writer != null)
                    writer.Close();
            }
        }

        private void createBtn_Click(object sender, EventArgs e)
        {
            WriteToFile(fileName, uGrid);
            WriteToFile("S_" + fileName, sGrid);
            MessageBox.Show("Map files Created/Appended!");
        }

        /// <summary>
        /// Cycles through the tile types with each click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Button_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (uGrid.Contains(b))
            {
                //Black = Wall
                //Tan = Regular
                //White = Lit
                //Gray = unlit
                //Yellow = door
                switch (b.BackColor.Name)
                {
                    case ("Black"):
                        b.BackColor = Color.Tan;
                        b.ForeColor = Color.Black;
                        break;
                    case ("Tan"):
                        b.BackColor = Color.White;
                        break;
                    case ("White"):
                        b.BackColor = Color.Gray;
                        break;
                    case ("Gray"):
                        b.BackColor = Color.Yellow;
                        break;
                    case ("Yellow"):
                        b.BackColor = Color.Black;
                        b.ForeColor = Color.White;
                        break;
                    default:
                        b.BackColor = Color.Red;
                        break;
                }
                ButtonCheck(b);
            }
            else
            {
                switch (b.BackColor.Name)
                {
                    case ("Black"):
                        break;
                    case ("Tan"):
                        break;
                    case ("White"):
                        b.BackColor = Color.Gray;
                        break;
                    case ("Gray"):
                        b.BackColor = Color.White;
                        break;
                    case ("Yellow"):
                        break;
                    default:
                        b.BackColor = Color.Red;
                        break;
                }
            }
        }
        //Haydon...you're awesome..doubly awesome
        private void ButtonCheck(Button butt)
        {
            //Step by step for testing V
            /*string index = butt.Text.Substring(2,2);
            string sub1 = index.ElementAt(0).ToString();
            string sub2 = index.ElementAt(1).ToString();
            int n1 = int.Parse(sub1) * r;
            int n2 = int.Parse(sub2);
            int ndx = n1 + n2;*/

            //int ndx = int.Parse(butt.Text.ElementAt(2).ToString()) * r + 
                //int.Parse(butt.Text.ElementAt(3).ToString());

            int ndx = uGrid.IndexOf(butt);

            sGrid[ndx].BackColor = uGrid[ndx].BackColor;
            sGrid[ndx].ForeColor = uGrid[ndx].ForeColor;
            
        }
        /// <summary>
        /// Resets the color of all Grid tiles to default Tan color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetColor(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                foreach (Button b in uGrid)
                {
                    if (b.Text != "Back" && b.Text != "Clear" && b.Text != "Create")
                    {
                        b.BackColor = Color.Tan;
                        b.ForeColor = Color.Black;
                    }
                }
                foreach (Button b in sGrid)
                {
                    if (b.Text != "Back" && b.Text != "Clear" && b.Text != "Create")
                    {
                        b.BackColor = Color.Tan;
                        b.ForeColor = Color.Black;
                    }
                }
            }
            /*backBtn.BackColor = Control.DefaultBackColor;
            clearBtn.BackColor = Control.DefaultBackColor;
            createBtn.BackColor = Control.DefaultBackColor;*/
        }
    }
}
