using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;


namespace Quoridor.UI
{
    public class BoardSizeProvider : INotifyPropertyChanged
    {
        private const double placePercentage = 0.08;
        private const double wallPercentage = 0.015;

        public BoardSizeProvider()
        {
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            OnPropertyChanged(nameof(FieldSize));
            OnPropertyChanged(nameof(WallSize));
        }

        public double WallSize { get { return Math.Min(Window.Current.Bounds.Height, Window.Current.Bounds.Width) * wallPercentage; } }

        public double FieldSize { get { return Math.Min(Window.Current.Bounds.Height, Window.Current.Bounds.Width) * placePercentage; } }
        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}