# Codificador de Código Morse

> Este projeto consiste em um algoritmo simples que fará a codificação de arquivos **.txt** e terá também a funcionalidade de decodificar um arquivo já codificado.

### Tecnologias

- `.NET 9.0`
- `MAUI`
- `XAML`
---
### Principais Funções 
#### EncodeToMorse
- A função `EncodeToMorse` é responsável por fazer a codificação do texto (O conteúdo é lido na função `OnSelectFileBtn`). 
- Dentro desta função, converteremos todo o texto em maiúsculo (para corresponder a o dicionário que foi criado) - `foreach (char character in text.ToUpper())`
- Para cada código morse adicionado, o mesmo é seguido por um espaço: 
```cs
if(morseCodeMap.TryGetValue(character, out var morseChar))
{morseBuilder.Append(morseChar).Append(' ');}
```
---
#### DecodeToMorse
- Esta função será responsável por decodificar as palavras que antes estavam codificados.
- Separamos o texto '/' para identificar as palavras:
```cs
string[] words = morseText.Trim().Split(new[] {" / "}, StringSplitOptions.None);
```
- E Dentro de cada palvara, separa por espaço para obter as letras: 
```cs
string[] letters = words[i].Split(' ');
foreach(var letter in letters)
{
    if (_reverseMorseCodeMap.TryGetValue(letter, out var character))
    {
        textBuilder.Append(character);
    }
}
```
- Por fim, adicionaremos um espaço entre as palavras (exceto a última)
```cs
if (i < words.Length - 1) {
    textBuilder.Append(' ');
}
```
---
### Melhorias Futuras
1. Permitir o download de um novo arquivo, com o seu conteúdo codificado.
2. Melhorar interface UI; Está crua.
