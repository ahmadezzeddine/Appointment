using System;

namespace App.Schedule.Domains.Helpers
{
    public class Unique
    {
        public static string GetValue()
        {
            try
            {
                Guid g = Guid.NewGuid();
                var uniqueText = Convert.ToBase64String(g.ToByteArray());
                uniqueText = uniqueText.Replace("=", "").Replace("+", "");
                return uniqueText;
            }
            catch
            {
                return "";
            }
        }
    }
}
