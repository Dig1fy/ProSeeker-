namespace ProSeeker.Common
{
    using System;
    using System.Text;

    public static class GlobalMethods
    {
        // starA zAgora -> Stara Zagora; george-johnson -> George-Johnson
        public static string UpperFirstLetterOfEachWord(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();


            if (input.Contains(' '))
            {
                var inputParts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (var part in inputParts)
                {
                    stringBuilder.Append($"{part[0].ToString().ToUpper()}{part.Substring(1)} ");
                }
            }
            else if (input.Contains('-'))
            {
                var inputParts = input.Split('-', StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < inputParts.Length; i++)
                {
                    inputParts[i] = inputParts[i][0].ToString().ToUpper() + inputParts[i].Substring(1);
                }

                stringBuilder.Append(string.Join('-', inputParts));
            }
            else
            {
                stringBuilder.Append(input[0].ToString().ToUpper() + input.Substring(1));

            }

            return stringBuilder.ToString().Trim();
        }
    }
}
