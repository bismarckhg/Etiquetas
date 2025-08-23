using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.Comum.Datas
{
    public class ConverteDateTimeEmArrayString
    {
        public static string[] ObtemAnoMesDiaArrayString(DateTime datetime)
        {
            return new string[]
            {
                ConverteDateTimeEmString.ObtemAnoEmString(datetime),
                ConverteDateTimeEmString.ObtemMesEmString(datetime),
                ConverteDateTimeEmString.ObtemDiaEmString(datetime)
            };
        }

        public static string[] ObtemAnoMesDiaHoraMinutoArrayString(DateTime datetime)
        {
            return new string[]
            {
                ConverteDateTimeEmString.ObtemAnoEmString(datetime),
                ConverteDateTimeEmString.ObtemMesEmString(datetime),
                ConverteDateTimeEmString.ObtemDiaEmString(datetime),
                ConverteDateTimeEmString.ObtemHoraString(datetime),
                ConverteDateTimeEmString.ObtemMinutoString(datetime)
            };
        }

        public static string[] ObtemAnoMesDiaHoraMinutoSegundoArrayString(DateTime datetime)
        {
            return new string[]
            {
                ConverteDateTimeEmString.ObtemAnoEmString(datetime),
                ConverteDateTimeEmString.ObtemMesEmString(datetime),
                ConverteDateTimeEmString.ObtemDiaEmString(datetime),
                ConverteDateTimeEmString.ObtemHoraString(datetime),
                ConverteDateTimeEmString.ObtemMinutoString(datetime),
                ConverteDateTimeEmString.ObtemSegundoString(datetime)
            };
        }

        public static string[] ObtemAnoMesDiaHoraMinutoSegundoMilesimoArrayString(DateTime datetime)
        {
            return new string[]
            {
                ConverteDateTimeEmString.ObtemAnoEmString(datetime),
                ConverteDateTimeEmString.ObtemMesEmString(datetime),
                ConverteDateTimeEmString.ObtemDiaEmString(datetime),
                ConverteDateTimeEmString.ObtemHoraString(datetime),
                ConverteDateTimeEmString.ObtemMinutoString(datetime),
                ConverteDateTimeEmString.ObtemSegundoString(datetime),
                ConverteDateTimeEmString.ObtemMilesimoString(datetime)
            };
        }
    }
}
