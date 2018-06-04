using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphDrawn
{
    /// <summary>
    /// Клас основної форми програми
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Чи збережений граф
        /// </summary>
        bool isSaved = false;
        /// <summary>
        /// Вага ребра до зміни на нову вагу
        /// </summary>
        int cellValueBeforeEdit = 0;
        /// <summary>
        /// Чи потрібно розмір поля для малювання
        /// </summary>
        bool notResize = false;
        /// <summary>
        /// Вершина яку переміщають
        /// </summary>
        Vertex moveVertex;
        /// <summary>
        /// Чи перейдено до режиму переміження вершин
        /// </summary>
        bool moveDot = false;
        /// <summary>
        /// Чи обрано режим малювання графу
        /// </summary>
        bool PenForDrawGraph = true;
        /// <summary>
        /// Чи обрано режим видалення ребер чи вершин
        /// </summary>
        bool DeleteLineNNode = false;
        /// <summary>
        /// Чи ртогональний граф
        /// </summary>
        bool isOrtogonal = false;

        /// <summary>
        /// Зображення яке відображає pictureBox
        /// </summary>
        private Image image;
        /// <summary>
        /// Елемент класу який малює елементи графу
        /// </summary>
        GraphDraw draw;
        /// <summary>
        /// Чи було натиснуто клавішу миші на вершині
        /// </summary>
        bool DownOnVertex = false;
        /// <summary>
        /// Пара вершин між якими потрібно побудувати ребро
        /// </summary>
        Vertex[] coupleDotTOLine;

        /// <summary>
        /// Конструктор класу
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            toolStripStatusLabel1.Text = "( 0; 0)";

            draw = new GraphDraw(pictureBox1.Width, pictureBox1.Height);

            image = draw.Image;
            pictureBox1.Image = image;

            coupleDotTOLine = new Vertex[2];

            toolStripComboBox1.Items.Add("Орієнтований");
            toolStripComboBox1.Items.Add("Нерієнтований");
            toolStripComboBox1.SelectedIndex = 1;

            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;


        }

        /// <summary>
        /// Обробка події кліку клавіши миші
        /// </summary>
        /// <param name="sender">Відправник</param>
        /// <param name="e">Подія</param>
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!moveDot)
            {
                if (PenForDrawGraph)
                {
                    if (!DownOnVertex)
                    {
                        pictureBox1.Image = draw.DrawDot(e.X, e.Y);
                    }
                }
                else if (DeleteLineNNode)
                {
                    pictureBox1.Image = draw.DeleteVertexOrNode(e.X, e.Y, isOrtogonal);

                }
                RefreshTable();
            }
        }

        /// <summary>
        /// Обробка події пересування курсору миші
        /// </summary>
        /// <param name="sender">Відправник</param>
        /// <param name="e">Подія</param>
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripStatusLabel1.Text = "( " + e.X + "; " + e.Y + ")";


            Vertex find = new Vertex(-10, -10, -10);
            int numberOfNode = -1;
            if (!moveDot)
            {
                find = draw.FindVertex(e.X, e.Y);
                numberOfNode = -1;
            }
                if (moveDot)
                {
                    if (e.X <= pictureBox1.Width && e.Y <= pictureBox1.Height && e.X>=0 && e.Y>=0)
                    {
                        pictureBox1.Image = draw.ChangeDotPosition(e.X, e.Y, moveVertex.Number, isOrtogonal);
                    }
                }
                else if (find.X > 0)
                {
                    Image temp = new Bitmap(draw.Image);
                    pictureBox1.Image = draw.drawRedDot(find);
                    draw.SetDrawingImage(temp);
                }
                else if (DeleteLineNNode)
                {
                    numberOfNode = draw.FindNodes(e.X, e.Y);
                    if (numberOfNode >= 0)
                    {
                        Image temp = new Bitmap(draw.Image);
                        pictureBox1.Image = draw.DrawRedLine(numberOfNode, isOrtogonal);

                        draw.SetDrawingImage(temp);
                    }
                    else
                    {
                        pictureBox1.Image = draw.Image;
                    }
                }
                else
                {
                    pictureBox1.Image = draw.Image;
                }
            


        }

        /// <summary>
        /// Обробка події натиснення клавіши миші
        /// </summary>
        /// <param name="sender">Відправник</param>
        /// <param name="e">Подія</param>
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                moveDot = true;
                moveVertex = draw.FindVertex(e.X, e.Y);
            }
            else if (PenForDrawGraph)
            {
                image = pictureBox1.Image;

                DownOnVertex = false;


                Vertex find = draw.FindVertex(e.X, e.Y);

                if (find.X > 0)
                {
                    DownOnVertex = true;
                    coupleDotTOLine[0] = find;
                }
                else
                {
                    DownOnVertex = true;
                    pictureBox1.Image = draw.DrawDot(e.X, e.Y);
                    coupleDotTOLine[0] = draw.LastVertex();
                }
            }
            
        }
        
        /// <summary>
        /// Обробка події відпускання клавіши миші
        /// </summary>
        /// <param name="sender">Відправник</param>
        /// <param name="e">Подія</param>
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            

            if (e.X <= pictureBox1.Width && e.Y <= pictureBox1.Height)
            {
                if (e.Button == MouseButtons.Right)
                {
                    moveDot = false;
                } 
                else if (PenForDrawGraph)
                {
                    if (DownOnVertex == true)
                    {

                        Vertex find = draw.FindVertex(e.X, e.Y);
                        if (find.X > 0)
                        {
                            coupleDotTOLine[1] = find;
                        }
                        else
                        {
                            pictureBox1.Image = draw.DrawDot(e.X, e.Y);

                            coupleDotTOLine[1] = draw.LastVertex();
                        }
                        pictureBox1.Image = draw.LineDraw(coupleDotTOLine[0], coupleDotTOLine[1],0, isOrtogonal);
                        if (!coupleDotTOLine[0].Equals(coupleDotTOLine[1]))
                        {
                            dataGridView1.Rows.Add(coupleDotTOLine[0].Number, coupleDotTOLine[1].Number, 0);
                            RefreshTable();
                        }

                        DownOnVertex = false;
                    }


                    coupleDotTOLine[0] = new Vertex(-10, -10, -10);
                    coupleDotTOLine[1] = new Vertex(-10, -10, -10);
                }
            }

        }

        /// <summary>
        /// Обробка події натиснення кнопки Олівець для малювання графу
        /// </summary>
        /// <param name="sender">Відправник</param>
        /// <param name="e">Подія</param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            PenForDrawGraph = true;
            DeleteLineNNode = false;
        }

        /// <summary>
        /// Обробка події натиснення кнопки Видалити для видалення елементу графу
        /// </summary>
        /// <param name="sender">Відправник</param>
        /// <param name="e">Подія</param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            DeleteLineNNode = true;
            PenForDrawGraph = false;
        }

        /// <summary>
        /// Обробка події зміни типу графу Орієнтований чи Неорієнтований
        /// </summary>
        /// <param name="sender">Відправник</param>
        /// <param name="e">Подія</param>
        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedIndex = toolStripComboBox1.SelectedIndex;
            if(selectedIndex==1)
            {
                if(isOrtogonal)
                {
                    pictureBox1.Image = draw.DrawAllFromScrath(false, pictureBox1.Width,pictureBox1.Height);
    
                }
                isOrtogonal = false;
                
            }
            else if(selectedIndex==0)
            {
                if (!isOrtogonal)
                {
                    pictureBox1.Image = draw.DrawAllFromScrath(true, pictureBox1.Width, pictureBox1.Height);
                }
                
                isOrtogonal = true;
            }
            RefreshTable();
        }

        /// <summary>
        /// Обробка події натиснення кнопки Видалити весь граф
        /// </summary>
        /// <param name="sender">Відправник</param>
        /// <param name="e">Подія</param>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Ви впевнені?", "Граф буде видалено", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                pictureBox1.Image = draw.ClearAll();
                dataGridView1.Rows.Clear();
            }
        }

        /// <summary>
        /// Метод відновлення даних у таблиці з ребрами
        /// </summary>
        public void RefreshTable()
        {
            List<int> delete=new List<int>();   
            int[] used = new int[draw.Nodes.Count];


            for(int i=0;i<dataGridView1.RowCount;i++)
            {
                int index=draw.IsNodeConsist((int)dataGridView1.Rows[i].Cells[0].Value, (int)dataGridView1.Rows[i].Cells[1].Value);
                if (index != -1 && used[index]!=1)
                {                    
                    used[index] = 1;
                    if(!isOrtogonal)
                    {
                        int temp = draw.FindOrtogonalNode((int)dataGridView1.Rows[i].Cells[0].Value, (int)dataGridView1.Rows[i].Cells[1].Value);
                        if (temp != -1)
                        {
                            used[temp] = 1;
                        }
                    }
                }
                else
                {
                    delete.Add(i);
                }
            }
            for(int i=0;i<delete.Count;i++)
            {
                dataGridView1.Rows.RemoveAt(delete[i]-i);
            }
            for(int i=0;i<used.Length;i++)
            {
                if(used[i]!=1)
                {
                    dataGridView1.Rows.Add(draw.Nodes[i].First.Number, draw.Nodes[i].Second.Number, draw.Nodes[i].Weight);
                }
            }
        }
        
        /// <summary>
        /// Обробка події зміна значення у таблиці з ребрами
        /// </summary>
        /// <param name="sender">Відправник</param>
        /// <param name="e">Подія</param>
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                int temp;
                int index = draw.IsNodeConsist((int)dataGridView1.Rows[e.RowIndex].Cells[0].Value, (int)dataGridView1.Rows[e.RowIndex].Cells[1].Value);


                var str = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (int.TryParse(str.ToString(), out temp))
                {
                    draw.Nodes[index] = new Node(draw.Nodes[index].First, draw.Nodes[index].Second, temp);
                    draw.DrawAllFromScrath(isOrtogonal,pictureBox1.Width,pictureBox1.Height);
                    pictureBox1.Image = draw.drawWeight(draw.Nodes[index],isOrtogonal);
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].Cells[2].Value = cellValueBeforeEdit;
                    MessageBox.Show("Введено некоректні дані в таблицю", "Помилка");
                }

            }
        }

        /// <summary>
        /// Обробка події натиснення кнопки Відкрити з файлу для завантаження графу з файлу
        /// </summary>
        /// <param name="sender">Відправник</param>
        /// <param name="e">Подія</param>
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Ви впевнені?", "Граф буде видалено", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                pictureBox1.Image = draw.ClearAll();
                dataGridView1.Rows.Clear();
            }
            else
            {
                return;
            }

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";

            

            int[,] matrix;

            List<int> vertexes = new List<int>();

            int n=-1, m=-1, o=-1;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.OpenFile(), System.Text.Encoding.Default))
                    {

                        string[] lineArray;
                        string line;
                        line = sr.ReadLine();
                        lineArray = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (!int.TryParse(lineArray[0], out n) || n < 1)
                        {
                            MessageBox.Show("Кількість вершин неправильно задана", "Помилка");
                            return;
                        }
                        if (!int.TryParse(lineArray[1], out m) || m < 0)
                        {
                            MessageBox.Show("Кількість ребер неправильно задана", "Помилка");
                            return;
                        }
                        if (!int.TryParse(lineArray[2], out o) || o < 0)
                        {
                            MessageBox.Show("Показник ортогональності неправильно задано", "Помилка");
                            return;
                        }

                        matrix = new int[n, n];


                        //Read matrix
                        for (int i = 0; i < m; i++)
                        {
                            line = sr.ReadLine();
                            lineArray = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            int d1, d2, w;

                            if (!int.TryParse(lineArray[0], out d1) || d1 < 0 || d1 > n)
                            {
                                MessageBox.Show("Вершина в рядку " + (i + 2) + " має некоректне значення", "Помилка");
                                return;
                            }
                            if (!int.TryParse(lineArray[1], out d2) || d2 < 0 || d2 > n)
                            {
                                MessageBox.Show("Вершина в рядку " + (i + 2) + " має некоректне значення", "Помилка");
                                return;
                            }

                            if (!int.TryParse(lineArray[2], out w) || w < 0)
                            {
                                MessageBox.Show("Вага ребра в рядку " + (i + 2) + " має некоректне значення", "Помилка");
                                return;
                            }

                            vertexes.Add(d1);
                            vertexes.Add(d2);


                            matrix[d1 - 1, d2 - 1] = w;

                        }


                    }
                }  
                catch (Exception ex)
                {
                    MessageBox.Show("Неможливо зчитати файл з диску:	" + ex.Message,"Помилка");
                    return;
                }
            }
            else
            {
                return;
            }
            int sizeBeforeAddSingelDot = vertexes.Count;

            for(int i=0;i<n;i++)
            {
                bool isSingle = true;
                for(int j=0;j<n;j++)
                {
                    if(matrix[i,j]!=0)
                    {
                        isSingle = false;
                    }
                }
                for (int j = 0; j < n; j++)
                {
                    if(matrix[j,i]!=0)
                    {
                        isSingle = false;
                    }
                    
                }
                if(isSingle)
                {
                    vertexes.Add(i + 1);
                }
            }
            Vertex current1 = new Vertex(-10, -10, -10);
            Vertex current2 = new Vertex(-10, -10, -10);
            
            int cur=-1;
            
            bool isorogonal=false;
            if(o==0)
            {
                toolStripComboBox1.SelectedIndex = 1;
                isorogonal=false;
            }
            else
            {

                toolStripComboBox1.SelectedIndex = 0;
                isorogonal=true;
            }

            foreach(Vertex v in GetVertex(n,vertexes))
            {
                cur++;
                if (cur < sizeBeforeAddSingelDot+1)
                {
                    if (current1.Number < 0)
                    {
                        current1 = v;
                    }
                    else if (current2.Number < 0)
                    {
                        current2 = v;
                    }
                    else
                    {

                        draw.LineDraw(current1, current2, matrix[current1.Number - 1, current2.Number - 1], isorogonal);
                        current1 = v;
                        current2 = new Vertex(-10, -10, -10);
                    }
                }
            }
            RefreshTable();
            pictureBox1.Image = draw.Image;


            
        }

        /// <summary>
        /// Перебір всіх вершин у графі
        /// </summary>
        /// <param name="n">Кількість вершин у графі</param>
        /// <param name="numbers">Список вершин впорядковані по парам</param>
        /// <returns>Нумератор з вершинами. Повертає по одній вершині за раз</returns>
        internal  IEnumerable<Vertex> GetVertex(int n,List<int> numbers)
        {
            int index = -1;
            Random rand = new Random();
            int rows = (int)((double)n / Math.Sqrt(n));
            int columnes = (n / rows) + Math.Sign(n % (int)Math.Sqrt(n));
            double width = (pictureBox1.Width - 10) / rows;
            double height = (pictureBox1.Height - 10) / columnes;

            Vertex current;

            for(int i=0;i<columnes;i++)
            {
                for(int j=0;j<rows;j++)
                {
                    index++;
                    if(index==numbers.Count)
                    {
                        yield break;
                    }
                    int x = rand.Next((int)(j * width), (int)(width * (j + 1)));
                    int y = rand.Next((int)(i * height), (int)((i + 1) * height));
                    Vertex v = draw.FindVertex(numbers[index]);
                    draw.DrawDot(x+2, y+2, numbers[index]);
                    current = draw.LastVertex();
                    if (v.Number>=0 || current.Number != numbers[index])
                    {
                        current = draw.FindVertex(numbers[index]);
                        j--;
                        yield return current;
                    }
                    else
                    {
                        yield return current;
                    }
                    
                }
            }
            while(index<numbers.Count-1)
            {
                index++;
                Vertex v = draw.FindVertex(numbers[index]);
                yield return v;
            }
        }

        /// <summary>
        /// Обробка події звернення вікна
        /// </summary>
        /// <param name="sender">Відправник</param>
        /// <param name="e">Подія</param>
        private void Form1_Deactivate(object sender, EventArgs e)
        {
            notResize = true;
        }

        /// <summary>
        /// Обробка події розгортання вікна
        /// </summary>
        /// <param name="sender">Відправник</param>
        /// <param name="e">Подія</param>
        private void Form1_Activated(object sender, EventArgs e)
        {
            notResize = false;
        }

        /// <summary>
        /// Обробка події зміни розміру вікна
        /// </summary>
        /// <param name="sender">Відправник</param>
        /// <param name="e">Подія</param>
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (!notResize && pictureBox1.Width>0 && pictureBox1.Height>0)
            {
                pictureBox1.Image = draw.DrawAllFromScrath(isOrtogonal, pictureBox1.Width, pictureBox1.Height);
            }
        }

        /// <summary>
        /// Обробка події натиснення кнопки Зберегти граф до файлу
        /// </summary>
        /// <param name="sender">Відправник</param>
        /// <param name="e">Подія</param>
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            var save= saveFileDialog1.ShowDialog();
            if (save == System.Windows.Forms.DialogResult.OK && saveFileDialog1.FileName.Length > 0)
            {
                isSaved = true;
                using (StreamWriter write = new StreamWriter(saveFileDialog1.OpenFile()))
                {
                    write.WriteLine(draw.CountOfVertex() + " " + draw.CountOfNodes() + " " + (isOrtogonal ? 1 : 0));
                    for(int i=0;i<draw.Nodes.Count;i++)
                    {
                        write.WriteLine(draw.Nodes[i].First.Number + " " + draw.Nodes[i].Second.Number + " " + draw.Nodes[i].Weight);
                    }

                }
            }
            else if(save == System.Windows.Forms.DialogResult.Cancel)
            {

                isSaved = false;
            }
        }

        /// <summary>
        /// Обробка події натиснення кнопки Довідка
        /// </summary>
        /// <param name="sender">Відправник</param>
        /// <param name="e">Подія</param>
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, helpProvider1.HelpNamespace);
        }

        /// <summary>
        /// Обробка події початку редагування таблиці з ребрами, потрібна для збереження даних перед редагуванням
        /// </summary>
        /// <param name="sender">Відправник</param>
        /// <param name="e">Подія</param>
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int.TryParse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(), out cellValueBeforeEdit);
            }
        }

        /// <summary>
        /// Обробка події закриття форми
        /// </summary>
        /// <param name="sender">Відправник</param>
        /// <param name="e">Подія</param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (draw.CountOfVertex() != 0)
            {
                DialogResult dialogResult = MessageBox.Show("Зберегти граф", "Закриття програми", MessageBoxButtons.YesNoCancel);
                if (dialogResult == DialogResult.Yes)
                {
                    toolStripButton5.PerformClick();
                    if (!isSaved)
                    {
                        e.Cancel = true;
                    }
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Обробка події натиснення кнопки Про програму
        /// </summary>
        /// <param name="sender">Відправник</param>
        /// <param name="e">Подія</param>
        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            About form = new About();
            form.Show();
        }

        /// <summary>
        /// Обробка події натиснення кнопки Матриця
        /// </summary>
        /// <param name="sender">Відправник</param>
        /// <param name="e">Подія</param>
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            Matrixes form = new Matrixes(draw, this);
            form.Show();
        }
        
        /// <summary>
        /// Чи є граф ортогональний
        /// </summary>
        /// <returns>Атрибут класу який показує ортогональність графу</returns>
        public bool ISOrtogonal()
        {
            return isOrtogonal;
        }


    }
}
