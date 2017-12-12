using System.ComponentModel;
using System.Runtime.CompilerServices;
using Quoridor.DataContracts;

namespace Quoridor.Model
{
    public class BoardField : INotifyPropertyChanged
    {
        public BoardField()
        {
            
        }

        public BoardField(BoardField boardField)
        {
            Position = new Position(boardField.Position);
            _fieldType = boardField.FieldType;
        }

        private BoardElementType _fieldType;
        public Position Position { get; set; }

        public QuoridorPlayer Player { get; set; } = null;
        

        public BoardElementType FieldType
        {
            get { return _fieldType; }
            set
            {
                _fieldType = value;
                OnPropertyChanged();
                /* OnPropertyChanged(nameof(IsWall));
                OnPropertyChanged(nameof(IsField));
                OnPropertyChanged(nameof(IsVerticalWall));
                OnPropertyChanged(nameof(IsHorizontalWall));
                OnPropertyChanged(nameof(IsMicroWall));*/
            }
        }

        public bool IsWall
        {
            get { return FieldType == BoardElementType.EmptyForWall || FieldType == BoardElementType.Wall; }
        }

        public bool IsField
        {
            get { return !IsWall; }
        }

        public bool IsMicroWall
        {
            get { return IsWall && Position.Y%2 == 1 && Position.X%2 == 1; }
        }


        public bool IsHorizontalWall
        {
            get { return IsWall && Position.Y%2 == 1 && Position.X%2 == 0; }
        }

        public bool IsVerticalWall
        {
            get { return IsWall && Position.X%2 == 1 && Position.Y%2 == 0; }
        }


        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}