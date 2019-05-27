﻿using System.Windows;
using System;
using System.Windows.Threading;
using System.Threading.Tasks;
using WPF.Sample.ViewModelLayer;
using System.Windows.Controls;
using Common.Library;

namespace WPF.Sample
{
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      // Connect to instance of the view model created by the XAML
      _viewModel = (MainWindowViewModel)this.Resources["viewModel"];

      // Initialize the Message Broker Events
      MessageBroker.Instance.MessageReceived += Instance_MessageReceived;
    }

    private void Instance_MessageReceived(object sender, MessageBrokerEventArgs e)
    {
      switch (e.MessageName) {
        case MessageBrokerMessages.CLOSE_USER_CONTROL:
          CloseUserControl();
          break;
      }
    }

    // Main window's view model class
    private MainWindowViewModel _viewModel = null;

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
      MenuItem mnu = (MenuItem)sender;
      string cmd = string.Empty;

      // The Tag property contains a command 
      // or the name of a user control to load
      if (mnu.Tag != null) {
        cmd = mnu.Tag.ToString();
        if (cmd.Contains(".")) {
          // Display a user control
          LoadUserControl(cmd);
        }
        else {
          // Process special commands
          ProcessMenuCommands(cmd);
        }
      }
    }

    private bool ShouldLoadUserControl(string controlName)
    {
      bool ret = true;

      // Make sure you don't reload a control already loaded.
      if (contentArea.Children.Count > 0) {
        if (((UserControl)contentArea.Children[0]).GetType().Name ==
            controlName.Substring(controlName.LastIndexOf(".") + 1)) {
          ret = false;
        }
      }

      return ret;
    }

    private void LoadUserControl(string controlName)
    {
      Type ucType = null;
      UserControl uc = null;

      if (ShouldLoadUserControl(controlName)) {
        // Create a Type from controlName parameter
        ucType = Type.GetType(controlName);
        if (ucType == null) {
          MessageBox.Show("The Control: " + controlName
                           + " does not exist.");
        }
        else {
          // Close current user control in content area
          // NOTE: Optionally add current user control to a list 
          //     so you can restore it when you close the newly added one
          CloseUserControl();

          // Create an instance of this control
          uc = (UserControl)Activator.CreateInstance(ucType);
          if (uc != null) {
            // Display control in content area
            DisplayUserControl(uc);
          }
        }
      }
    }

    private void ProcessMenuCommands(string command)
    {
      switch (command.ToLower()) {
        case "exit":
          this.Close();
          break;

        default:
          break;
      }
    }


    private void CloseUserControl()
    {
      // Remove current user control
      contentArea.Children.Clear();
    }

    public void DisplayUserControl(UserControl uc)
    {
      // Add new user control to content area
      contentArea.Children.Add(uc);
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      // Call method to load resources application
      await LoadApplication();

      // Turn off informational message area
      _viewModel.ClearInfoMessages();
    }

    public async Task LoadApplication()
    {
      _viewModel.InfoMessage = "Loading State Codes...";
      await Dispatcher.BeginInvoke(new Action(() => {
        _viewModel.LoadStateCodes();
      }), DispatcherPriority.Background);

      _viewModel.InfoMessage = "Loading Country Codes...";
      await Dispatcher.BeginInvoke(new Action(() => {
        _viewModel.LoadCountryCodes();
      }), DispatcherPriority.Background);

      _viewModel.InfoMessage = "Loading Employee Types...";
      await Dispatcher.BeginInvoke(new Action(() => {
        _viewModel.LoadEmployeeTypes();
      }), DispatcherPriority.Background);
    }

  }
}