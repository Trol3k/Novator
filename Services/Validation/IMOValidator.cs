namespace Novator.Services.Validation
{
    public class IMOValidator
    {
        public static bool IsIMOValid(string IMO)
        {
            if (string.IsNullOrWhiteSpace(IMO))
                return false;

            if (IMO.Length != 7 || !IMO.All(char.IsDigit))
                return false;

            int sum = 0;
            for (int i = 0; i < 6; i++)
            {
                int digit = int.Parse(IMO[i].ToString());
                sum += digit * (7 - i);
            }
            int checksum = int.Parse(IMO[6].ToString());

            return sum % 10 == checksum;
        }
    }
}
