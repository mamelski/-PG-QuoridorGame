using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI;
using Quoridor.Annotations;

namespace Quoridor.Model
{
    public class PlayerParameters : INotifyPropertyChanged, IEquatable<PlayerParameters>
    {
        public bool Equals(PlayerParameters other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_name, other._name) && _pawnColor.Equals(other._pawnColor) && _playerType == other._playerType && _startingPosition == other._startingPosition;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PlayerParameters) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_name != null ? _name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ _pawnColor.GetHashCode();
                hashCode = (hashCode*397) ^ (int) _playerType;
                hashCode = (hashCode*397) ^ (int) _startingPosition;
                return hashCode;
            }
        }

        public static bool operator ==(PlayerParameters left, PlayerParameters right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PlayerParameters left, PlayerParameters right)
        {
            return !Equals(left, right);
        }

        private string _name;
        private Color _pawnColor;
        private PlayerType _playerType;
        private PlayerStartingPosition _startingPosition;

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public PlayerStartingPosition StartingPosition
        {
            get { return _startingPosition; }
            set
            {
                if (value == _startingPosition) return;
                _startingPosition = value;
                OnPropertyChanged();
            }
        }

        public Color PawnColor
        {
            get { return _pawnColor; }
            set
            {
                if (value.Equals(_pawnColor)) return;
                _pawnColor = value;
                OnPropertyChanged();
            }
        }

        public PlayerType PlayerType
        {
            get { return _playerType; }
            set
            {
                if (value == _playerType) return;
                _playerType = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}