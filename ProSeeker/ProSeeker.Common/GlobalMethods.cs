namespace ProSeeker.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
                    stringBuilder.Append($"{part[0].ToString().ToUpper()}{part.Substring(1).ToLower()} ");
                }
            }
            else if (input.Contains('-'))
            {
                var inputParts = input.Split('-', StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < inputParts.Length; i++)
                {
                    inputParts[i] = inputParts[i][0].ToString().ToUpper() + inputParts[i].Substring(1).ToLower();
                }

                stringBuilder.Append(string.Join('-', inputParts));
            }
            else
            {
                stringBuilder.Append(input[0].ToString().ToUpper() + input.Substring(1).ToLower());
            }

            return stringBuilder.ToString().Trim();
        }

        public static string TranslateVoteType(string input)
        {
            return input switch
            {
                GlobalConstants.ByDateDescending => "дата",
                GlobalConstants.ByOpinionsDescending => "брой коментари",
                GlobalConstants.ByUpVotesDescending => "брой харесвания",
                GlobalConstants.ByDownVotesDescending => "брой нехаресвания",
                GlobalConstants.ByRatingDesc => "най-висок рейтинг",
                _ => string.Empty,
            };
        }

        public static string CalculateElapsedTime(DateTime initialTime, bool isItFutureTime)
        {
            DateTime currentTime = DateTime.UtcNow;
            TimeSpan timeSpan;

            // If we want to calculate the elapsed time (isItFutureTime = false)
            // In case of calculating a period until something happens -> isitFutureTime = true
            if (!isItFutureTime)
            {
                timeSpan = currentTime - initialTime;
            }
            else
            {
                timeSpan = initialTime - currentTime;
            }

            if (timeSpan.TotalMinutes < 1)
            {
                return "по-малко от минута";
            }

            if (timeSpan.TotalHours < 1)
            {
                return (int)timeSpan.TotalMinutes == 1 ? "1 минута" : $"{(int)timeSpan.TotalMinutes} минути";
            }

            if (timeSpan.TotalDays < 1)
            {
                return (int)timeSpan.TotalHours == 1 ? "1 час" : $"{(int)timeSpan.TotalHours} часа";
            }

            if (timeSpan.TotalDays < 7)
            {
                return (int)timeSpan.TotalDays == 1 ? "1 ден" : $"{(int)timeSpan.TotalDays} дни";
            }

            if (timeSpan.TotalDays < 30.4368)
            {
                return (int)(timeSpan.TotalDays / 7) == 1 ? "1 седмица" : $"{(int)(timeSpan.TotalDays / 7)} седмици";
            }

            if (timeSpan.TotalDays < 365.242)
            {
                return (int)(timeSpan.TotalDays / 30.4368) == 1 ? "1 месец" : $"{(int)(timeSpan.TotalDays / 30.4368)} месеца";
            }

            return (int)(timeSpan.TotalDays / 365.242) == 1 ? "1 година" : $"{(int)(timeSpan.TotalDays / 365.242)} години";
        }
    }
}
