using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        string dados_previsao = $"Latitude= {t.lat}\n" + 
                                                $"Longetude= {t.lon}\n" +
                                                $"Nascer do sol= {t.sunrise}\n" +
                                                $"Por do sol= {t.sunset}\n" +
                                                $"Tempo Mínimo= {t.temp_min}ºC\n" +
                                                $"Tempo Máximo= {t.temp_max}ºC" +
                                                $"Descrição= {t.description}\n" +
                                                $"Velocidade do vento= {t.speed} m/s\n" +
                                                $"Visibilidade= {t.visibility} m";

                        lbl_res.Text = dados_previsao;
                    }
                    else
                    {
                        lbl_res.Text = "Sem dados de previsão";
                    }
                }
                else
                {
                    lbl_res.Text = "Preencha a cidade:";
                }

            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Ops", ex.Message, "OK");
            }
        }
    }
}

   