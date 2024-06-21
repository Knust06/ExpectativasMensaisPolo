using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ExpectativaMensalPolo.Models;
using ExpectativaMensalPolo.Services;
using ExpectativaMensalPolo.Helpers;
using Microsoft.Win32;

namespace ExpectativaMensalPolo.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private DateTime _dataInicio;
        private DateTime _dataFim;
        private Indicador _indicadorSelecionado;
        private ObservableCollection<Expectativa> _expectativas;

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

        public ICommand BuscarExpectativasCommand { get; }
        public ICommand ExportarCsvCommand { get; }

        private async Task BuscarExpectativas()
        {
            if (IndicadorSelecionado != null)
            {
                Expectativas = new ObservableCollection<Expectativa>(await _apiService.ObterExpectativasAsync(IndicadorSelecionado.Nome, DataInicio, DataFim));
            }
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
