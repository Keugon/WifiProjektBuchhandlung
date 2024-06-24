using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace WIFI.Buchandlung.Client.Tools
{
    /// <summary>
    /// Stellt hilfs Klassen und Methoden wie 
    /// Converter für Visualisierung bereit
    /// </summary>
    public static class General : System.Object
    {
        /// <summary>
        /// Prüft alle angegebenen (Strings) auf 
        /// Null oder Empty und eine Eingabe prüfung 
        /// "ist eine eingabe vorhanden" zu simplifizieren!
        /// </summary>
        /// <param name="strings">zu prüfende Variablen müssen 
        /// String sein</param>
        /// <returns>True für alle variablen enthalten Etwas/False
        /// eine oder mehrere Variablen sind Null oder 
        /// enthalten nichts.</returns>
        public static bool AreStringsValid(params string[] strings)
        {
            foreach (var str in strings)
            {
                if (string.IsNullOrEmpty(str))
                {
                    return false;
                }
            }
            return true;
        }
    }
    /// <summary>
    /// Konverter der das AusleihDatum nimmt und den Wert 
    /// für maximale Ausleihzeit hier 14 Tage darauf rechnet 
    /// um ein Vorraussichtliches RückgabeDatum zu erhalten! Hat kein ConverBack
    /// </summary>
    public class AusleihZuRückgabeDatum : IValueConverter
    {
        /// <summary>
        /// Konvertiert das Ausleihdatum zum Vorraussichtlichen Rückgabe Datum
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                //Todo Rückgabedatum hier müssen die Hardcodierten
                //14tagen noch an die aktuellen Gebühren sätz gekoppelt werden
                DateTime postConvertDateTime = date.AddDays(14);
                return postConvertDateTime;

            }
            return value!;

        }
        /// <summary>
        /// Keine Rückwandlung!
        /// </summary>
        /// <param name="value">Datetime</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    /// <summary>
    /// Konverter der eine Decimal ausgangsvariable zum
    /// visualisieren für eine Textbox in einen String
    /// umwandelt und wieder retour wandelt
    /// </summary>
    [ValueConversion(typeof(decimal), typeof(String))]
    public class DecimalToString : IValueConverter
    {
        /// <summary>
        /// Konvertiert ein Decimal zu einem String
        /// </summary>
        /// <param name="value">Decimal</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value?.ToString() ?? string.Empty;
        }
        /// <summary>
        /// Konvetiert den String aus der Textbox wieder in einen Decimal zurück falls möglich sonst Rückgabe Decimal = 0
        /// </summary>
        /// <param name="value">String aus der Textbox</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            decimal retourn = 0;
            return decimal.TryParse((string)value!, out retourn) ? retourn : 0m;
        }
    }
    /// <summary>
    /// Konverter der eine Int ausgangsvariable zum
    /// visualisieren für eine Textbox in einen String
    /// umwandelt und wieder retour wandelt
    /// </summary>
    [ValueConversion(typeof(int), typeof(String))]
    public class IntToString : IValueConverter
    {
        /// <summary>
        /// Konvertiert einen Int zu String
        /// </summary>
        /// <param name="value">Int</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value?.ToString() ?? string.Empty;
        }
        /// <summary>
        /// Konvertiert einen String wieder in Int falls möglich sonst rückgabe Int = 0
        /// </summary>
        /// <param name="value">String</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            int retourn = 0;
            return int.TryParse((string)value!, out retourn) ? retourn : 0;
        }
    }
    /// <summary>
    /// Konverter der Speziel den SelectedIndex 
    /// einer Combobox wandelt um mit einer String variable zu funktionieren
    /// </summary>
    [ValueConversion(typeof(string), typeof(int))]
    public class StringToIntSelectedIndexComboBox : IValueConverter
    {
        /// <summary>
        /// Konvertiert den Stringwert einer Variable Zu Int zum 
        /// anzeigen in der Combobox fall der String NULL ist wird -1 
        /// zurückgegeben um die ComboBox keinen Eintrag zu selectieren
        /// </summary>
        /// <param name="value">String</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            int retourn;
            
            return int.TryParse((string)value!, out retourn) ? retourn : -1;//Combobox if nothing is choosn -1 eg null
        }
        /// <summary>
        /// Konvertiert den Int wer der SelectedIndex in String zurück
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value?.ToString() ?? string.Empty;
        }
    }
}

