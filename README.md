# ExpectativaMensalPolo

Este projeto é uma aplicação WPF em C# que exibe a série histórica da expectativa do mercado mensal fornecida pelo Banco Central do Brasil. A aplicação permite aos usuários selecionar diferentes indicadores econômicos (IPCA, IGP-M, e Selic) e o período de dados a ser exibido. Os dados são consultados através de uma API pública do Banco Central e apresentados em um DataGrid. A aplicação também oferece a funcionalidade de exportar os dados exibidos para um arquivo CSV e exibir um gráfico linear das médias diárias.

## Estrutura do Projeto

```
ExpectativaMensalPolo/
├── Helpers/
│   ├── NotifyPropertyChanged.cs
│   ├── RelayCommand.cs
├── Models/
│   ├── Expectativa.cs
│   ├── Indicadores.cs
├── Services/
│   ├── ApiService.cs
├── ViewModels/
│   ├── MainViewModel.cs
├── Views/
│   ├── MainWindow.xaml
│   ├── MainWindow.xaml.cs
├── App.xaml
├── App.xaml.cs
├── ExpectativaMensalPolo.csproj
└── README.md
```

### Pastas e Arquivos

- **Helpers/**: Contém classes auxiliares para implementar o padrão MVVM.
  - `NotifyPropertyChanged.cs`: Implementa a interface `INotifyPropertyChanged` para notificar a UI sobre mudanças nas propriedades.
  - `RelayCommand.cs`: Implementa a interface `ICommand` para comandos no MVVM.

- **Models/**: Contém as classes de modelo que representam os dados.
  - `Expectativa.cs`: Representa os dados de expectativa do mercado.
  - `Indicadores.cs`: Representa os indicadores econômicos disponíveis.

- **Services/**: Contém serviços para acessar a API do Banco Central.
  - `ApiService.cs`: Serviço responsável por fazer chamadas assíncronas à API.

- **ViewModels/**: Contém a ViewModel principal.
  - `MainViewModel.cs`: Gerencia a lógica da aplicação e a interação entre os modelos e as views.

- **Views/**: Contém as views (interfaces de usuário) da aplicação.
  - `MainWindow.xaml`: Define a interface de usuário principal.
  - `MainWindow.xaml.cs`: Codigo do `MainWindow.xaml`.

- **App.xaml**: Define a aplicação WPF.
- **App.xaml.cs**: Codigo do `App.xaml`.

## Funcionalidades

- **Seleção de Indicadores**: Permite selecionar entre IPCA, IGP-M e Selic.
- **Seleção de Período**: Permite definir a data de início e fim para a consulta dos dados.
- **Exibição de Dados**: Mostra os dados consultados em um DataGrid.
- **Exibição de Gráfico**: Exibe um gráfico linear das médias diárias das expectativas.
- **Exportação para CSV**: Exporta os dados exibidos para um arquivo CSV.

## Requisitos

- .NET 6 ou mais recente
- Visual Studio 2019 ou mais recente

## Dependências

- OxyPlot.Wpf: Para exibição de gráficos.
  - Para instalar, use o comando do NuGet:
    ```sh
    Install-Package OxyPlot.Wpf
    ```

## Autor

- [Lucas Knust](https://github.com/Knust06)
