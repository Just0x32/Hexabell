using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using IOExtension;

namespace Hexabell
{
    public class ViewModel : INotifyPropertyChanged
    {
        private Model model;
        private static readonly int taskQuantity = 6;

        #region [ Hexagon Design Properties ]
        public double HexagonStrokeThickness { get; private set; } = 3;

        public Brush[] buttonBackgroundColors;
        public Brush[] ButtonBackgroundColors
        {
            get => buttonBackgroundColors;
            private set
            {
                if (buttonBackgroundColors != value)
                    buttonBackgroundColors = value;

                OnPropertyChanged();
            }
        }

        private Brush[] buttonBorderBrushColors;
        public Brush[] ButtonBorderBrushColors
        {
            get => buttonBorderBrushColors;
            private set
            {
                if (buttonBorderBrushColors != value)
                    buttonBorderBrushColors = value;

                OnPropertyChanged();
            }
        }

        public Brush DisabledTaskButtonBackground { get; } = new SolidColorBrush(Color.FromArgb(93, 135, 206, 250));       // LightSkyBlue
        public Brush DisabledTaskButtonBorderBrush { get; } = new SolidColorBrush(Color.FromArgb(120, 135, 206, 250));       // LightSkyBlue
        public Brush EnabledTaskButtonBackground { get; } = new SolidColorBrush(Color.FromArgb(93, 255, 182, 193));        // LightPink
        public Brush EnabledTaskButtonBorderBrush { get; } = new SolidColorBrush(Color.FromArgb(120, 255, 182, 193));         // LightPink
        public Brush IsMouseOverTaskButtonBorderBrush { get; } = new SolidColorBrush(Color.FromArgb(200, 135, 206, 250));         // LightSkyBlue
        public Brush BasicPolygonBackground { get; } = new SolidColorBrush(Color.FromArgb(93, 135, 206, 250));       // LightSkyBlue
        public Brush BasicPolygonBorderBrush { get; } = new SolidColorBrush(Color.FromArgb(120, 135, 206, 250));       // LightSkyBlue
        public Brush TimeFontColor { get; } = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60));       // DimGray
        #endregion

        #region [ Hexagon Shape and Location ]
        private PointCollection hexagonPoints = new PointCollection
        {
            { new Point(50, 0) },
            { new Point(150, 0) },
            { new Point(200, 87) },
            { new Point(150, 173) },
            { new Point(50, 173) },
            { new Point(0, 87) },
        };
        public PointCollection HexagonPoints
        {
            get => hexagonPoints;
            private set
            {
                hexagonPoints = value;
                OnPropertyChanged();
            }
        }

        private Point[] taskButtonPivots = new Point[]
        {
            new Point(0, 107),
            new Point(185, 0),
            new Point(370, 107),
            new Point(370, 320),
            new Point(185, 426),
            new Point(0, 320),
        };
        public Point[] TaskButtonPivots
        {
            get => taskButtonPivots;
            private set
            {
                taskButtonPivots = value;
                OnPropertyChanged();
            }
        }

        private Point basicPolygonPivot = new Point(185, 213);
        public Point BasicPolygonPivot
        {
            get => basicPolygonPivot;
            private set
            {
                basicPolygonPivot = value;
                OnPropertyChanged();
            }
        }

        public GridPosition BasicPolygonGridPosition { get; private set; } = new GridPosition(2, 3);
        public GridPosition[] TaskButtonGridPositions { get; private set; } = new GridPosition[]
        {
            new GridPosition(2, 1),
            new GridPosition(1, 2),
            new GridPosition(1, 4),
            new GridPosition(2, 5),
            new GridPosition(3, 4),
            new GridPosition(3, 2)
        };
        #endregion

        #region [ Size Changing ]
        private static readonly int minHexagonSize = 20;
        private static readonly int maxHexagonSize = 70;
        private int hexagonSize = minHexagonSize;
        public int HexagonSize
        {
            get => hexagonSize;
            set
            {
                if (value != hexagonSize)
                    hexagonSize = ValidValue(value, minHexagonSize, maxHexagonSize);

                ChangeHexagonPoints(hexagonSize);
                UpdateFontSizeAndInterval();
                OnPropertyChanged();

                void ChangeHexagonPoints(int size)
                {
                    HexagonPoints = GetHexagonPoints();

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

                void UpdateFontSizeAndInterval()
                {
                    TimeFontSize = TimeFontSize;
                    HexagonInterval = HexagonInterval;
                }
            }
        }

        private static readonly int minHexagonInterval = 0;
        private static readonly int maxHexagonInterval = 50;
        private int hexagonInterval = minHexagonInterval;
        public int HexagonInterval
        {
            get => hexagonInterval;
            set
            {
                if (value != hexagonInterval)
                    hexagonInterval = ValidValue(value, minHexagonInterval, maxHexagonInterval);

                ChangeHexagonPivots(HexagonSize, hexagonInterval);
                CalculateWindowSize(HexagonSize, hexagonInterval);
                OnPropertyChanged();

                void ChangeHexagonPivots(int size, int interval)
                {
                    int xIndex = BasicPolygonGridPosition.XIndex;
                    int yIndex = BasicPolygonGridPosition.YIndex;
                    BasicPolygonPivot = new Point(XOffset(xIndex), YOffset(yIndex));

                    Point[] pivots = new Point[taskQuantity];
                    for (int i = 0; i < pivots.Length; i++)
                    {
                        xIndex = TaskButtonGridPositions[i].XIndex;
                        yIndex = TaskButtonGridPositions[i].YIndex;
                        pivots[i] = new Point(XOffset(xIndex), YOffset(yIndex));
                    }
                    TaskButtonPivots = pivots;

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

        private static readonly double minTimeFontSizeScale = 0.6;
        private static readonly double maxTimeFontSizeScale = 1.3;
        private double timeFontSizeScale = minTimeFontSizeScale;
        private double TimeFontSizeScale
        {
            get => timeFontSizeScale;
            set
            {
                if (timeFontSizeScale != value)
                    timeFontSizeScale = ValidValue(value, minTimeFontSizeScale, maxTimeFontSizeScale);

                TimeFontSize = TimeFontSize;
            }
        }

        public double TimeFontSize
        {
            get => HexagonSize * TimeFontSizeScale;
            private set => OnPropertyChanged();
        }

        private void CalculateWindowSize(int size, int interval)
        {
            double windowWidth = 10 * size + 1.73205d * interval;
            double windowHeight = 10.39236d * size + 2 * interval;

            if (windowWidth != WindowWidth)
                WindowWidth = windowWidth;

            if (windowHeight != WindowHeight)
                WindowHeight = windowHeight;
        }

        private double windowWidth;
        public double WindowWidth
        {
            get => windowWidth;
            private set
            {
                windowWidth = value;
                OnPropertyChanged();
            }
        }

        private double windowHeight;
        public double WindowHeight
        {
            get => windowHeight;
            private set
            {
                windowHeight = value;
                OnPropertyChanged();
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

        private double ValidValue(double value, double minValidValue, double maxValidValue)
        {
            double validValue;

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

        public List<string> ValidHours { get; private set; }
        public List<string> ValidMinutes { get; private set; }
        public string ValidSeparator { get; private set; }
        public string[] SoundPaths { get => model.SoundPaths; }
        public bool[] IsTaskEnabled { get => model.IsTaskEnabled; }

        private string[] taskTimes;
        public string[] TaskTimes
        {
            get => taskTimes;
            private set
            {
                if (taskTimes != value)
                    taskTimes = value;

                OnPropertyChanged();
            }
        }

        public string IsDefaultSettings { get => TryWriteDefaultSettings().ToString(); }      // Debug

        public ViewModel()
        {
            model = new Model(taskQuantity);
            model.PropertyChanged += ModelNotify;

            InitializeTime();
            InitializeSizes();
            InitializeColors();

            LoadSettings();

            void InitializeTime()
            {
                TaskTimes = new string[taskQuantity];

                ValidHours = GetIncrementedValues(23);
                ValidMinutes = GetIncrementedValues(59);
                ValidSeparator = ":";

                List<string> GetIncrementedValues(int maxValue)
                {
                    List<string> list = new List<string>();

                    for (int i = 0; i <= maxValue; i++)
                        list.Add(string.Format("{0:d2}", i));

                    return list;
                }
            }

            void InitializeSizes() => HexagonSize = HexagonSize;

            void InitializeColors()
            {
                ButtonBackgroundColors = new Brush[taskQuantity];
                ButtonBorderBrushColors = new Brush[taskQuantity];
                SetTaskButtonColors();
            }

            void LoadSettings()                 // From settings
            {
                HexagonSize = 50;
                HexagonInterval = 20;
                TimeFontSizeScale = 0.9;

                for (int i = 0; i < taskQuantity; i++)
                    SetTaskTime(i, i + 1, i + 1);

                for (int i = 0; i < taskQuantity; i++)
                    SetSoundPath(i, @"D:\music.mp3");
            }
        }

        private bool TryWriteDefaultSettings()
        {
            SettingsFile.TryDeleteSettingsFile();
            SettingsFile.SettingsFilePath = "settings.txt";

            if (SettingsFile.IsSettingsFileAvailable(true))
            {
                List<string[]> propertyValuePairs = new List<string[]>();
                propertyValuePairs.Add(new string[] { nameof(HexagonSize), HexagonSize.ToString() });
                propertyValuePairs.Add(new string[] { nameof(HexagonInterval), HexagonInterval.ToString() });
                propertyValuePairs.Add(new string[] { nameof(TimeFontSizeScale), string.Format(new CultureInfo("en-US"), "{0:f1}", TimeFontSizeScale) });
                for (int i = 0; i < taskQuantity; i++)
                    propertyValuePairs.Add(new string[] { nameof(TaskTimes)[..^1] + $"{i + 1}", TaskTimes[i] });

                for (int i = 0; i < propertyValuePairs.Count; i++)
                {
                    bool isWritingSuccessful = TryWritePropertyValuePair(propertyValuePairs[i][0], propertyValuePairs[i][1]);

                    if (!isWritingSuccessful)
                        return false;
                }

                return true;
            }
            else
            {
                return false;
            }

            bool TryWritePropertyValuePair(string property, string value)
            {
                SettingsFile.SetPropertyValue(property, value);

                if (SettingsFile.GetPropertyValue(property) == value)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void ChangeTaskState(int taskIndex) => model.ChangeTaskState(taskIndex);

        private void ModelNotify(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(model.TaskTimes))
                SetTaskTimes();

            if (e.PropertyName == nameof(model.IsTaskEnabled))
                SetTaskButtonColors();

            void SetTaskTimes()
            {
                for (int i = 0; i < taskQuantity; i++)
                    TaskTimes[i] = string.Format("{0:d2}", ValidHours[model.Hours[i]]) + ValidSeparator + string.Format("{0:d2}", ValidMinutes[model.Minutes[i]]);

                TaskTimes = TaskTimes;
            }
        }

        private void SetTaskButtonColors()
        {
            for (int i = 0; i < taskQuantity; i++)
            {
                ButtonBackgroundColors[i] = GetBackgroundColor(model.IsTaskEnabled[i]);
                ButtonBorderBrushColors[i] = GetBorderBrushColor(model.IsTaskEnabled[i]);
            }

            ColorsChangedNotify();

            Brush GetBackgroundColor(bool isTaskEnabled)
            {
                if (isTaskEnabled)
                {
                    return EnabledTaskButtonBackground;
                }
                else
                {
                    return DisabledTaskButtonBackground;
                }
            }

            Brush GetBorderBrushColor(bool isTaskEnabled)
            {
                if (isTaskEnabled)
                {
                    return EnabledTaskButtonBorderBrush;
                }
                else
                {
                    return DisabledTaskButtonBorderBrush;
                }
            }

            void ColorsChangedNotify()
            {
                ButtonBackgroundColors = ButtonBackgroundColors;
                ButtonBorderBrushColors = ButtonBorderBrushColors;
            }
        }

        private void SetTaskTime(int taskIndex, int hour, int minute) => model.SetTaskTime(taskIndex, hour, minute);

        private void SetSoundPath(int taskIndex, string soundPath) => model.SetSoundPath(taskIndex, soundPath);

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

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
