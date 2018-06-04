using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphDrawn
{
    /// <summary>
    /// Вікно відображення матриці
    /// </summary>
    public partial class Matrixes : Form
    {
        /// <summary>
        /// Кількість вершин
        /// </summary>
        int N;
        /// <summary>
        /// Кількість ребер класу
        /// </summary>
        int M;
        /// <summary>
        /// Головна форма
        /// </summary>
        MainForm mainForm;
        /// <summary>
        /// Чи ортогональний граф
        /// </summary>
        bool isOrtogonal;
        /// <summary>
        /// Масив всіх стовпців
        /// </summary>
        DataGridViewTextBoxColumn[] columns;
        /// <summary>
        /// Елемент класу який малює елементи графу
        /// </summary>
        GraphDraw draw;

        /// <summary>
        /// Конструктор ініціалізації
        /// </summary>
        /// <param name="draw">Елемент класу для відображення графу</param>
        /// <param name="mainForm">Головна форма</param>
        internal Matrixes(GraphDraw draw, MainForm mainForm)
        {
            InitializeComponent();

            this.draw = draw;
            this.mainForm = mainForm;
            this.dataGridView1.RowHeadersVisible = false;
            
        }


        /// <summary>
        /// Обробка події натискання кнопки Суміжності
        /// </summary>
        /// <param name="sender">Відправник</param>
        /// <param name="e">Подія</param>
        private void ABSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.isOrtogonal = mainForm.ISOrtogonal();
            this.N = draw.MaxVertexValue();
            this.M = draw.CountOfNodes();

            this.dataGridView1.Columns.Clear();
            this.dataGridView1.Rows.Clear();


            if (N > 0)
            {

                this.columns = new DataGridViewTextBoxColumn[N + 1];
                for (int i = 0; i < N + 1; i++)
                {
                    this.columns[i] = new DataGridViewTextBoxColumn();
                    if (i != 0)
                    {
                        this.columns[i].HeaderText = (i).ToString();
                    }

                    this.columns[i].Width = 30;
                    this.columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

                }
                this.dataGridView1.Columns.AddRange(this.columns);


                this.dataGridView1.Rows.Add(N);
                for (int i = 0; i < N; i++)
                {
                    this.dataGridView1.Rows[i].Cells[0].Value = i + 1;
                }

                for (int i = 0; i < N; i++)
                {
                    for (int j = 1; j < N + 1; j++)
                    {

                        this.dataGridView1.Rows[i].Cells[j].Value = "-";
                    }
                }
                for (int i = 0; i < draw.Nodes.Count; i++)
                {
                    dataGridView1.Rows[draw.Nodes[i].First.Number - 1].Cells[draw.Nodes[i].Second.Number].Value = draw.Nodes[i].Weight;
                    if (!isOrtogonal)
                    {
                        dataGridView1.Rows[draw.Nodes[i].Second.Number - 1].Cells[draw.Nodes[i].First.Number].Value = draw.Nodes[i].Weight;
                    }
                }

            }
        }


        /// <summary>
        /// Обробка події натискання кнопки Інциндентності
        /// </summary>
        /// <param name="sender">Відправник</param>
        /// <param name="e">Подія</param>
        private void INCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.isOrtogonal = mainForm.ISOrtogonal();
            this.N = draw.MaxVertexValue();
            this.M = draw.CountOfNodes();

            this.dataGridView1.Columns.Clear();
            this.dataGridView1.Rows.Clear();

            if (M > 0 && N > 0)
            {
                this.columns = new DataGridViewTextBoxColumn[M + 1];
                for (int i = 0; i < M + 1; i++)
                {
                    this.columns[i] = new DataGridViewTextBoxColumn();
                    if (i != 0)
                    {
                        this.columns[i].HeaderText = draw.Nodes[i - 1].First.Number + "-" + draw.Nodes[i - 1].Second.Number;
                    }

                    if (isOrtogonal)
                    {
                        this.columns[i].Width = 50;
                    }
                    else
                    {
                        this.columns[i].Width = 30;
                    }
                    this.columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                this.dataGridView1.Columns.AddRange(this.columns);


                this.dataGridView1.Rows.Add(N);

                for (int i = 0; i < N; i++)
                {
                    this.dataGridView1.Rows[i].Cells[0].Value = i + 1;
                }

                for (int i = 0; i < N; i++)
                {
                    for (int j = 1; j < M + 1; j++)
                    {

                        this.dataGridView1.Rows[i].Cells[j].Value = "-";
                    }
                }

                for (int i = 0; i < draw.Nodes.Count; i++)
                {
                    if (!isOrtogonal)
                    {
                        dataGridView1.Rows[draw.Nodes[i].First.Number - 1].Cells[i + 1].Value = draw.Nodes[i].Weight;

                        dataGridView1.Rows[draw.Nodes[i].Second.Number - 1].Cells[i + 1].Value = draw.Nodes[i].Weight;
                    }
                    else
                    {
                        dataGridView1.Rows[draw.Nodes[i].First.Number - 1].Cells[i + 1].Value = (-1) + "{" + draw.Nodes[i].Weight + "}";

                        dataGridView1.Rows[draw.Nodes[i].Second.Number - 1].Cells[i + 1].Value = (1) + "{" + draw.Nodes[i].Weight + "}";
                    }
                }
            }
        }

    }
}
