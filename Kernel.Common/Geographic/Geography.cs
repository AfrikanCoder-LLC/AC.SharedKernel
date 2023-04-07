
using System.Globalization;

namespace Kernel.Common.Geographic
{
    public static class Geography
    {
        public static List<string> GetListOfCountries()
        {
            var list = new List<string>();
            CultureInfo[] cultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (var info in cultureInfo)
            {
                list.Add(info.EnglishName);
            }
            list.Sort();
            return list;
        }
    }
}
