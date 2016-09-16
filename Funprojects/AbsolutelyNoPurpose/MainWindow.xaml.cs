using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace AbsolutelyNoPurpose
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DoubleAnimation blinkEffect;
        System.Timers.Timer timer = new System.Timers.Timer(1000);
        readonly System.Timers.Timer _exitTimer = new System.Timers.Timer(1000);
        public MainWindow()
        {//blue = idle, Limegreen=on, red=off, gold=warning
            InitializeComponent();

            AiCoreEllipse.Fill = Brushes.Blue;
            AiControlEllipse.Fill = Brushes.Blue;
            DoorControlEllipse.Fill = Brushes.Blue;
            ShipEnginesEllipse.Fill = Brushes.Blue;
            ContainmentFieldEllipse.Fill = Brushes.Blue;
            PowerUnitEllipse.Fill = Brushes.Blue;

            OutputBox.AppendText("All system ready, status... idle.\nAwaiting command.");
            InputBox.Focus();
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
            {
                char[] separator = { ' ' };

                string[] commands = InputBox.Text.Split(separator, 2, StringSplitOptions.RemoveEmptyEntries);
                ChangeStatus(commands);

                InputBox.Text = "";
            }
        }

        private void ChangeStatus(string[] input)
        {
            switch (input[0].ToLower())
            {
                case "connect":
                    ConnectCommand(input[1]);
                    break;
                case "disconnect":
                    DisconnectCommand(input[1]);
                    break;
                case "status":
                    StatusCommand(input[1]);
                    break;
                case "activate":
                    ActivateCommand(input[1]);
                    break;
                case "deactivate":
                    DeactivateCommand(input[1]);
                    break;
                case "lock":
                    LockCommand(input[1]);
                    break;
                case "reset":
                    ResetCommand(input[1]);
                    break;
                case "fail":
                    FailCommand(input[1]);
                    break;
                case "help":
                    OutputBox.AppendText("\nFollowing commands are available:\nconnect\ndisconnect\nstatus\nactivate\nlock");
                    break;
                case "exit":
                    Window.Close();
                    break;
                default:
                    OutputBox.AppendText("\nIllegal command!");
                    break;
            }
            OutputBox.ScrollToEnd();
        }

        private void FlashWarningContainment()
        {
            WarningBlock.Text = "Containment field deactivated!";
            WarningBlock.Opacity = 1.0;
            WarningBlock.Foreground = Brushes.Red;
            WarningBlock.TextEffects = new TextEffectCollection();

            blinkEffect = new DoubleAnimation {To = 0.4, Duration = TimeSpan.FromMilliseconds(1000), RepeatBehavior = RepeatBehavior.Forever};

            WarningBlock.BeginAnimation(OpacityProperty, blinkEffect);
        }

        private void FlashWarningContainmentOff()
        {
            WarningBlock.Text = "";
        }

        #region Commands

        private void FailCommand(string input)
        {
            
        }

        private void ConnectCommand(string input)
        {
            switch (input.ToLower())
            {
                case "ai core":
                    if (AiCoreEllipse.Fill.Equals(Brushes.LimeGreen))
                    {
                        OutputBox.AppendText("\nAi Core already connected.");
                    }
                    else if (AiCoreEllipse.Fill.Equals(Brushes.Gold))
                    {
                        OutputBox.AppendText("\nSystem in Warning Mode! Resolve problems before continuing.");
                    }
                    else
                    {
                        AiCoreEllipse.Fill = Brushes.LimeGreen;
                        OutputBox.AppendText("\nAi Core connected. System ready.");
                    }
                    break;
                case "power unit":
                    if (PowerUnitEllipse.Fill.Equals(Brushes.LimeGreen))
                    {
                        OutputBox.AppendText("\nPower Unit is already connected.");
                    }
                    else if (PowerUnitEllipse.Fill.Equals(Brushes.Gold))
                    {
                        OutputBox.AppendText("\nSystem in Warning Mode! Resolve problems before continuing.");
                    }
                    else
                    {
                        PowerUnitEllipse.Fill = Brushes.LimeGreen;
                        OutputBox.AppendText("\nPower Unit connected. System ready.");
                    }
                    break;
            }
        }

        private void DisconnectCommand(string input)
        {
            switch (input.ToLower())
            {
                case "ai core":
                    if (AiCoreEllipse.Fill.Equals(Brushes.LimeGreen))
                    {
                        AiCoreEllipse.Fill = Brushes.Red;
                        OutputBox.AppendText("\nAi Core disconnected.");
                    }
                    else if (AiCoreEllipse.Fill.Equals(Brushes.Gold))
                    {
                        AiCoreEllipse.Fill = Brushes.Red;
                        OutputBox.AppendText("\nSystem in Warning Mode! Shutting system down.");
                    }
                    else
                    {
                        OutputBox.AppendText("\nAi Core already disconnected.");
                    }
                    break;
                case "power unit":
                    if (PowerUnitEllipse.Fill.Equals(Brushes.LimeGreen))
                    {
                        PowerUnitEllipse.Fill = Brushes.Red;
                        OutputBox.AppendText("\nPower Unit disconnected.");
                    }
                    else if (PowerUnitEllipse.Fill.Equals(Brushes.Gold))
                    {
                        PowerUnitEllipse.Fill = Brushes.Red;
                        OutputBox.AppendText("\nSystem in Warning Mode! Shutting system down.");
                    }
                    else
                    {
                        OutputBox.AppendText("\nPower Unit already disconnected.");
                    }
                    break;
            }
        }

        private void StatusCommand(string input)
        {
            switch (input)
            {
                case "all":
                    foreach (var status in SystemStatus())
                    {
                        OutputBox.AppendText(status);
                    }
                    break;
            }
        }

        private void ActivateCommand(string input)
        {
            switch (input.ToLower())
            {
                case "ai control":
                    if (AiControlEllipse.Fill.Equals(Brushes.LimeGreen))
                    {
                        OutputBox.AppendText("\nAI Control already activated.");
                    }
                    else if (AiControlEllipse.Fill.Equals(Brushes.Gold))
                    {
                        OutputBox.AppendText("\nSystem in Warning Mode! Resolve problems before continuing.");
                    }
                    else
                    {
                        AiControlEllipse.Fill = Brushes.LimeGreen;
                        OutputBox.AppendText("\nAi Control activated. System ready.");
                    }
                    break;
                case "containment field":
                    if (ContainmentFieldEllipse.Fill.Equals(Brushes.LimeGreen))
                    {
                        OutputBox.AppendText("\nContainment Field is already connected.");
                    }
                    else if (ContainmentFieldEllipse.Fill.Equals(Brushes.Gold))
                    {
                        OutputBox.AppendText("\nSystem in Warning Mode! Resolve problems before continuing.");
                    }
                    else
                    {
                        ContainmentFieldEllipse.Fill = Brushes.LimeGreen;
                        OutputBox.AppendText("\nContainment Field connected. System ready.");
                    }
                    break;
                case "ship engines":
                    if (ShipEnginesEllipse.Fill.Equals(Brushes.LimeGreen))
                    {
                        OutputBox.AppendText("\nPower Unit is already connected.");
                    }
                    else if (ShipEnginesEllipse.Fill.Equals(Brushes.Gold))
                    {
                        OutputBox.AppendText("\nSystem in Warning Mode! Resolve problems before continuing.");
                    }
                    else
                    {
                        ShipEnginesEllipse.Fill = Brushes.LimeGreen;
                        OutputBox.AppendText("\nPower Unit connected. System ready.");
                    }
                    break;
            }
            
        }

        void DeactivateCommand(string input)
        {
            switch (input.ToLower())
            {
                case "ai control":
                    if (AiControlEllipse.Fill.Equals(Brushes.LimeGreen))
                    {
                        AiControlEllipse.Fill = Brushes.Red;
                        OutputBox.AppendText("\nAi Control deactivated.");
                    }
                    else if (AiControlEllipse.Fill.Equals(Brushes.Gold))
                    {
                        AiControlEllipse.Fill = Brushes.Red;
                        OutputBox.AppendText("\nSystem in Warning Mode! Shutting system down.");
                    }
                    else
                    {
                        OutputBox.AppendText("\nAi Control already deactivated.");
                    }
                    break;
                case "containment field":
                    if (ContainmentFieldEllipse.Fill.Equals(Brushes.LimeGreen))
                    {
                        ContainmentFieldEllipse.Fill = Brushes.Red;
                        OutputBox.AppendText("\nContainment Field deactivated.");
                        FlashWarningContainment();
                    }
                    else if (ContainmentFieldEllipse.Fill.Equals(Brushes.Gold))
                    {
                        ContainmentFieldEllipse.Fill = Brushes.Red;
                        OutputBox.AppendText("\nSystem in Warning Mode! Shutting system down.");
                    }
                    else
                    {
                        OutputBox.AppendText("\nContainment Field already deactivated.");
                    }
                    break;
                case "ship engines":
                    if (ShipEnginesEllipse.Fill.Equals(Brushes.LimeGreen))
                    {
                        ShipEnginesEllipse.Fill = Brushes.Red;
                        OutputBox.AppendText("\nShip Engines deactivated.");
                    }
                    else if (ShipEnginesEllipse.Fill.Equals(Brushes.Gold))
                    {
                        ShipEnginesEllipse.Fill = Brushes.Red;
                        OutputBox.AppendText("\nSystem in Warning Mode! Shutting system down.");
                    }
                    else
                    {
                        OutputBox.AppendText("\nShip Engines already deactivated.");
                    }
                    break;
            }
        }

        private void LockCommand(string input)
        {
            switch (input)
            {
                case "door control":
                    if (DoorControlEllipse.Fill.Equals(Brushes.Red))
                    {
                        OutputBox.AppendText("\nDoors locked already!");
                    }
                    else if (DoorControlEllipse.Fill.Equals(Brushes.Gold))
                    {
                        OutputBox.AppendText("\nSystem in Warning Mode! Resolve problems before continuing.");
                    }
                    else
                    {
                        DoorControlEllipse.Fill = Brushes.Red;
                        OutputBox.AppendText("\nBuilding lockdown! Locking all doors.");
                    }
                    break;
            }
        }

        private void ResetCommand(string input)
        {
            switch (input)
            {
                case "door control":
                    if (ShipEnginesEllipse.Fill.Equals(Brushes.Gold))
                    {
                        OutputBox.AppendText("\nSystem in Warning Mode! Resolve problems before continuing.");
                    }
                    else
                    {
                        DoorControlEllipse.Fill = Brushes.LimeGreen;
                        OutputBox.AppendText("\nDoors unlocked. Door Control set to manual.");
                    }
                    break;
            }
        }

        #endregion 

        IEnumerable<string> SystemStatus()
        {
            var status = new List<string>();
            var output = new StringBuilder("");
            string name;
            foreach (var visual in FindVisualChildren<Ellipse>(Window))
            {
                foreach (var letter in visual.Name)
                {
                    if (Char.IsUpper(letter) && output.Length>0)
                    {
                        output.Append(" " + letter);
                    }
                    else
                    {
                        output.Append(letter);
                    }
                }

                name = output.ToString();
                name = name.Remove(name.Length - 7);

                if (visual.Fill.Equals(Brushes.Blue))
                {
                    status.Add("\n"+name+" Idle...");
                }
                else if (visual.Fill.Equals(Brushes.Gold))
                {
                    status.Add("\n" + name + " has encountered a warning! System halted!");
                }
                else if (visual.Fill.Equals(Brushes.Red))
                {
                    status.Add("\n" + name + " is in alert/error mode!");
                }
                else if (visual.Fill.Equals(Brushes.LimeGreen))
                {
                    status.Add("\n" + name + " is ready.");
                }
                output.Clear();
            }
            return status;
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    var child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

    }
}
