using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace GreenerConfigurator.Controls
{
    /// <summary>
    /// Interaction logic for ToggleSwitch.xaml
    /// </summary>
    public partial class ToggleSwitch : UserControl
    {
        #region [ Constructor(s) ]

        public ToggleSwitch()
        {
            InitializeComponent();
            //Switched = new EventHandler((send, arg) =>{      });
        }

        #endregion

        #region [ Dependency Property(s) ]

        public static DependencyProperty IsBubbleSourceProperty = DependencyProperty.RegisterAttached(
                                        "IsBubbleSource", typeof(Boolean), typeof(ToggleSwitch),
                                        new PropertyMetadata(false, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var tempValue = (bool)e.NewValue;
            if (tempValue)
            {
                var control = ((ToggleSwitch)source);

                control.borderTrack.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#008d36"));
                var ca = new ColorAnimation((Color)ColorConverter.ConvertFromString("#008d36"), TimeSpan.FromSeconds(.25));
                control.borderTrack.Background.BeginAnimation(SolidColorBrush.ColorProperty, ca);
                var da = new DoubleAnimation(10, TimeSpan.FromSeconds(.25));
                control.translateTransform.BeginAnimation(TranslateTransform.XProperty, da);
            }

        }

        public bool IsBubbleSource
        {
            get
            {
                return (bool)this.GetValue(IsBubbleSourceProperty);
            }
            set
            {
                this.SetValue(IsBubbleSourceProperty, value);
            }
        }

        public static void SetIsBubbleSource(UIElement element, Boolean value)
        {
            element.SetValue(IsBubbleSourceProperty, value);
        }

        public static Boolean GetIsBubbleSource(UIElement element)
        {
            return (Boolean)element.GetValue(IsBubbleSourceProperty);
        }

        #endregion

        #region [ Public Property(s) ]

        public ICommand ClickCommand;

        public Color TrackBackgroundOffColor
        {
            get
            {
                return trackBackgroundOffColor;
            }
            set
            {
                trackBackgroundOffColor = value;
                if (!IsOn)
                {
                    borderTrack.Background = new SolidColorBrush(value);
                }
            }
        }

        public Color CircleBackgroundColor
        {
            get
            {
                return circleBackgroundColor;
            }
            set
            {
                circleBackgroundColor = value;
                ellipseToggle.Fill = new SolidColorBrush(value);
            }
        }

        public Color CircleBorderColor
        {
            get
            {
                return circleBorderColor;
            }
            set
            {
                circleBorderColor = value;
                ellipseToggle.Stroke = new SolidColorBrush(value);
            }
        }

        public bool IsOn
        {
            get
            {
                if (buttonToggle.Tag.ToString() == "On")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value)
                {
                    buttonToggle.Tag = "On";
                    borderTrack.Background = new SolidColorBrush(trackBackgroundOnColor);
                    var ca = new ColorAnimation(TrackBackgroundOnColor, TimeSpan.FromSeconds(.25));
                    borderTrack.Background.BeginAnimation(SolidColorBrush.ColorProperty, ca);
                    var da = new DoubleAnimation(10, TimeSpan.FromSeconds(.25));
                    translateTransform.BeginAnimation(TranslateTransform.XProperty, da);
                    SetIsBubbleSource(this, true);
                }
                else
                {
                    buttonToggle.Tag = "Off";
                    borderTrack.Background = new SolidColorBrush(TrackBackgroundOnColor);
                    var ca = new ColorAnimation(TrackBackgroundOffColor, TimeSpan.FromSeconds(.25));
                    borderTrack.Background.BeginAnimation(SolidColorBrush.ColorProperty, ca);
                    var da = new DoubleAnimation(-10, TimeSpan.FromSeconds(.25));
                    translateTransform.BeginAnimation(TranslateTransform.XProperty, da);
                    SetIsBubbleSource(this, false);
                }
                //Switched(this, EventArgs.Empty);        
            }
        }

        public Color TrackBackgroundOnColor
        {
            get
            {
                return trackBackgroundOnColor;
            }
            set
            {
                trackBackgroundOnColor = value;
                if (IsOn)
                {
                    borderTrack.Background = new SolidColorBrush(value);
                }
            }
        }

        #endregion

        #region [ Public Method(s) ]

        public void buttonToggle_Click(object sender, RoutedEventArgs e)
        {
            IsOn = !IsOn;
        }

        #endregion

        #region [ Private Field(s) ]

        private Color trackBackgroundOffColor = Colors.Gray;

        private Color trackBackgroundOnColor = (Color)ColorConverter.ConvertFromString("#008d36");// System.Drawing.ColorTranslator.FromHtml("#008d36");// Color.froin.Red;

        private Color circleBackgroundColor = Colors.SteelBlue;

        private Color circleBorderColor = Colors.White;

        #endregion

    }
}
