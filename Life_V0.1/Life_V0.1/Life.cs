using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Life_V0._1
{
    public partial class Main : Form
    {

        public Rectangle Border { get; set; } // Declares border of life (gray square near edges)
        public int BorderSize { get; set; }

        RenderEngine render;

        public Main()
        {
            InitializeComponent();
            render =  new RenderEngine();
            BorderSize = 10;
            UpdateBorder();
        }

        /// <summary>
        /// Calculates everything
        /// </summary>
        private void Claculations()
        {

        }

        /// <summary>
        /// Updates border size
        /// </summary>
        public void UpdateBorder()
        {
            Border = new Rectangle(BorderSize, BorderSize, this.Width - BorderSize, this.Height - BorderSize);
        }

        /// <summary>
        /// Triggers next frame
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefTimer_Tick(object sender, EventArgs e)
        {
            Claculations();
            this.Refresh();
        }


        /// <summary>
        /// Renders everything
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
