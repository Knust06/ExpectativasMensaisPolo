using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ExpectativaMensalPolo.Models;
using ExpectativaMensalPolo.Services;
using ExpectativaMensalPolo.Helpers;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace ExpectativaMensalPolo.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private DateTime _dataInicio;
        private DateTime _dataFim;
        private Indicador _indicadorSelecionado;
        private ObservableCollection<Expectativa> _expectativas;
        private PlotModel _plotModel;

        public MainViewModel()
        {
            _apiService = new ApiService();
            Indicadores = new ObservableCollection<Indicador>
            {
                new Indicador { Nome = "IPCA" },
                new Indicador { Nome = "IGP-M" },
                new Indicador { Nome = "Selic" }
            };
            DataInicio = DateTime.Now.AddMonths(-1);
            DataFim = DateTime.Now;
            BuscarExpectativasCommand = new RelayCommand(async (param) => await BuscarExpectativas());
            ExportarCsvCommand = new RelayCommand((param) => ExportarCsv());
            InitializePlotModel();
        }

        public ObservableCollection<Indicador> Indicadores { get; }
        public ObservableCollection<Expectativa> Expectativas
        {
            get => _expectativas;
            set
            {
                _expectativas = value;
                OnPropertyChanged();
            }
        }

        public DateTime DataInicio
        {
            get => _dataInicio;
            set
            {
                if (value <= DateTime.Now)
                {
                    _dataInicio = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime DataFim
        {
            get => _dataFim;
            set
            {
                if (value <= DateTime.Now)
                {
                    _dataFim = value;
                    OnPropertyChanged();
                }
            }
        }

        public Indicador IndicadorSelecionado
        {
            get => _indicadorSelecionado;
            set
            {
                _indicadorSelecionado = value;
                OnPropertyChanged();
            }
        }

        public PlotModel PlotModel
        {
            get => _plotModel;
            set
            {
                _plotModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand BuscarExpectativasCommand { get; }
        public ICommand ExportarCsvCommand { get; }

        private void InitializePlotModel()
        {
            PlotModel = new PlotModel { Title = "Expectativa Mensal" };
            PlotModel.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "yyyy-MM-dd" });
            PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
        }

        private async Task BuscarExpectativas()
        {
            if (IndicadorSelecionado != null)
            {
                var expectativas = await _apiService.ObterExpectativasAsync(IndicadorSelecionado.Nome, DataInicio, DataFim);
                Expectativas = new ObservableCollection<Expectativa>(expectativas);
                AtualizarGrafico();
            }
        }

        private void AtualizarGrafico()
        {
            var series = new LineSeries
            {
                Title = IndicadorSelecionado.Nome,
                MarkerType = MarkerType.Circle
            };

            var mediaDiaria = Expectativas
                .GroupBy(e => e.Data)
                .Select(g => new
                {
                    Data = g.Key,
                    Media = g.Average(e => e.Media)
                })
                .OrderBy(x => x.Data);

            foreach (var data in mediaDiaria)
            {
                series.Points.Add(new DataPoint(DateTimeAxis.ToDouble(data.Data), data.Media));
            }

            PlotModel.Series.Clear();
            PlotModel.Series.Add(series);
            PlotModel.InvalidatePlot(true);
        }

        private void ExportarCsv()
        {
            if (Expectativas == null || Expectativas.Count == 0)
            {
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = "Expectativas.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var csv = new StringBuilder();
                csv.AppendLine("Indicador,Data,DataReferencia,Media,Mediana,DesvioPadrao,Minimo,Maximo,NumeroRespondentes,BaseCalculo");

                foreach (var expectativa in Expectativas)
                {
                    csv.AppendLine($"{expectativa.Indicador},{expectativa.Data:yyyy-MM-dd},{expectativa.DataReferencia},{expectativa.Media},{expectativa.Mediana},{expectativa.DesvioPadrao},{expectativa.Minimo},{expectativa.Maximo},{expectativa.NumeroRespondentes},{expectativa.BaseCalculo}");
                }

                File.WriteAllText(saveFileDialog.FileName, csv.ToString());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
