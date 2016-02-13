using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CsGoBindingManager.Keyboard
{
    public class TouchScreenKeyboard : Window
    {
        #region Property & Variable & Constructor

        private static double _widthTouchKeyboard = 830;

        public static double WidthTouchKeyboard
        {
            get { return _widthTouchKeyboard; }
            set { _widthTouchKeyboard = value; }
        }

        protected static bool ShiftFlag { get; set; }

        protected static bool CapsLockFlag { get; set; }

        private static Window _InstanceObject;

        private static Brush _PreviousTextBoxBackgroundBrush;
        private static Brush _PreviousTextBoxBorderBrush;
        private static Thickness _PreviousTextBoxBorderThickness;

        private static Control _CurrentControl;

        public static string TouchScreenText
        {
            get
            {
                if (_CurrentControl is TextBox)
                {
                    return ((TextBox)_CurrentControl).Text;
                }
                if (_CurrentControl is PasswordBox)
                {
                    return ((PasswordBox)_CurrentControl).Password;
                }
                return "";
            }
            set
            {
                if (_CurrentControl is TextBox)
                {
                    ((TextBox)_CurrentControl).Text = value;
                }
                else if (_CurrentControl is PasswordBox)
                {
                    ((PasswordBox)_CurrentControl).Password = value;
                }
            }
        }

        public static RoutedUICommand CmdTlide = new RoutedUICommand();
        public static RoutedUICommand Cmd1 = new RoutedUICommand();
        public static RoutedUICommand Cmd2 = new RoutedUICommand();
        public static RoutedUICommand Cmd3 = new RoutedUICommand();
        public static RoutedUICommand Cmd4 = new RoutedUICommand();
        public static RoutedUICommand Cmd5 = new RoutedUICommand();
        public static RoutedUICommand Cmd6 = new RoutedUICommand();
        public static RoutedUICommand Cmd7 = new RoutedUICommand();
        public static RoutedUICommand Cmd8 = new RoutedUICommand();
        public static RoutedUICommand Cmd9 = new RoutedUICommand();
        public static RoutedUICommand Cmd0 = new RoutedUICommand();
        public static RoutedUICommand CmdMinus = new RoutedUICommand();
        public static RoutedUICommand CmdPlus = new RoutedUICommand();
        public static RoutedUICommand CmdBackspace = new RoutedUICommand();

        public static RoutedUICommand CmdTab = new RoutedUICommand();
        public static RoutedUICommand CmdQ = new RoutedUICommand();
        public static RoutedUICommand Cmdw = new RoutedUICommand();
        public static RoutedUICommand CmdE = new RoutedUICommand();
        public static RoutedUICommand CmdR = new RoutedUICommand();
        public static RoutedUICommand CmdT = new RoutedUICommand();
        public static RoutedUICommand CmdY = new RoutedUICommand();
        public static RoutedUICommand CmdU = new RoutedUICommand();
        public static RoutedUICommand CmdI = new RoutedUICommand();
        public static RoutedUICommand CmdO = new RoutedUICommand();
        public static RoutedUICommand CmdP = new RoutedUICommand();
        public static RoutedUICommand CmdOpenCrulyBrace = new RoutedUICommand();
        public static RoutedUICommand CmdEndCrultBrace = new RoutedUICommand();
        public static RoutedUICommand CmdOR = new RoutedUICommand();

        public static RoutedUICommand CmdCapsLock = new RoutedUICommand();
        public static RoutedUICommand CmdA = new RoutedUICommand();
        public static RoutedUICommand CmdS = new RoutedUICommand();
        public static RoutedUICommand CmdD = new RoutedUICommand();
        public static RoutedUICommand CmdF = new RoutedUICommand();
        public static RoutedUICommand CmdG = new RoutedUICommand();
        public static RoutedUICommand CmdH = new RoutedUICommand();
        public static RoutedUICommand CmdJ = new RoutedUICommand();
        public static RoutedUICommand CmdK = new RoutedUICommand();
        public static RoutedUICommand CmdL = new RoutedUICommand();
        public static RoutedUICommand CmdColon = new RoutedUICommand();
        public static RoutedUICommand CmdDoubleInvertedComma = new RoutedUICommand();
        public static RoutedUICommand CmdEnter = new RoutedUICommand();

        public static RoutedUICommand CmdShift = new RoutedUICommand();
        public static RoutedUICommand CmdZ = new RoutedUICommand();
        public static RoutedUICommand CmdX = new RoutedUICommand();
        public static RoutedUICommand CmdC = new RoutedUICommand();
        public static RoutedUICommand CmdV = new RoutedUICommand();
        public static RoutedUICommand CmdB = new RoutedUICommand();
        public static RoutedUICommand CmdN = new RoutedUICommand();
        public static RoutedUICommand CmdM = new RoutedUICommand();
        public static RoutedUICommand CmdGreaterThan = new RoutedUICommand();
        public static RoutedUICommand CmdLessThan = new RoutedUICommand();
        public static RoutedUICommand CmdQuestion = new RoutedUICommand();

        public static RoutedUICommand CmdSpaceBar = new RoutedUICommand();
        public static RoutedUICommand CmdClear = new RoutedUICommand();

        public TouchScreenKeyboard()
        {
            Width = WidthTouchKeyboard;
        }

        static TouchScreenKeyboard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TouchScreenKeyboard), new FrameworkPropertyMetadata(typeof(TouchScreenKeyboard)));

            SetCommandBinding();
        }

        #endregion Property & Variable & Constructor

        #region CommandRelatedCode

        private static void SetCommandBinding()
        {
            var CbTlide = new CommandBinding(CmdTlide, RunCommand);
            var Cb1 = new CommandBinding(Cmd1, RunCommand);
            var Cb2 = new CommandBinding(Cmd2, RunCommand);
            var Cb3 = new CommandBinding(Cmd3, RunCommand);
            var Cb4 = new CommandBinding(Cmd4, RunCommand);
            var Cb5 = new CommandBinding(Cmd5, RunCommand);
            var Cb6 = new CommandBinding(Cmd6, RunCommand);
            var Cb7 = new CommandBinding(Cmd7, RunCommand);
            var Cb8 = new CommandBinding(Cmd8, RunCommand);
            var Cb9 = new CommandBinding(Cmd9, RunCommand);
            var Cb0 = new CommandBinding(Cmd0, RunCommand);
            var CbMinus = new CommandBinding(CmdMinus, RunCommand);
            var CbPlus = new CommandBinding(CmdPlus, RunCommand);
            var CbBackspace = new CommandBinding(CmdBackspace, RunCommand);

            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbTlide);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), Cb1);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), Cb2);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), Cb3);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), Cb4);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), Cb5);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), Cb6);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), Cb7);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), Cb8);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), Cb9);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), Cb0);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbMinus);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbPlus);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbBackspace);

            var CbTab = new CommandBinding(CmdTab, RunCommand);
            var CbQ = new CommandBinding(CmdQ, RunCommand);
            var Cbw = new CommandBinding(Cmdw, RunCommand);
            var CbE = new CommandBinding(CmdE, RunCommand);
            var CbR = new CommandBinding(CmdR, RunCommand);
            var CbT = new CommandBinding(CmdT, RunCommand);
            var CbY = new CommandBinding(CmdY, RunCommand);
            var CbU = new CommandBinding(CmdU, RunCommand);
            var CbI = new CommandBinding(CmdI, RunCommand);
            var Cbo = new CommandBinding(CmdO, RunCommand);
            var CbP = new CommandBinding(CmdP, RunCommand);
            var CbOpenCrulyBrace = new CommandBinding(CmdOpenCrulyBrace, RunCommand);
            var CbEndCrultBrace = new CommandBinding(CmdEndCrultBrace, RunCommand);
            var CbOR = new CommandBinding(CmdOR, RunCommand);

            var CbCapsLock = new CommandBinding(CmdCapsLock, RunCommand);
            var CbA = new CommandBinding(CmdA, RunCommand);
            var CbS = new CommandBinding(CmdS, RunCommand);
            var CbD = new CommandBinding(CmdD, RunCommand);
            var CbF = new CommandBinding(CmdF, RunCommand);
            var CbG = new CommandBinding(CmdG, RunCommand);
            var CbH = new CommandBinding(CmdH, RunCommand);
            var CbJ = new CommandBinding(CmdJ, RunCommand);
            var CbK = new CommandBinding(CmdK, RunCommand);
            var CbL = new CommandBinding(CmdL, RunCommand);
            var CbColon = new CommandBinding(CmdColon, RunCommand);
            var CbDoubleInvertedComma = new CommandBinding(CmdDoubleInvertedComma, RunCommand);
            var CbEnter = new CommandBinding(CmdEnter, RunCommand);

            var CbShift = new CommandBinding(CmdShift, RunCommand);
            var CbZ = new CommandBinding(CmdZ, RunCommand);
            var CbX = new CommandBinding(CmdX, RunCommand);
            var CbC = new CommandBinding(CmdC, RunCommand);
            var CbV = new CommandBinding(CmdV, RunCommand);
            var CbB = new CommandBinding(CmdB, RunCommand);
            var CbN = new CommandBinding(CmdN, RunCommand);
            var CbM = new CommandBinding(CmdM, RunCommand);
            var CbGreaterThan = new CommandBinding(CmdGreaterThan, RunCommand);
            var CbLessThan = new CommandBinding(CmdLessThan, RunCommand);
            var CbQuestion = new CommandBinding(CmdQuestion, RunCommand);

            var CbSpaceBar = new CommandBinding(CmdSpaceBar, RunCommand);
            var CbClear = new CommandBinding(CmdClear, RunCommand);

            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbTab);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbQ);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), Cbw);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbE);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbR);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbT);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbY);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbU);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbI);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), Cbo);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbP);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbOpenCrulyBrace);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbEndCrultBrace);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbOR);

            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbCapsLock);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbA);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbS);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbD);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbF);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbG);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbH);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbJ);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbK);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbL);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbColon);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbDoubleInvertedComma);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbEnter);

            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbShift);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbZ);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbX);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbC);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbV);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbB);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbN);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbM);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbGreaterThan);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbLessThan);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbQuestion);

            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbSpaceBar);
            CommandManager.RegisterClassCommandBinding(typeof(TouchScreenKeyboard), CbClear);
        }

        private static void RunCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == CmdTlide)  // First Row
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += "`";
                }
                else
                {
                    TouchScreenText += "~";
                    ShiftFlag = false;
                }
            }
            else if (e.Command == Cmd1)
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += "1";
                }
                else
                {
                    TouchScreenText += "!";
                    ShiftFlag = false;
                }
            }
            else if (e.Command == Cmd2)
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += "2";
                }
                else
                {
                    TouchScreenText += "@";
                    ShiftFlag = false;
                }
            }
            else if (e.Command == Cmd3)
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += "3";
                }
                else
                {
                    TouchScreenText += "#";
                    ShiftFlag = false;
                }
            }
            else if (e.Command == Cmd4)
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += "4";
                }
                else
                {
                    TouchScreenText += "$";
                    ShiftFlag = false;
                }
            }
            else if (e.Command == Cmd5)
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += "5";
                }
                else
                {
                    TouchScreenText += "%";
                    ShiftFlag = false;
                }
            }
            else if (e.Command == Cmd6)
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += "6";
                }
                else
                {
                    TouchScreenText += "^";
                    ShiftFlag = false;
                }
            }
            else if (e.Command == Cmd7)
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += "7";
                }
                else
                {
                    TouchScreenText += "&";
                    ShiftFlag = false;
                }
            }
            else if (e.Command == Cmd8)
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += "8";
                }
                else
                {
                    TouchScreenText += "*";
                    ShiftFlag = false;
                }
            }
            else if (e.Command == Cmd9)
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += "9";
                }
                else
                {
                    TouchScreenText += "(";
                    ShiftFlag = false;
                }
            }
            else if (e.Command == Cmd0)
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += "0";
                }
                else
                {
                    TouchScreenText += ")";
                    ShiftFlag = false;
                }
            }
            else if (e.Command == CmdMinus)
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += "-";
                }
                else
                {
                    TouchScreenText += "_";
                    ShiftFlag = false;
                }
            }
            else if (e.Command == CmdPlus)
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += "=";
                }
                else
                {
                    TouchScreenText += "+";
                    ShiftFlag = false;
                }
            }
            else if (e.Command == CmdBackspace)
            {
                if (!string.IsNullOrEmpty(TouchScreenText))
                {
                    TouchScreenText = TouchScreenText.Substring(0, TouchScreenText.Length - 1);
                }
            }
            else if (e.Command == CmdTab)  // Second Row
            {
                TouchScreenText += "     ";
            }
            else if (e.Command == CmdQ)
            {
                AddKeyboardInput('Q');
            }
            else if (e.Command == Cmdw)
            {
                AddKeyboardInput('w');
            }
            else if (e.Command == CmdE)
            {
                AddKeyboardInput('E');
            }
            else if (e.Command == CmdR)
            {
                AddKeyboardInput('R');
            }
            else if (e.Command == CmdT)
            {
                AddKeyboardInput('T');
            }
            else if (e.Command == CmdY)
            {
                AddKeyboardInput('Y');
            }
            else if (e.Command == CmdU)
            {
                AddKeyboardInput('U');
            }
            else if (e.Command == CmdI)
            {
                AddKeyboardInput('I');
            }
            else if (e.Command == CmdO)
            {
                AddKeyboardInput('O');
            }
            else if (e.Command == CmdP)
            {
                AddKeyboardInput('P');
            }
            else if (e.Command == CmdOpenCrulyBrace)
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += "[";
                }
                else
                {
                    TouchScreenText += "{";
                    ShiftFlag = false;
                }
            }
            else if (e.Command == CmdEndCrultBrace)
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += "]";
                }
                else
                {
                    TouchScreenText += "}";
                    ShiftFlag = false;
                }
            }
            else if (e.Command == CmdOR)
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += @"\";
                }
                else
                {
                    TouchScreenText += "|";
                    ShiftFlag = false;
                }
            }
            else if (e.Command == CmdCapsLock)  // Third Row
            {
                if (!CapsLockFlag)
                {
                    CapsLockFlag = true;
                }
                else
                {
                    CapsLockFlag = false;
                }
            }
            else if (e.Command == CmdA)
            {
                AddKeyboardInput('A');
            }
            else if (e.Command == CmdS)
            {
                AddKeyboardInput('S');
            }
            else if (e.Command == CmdD)
            {
                AddKeyboardInput('D');
            }
            else if (e.Command == CmdF)
            {
                AddKeyboardInput('F');
            }
            else if (e.Command == CmdG)
            {
                AddKeyboardInput('G');
            }
            else if (e.Command == CmdH)
            {
                AddKeyboardInput('H');
            }
            else if (e.Command == CmdJ)
            {
                AddKeyboardInput('J');
            }
            else if (e.Command == CmdK)
            {
                AddKeyboardInput('K');
            }
            else if (e.Command == CmdL)
            {
                AddKeyboardInput('L');
            }
            else if (e.Command == CmdColon)
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += ";";
                }
                else
                {
                    TouchScreenText += ":";
                    ShiftFlag = false;
                }
            }
            else if (e.Command == CmdDoubleInvertedComma)
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += "'";
                }
                else
                {
                    TouchScreenText += Char.ConvertFromUtf32(34);
                    ShiftFlag = false;
                }
            }
            else if (e.Command == CmdEnter)
            {
                if (_InstanceObject != null)
                {
                    _InstanceObject.Close();
                    _InstanceObject = null;
                }
                _CurrentControl.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
            else if (e.Command == CmdShift) // Fourth Row
            {
                ShiftFlag = true; ;
            }
            else if (e.Command == CmdZ)
            {
                AddKeyboardInput('Z');
            }
            else if (e.Command == CmdX)
            {
                AddKeyboardInput('X');
            }
            else if (e.Command == CmdC)
            {
                AddKeyboardInput('C');
            }
            else if (e.Command == CmdV)
            {
                AddKeyboardInput('V');
            }
            else if (e.Command == CmdB)
            {
                AddKeyboardInput('B');
            }
            else if (e.Command == CmdN)
            {
                AddKeyboardInput('N');
            }
            else if (e.Command == CmdM)
            {
                AddKeyboardInput('M');
            }
            else if (e.Command == CmdLessThan)
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += ",";
                }
                else
                {
                    TouchScreenText += "<";
                    ShiftFlag = false;
                }
            }
            else if (e.Command == CmdGreaterThan)
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += ".";
                }
                else
                {
                    TouchScreenText += ">";
                    ShiftFlag = false;
                }
            }
            else if (e.Command == CmdQuestion)
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += "/";
                }
                else
                {
                    TouchScreenText += "?";
                    ShiftFlag = false;
                }
            }
            else if (e.Command == CmdSpaceBar) // Last row
            {
                TouchScreenText += " ";
            }
            else if (e.Command == CmdClear) // Last row
            {
                TouchScreenText = "";
            }
        }

        #endregion CommandRelatedCode

        #region Main Functionality

        private static void AddKeyboardInput(char input)
        {
            if (CapsLockFlag)
            {
                if (ShiftFlag)
                {
                    TouchScreenText += char.ToLower(input).ToString();
                    ShiftFlag = false;
                }
                else
                {
                    TouchScreenText += char.ToUpper(input).ToString();
                }
            }
            else
            {
                if (!ShiftFlag)
                {
                    TouchScreenText += char.ToLower(input).ToString();
                }
                else
                {
                    TouchScreenText += char.ToUpper(input).ToString();
                    ShiftFlag = false;
                }
            }
        }

        private static void SyncChild()
        {
            if (_CurrentControl != null && _InstanceObject != null)
            {
                var virtualpoint = new Point(0, _CurrentControl.ActualHeight + 3);
                var actualpoint = _CurrentControl.PointToScreen(virtualpoint);

                if (WidthTouchKeyboard + actualpoint.X > SystemParameters.VirtualScreenWidth)
                {
                    var difference = WidthTouchKeyboard + actualpoint.X - SystemParameters.VirtualScreenWidth;
                    _InstanceObject.Left = actualpoint.X - difference;
                }
                else if (!(actualpoint.X > 1))
                {
                    _InstanceObject.Left = 1;
                }
                else
                    _InstanceObject.Left = actualpoint.X;

                _InstanceObject.Top = actualpoint.Y;
                _InstanceObject.Show();
            }
        }

        public static bool GetTouchScreenKeyboard(DependencyObject obj)
        {
            return (bool)obj.GetValue(TouchScreenKeyboardProperty);
        }

        public static void SetTouchScreenKeyboard(DependencyObject obj, bool value)
        {
            obj.SetValue(TouchScreenKeyboardProperty, value);
        }

        public static readonly DependencyProperty TouchScreenKeyboardProperty =
            DependencyProperty.RegisterAttached("TouchScreenKeyboard", typeof(bool), typeof(TouchScreenKeyboard), new UIPropertyMetadata(default(bool), TouchScreenKeyboardPropertyChanged));

        private static void TouchScreenKeyboardPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var host = sender as FrameworkElement;
            if (host != null)
            {
                host.GotFocus += OnGotFocus;
                host.LostFocus += OnLostFocus;
            }
        }

        private static void OnGotFocus(object sender, RoutedEventArgs e)
        {
            var host = sender as Control;

            _PreviousTextBoxBackgroundBrush = host.Background;
            _PreviousTextBoxBorderBrush = host.BorderBrush;
            _PreviousTextBoxBorderThickness = host.BorderThickness;

            host.Background = Brushes.Yellow;
            host.BorderBrush = Brushes.Red;
            host.BorderThickness = new Thickness(4);

            _CurrentControl = host;

            if (_InstanceObject == null)
            {
                FrameworkElement ct = host;
                while (true)
                {
                    if (ct is Window)
                    {
                        ((Window)ct).LocationChanged += TouchScreenKeyboard_LocationChanged;
                        ((Window)ct).Activated += TouchScreenKeyboard_Activated;
                        ((Window)ct).Deactivated += TouchScreenKeyboard_Deactivated;
                        break;
                    }
                    ct = (FrameworkElement)ct.Parent;
                }

                _InstanceObject = new TouchScreenKeyboard
                                  {
                                      AllowsTransparency = true,
                                      WindowStyle = WindowStyle.None,
                                      ShowInTaskbar = false
                                  };
                _InstanceObject.ShowInTaskbar = false;
                _InstanceObject.Topmost = true;

                host.LayoutUpdated += tb_LayoutUpdated;
            }
        }

        private static void TouchScreenKeyboard_Deactivated(object sender, EventArgs e)
        {
            if (_InstanceObject != null)
            {
                _InstanceObject.Topmost = false;
            }
        }

        private static void TouchScreenKeyboard_Activated(object sender, EventArgs e)
        {
            if (_InstanceObject != null)
            {
                _InstanceObject.Topmost = true;
            }
        }

        private static void TouchScreenKeyboard_LocationChanged(object sender, EventArgs e)
        {
            SyncChild();
        }

        private static void tb_LayoutUpdated(object sender, EventArgs e)
        {
            SyncChild();
        }

        private static void OnLostFocus(object sender, RoutedEventArgs e)
        {
            var host = sender as Control;
            host.Background = _PreviousTextBoxBackgroundBrush;
            host.BorderBrush = _PreviousTextBoxBorderBrush;
            host.BorderThickness = _PreviousTextBoxBorderThickness;

            if (_InstanceObject != null)
            {
                _InstanceObject.Close();
                _InstanceObject = null;
            }
        }

        #endregion Main Functionality
    }
}