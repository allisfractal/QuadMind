using System.Text;

const int MaxDigitValue = 6;
const int NumberOfDigits = 4;
const int NumberOfGuesses = 10;

Random rand = new();
bool IsMatch = false;
int guessesLeft = NumberOfGuesses;
PrintHeader();
Dictionary<int, List<int>> target = GenerateRandomTarget(out string targetString);

while(guessesLeft > 0 && !IsMatch)
{
    List<char> hint = new();
    Console.Write($"Enter your guess. {guessesLeft} guesses left: ");
    string? guess = Console.ReadLine();
    if(IsInputValid(guess))
    {
        hint = GenerateHint(guess!, out IsMatch);
        if (!IsMatch) PrintHint(hint);
        guessesLeft--;
    }
}
if (IsMatch)
    Console.WriteLine("Congratulations!");
else
    Console.WriteLine($"You have failed. The secret target was {targetString}.");
    

List<char> GenerateHint(string guess, out bool IsMatch)
{
    IsMatch = true;
    List<char> hint = [];
    for (int k = 0; k < NumberOfDigits; k++)
    {
        int digit = (int)char.GetNumericValue(guess[k]);
        if (target.TryGetValue(digit, out List<int>? posList))
        {
            if (posList.Contains(k))
            {
                hint.Add('+');
            }
            else
            {
                hint.Add('-');
                IsMatch = false;
            }
        }
        else
            IsMatch = false;
    }
    return hint;
}
Dictionary<int, List<int>> GenerateRandomTarget(out string secretTarget)
{
    StringBuilder sb = new();
    Dictionary<int, List<int>> result = new();
    for (int k = 0; k < NumberOfDigits; k++)
    {
        int digit = rand.Next(MaxDigitValue - 1) + 1;
        if (result.ContainsKey(digit))
            result[digit].Add(k);
        else
            result.Add(digit, [k]);
        sb.Append(digit);
    }
    secretTarget = sb.ToString();
    return result;
}
bool IsInputValid(string? guess)
{
    if (string.IsNullOrEmpty(guess))
    {
        Console.WriteLine($"Must enter a valid guess: {NumberOfDigits} numeric digits only.");
        return false;
    }
    else if (guess.Length != NumberOfDigits)
    {
        Console.WriteLine($"Guess must be N digits long.");
        return false;
    }
    return true;
}
void PrintHint(List<char> unsortedHint)
{
    Console.Write("Your hint is: ");
    foreach (char c in unsortedHint.OrderBy(k => k))
    {
        Console.Write(c);
    }
    Console.WriteLine();
}
void PrintHeader()
{
    Console.WriteLine($"Welcome. You will have {NumberOfGuesses} guesses to guess a {NumberOfDigits} digit number with each digit between 1 and {MaxDigitValue}.");

}

