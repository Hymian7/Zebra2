using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ZebraDesktop.Enums
{
    public class EnumSelection<T> : INotifyPropertyChanged where T : struct, IComparable, IFormattable, IConvertible
    {
        private T value; // stored value of the Enum
        private bool isFlagged; // Enum uses flags?
        private bool canDeselect; // Can be deselected? (Radio buttons cannot deselect, checkboxes can)
        private T blankValue; // what is considered the "blank" value if it can be deselected?

        public EnumSelection(T value) : this(value, false, default(T)) { }
        public EnumSelection(T value, bool canDeselect) : this(value, canDeselect, default(T)) { }
        public EnumSelection(T value, T blankValue) : this(value, true, blankValue) { }
        public EnumSelection(T value, bool canDeselect, T blankValue)
        {
            if (!typeof(T).IsEnum) throw new ArgumentException($"{nameof(T)} must be an enum type"); // I really wish there was a way to constrain generic types to enums...
            isFlagged = typeof(T).IsDefined(typeof(FlagsAttribute), false);

            this.value = value;
            this.canDeselect = canDeselect;
            this.blankValue = blankValue;
        }

        public T Value
        {
            get { return value; }
            set
            {
                if (this.value.Equals(value)) return;
                this.value = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Item[]"); // Notify that the indexer property has changed
            }
        }

        [IndexerName("Item")]
        public bool this[T key]
        {
            get
            {
                int iKey = (int)(object)key;
                return isFlagged ? ((int)(object)value & iKey) == iKey : value.Equals(key);
            }
            set
            {
                if (isFlagged)
                {
                    int iValue = (int)(object)this.value;
                    int iKey = (int)(object)key;

                    if (((iValue & iKey) == iKey) == value) return;

                    if (value)
                        Value = (T)(object)(iValue | iKey);
                    else
                        Value = (T)(object)(iValue & ~iKey);
                }
                else
                {
                    if (this.value.Equals(key) == value) return;
                    if (!value && !canDeselect) return;

                    Value = value ? key : blankValue;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
