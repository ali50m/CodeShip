﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Dapplo.Log;
using DataCon101.Models;


namespace DataCon101
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LogSource mahLogger = new LogSource();

            mahLogger.Info().WriteLine("Do this now");

            WPFCustomMessageBox.CustomMessageBox.Show("Wwawa");

            AbstractCars somecar = new AbstractCars(30,"wawa");

            somecar.CheckNullableParams();



        }
    }
}
