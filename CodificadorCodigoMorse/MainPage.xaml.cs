using System.Globalization;
using System.Text;
using System.Text.Json;

namespace CodificadorCodigoMorse
{
    public partial class MainPage : ContentPage
    {
        private static readonly Dictionary<char, string> morseCodeMap = new() {
            // --- Letras (A-Z) ---
            { 'A', ".-" },
            { 'B', "-..." },
            { 'C', "-.-." },
            { 'D', "-.." },
            { 'E', "." },
            { 'F', "..-." },
            { 'G', "--." },
            { 'H', "...." },
            { 'I', ".." },
            { 'J', ".---" },
            { 'K', "-.-" },
            { 'L', ".-.." },
            { 'M', "--" },
            { 'N', "-." },
            { 'O', "---" },
            { 'P', ".--." },
            { 'Q', "--.-" },
            { 'R', ".-." },
            { 'S', "..." },
            { 'T', "-" },
            { 'U', "..-" },
            { 'V', "...-" },
            { 'W', ".--" },
            { 'X', "-..-" },
            { 'Y', "-.--" },
            { 'Z', "--.." },

            // --- Números (0-9) ---
            { '0', "-----" },
            { '1', ".----" },
            { '2', "..---" },
            { '3', "...--" },
            { '4', "....-" },
            { '5', "....." },
            { '6', "-...." },
            { '7', "--..." },
            { '8', "---.." },
            { '9', "----." },

            // --- Sinais de Pontuação Comuns ---
            { '.', ".-.-.-" },  // Ponto final
            { ',', "--..--" },  // Vírgula
            { '?', "..--.." },  // Ponto de interrogação
            { '\'', ".----." }, // Apóstrofo
            { '!', "-.-.--" },  // Ponto de exclamação
            { '/', "-..-." },   // Barra (sinal de divisão)
            { '(', "-.--." },   // Parênteses de abertura
            { ')', "-.--.-" },  // Parênteses de fechamento
            { '&', ".-..." },   // E comercial
            { ':', "---..." },  // Dois pontos
            { ';', "-.-.-." },  // Ponto e vírgula
            { '=', "-...-" },   // Sinal de igual
            { '+', ".-.-." },   // Sinal de adição
            { '-', "-....-" },  // Hífen / Travessão
            { '_', "..--.-" },  // Sublinhado
            { '"', ".-..-." },  // Aspas
            { '$', "...-..-" }, // Sinal de dólar
            { '@', ".--.-." },  // Arroba
        };
        private static Dictionary<string, char> _reverseMorseCodeMap;
        public MainPage()
        {
            InitializeComponent();
            _reverseMorseCodeMap = morseCodeMap.ToDictionary(k => k.Value, k => k.Key);
        }

        private async void OnSelectFileBtn(object sender, EventArgs e)
        {
            try
            {
                var fileResult = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Selecione um arquivo de texto (.txt)"
                });

                if (fileResult == null) return;

                using var stream = await fileResult.OpenReadAsync();
                using var reader = new StreamReader(stream);

                // Coloca o texto original no editor a esquerda
                InputOutputEditor.Text = await reader.ReadToEndAsync();
                MorseCodeEditor.Text = string.Empty;


                // Habilita o botão de codificar e desabilita o de decodificar
                EncodeBtn.IsEnabled = true;
                DecodeBtn.IsEnabled = false;

            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Ocorreu um erro: {ex.Message}", "OK");
            }
        }

        private void OnEncodeClicked(object sender, EventArgs e)
        {
            var textToEncode = InputOutputEditor.Text;
            if (string.IsNullOrEmpty(textToEncode))
            {
                DisplayAlert("Aviso", "Não há texto para codificar.", "OK");
                return;
            }

            // Codifica o texto e mostra no editor à direita
            MorseCodeEditor.Text = EncodeToMorse(textToEncode);

            // Habilita o botão de decodificar 
            DecodeBtn.IsEnabled = true;
        }

        private void OnDecodeClicked(object sender, EventArgs e)
        {
            var textToDecode = MorseCodeEditor.Text;
            if (string.IsNullOrEmpty(textToDecode))
            {
                DisplayAlert("Aviso", "Não há texto para decodificar.", "OK");
                return;
            }
            InputOutputEditor.Text = DecodeFromMorse(textToDecode);
        }

        private string DecodeFromMorse(string morseText)
        {
            var textBuilder = new StringBuilder();
            // Separa o texto por / para identificar as palavras
            string[] words = morseText.Trim().Split(new[] { " / " }, StringSplitOptions.None);

            for (int i = 0; i < words.Length; i++)
            {
                // Dentro de cada palavra, separa por espaço para obter as letras
                string[] letters = words[i].Split(' ');
                foreach (var letter in letters)
                {
                    if (_reverseMorseCodeMap.TryGetValue(letter, out var character))
                    {
                        textBuilder.Append(character);
                    }
                }

                // Adiciona um espaço entre as palavras (exceto a última)
                if (i < words.Length - 1)
                {
                    textBuilder.Append(' ');
                }
            }
            return textBuilder.ToString();
        }

        private string EncodeToMorse(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            var morseBuilder = new StringBuilder();

            // Converte o texto para maiúsculas para corresponder ao dicionário
            foreach (char character in text.ToUpper())
            {
                if (morseCodeMap.TryGetValue(character, out var morseChar))
                {
                    // Adiciona o código morse do caractere seguido de um espaço
                    morseBuilder.Append(morseChar).Append(' ');

                }
                else if (character == ' ')
                {
                    // Usa uma barra para representar o espaço entre palavras
                    morseBuilder.Append("/ ");
                }
            }
            return morseBuilder.ToString().Trim();
        }
    }
}
