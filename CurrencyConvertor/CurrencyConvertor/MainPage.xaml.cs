using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;

namespace CurrencyConvertor
{
    public partial class MainPage : ContentPage
    {
        public string url = "https://www.nbrb.by/services/xmlexrates.aspx?ondate=";
        public MainPage()
        {
            InitializeComponent();
            GetAllCurrencies(null);
        }

        public async void GetAllCurrencies(string date, int column =3)
        {
            try
            {
                if(date==null) date = DateTime.Now.ToString("MM/dd/yyyy").Replace(".", "/");
                
                string lasturl =  url + date; 
                HttpClient client = new HttpClient();

                var result = await client.GetAsync(lasturl);
                string xml = await result.Content.ReadAsStringAsync();

                string jsonText = XmlDocumentToJson(xml);

                StackLayout main = new StackLayout();
                main.Orientation = StackOrientation.Vertical;

                Root dailyExRates = new Root();
                dailyExRates = JsonConvert.DeserializeObject<Root>(jsonText);

                currencyname.Children.Clear();

                foreach (var item in dailyExRates.DailyExRates.Currency)
                {
                    
                    StackLayout stackLayout = new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        Margin = new Thickness(10, 0, 0, 0)
                    };

                   
                    Label label = new Label()
                    {
                        TextColor = Color.Black,
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 15,
                        Text = item.CharCode
                    };

                    Label currencyName = new Label()
                    {
                        TextColor = Color.Black,
                        FontAttributes = FontAttributes.None,
                        FontSize = 10,
                        Text = "1 " + item.Name
                    };
                    currencyname.Children.Add(label);
                    currencyname.Children.Add(currencyName);

                    if(column ==1)
                    {
                        Label first = new Label()
                        {
                            Text = item.Rate,
                            FontSize = 15,
                            TextColor = Color.Black,
                            FontAttributes = FontAttributes.Bold,
                            VerticalTextAlignment = TextAlignment.Center,

                        };
                        StackLayout temp = new StackLayout()
                        {
                            HeightRequest = 13.7
                        };

                        firstcurrency.Children.Add(first);
                        firstcurrency.Children.Add(temp);
                    }
                    if (column==2)
                    {
                        Label second = new Label()
                        {

                            Text = item.Rate,
                            FontSize = 15,
                            TextColor = Color.Black,
                            FontAttributes = FontAttributes.Bold,
                            VerticalTextAlignment = TextAlignment.Center,
                            Margin = new Thickness(40, 0, 0, 0)
                        };
                        StackLayout temp = new StackLayout()
                        {
                            HeightRequest = 13.7
                        };
                        secondcurrency.Children.Add(second);
                        secondcurrency.Children.Add(temp);

                    }
                    if(column==3)
                    {
                        Label first = new Label()
                        {
                            Text = item.Rate,
                            FontSize = 15,
                            TextColor = Color.Black,
                            FontAttributes = FontAttributes.Bold,
                            VerticalTextAlignment = TextAlignment.Center,

                        };
                        StackLayout temp = new StackLayout()
                        {
                            HeightRequest = 13.7
                        };

                        firstcurrency.Children.Add(first);
                        firstcurrency.Children.Add(temp);

                        Label second = new Label()
                        {

                            Text = item.Rate,
                            FontSize = 15,
                            TextColor = Color.Black,
                            FontAttributes = FontAttributes.Bold,
                            VerticalTextAlignment = TextAlignment.Center,
                            Margin = new Thickness(40, 0, 0, 0)
                        };
                        StackLayout temp2 = new StackLayout()
                        {
                            HeightRequest = 13.7
                        };
                        secondcurrency.Children.Add(second);
                        secondcurrency.Children.Add(temp2);
                    }

                    

                }
            }
            catch (Exception ex)
            {

                throw;
            }


        }
        public static string XmlDocumentToJson(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return JsonConvert.SerializeXmlNode(doc);
        }

        private void firstDataPicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            
            firstcurrency.Children.Clear();
            string date = firstDataPicker.Date.ToString("MM/dd/yyyy").Replace(".","/");
            GetAllCurrencies(date,1);

        }

        private void secondDataPicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            secondcurrency.Children.Clear();
            string date = secondDataPicker.Date.ToString("MM/dd/yyyy").Replace(".", "/");
            GetAllCurrencies(date,2);

        }

        private void btn2_Clicked(object sender, EventArgs e)
        {

        }
    }

    public class Currency
    {
        [JsonProperty("@Id")]
        public string Id { get; set; }
        public string NumCode { get; set; }
        public string CharCode { get; set; }
        public string Scale { get; set; }
        public string Name { get; set; }
        public string Rate { get; set; }
    }

    public class DailyExRates
    {
        [JsonProperty("@Date")]
        public string Date { get; set; }
        public List<Currency> Currency { get; set; }
    }

    public class Root
    {
        [JsonProperty("?xml")]
        public Xml xml { get; set; }
        public DailyExRates DailyExRates { get; set; }
    }

    public class Xml
    {
        [JsonProperty("@version")]
        public string version { get; set; }

        [JsonProperty("@encoding")]
        public string encoding { get; set; }
    }
}
