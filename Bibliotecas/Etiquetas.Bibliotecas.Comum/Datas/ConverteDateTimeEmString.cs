using System;
using System.Globalization;
using Etiquetas.Bibliotecas.Comum.Caracteres;
using Etiquetas.Bibliotecas.Comum.Geral;

namespace Etiquetas.Bibliotecas.Comum.Datas
{
    public static class ConverteDateTimeEmString
    {
        public static string DiaMesAnoFormatoCurtoLocal(DateTime datetime, CultureInfo cultura = null)
        {
            var culturaPadrao = cultura ?? CulturaPadrao();
            return datetime.ToString(culturaPadrao.DateTimeFormat.ShortDatePattern, culturaPadrao);
        }

        public static string HoraMinutoSegundoLocal(DateTime datetime, CultureInfo cultura = null)
        {
            var culturaPadrao = cultura ?? CulturaPadrao();
            string formatoDataHora = $"{culturaPadrao.DateTimeFormat.LongTimePattern}";
            return datetime.ToString(formatoDataHora, culturaPadrao);
        }

        public static string DataHoraFormatoLocal(DateTime datetime, CultureInfo cultura = null)
        {
            var culturaPadrao = cultura ?? CulturaPadrao();
            return datetime.ToString(culturaPadrao);
        }

        public static string DataHoraMinutoSegundosMilesimosFormatoLocal(DateTime datetime, CultureInfo cultura = null)
        {
            var culturaPadrao = cultura ?? CulturaPadrao();
            string formatoDataHora = $"{culturaPadrao.DateTimeFormat.ShortDatePattern} {culturaPadrao.DateTimeFormat.LongTimePattern}.fff";
            return datetime.ToString(formatoDataHora, culturaPadrao);
        }

        public static string ObtemAnoEmString(DateTime datetime)
        {
            return datetime.ToString("yyyy");
        }

        public static string ObtemMesEmString(DateTime datetime)
        {
            return datetime.ToString("MM");
        }

        public static string ObtemDiaEmString(DateTime datetime)
        {
            return datetime.ToString("dd");
        }

        public static string ObtemAnoMesDiaString(DateTime datetime)
        {
            return ConcatenarTexto.Execute(ObtemAnoEmString(datetime), ObtemMesEmString(datetime), ObtemDiaEmString(datetime));
        }

        public static string ObtemHoraString(DateTime datetime)
        {
            return datetime.ToString("HH");
        }

        public static string ObtemMinutoString(DateTime datetime)
        {
            return datetime.ToString("mm");
        }

        public static string ObtemSegundoString(DateTime datetime)
        {
            return datetime.ToString("ss");
        }

        public static string ObtemMilesimoString(DateTime datetime)
        {
            return datetime.ToString("fff");
        }

        public static string ObtemHoraMinutoString(DateTime datetime)
        {
            return ConcatenarTexto.Execute
                    (
                        ObtemHoraString(datetime),
                        ObtemMinutoString(datetime)
                    );
        }

        public static string ObtemHoraMinutoSegundoString(DateTime datetime)
        {
            return  ConcatenarTexto.Execute
                    (
                        ObtemHoraMinutoString(datetime),
                        ObtemSegundoString(datetime)
                    );
        }

        public static string ObtemHoraMinutoSegundoMilesimoString(DateTime datetime)
        {
            return ConcatenarTexto.Execute
                    (
                        ObtemHoraMinutoSegundoString(datetime),
                        ObtemMilesimoString(datetime)
                    );
        }

        public static string ObtemAnoMesDiaHoraMinutoString(DateTime datetime)
        {
            return ConcatenarTexto.Execute
                    (
                        ObtemAnoMesDiaString(datetime),
                        ObtemHoraMinutoString(datetime)
                    );
        }

        public static string ObtemAnoMesDiaHoraMinutoSegundoString(DateTime datetime)
        {
            return ConcatenarTexto.Execute
                (
                    ObtemAnoMesDiaString(datetime),
                    ObtemHoraMinutoSegundoString(datetime)
                );
        }

        public static string ObtemAnoMesDiaHoraMinutoSegundoMilesimoString(DateTime datetime)
        {
            return ConcatenarTexto.Execute
                (
                    ObtemAnoMesDiaString(datetime),
                    ObtemHoraMinutoSegundoMilesimoString(datetime)
                );
        }

        private static CultureInfo CulturaPadrao()
        {
            // Substitua esta implementação com a lógica correta para obter a cultura padrão.
            // Por exemplo, você pode retornar CultureInfo.CurrentCulture ou outra cultura específica.
            return Cultura.Padrao; // Ajuste conforme necessário
        }
    }
}
