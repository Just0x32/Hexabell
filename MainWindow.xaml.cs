using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Hexabell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region [ Fields and Properties ]
        ViewModel viewModel;

        public bool[] IsTaskEnabled { get => viewModel.IsTaskEnabled; }

        public Brush[] ButtonBackgroundColors { get; private set; }

        public Brush[] ButtonBorderBrushColors { get; private set; }

        private readonly Brush disabledTaskButtonBackground = new SolidColorBrush(Color.FromArgb(93, 135, 206, 250));       // LightSkyBlue

        private readonly Brush disabledTaskButtonBorderBrush = new SolidColorBrush(Color.FromArgb(120, 135, 206, 250));       // LightSkyBlue

        private readonly Brush enabledTaskButtonBackground = new SolidColorBrush(Color.FromArgb(93, 255, 182, 193));        // LightPink

        public readonly Brush enabledTaskButtonBorderBrush = new SolidColorBrush(Color.FromArgb(120, 255, 182, 193));         // LightPink

        private Dictionary<Button, int> indexFromButton = new Dictionary<Button, int>();
        private Dictionary<int, Button> buttonFromIndex = new Dictionary<int, Button>();

        private int DefaultSize { get; set; }
        private int DefaultInterval { get; set; }
        public Hexagon[] Hexagons { get; private set; }

        public Polygon polygon;

        public PointCollection HexagonShape { get; private set; } = new PointCollection
        {
            { new Point(50, 0) },
            { new Point(150, 0) },
            { new Point(200, 87) },
            { new Point(150, 173) },
            { new Point(50, 173) },
            { new Point(0, 87) },
        };
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            int taskQuantity = 6;
            viewModel = new ViewModel(taskQuantity);

            InitializeShapes();

            void InitializeShapes()
            {
                BasicPolygon.Fill = disabledTaskButtonBackground;
                BasicPolygon.Stroke = disabledTaskButtonBorderBrush;

                InitializeTaskButtonIndexesAndColors(taskQuantity);
            }

            void InitializeTaskButtonIndexesAndColors(int taskQuantity)
            {
                ButtonBackgroundColors = new Brush[taskQuantity];
                ButtonBorderBrushColors = new Brush[taskQuantity];

                buttonFromIndex.Add(0, FirstTaskButton);
                buttonFromIndex.Add(1, SecondTaskButton);
                buttonFromIndex.Add(2, ThirdTaskButton);
                buttonFromIndex.Add(3, FourthTaskButton);
                buttonFromIndex.Add(4, FifthTaskButton);
                buttonFromIndex.Add(5, SixthTaskButton);

                for (int i = 0; i < taskQuantity; i++)
                {
                    ButtonBackgroundColors[i] = disabledTaskButtonBackground;
                    ButtonBorderBrushColors[i] = disabledTaskButtonBorderBrush;

                    indexFromButton.Add(buttonFromIndex[i], i);
                    SetTaskButtonColor(buttonFromIndex[i]);
                }
            }

            void InitializeAllHexagonPoints(int taskQuantity)
            {
                
            }
        }

        private void SetTaskButtonColor(Button taskButton)
        {
            int index = indexFromButton[taskButton];

            if (IsTaskEnabled[index])
            {
                ButtonBackgroundColors[index] = enabledTaskButtonBackground;
                ButtonBorderBrushColors[index] = enabledTaskButtonBorderBrush;
            }
            else
            {
                ButtonBackgroundColors[index] = disabledTaskButtonBackground;
                ButtonBorderBrushColors[index] = disabledTaskButtonBorderBrush;
            }

            taskButton.Background = ButtonBackgroundColors[index];
            taskButton.BorderBrush = ButtonBorderBrushColors[index];
        }

        private void ChangeTaskState(Button taskButton)
        {
            int index = indexFromButton[taskButton];

            viewModel.ChangeTaskState(index);
            SetTaskButtonColor(taskButton);
        }

        

        private bool IsNoError()
        {
            return false;
        }

        #region [ Handlers ]
        private void BasicPolygon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void TaskButton_Click(object sender, RoutedEventArgs e) => ChangeTaskState(sender as Button);
        #endregion

        public struct Hexagon
        {
            #region [ Properties ]
            public int Size { get; private set; }
            public int X1 { get; private set; }
            public int Y1 { get; private set; }
            public int X2 { get; private set; }
            public int Y2 { get; private set; }
            public int X3 { get; private set; }
            public int Y3 { get; private set; }
            public int X4 { get; private set; }
            public int Y4 { get; private set; }
            public int X5 { get; private set; }
            public int Y5 { get; private set; }
            public int X6 { get; private set; }
            public int Y6 { get; private set; }
            public int ElementXAxisNumber { get; private set; }
            public int ElementYAxisNumber { get; private set; }
            public int Interval { get; private set; }
            #endregion

            public Hexagon(int size, int interval, int elementXAxisNumber, int elementYAxisNumber)
            {
                Size = ValidValue(size, 10, 100);
                ElementXAxisNumber = ValidValue(elementXAxisNumber, 1, 5);
                ElementYAxisNumber = ValidValue(elementYAxisNumber, 1, 9);
                Interval = ValidValue(interval, 0, 10);

                X1 = ByCanvasZeroXOffset(Size, Interval, ElementXAxisNumber, Size);
                Y1 = ByCanvasZeroYOffset(Size, Interval, ElementYAxisNumber, 0);
                X2 = ByCanvasZeroXOffset(Size, Interval, ElementXAxisNumber, 3 * Size);
                Y2 = Y1;
                X3 = ByCanvasZeroXOffset(Size, Interval, ElementXAxisNumber, 4 * Size);
                Y3 = ByCanvasZeroYOffset(Size, Interval, ElementYAxisNumber, 1.73206d * Size);
                X4 = X2;
                Y4 = ByCanvasZeroYOffset(Size, Interval, ElementYAxisNumber, 3.4641d * Size);
                X5 = X1;
                Y5 = Y4;
                X6 = ByCanvasZeroXOffset(Size, Interval, ElementXAxisNumber, 0);
                Y6 = Y3;

                int ValidValue(int value, int minValidValue, int maxValidValue)
                {
                    if (value < minValidValue)
                    {
                        value = minValidValue;
                    }
                    else if (value > maxValidValue)
                    {
                        value = maxValidValue;
                    }

                    return value;
                }

                int ByCanvasZeroXOffset(int size, int interval, int elementXAxisNumber, int byThisShapeZeroOffset)
                {
                    if (elementXAxisNumber == 1)
                    {
                        return 0;
                    }
                    else
                    {
                        return (int)(size * (0.25d - 0.75d * elementXAxisNumber - 1) + 0.866025d * interval * (elementXAxisNumber - 1)) + byThisShapeZeroOffset;
                    }
                }

                int ByCanvasZeroYOffset(int size, int interval, int elementYAxisNumber, double byThisShapeZeroOffset)
                {
                    if (elementYAxisNumber == 1)
                    {
                        return 0;
                    }
                    else
                    {
                        return (int)(size * 0.5d * (elementYAxisNumber - 1) + interval * (elementYAxisNumber - 1) + byThisShapeZeroOffset);
                    }
                }
            }
        }

        //public struct Hexagon
        //{
        //    #region [ Properties ]
        //    public int Size { get; private set; }
        //    public int X1 { get; private set; }
        //    public int Y1 { get; private set; }
        //    public int X2 { get; private set; }
        //    public int Y2 { get; private set; }
        //    public int X3 { get; private set; }
        //    public int Y3 { get; private set; }
        //    public int X4 { get; private set; }
        //    public int Y4 { get; private set; }
        //    public int X5 { get; private set; }
        //    public int Y5 { get; private set; }
        //    public int X6 { get; private set; }
        //    public int Y6 { get; private set; }
        //    public int ElementXAxisNumber { get; private set; }
        //    public int ElementYAxisNumber { get; private set; }
        //    public int Interval { get; private set; }
        //    #endregion

        //    public Hexagon(int size, int interval, int elementXAxisNumber, int elementYAxisNumber)
        //    {
        //        Size = ValidValue(size, 10, 100);
        //        ElementXAxisNumber = ValidValue(elementXAxisNumber, 1, 5);
        //        ElementYAxisNumber = ValidValue(elementYAxisNumber, 1, 9);
        //        Interval = ValidValue(interval, 0, 10);

        //        X1 = ByCanvasZeroXOffset(Size, Interval, ElementXAxisNumber, Size);
        //        Y1 = ByCanvasZeroYOffset(Size, Interval, ElementYAxisNumber, 0);
        //        X2 = ByCanvasZeroXOffset(Size, Interval, ElementXAxisNumber, 3 * Size);
        //        Y2 = Y1;
        //        X3 = ByCanvasZeroXOffset(Size, Interval, ElementXAxisNumber, 4 * Size);
        //        Y3 = ByCanvasZeroYOffset(Size, Interval, ElementYAxisNumber, 1.73206d * Size);
        //        X4 = X2;
        //        Y4 = ByCanvasZeroYOffset(Size, Interval, ElementYAxisNumber, 3.4641d * Size);
        //        X5 = X1;
        //        Y5 = Y4;
        //        X6 = ByCanvasZeroXOffset(Size, Interval, ElementXAxisNumber, 0);
        //        Y6 = Y3;

        //        int ValidValue(int value, int minValidValue, int maxValidValue)
        //        {
        //            if (value < minValidValue)
        //            {
        //                value = minValidValue;
        //            }
        //            else if (value > maxValidValue)
        //            {
        //                value = maxValidValue;
        //            }

        //            return value;
        //        }

        //        int ByCanvasZeroXOffset(int size, int interval, int elementXAxisNumber, int byThisShapeZeroOffset)
        //        {
        //            if (elementXAxisNumber == 1)
        //            {
        //                return 0;
        //            }
        //            else
        //            {
        //                return (int)(size * (0.25d - 0.75d * elementXAxisNumber - 1) + 0.866025d * interval * (elementXAxisNumber - 1)) + byThisShapeZeroOffset;
        //            }
        //        }

        //        int ByCanvasZeroYOffset(int size, int interval, int elementYAxisNumber, double byThisShapeZeroOffset)
        //        {
        //            if (elementYAxisNumber == 1)
        //            {
        //                return 0;
        //            }
        //            else
        //            {
        //                return (int)(size * 0.5d * (elementYAxisNumber - 1) + interval * (elementYAxisNumber - 1) + byThisShapeZeroOffset);
        //            }
        //        }
        //    }
        //}
    }
}
