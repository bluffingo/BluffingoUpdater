﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BluffingoUpdater
{
    public partial class TimeMachineDialog : Form
    {
        public DateTime time = DateTime.Now;

        public TimeMachineDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            time = e.Start;
        }
    }
}
