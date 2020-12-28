namespace ProSeeker.Common
{
    using System;
    using System.Text;

    public static class GlobalMethods
    {
        // starA zAgoRa -> Stara Zagora; george-johnson -> George-Johnson
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

        public static string GetContentForAcceptedOffer(
            string userName,
            string userEmail,
            string userPhone,
            string specialistPhone,
            string specialistName,
            string specialistEmail,
            string offerDescription,
            decimal price)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<h3>Здравейте,</h3>");
            sb.AppendLine("<h3>Офертата е приета и вече можете да започнете съвместна работа!</h3>");
            sb.AppendLine($"<h4>Контакти на потребител:</h4>");
            sb.AppendLine($"<p>Имена: {userName}</p>");
            sb.AppendLine($"<p>Телефон: {userPhone}</p>");
            sb.AppendLine($"<p>Email: {userEmail}</p>");

            sb.AppendLine($"<h4>Контакти на специалист:</h4>");
            sb.AppendLine($"<p>Имена: {specialistName}</p>");
            sb.AppendLine($"<p>Телефон: {specialistPhone}</p>");
            sb.AppendLine($"<p>Email: {specialistEmail}</p>");
            sb.AppendLine(string.Empty);
            sb.AppendLine($"<h4>Офертна цена: {price}</h4>");
            sb.AppendLine($"<p>Описание на офертата: {offerDescription}</p>");

            return sb.ToString();
        }

        public static string GetUpdatedContentFromContactsForm(string content, string userEmail, string userName)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<h3>Здравейте,</h3>");
            sb.AppendLine("<h3>Имате ново запитване от:</h3>");
            sb.AppendLine($"<h4>Имена: {userName}</h4>");
            sb.AppendLine($"<h4>Email: {userEmail}</h4>");
            sb.AppendLine();
            sb.AppendLine("<h4>Съобщение:</h4>");
            sb.AppendLine($"{content}");
            return sb.ToString();
        }
    }
}
