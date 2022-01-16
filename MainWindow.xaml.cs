﻿using System;
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

        private readonly Brush enabledTaskButtonBorderBrush = new SolidColorBrush(Color.FromArgb(120, 255, 182, 193));         // LightPink
        
        private readonly Brush isMouseOverTaskButtonBorderBrush = new SolidColorBrush(Color.FromArgb(200, 135, 206, 250));         // LightSkyBlue
        public Brush IsMouseOverTaskButtonBorderBrush { get => isMouseOverTaskButtonBorderBrush; }

        public double HexagonStrokeThickness { get; private set; } = 3;

        private Dictionary<Button, int> indexFromButton = new Dictionary<Button, int>();
        private Dictionary<int, Button> buttonFromIndex = new Dictionary<int, Button>();

        private readonly GridPosition basicPolygonGridPosition = new GridPosition(2, 3);
        private readonly GridPosition[] taskButtonGridPositions = new GridPosition[]
        {
            new GridPosition(2, 5),
            new GridPosition(1, 4),
            new GridPosition(1, 2),
            new GridPosition(2, 1),
            new GridPosition(3, 2),
            new GridPosition(3, 4)
        };

        public PointCollection HexagonPoints { get; private set; } = new PointCollection
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
                InitializeBasicPolygon();
                InitializeTaskButtonIndexesAndColors(taskQuantity);
                HexagonSize = 30;
                HexagonInterval = 30;

                void InitializeBasicPolygon()
                {
                    BasicPolygon.Fill = disabledTaskButtonBackground;
                    BasicPolygon.Stroke = disabledTaskButtonBorderBrush;
                    BasicPolygon.Points = HexagonPoints;
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

        #region [ Changing UI size ]
        private int hexagonSize = 20;
        private int HexagonSize
        {
            get => hexagonSize;
            set
            {
                if (value != hexagonSize)
                    hexagonSize = ValidValue(value, 20, 70);

                ChangeHexagonPoints(hexagonSize);
                HexagonInterval = HexagonInterval;

                void ChangeHexagonPoints(int size)
                {
                    HexagonPoints = GetHexagonPoints();
                    BasicPolygon.Points = HexagonPoints;

                    PointCollection GetHexagonPoints()
                    {
                        PointCollection points = new PointCollection();

                        CalculatePoints();

                        return points;

                        void CalculatePoints()
                        {
                            points.Add(new Point(size, 0));
                            points.Add(new Point(3 * size, 0));
                            points.Add(new Point(4 * size, 1.73206 * size));
                            points.Add(new Point(3 * size, 3.46412 * size));
                            points.Add(new Point(size, 3.46412 * size));
                            points.Add(new Point(0, 1.73206 * size));
                        }
                    }
                }
            }
        }

        private int hexagonInterval = 0;
        private int HexagonInterval
        {
            get => hexagonInterval;
            set
            {
                if (value != hexagonInterval)
                    hexagonInterval = ValidValue(value, 0, 50);

                ChangeHexagonPivots(HexagonSize, hexagonInterval);
                
                void ChangeHexagonPivots(int size, int interval)
                {
                    int xIndex = basicPolygonGridPosition.XIndex;
                    int yIndex = basicPolygonGridPosition.YIndex;
                    Canvas.SetLeft(BasicPolygonGrid, XOffset(xIndex));
                    Canvas.SetTop(BasicPolygonGrid, YOffset(yIndex));

                    foreach (var taskButton in buttonFromIndex)
                    {
                        xIndex = taskButtonGridPositions[taskButton.Key].XIndex;
                        yIndex = taskButtonGridPositions[taskButton.Key].YIndex;
                        Canvas.SetLeft(taskButton.Value, XOffset(xIndex));
                        Canvas.SetTop(taskButton.Value, YOffset(yIndex));
                    }

                    double XOffset(int xIndex)
                    {
                        if (xIndex == 1)
                        {
                            return 0;
                        }
                        else
                        {
                            return (xIndex - 1) * (3 * size + 0.866025 * interval);
                        }
                    }

                    double YOffset(int yIndex)
                    {
                        if (yIndex == 1)
                        {
                            return 0;
                        }
                        else
                        {
                            return (yIndex - 1) * (1.73206 * size + 0.5 * interval);
                        }
                    }
                }
            }
        }

        private int ValidValue(int value, int minValidValue, int maxValidValue)
        {
            int validValue;

            if (value < minValidValue)
            {
                validValue = minValidValue;
            }
            else if (value > maxValidValue)
            {
                validValue = maxValidValue;
            }
            else
            {
                validValue = value;
            }

            return validValue;
        }
        #endregion

        private bool IsNoError()
        {
            return false;
        }

        #region [ Handlers ]
        private void BasicPolygon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void TaskButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeTaskState(sender as Button);
        }
        #endregion

        public struct GridPosition
        {
            public int XIndex { get; private set; }
            public int YIndex { get; private set; }

            public GridPosition(int xIndex, int yIndex)
            {
                XIndex = ValidValue(xIndex, 1, 100);
                YIndex = ValidValue(yIndex, 1, 100);

                int ValidValue(int value, int minValidValue, int maxValidValue)
                {
                    if (value < minValidValue)
                    {
                        return minValidValue;
                    }
                    else if (value > maxValidValue)
                    {
                        return maxValidValue;
                    }
                    else
                    {
                        return value;
                    }
                }
            }
        }
    }
}
