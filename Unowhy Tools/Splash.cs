﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Unowhy_Tools
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
            System.Threading.Thread.Sleep(100);
            this.FormBorderStyle = FormBorderStyle.None ;
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            
        }
    }
}
