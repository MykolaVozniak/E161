using E161.Data.Standarts;
using System.Linq;
using System.Text;

namespace E161.Data
{
    public class Algorithm
    {
        public string Encode(string str) // ILoveSausages -> #444-555#-666-888-33-#7777#-2-88-2-4-33-7777
        {
            var standart = new E161VLEnglish();

            string encodedStr = EncodingPhase1(str, standart);
            encodedStr = EncodingPhase2(encodedStr, standart);

            return encodedStr;
        }

        private string EncodingPhase1(string str, Standart standart)
        {
            StringBuilder encodedBuilder = new StringBuilder();

            bool isUpperCaseOpen = false;
            bool isDirectCaseOpen = false;

            for (int i = 0; i < str.Length; i++)
            {
                if (standart.KeyChars.Contains(str[i]) && !isDirectCaseOpen)
                {
                    isDirectCaseOpen = true;
                    encodedBuilder.Append("(");
                }
                else if (!standart.KeyChars.Contains(str[i]) && isDirectCaseOpen)
                {
                    isDirectCaseOpen = false;
                    encodedBuilder.Append(")-");
                }

                if (char.IsUpper(str[i]) && !isUpperCaseOpen)
                {
                    isUpperCaseOpen = true;
                    encodedBuilder.Append("#");
                }
                else if ((char.IsLower(str[i]) || !char.IsLetterOrDigit(str[i])) && isUpperCaseOpen)
                {
                    encodedBuilder.Append("#-");
                    isUpperCaseOpen = false;
                }

                if (isUpperCaseOpen)
                {
                    encodedBuilder.Append(char.ToLower(str[i]));
                }
                else
                {
                    encodedBuilder.Append(str[i]);
                }

                if (encodedBuilder.Length > 0 && i < str.Length-1)
                {
                    if (!standart.KeyChars.Contains(encodedBuilder[encodedBuilder.Length - 1]))
                    {
                        if (standart.KeyChars.Contains(str[i + 1]))
                        {
                            encodedBuilder.Append('-');
                        }
                        else if(isUpperCaseOpen && !char.IsUpper(str[i + 1]))
                        {

                        }
                        else
                        {
                            encodedBuilder.Append('-');
                        }
                    }
                }

                if ((i == str.Length - 1) && isDirectCaseOpen)
                {
                    isDirectCaseOpen = false;
                    encodedBuilder.Append(")");
                }
                if ((i == str.Length - 1) && isUpperCaseOpen)
                {
                    encodedBuilder.Append("#");
                    isUpperCaseOpen = false;
                }
            }

            return encodedBuilder.ToString();
        }

        private string EncodingPhase2(string str, Standart standart)
        {
            StringBuilder encodedBuilder = new StringBuilder();
            bool isUnicodeOpen = false;

            for (int i = 0; i < str.Length; i++)
            {
                int history = 0;
                for (int j = 0; j < standart.Assignments.Length; j++)
                {
                    if (standart.Assignments[j].Contains(str[i]) && !standart.KeyChars.Contains(str[i]))
                    {
                        if (isUnicodeOpen)
                        {
                            encodedBuilder.Remove(encodedBuilder.Length - 1, 1);
                            encodedBuilder.Append("*-");
                            isUnicodeOpen = false;
                        }
                        if (history != 0)
                        {
                            encodedBuilder.Remove(encodedBuilder.Length - history, history);
                        }
                        for (int k = 0; k < (standart.Assignments[j].IndexOf(str[i]) + 1); k++)
                        {
                            encodedBuilder.Append(j);
                        }
                        j = standart.Assignments.Length;
                    }
                    else if (standart.KeyChars.Contains(str[i]))
                    {
                        if (isUnicodeOpen && str[i] != '-')
                        {
                            encodedBuilder.Remove(encodedBuilder.Length - 1, 1);
                            encodedBuilder.Append("*-");
                            isUnicodeOpen = false;
                        }
                        if (history != 0)
                        {
                            encodedBuilder.Remove(encodedBuilder.Length - history, history);
                        }
                        encodedBuilder.Append(str[i]);
                        j = standart.Assignments.Length;
                    }
                    else if (!standart.Assignments[j].Contains(str[i]) && !standart.KeyChars.Contains(str[i]))
                    {
                        if (history == 9 && !isUnicodeOpen)
                        {
                            isUnicodeOpen = true;
                            encodedBuilder.Remove(encodedBuilder.Length - history, history);
                            encodedBuilder.Append('*');

                            string uniString = EncodingPhase1(Convert.ToUInt16(str[i]).ToString("X4"), standart);
                            uniString = EncodingPhase2(uniString, standart);
                            encodedBuilder.Append(uniString);
                        }
                        else if (history == 9 && isUnicodeOpen)
                        {
                            encodedBuilder.Remove(encodedBuilder.Length - history, history);

                            string uniString = EncodingPhase1(Convert.ToUInt16(str[i]).ToString("X4"), standart);
                            uniString = EncodingPhase2(uniString, standart);
                            encodedBuilder.Append(uniString);
                        }
                        else
                        {
                            encodedBuilder.Append(str[i]);
                        }
                    }
                    history++;
                }
            }

            if(isUnicodeOpen)
            {
                encodedBuilder.Append('*');
                isUnicodeOpen = false;
            }

            string str2 = encodedBuilder.ToString();
            return str2;
        }

        public string Decode(string str) // #444-555#-666-888-33-#7777#-2-88-2-4-33-7777 -> ILoveSausages
        {

            return str;
        }
    }
}
