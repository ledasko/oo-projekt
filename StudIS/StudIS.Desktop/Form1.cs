﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StudIS.Models;
using StudIS.DAL;

namespace StudIS.Desktop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var userRepo = new DAL.Repositories.UserRepository();
            MessageBox.Show(userRepo.GetAll().ToString());
        }
    }
}
