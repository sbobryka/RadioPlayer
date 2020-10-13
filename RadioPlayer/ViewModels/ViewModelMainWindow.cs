using RadioPlayer.Commands;
using RadioPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace RadioPlayer.ViewModels
{
    internal class ViewModelMainWindow : ViewModelBase
    {
        #region Статус

        private string status = "Готов";

        public string Status
        {
            get => status;
            set => Set(ref status, value);
        }

        #endregion

        #region Выбранная станция

        private Station selectedStation;

        public Station SelectedStation
        {
            get => selectedStation;
            set => Set(ref selectedStation, value);
        }

        #endregion

        #region Командa - добавить

        public ICommand AddStationCommand { get; }

        private void OnAddStationExecuted(object property)
        {
            Stations.Add(new Station("Новая станция", @"http:\\"));
        }

        private bool CanAddStationExecute(object property) => true;

        #endregion

        #region Командa - удалить

        public ICommand RemoveStationCommand { get; }

        private void OnRemoveStationExecuted(object property)
        {
            Stations.Remove(property as Station);
            SelectedStation = Stations.FirstOrDefault();
        }

        private bool CanRemoveStationExecute(object property) => Stations.Count > 0;

        #endregion

        #region Список станций

        public ObservableCollection<Station> Stations { get; set; }

        #endregion

        #region Конструктор

        public ViewModelMainWindow()
        {
            Stations = new ObservableCollection<Station>()
            {
                new Station("Record", @"http:\\record.ru"),
                new Station("Energy", @"http:\\energy.ru"),
                new Station("Авто радио", @"http:\\avtoradio.ru")
            };

            Stations = new ObservableCollection<Station>(Stations.OrderBy(s => s.Name));

            SelectedStation = Stations.FirstOrDefault();

            AddStationCommand = new RelayCommand(OnAddStationExecuted, CanAddStationExecute);
            RemoveStationCommand = new RelayCommand(OnRemoveStationExecuted, CanRemoveStationExecute);
        }

        #endregion
    }
}
