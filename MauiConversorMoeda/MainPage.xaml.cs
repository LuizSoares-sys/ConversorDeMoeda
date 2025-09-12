using System.Globalization;

namespace MauiConversorMoeda
{
    public partial class MainPage : ContentPage
    {

        private readonly Dictionary<string, decimal>
              _ToBRL = new()
              {
                { "USD", 5.60m },
                { "EUR", 6.10m },
                { "BRL", 1.00m },


              };

        private readonly Dictionary<string, string>
            _cultureByCurreny = new()
              {
                { "USD", "en-US" },
                { "EUR", "de-DE" },
                { "BRL", "pt-BR" },
              };



        public MainPage()
        {
            InitializeComponent();
            InitDefaults(); //define a escolha padrao de moedas
        }

        void InitDefaults()
        {
            FromPicker.SelectedIndex = IndexOf(FromPicker, "BRL");
            ToPicker.SelectedIndex = IndexOf(ToPicker, "USD");
            //define por padrao q o "A" seja BRL e o "B" USD

            InfoLabel.Text = "Valoeres ficticios.";
            ResultLabel.Text = string.Empty;

        }

        int IndexOf(Picker picker, string item) =>
            picker.Items.IndexOf(item);


        void OnInverterClicked(object sender, EventArgs e)
        {
            var fromIndex = FromPicker.SelectedIndex;
            FromPicker.SelectedIndex = ToPicker.SelectedIndex;
            ToPicker.SelectedIndex = fromIndex;
            // InfoRateHint();

        }


        void OnPickerChanged(object sender, EventArgs e)
        {
            // InfoRateHint();
            ResultLabel.Text = string.Empty;


        }

        void OnamountChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AmountEntry.Text))   //se o campo estiver vazio nao faz nada 
            {
                InfoLabel.Text = "Informe um valor para converter.";
            }
            else
            {
                // InfoRateHint();
            }
        }

        void InfoRateHint()//Chama a mensagem educativa
        {
            var from = GetFrom();
            var to = GetTo();

            if (from is null || to is null) return;

            if (from == to)
            {
                InfoLabel.Text = "Mesma moeda selecionada";
                return;
            }
            else
            {
                var rate = Rate(from, to);
                InfoLabel.Text = $"1 {from} = {rate: 0.####} {to}";


            }
        }
        async void OnConverterClicked(object sender, EventArgs e)
        {
            try
            {
                var from = GetFrom();
                var to = GetTo();

                if (string.IsNullOrWhiteSpace(AmountEntry.Text))

                {
                    await DisplayAlert("Atenção", "Informe um valor para converter.", "OK");
                    return;

                }
                if (decimal.TryParse(AmountEntry.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out var amount) || amount < 0)
                {

                    await DisplayAlert("Atençao", "Valor Inválido", "Ok");
                    return;
                }

                var result = Convert(from, to, amount);

                var culture = new CultureInfo
                    (_cultureByCurreny[to]);

                var formatted = result.ToString("C", culture);

                ResultLabel.Text = $"{amount} {from} = {formatted}";
                // 100 BRL = XXXX USD
                InfoRateHint();
            }


            catch (Exception ex)
            {
                await DisplayAlert("Erro", "Fala ao converter", "OK");
            }


        }

        decimal Convert(string from, string to, decimal amount)
        {
            if (from == to) return amount;

            var brl = amount * _ToBRL[from];
            var result = brl / _ToBRL[to];
            return decimal.Round(result, 4);
        }

        decimal Rate(string from, string to)
        {
            if (from == to) return 1m;
            var brl = 1m * _ToBRL[from];
            var toValue = brl / _ToBRL[to];
            return decimal.Round(toValue, 6);
        }
        string? GetFrom() =>
            FromPicker.SelectedIndex >= 0 ?
                FromPicker.Items[FromPicker.SelectedIndex] : null;

        string? GetTo() => ToPicker.SelectedIndex >= 0 ? //o ? reforça a validaçao
                ToPicker.Items[ToPicker.SelectedIndex] : null;






























    }

}