using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ExpectativaMensalPolo.Models;

namespace ExpectativaMensalPolo.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://olinda.bcb.gov.br/olinda/servico/Expectativas/versao/v1/odata/")
            };
        }

        public async Task<List<Expectativa>> ObterExpectativasAsync(string indicador, DateTime dataInicio, DateTime dataFim)
        {
            string endpoint = indicador == "Selic" ? "ExpectativasMercadoSelic" : "ExpectativaMercadoMensais";
            string url = $"{endpoint}?$filter=Indicador eq '{indicador}' and Data ge '{dataInicio:yyyy-MM-dd}' and Data le '{dataFim:yyyy-MM-dd}'";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ODataResponse<Expectativa>>(content);
            return result.Value;
        }

        private class ODataResponse<T>
        {
            public List<T> Value { get; set; }
        }
    }
}
