using RadioPlayer.Commands;
using RadioPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.Json;
using System.IO;
using RadioPlayer.Controllers;

namespace RadioPlayer.ViewModels
{
    internal class ViewModelMainWindow : ViewModelBase
    {
        public MediaPlayer MediaPlayer { get; } = new MediaPlayer();
        private StationsController stationsController = new StationsController();

        #region Флаг Играет

        private bool isPlayed = false;

        public bool IsPlayed
        {
            get => isPlayed;
            set => Set(ref isPlayed, value);
        }

        #endregion

        #region Статус

        private string status = "Готов";

        public string Status
        {
            get => status;
            set => Set(ref status, value);
        }

        #endregion

        #region Громкость
        private double volume = 0.5;

        public double Volume
        {
            get => MediaPlayer.Volume;
            set { MediaPlayer.Volume = volume = value; OnPropertyChanged(); }
        }

        #endregion

        #region Текущая станция

        private Station currentStation;

        public Station CurrentStation
        {
            get => currentStation;
            set => Set(ref currentStation, value);
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
            Station station = new Station("Новая станция", @"http://");
            Stations.Add(station);
            SelectedStation = station;
        }

        private bool CanAddStationExecute(object property) => true;

        #endregion

        #region Командa - удалить

        public ICommand RemoveStationCommand { get; }

        private void OnRemoveStationExecuted(object property)
        {
            Station station = (Station)property;

            MessageBoxResult result = MessageBox.Show($"Действительно хотите удалить станцию \"{station.Name}\"?", "Удаление станции", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Stations.Remove(station);
                SelectedStation = Stations.FirstOrDefault();
            }
        }

        private bool CanRemoveStationExecute(object property) => SelectedStation != null;

        #endregion

        #region Командa - сохранить

        public ICommand SaveStationsCommand { get; }

        private void OnSaveStationsExecuted(object property)
        {
            if (property != null)
            {
                IEnumerable<Station> stations = (IEnumerable<Station>)property;
                stationsController.SaveToFile(stations);
                Status = "Список станций сохранен";
            }
        }

        private bool CanSaveStationsExecute(object property) => Stations != null;

        #endregion

        #region Командa - загрузить

        public ICommand LoadStationsCommand { get; }

        private void OnLoadStationsExecuted(object property)
        {
            //if (fileStations.Exists)
            //{
            //    string json = fileStations.OpenText().ReadToEnd().ToString();
            //    var stations = JsonSerializer.Deserialize<ObservableCollection<Station>>(json);
            //    Stations = new ObservableCollection<Station>(stations.OrderBy(s => s.Name));
            //}

            //Status = "Список станций загружен";
        }

        private bool CanLoadStationsExecute(object property) => true;

        #endregion

        #region Командa - играть

        public ICommand PlayStationCommand { get; }

        private void OnPlayStationExecuted(object property)
        {
            try
            {
                Station station = property as Station;
                Status = $"Открытие {station.Address}";
                MediaPlayer.Close();
                MediaPlayer.Open(new Uri(station.Address));
                MediaPlayer.Play();
                Volume = volume;
                CurrentStation = station;
                IsPlayed = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanPlayStationExecute(object property) => SelectedStation != null;

        #endregion

        #region Командa - стоп

        public ICommand StopPlayingCommand { get; }

        private void OnStopPlayingExecuted(object property)
        {
            MediaPlayer.Stop();
            MediaPlayer.Close();
            Status = "Воспроизведения мультимедиа завершено.";
            IsPlayed = false;
        }

        private bool CanStopPlayingExecute(object property) => MediaPlayer != null;

        #endregion

        #region Список станций

        public ObservableCollection<Station> Stations { get; set; }

        #endregion

        #region Конструктор

        public ViewModelMainWindow()
        {
            #region Тестовые данные для списка станций

            //Stations = new ObservableCollection<Station>()
            //{
            //    new Station("Европа Плюс", @"http://ep128.hostingradio.ru:8030/ep128"),
            //    new Station("DiFM", @"https://dfm.hostingradio.ru/dfm128.mp3"),
            //};

            //Stations = new ObservableCollection<Station>(Stations.OrderBy(s => s.Name));

            //SelectedStation = Stations.FirstOrDefault();

            #endregion

            if (File.Exists(stationsController.FileName))
            {
                var stations = stationsController.LoadFromFile();
                Stations = new ObservableCollection<Station>(stations.OrderBy(s => s.Name));
                Status = "Список станций загружен";
            }
            else
            {
                Stations = new ObservableCollection<Station>();
            }

            #region Подписка на события проигрывателя

            MediaPlayer.MediaFailed += MediaPlayer_MediaFailed;
            MediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            MediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            MediaPlayer.BufferingStarted += MediaPlayer_BufferingStarted;
            MediaPlayer.BufferingEnded += MediaPlayer_BufferingEnded;

            #endregion

            AddStationCommand = new RelayCommand(OnAddStationExecuted, CanAddStationExecute);
            RemoveStationCommand = new RelayCommand(OnRemoveStationExecuted, CanRemoveStationExecute);
            PlayStationCommand = new RelayCommand(OnPlayStationExecuted, CanPlayStationExecute);
            SaveStationsCommand = new RelayCommand(OnSaveStationsExecuted, CanSaveStationsExecute);
            LoadStationsCommand = new RelayCommand(OnLoadStationsExecuted, CanLoadStationsExecute);
            StopPlayingCommand = new RelayCommand(OnStopPlayingExecuted, CanStopPlayingExecute);
        }

        #endregion

        #region События подключения

        private void MediaPlayer_MediaFailed(object sender, ExceptionEventArgs e)
        {
            Status = e.ErrorException.Message;
            if (IsPlayed)
                PlayStationCommand?.Execute(CurrentStation);
        }

        private void MediaPlayer_MediaOpened(object sender, EventArgs e) => Status = "Успешное открытие мультимедиа";

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            Status = "Завершение воспроизведения мультимедиа";
            if (IsPlayed)
                PlayStationCommand?.Execute(CurrentStation);
        }

        private void MediaPlayer_BufferingEnded(object sender, EventArgs e) => Status = "Начало буфферизации";

        private void MediaPlayer_BufferingStarted(object sender, EventArgs e) => Status = "Буфферизация завершена";

        #endregion
    }
}
