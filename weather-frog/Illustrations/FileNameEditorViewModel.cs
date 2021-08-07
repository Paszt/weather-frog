using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using weatherfrog.Extensions;
using weatherfrog.Infrastructure;

namespace weatherfrog.Illustrations
{
    public class FileNameEditorViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private WeatherCondition weatherCondition;
        private string location;
        private TimeOfDay timeofDay;
        private string activity;
        private bool isBelow45;
        private string fileName;
        private ImageSource icon;

        public WeatherCondition WeatherCondition { get => weatherCondition; set => SetProperty(ref weatherCondition, value); }
        public string Location { get => location; set => SetProperty(ref location, value); }
        public TimeOfDay TimeOfDay { get => timeofDay; set => SetProperty(ref timeofDay, value); }
        public string Activity { get => activity; set => SetProperty(ref activity, value); }
        public bool IsBelow45 { get => isBelow45; set => SetProperty(ref isBelow45, value); }
        public string FileName { get => fileName; set => SetProperty(ref fileName, value); }

        public ImageSource Icon => icon ??= (ImageSource)System.Windows.Application.Current.FindResource("BuildStyle");

        public bool IsFogOrVog => weatherCondition == WeatherCondition.Fog || WeatherCondition == WeatherCondition.Vog;

        public bool IsNotFogOrVog => !IsFogOrVog;

        private void CreateFilename() =>
            FileName = WeatherConditionPortion() + LocationPortion() + TimeOfDayPortion() + ActivityPortion() + Below45Portion();

        private string WeatherConditionPortion() =>
            WeatherCondition == WeatherCondition.Any ? string.Empty : WeatherCondition.ToString();

        private string LocationPortion() =>
            WeatherCondition == WeatherCondition.Fog || WeatherCondition == WeatherCondition.Vog
                ? string.Empty
                : WeatherCondition == WeatherCondition.Any
                    ? Location.ToPascalCase()
                    : "_" + Location.ToPascalCase();

        private string TimeOfDayPortion() =>
            TimeOfDay == TimeOfDay.Any || WeatherCondition == WeatherCondition.Fog || WeatherCondition == WeatherCondition.Vog
                ? string.Empty
                : "_" + TimeOfDay.ToString();

        private string ActivityPortion() => "_" + Activity.ToCapitalizedKebabCase();

        private string Below45Portion() => IsBelow45 ? "_Cold" : string.Empty;

        private RelayCommand copyToClipboardCommand;
        public RelayCommand CopyToClipboardCommand => copyToClipboardCommand ??= new(
            () => Clipboard.SetText(FileName), () => !string.IsNullOrEmpty(FileName));

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;
            storage = value;
            NotifyPropertyChanged(propertyName);
            if (propertyName != nameof(FileName))
                CreateFilename();
            if (propertyName == nameof(WeatherCondition))
            {
                NotifyPropertyChanged(nameof(IsFogOrVog));
                NotifyPropertyChanged(nameof(IsNotFogOrVog));
            }
            return true;
        }

        #endregion

        #region INotifyDataErrorInfo

        private readonly Dictionary<string, List<string>> errors = new();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public System.Collections.IEnumerable GetErrors(string propertyName) =>
            string.IsNullOrEmpty(propertyName) || !errors.ContainsKey(propertyName)
                ? null
                : (System.Collections.IEnumerable)errors[propertyName];

        public bool HasErrors => errors.Count > 0;

        public void RaiseErrorsChanged(string propertyName) =>
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

        // Adds the specified error to the errors collection if it is not 
        // already present, inserting it in the first position if isWarning is 
        // false. Raises the ErrorsChanged event if the collection changes. 
        public void AddError(string propertyName, string error, bool isWarning)
        {
            if (!errors.ContainsKey(propertyName))
                errors[propertyName] = new List<string>();

            if (!errors[propertyName].Contains(error))
            {
                if (isWarning) errors[propertyName].Add(error);
                else errors[propertyName].Insert(0, error);
                RaiseErrorsChanged(propertyName);
            }
        }

        // Removes the specified error from the errors collection if it is
        // present. Raises the ErrorsChanged event if the collection changes.
        public void RemoveError(string propertyName, string error)
        {
            if (errors.ContainsKey(propertyName) && errors[propertyName].Contains(error))
            {
                errors[propertyName].Remove(error);
                if (errors[propertyName].Count == 0) errors.Remove(propertyName);
                RaiseErrorsChanged(propertyName);
            }
        }




        #endregion
    }
}
