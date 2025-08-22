using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etiquetas.Bibliotecas.LibArrayChar
{
    public static class ConverteCharOUEArrayCharEmArrayByte
    {

        public static byte[] Execute(params object[] objetos)
        {
            //return System.Text.Encoding.UTF8.GetBytes(chararray);

            List<char> charList = new List<char>();

            foreach (var arg in objetos)
            {
                if (arg is char c)
                {
                    charList.Add(c);
                }
                else if (arg is char[] ca)
                {
                    charList.AddRange(ca);
                }
                else if (arg is string str)
                {
                    charList.AddRange(str);
                }
                else
                {
                    var tipo = arg.GetType().Name;
                    throw new Exception($"Tipo {tipo} invalido em ConverteArrayCharEmArrayByte.");
                }
            }

            var array = charList.ToArray();
            return System.Text.Encoding.UTF8.GetBytes(array);
        }

        public static byte[] Execute(params char[][] objetos)
        {
            //return System.Text.Encoding.UTF8.GetBytes(chararray);

            List<char> charList = new List<char>();

            foreach (var arg in objetos)
            {
                foreach (var item in arg)
                {
                    charList.Add(item);
                }
            }
            var array = charList.ToArray();
            return System.Text.Encoding.UTF8.GetBytes(array);
        }

        public static byte[] Execute(params char[] objetos)
        {
            //return System.Text.Encoding.UTF8.GetBytes(chararray);.

            List<char> charList = new List<char>();

            foreach (var arg in objetos)
            {
                charList.Add(arg);
            }

            var array = charList.ToArray();
            return System.Text.Encoding.UTF8.GetBytes(array);
        }

        public static byte[] Execute(string objetos)
        {
            //return System.Text.Encoding.UTF8.GetBytes(chararray);

            List<char> charList = new List<char>();
            charList.AddRange(objetos.ToArray());

            var array = charList.ToArray();
            return System.Text.Encoding.UTF8.GetBytes(array);
        }
    }
}
