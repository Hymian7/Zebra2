using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ZebraDesktop.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region Interfaces
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        /// <summary>
        /// Manually trigger the RaiseCanExecuteChanged method for all DelegateCommands
        /// </summary>
        protected void UpdateButtonStatus()
        {
            foreach (System.Reflection.PropertyInfo prop in this.GetType().GetProperties())
            {
                if (prop.PropertyType == typeof(DelegateCommand))
                {
                    var func = prop.GetValue(this);
                    (func as DelegateCommand).RaiseCanExecuteChanged();
                }
            }
        }

    }
}
