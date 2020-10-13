﻿using RadioPlayer.Commands;
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

namespace RadioPlayer.ViewModels
{
    internal class ViewModelMainWindow : ViewModelBase
    {
        public MediaPlayer MediaPlayer { get; } = new MediaPlayer();
        private FileInfo fileStations = new FileInfo("Stations.xml");

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
            Stations.Add(new Station("Новая станция", @"http://"));
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

        private bool CanRemoveStationExecute(object property) => SelectedStation != null;

        #endregion

        #region Командa - сохранить

        public ICommand SaveStationsCommand { get; }

        private void OnSaveStationsExecuted(object property)
        {
            var json = JsonSerializer.Serialize(property);
            File.WriteAllText(fileStations.Name, json);
            Status = "Список станций сохранен";
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
                MediaPlayer.Open(new Uri(((Station)property).Address));
                MediaPlayer.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanPlayStationExecute(object property) => SelectedStation != null;

        #endregion

        #region Список станций

        public ObservableCollection<Station> Stations { get; set; }

        #endregion

        #region Конструктор

        public ViewModelMainWindow()
        {
            //Stations = new ObservableCollection<Station>()
            //{
            //    new Station("Европа Плюс", @"http://ep128.hostingradio.ru:8030/ep128"),
            //    new Station("DiFM", @"https://dfm.hostingradio.ru/dfm128.mp3"),
            //};

            //Stations = new ObservableCollection<Station>(Stations.OrderBy(s => s.Name));

            //SelectedStation = Stations.FirstOrDefault();

            if (fileStations.Exists)
            {
                string json = fileStations.OpenText().ReadToEnd().ToString();
                var stations = JsonSerializer.Deserialize<ObservableCollection<Station>>(json);
                Stations = new ObservableCollection<Station>(stations.OrderBy(s => s.Name));
                Status = "Список станций загружен";
            }
            else
            {
                Stations = new ObservableCollection<Station>();
            }

            MediaPlayer.MediaFailed += MediaPlayer_MediaFailed;
            MediaPlayer.MediaOpened += MediaPlayer_MediaOpened;

            AddStationCommand = new RelayCommand(OnAddStationExecuted, CanAddStationExecute);
            RemoveStationCommand = new RelayCommand(OnRemoveStationExecuted, CanRemoveStationExecute);
            PlayStationCommand = new RelayCommand(OnPlayStationExecuted, CanPlayStationExecute);
            SaveStationsCommand = new RelayCommand(OnSaveStationsExecuted, CanSaveStationsExecute);
            LoadStationsCommand = new RelayCommand(OnLoadStationsExecuted, CanLoadStationsExecute);
        }

        #endregion

        #region События подключения

        private void MediaPlayer_MediaFailed(object sender, ExceptionEventArgs e)
        {
            Status = e.ErrorException.Message;
        }

        private void MediaPlayer_MediaOpened(object sender, EventArgs e)
        {
            Status = "Успешное открытие мультимедиа.";
        }

        #endregion
    }
}
