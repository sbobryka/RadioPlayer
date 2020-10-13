using RadioPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

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

        public ObservableCollection<Station> Stations { get; set; }

        public ViewModelMainWindow()
        {
            Stations = new ObservableCollection<Station>()
            {
                new Station("Record", @"http:\\record.ru"),
                new Station("Energy", @"http:\\energy.ru")
            };

            SelectedStation = Stations.FirstOrDefault();
        }
    }
}
