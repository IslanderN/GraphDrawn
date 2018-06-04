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
    /// Вікно Про програму
    /// </summary>
    public partial class About : Form
    {
        /// <summary>
        /// Конструктор класу
        /// </summary>
        public About()
        {
            InitializeComponent();
            BackgroundImage = Image.FromFile(@"Resources\background.jpg");
            BackgroundImageLayout = ImageLayout.Stretch;

            pictureBox1.BackColor = Color.Transparent;
            
            label1.BackColor = Color.Transparent;

            label2.BackColor = Color.Transparent;
        }
    }
}
