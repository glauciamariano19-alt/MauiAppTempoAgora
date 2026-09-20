using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "d31b98ab5f474b2a0d75bea318714c5a";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&units=metric&appid={chave}";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage resp = await client.GetAsync(url);

                    // Tratamento de cidade não encontrada

                    if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        await Application.Current.MainPage.DisplayAlert("Ops", "Cidade não encontrada. Verifique o nome e tente novamente.", "OK");
                        return null;
                    }

                    if (resp.IsSuccessStatusCode)
                    {
                        string json = await resp.Content.ReadAsStringAsync();

                        var rascunho = JObject.Parse(json);
                        if (rascunho["coord"] != null && rascunho["sys"] != null && rascunho["weather"] != null && rascunho["main"] != null)
                        {
                            DateTime time = DateTime.UnixEpoch;
                            DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                            DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                            t = new()
                            {
                                lat = (double?)rascunho["coord"]["lat"],
                                lon = (double?)rascunho["coord"]["lon"],
                                description = (string)rascunho["weather"][0]["description"],
                                main = (string)rascunho["weather"][0]["main"],
                                temp_min = (double?)rascunho["main"]["temp_min"],
                                temp_max = (double?)rascunho["main"]["temp_max"],
                                speed = (double?)rascunho["wind"]["speed"],
                                visibility = (int?)rascunho["visibility"],
                                sunrise = sunrise.ToString(),
                                sunset = sunset.ToString(),
                            }; // Fecha objeto Tempo
                        } // Fecha if se o status do servidor foi de sucesso 
                    } // Fecha laço using
                }
            }     //Tratamento de falta de internet
            catch (HttpRequestException)
            {
                await Application.Current.MainPage.DisplayAlert("Erro de conexão", "Sem acesso à internet. Verifique sua conexão e tente novamente.", "OK");
            }
            return t;
        }
    }
}
        


