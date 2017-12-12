using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Quoridor.Model;

namespace Quoridor.Converters
{
    public class BoardElementTypeToColorConverter : IValueConverter
    {
        private readonly SolidColorBrush EMPTY_COLOR = new SolidColorBrush(Colors.DimGray);
        private readonly SolidColorBrush EMPTY_FOR_WALL_COLOR = new SolidColorBrush(Colors.DarkSlateGray);
        private readonly SolidColorBrush ENEMY_COLOR = new SolidColorBrush(Colors.Brown);

        private readonly SolidColorBrush PLAYER_COLOR = new SolidColorBrush(Colors.LightBlue);
        private readonly SolidColorBrush WALL_COLOR = new SolidColorBrush(Colors.Orange);

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var boardField = value as BoardField;

            if(boardField==null)
                throw new ArgumentException();

            if (boardField.Player != null)
            {
                return new SolidColorBrush(boardField.Player.PawnColor);
            }

            var element = boardField.FieldType;
            if (targetType != typeof (Brush)) throw new ArgumentOutOfRangeException();
            switch (element)
            {
                case BoardElementType.Empty:
                    return EMPTY_COLOR;
                case BoardElementType.EmptyForWall:
                    return EMPTY_FOR_WALL_COLOR;
                case BoardElementType.Player:
                    return PLAYER_COLOR;
               // case BoardElementType.EnemyPlayer:
              //      return ENEMY_COLOR;
                case BoardElementType.Wall:
                    return WALL_COLOR;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
          /*  var color = (Brush) value;
            if (targetType != typeof (BoardElementType)) throw new ArgumentOutOfRangeException();


            if (color == EMPTY_COLOR)
                return BoardElementType.Empty;

            if (color == EMPTY_FOR_WALL_COLOR)
                return BoardElementType.EmptyForWall;


            if (color == PLAYER_COLOR)
                return BoardElementType.Player;


            if (color == ENEMY_COLOR)
                return BoardElementType.EnemyPlayer;


            if (color == WALL_COLOR)
                return BoardElementType.Wall;
                */

            throw new ArgumentOutOfRangeException();
        }
    }
}