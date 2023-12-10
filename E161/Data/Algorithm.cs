using E161.Data.Standarts;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace E161.Data
{
    public class Algorithm
    {
        public string Encode(string str)
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

                if (standart.KeyCharsWithoutDirectCase.Contains(str[i]) && !isDirectCaseOpen)
                {
                    isDirectCaseOpen = true;

                    if (encodedBuilder.Length > 0)
                    {
                        if (encodedBuilder[encodedBuilder.Length - 1] == '-')
                        {
                            encodedBuilder.Append('(');
                        }
                        else
                        {
                            encodedBuilder.Append("-(");
                        }
                    }
                    else
                    {
                        encodedBuilder.Append('(');
                    }
                }
                else if (!standart.KeyCharsWithoutDirectCase.Contains(str[i]) && isDirectCaseOpen)
                {
                    isDirectCaseOpen = false;
                    encodedBuilder.Append(")-");
                }

                if (char.IsUpper(str[i]) && !isUpperCaseOpen)
                {
                    isUpperCaseOpen = true;
                    if (encodedBuilder.Length > 0)
                    {
                        if (encodedBuilder[encodedBuilder.Length - 1] == '-')
                        {
                            encodedBuilder.Append('#');
                        }
                        else
                        {
                            encodedBuilder.Append("-#");
                        }
                    }
                    else
                    {
                        encodedBuilder.Append('#');
                    }
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
                    if (str[i] == '(')
                    {
                        encodedBuilder.Append("1111111111-");
                    }
                    else if (str[i] == ')')
                    {
                        encodedBuilder.Append("11111111111-");
                    }
                    else
                    {
                        encodedBuilder.Append(str[i]);
                    }  
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
            int uniCount = 0;

            for (int i = 0; i < str.Length; i++)
            {
                int history = 0;

                for (int j = 0; j < standart.Assignments.Length; j++)
                {
                    if (standart.Assignments[j].Contains(str[i]) && !standart.KeyChars.Contains(str[i]))
                    {
                        if (isUnicodeOpen)
                        {
                            //encodedBuilder.Remove(encodedBuilder.Length - 1, 1);
                            encodedBuilder.Append("*");
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
                            encodedBuilder.Append("*");
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
                            uniCount++;
                        }
                        else if (history == 9 && isUnicodeOpen)
                        {
                            encodedBuilder.Remove(encodedBuilder.Length - history, history);

                            string uniString = EncodingPhase1(Convert.ToUInt16(str[i]).ToString("X4"), standart);
                            uniString = EncodingPhase2(uniString, standart);
                            encodedBuilder.Append(uniString);
                            isUnicodeOpen = false;
                        }
                        else
                        {
                            encodedBuilder.Append(str[i]);
                        }
                    }
                    history++;
                }
                if (isUnicodeOpen)
                {
                    encodedBuilder.Append('*');
                    isUnicodeOpen = false;
                }
            }

            string str2 = encodedBuilder.ToString();
            string resultString = str2.Replace("*-*", "-");
            resultString = resultString.Replace("-(#-", "-#(");
            return resultString;
        }

        public string Decode(string str)
        {
            var standart = new E161VLEnglish();
            string decodedStr = DecodingPhase1(str, standart);

            return decodedStr;
        }

        private string DecodingPhase1(string str, Standart standart)
        {
            string cleanedString = Regex.Replace(str, @"-{2,}", "-");
            cleanedString = Regex.Replace(cleanedString, @"\({2,}", "(");
            cleanedString = Regex.Replace(cleanedString, @"\){2,}", ")");

            StringBuilder decodedBuilder = new StringBuilder();
            char[] chars = cleanedString.ToCharArray();

            bool isDirectCaseOpen = false;

            for (int i = 0; i < chars.Length; i++)
            {
                decodedBuilder.Append(chars[i]);

                if (chars[i] == '(')
                {
                    isDirectCaseOpen = true;
                }
                else if (chars[i] == ')')
                {
                    isDirectCaseOpen= false;
                }

                if (i + 1 < chars.Length)
                {
                    if (char.IsDigit(chars[i]) && char.IsDigit(chars[i + 1]) && (chars[i] != chars[i + 1]) && !isDirectCaseOpen)
                    {
                        decodedBuilder.Append("-");
                    }
                }
            }

            string str2 = decodedBuilder.ToString();
            chars = str2.ToCharArray();

            isDirectCaseOpen = false;
            bool isRegisterOpen = false;
            bool isUpperCaseOpen = false;
           
            int starN = 0;
            int upperN = 0;
            int n = 0;
            int arrayChar = 0;

            bool isUnicodeOpen = false;
            int uniN = 0;

            StringBuilder decodedBuilder2 = new StringBuilder();
            StringBuilder unicodeBuilder = new StringBuilder();

            for (int i = 0; i < chars.Length; i++)
            {
                if (i > 0)
                {
                    if (chars[i - 1] == '(')
                    {
                        isDirectCaseOpen = true;
                    }
                    if (chars[i - 1] == '#')
                    {
                        upperN++;
                        if (!isUpperCaseOpen && upperN == 1)
                        {
                            isUpperCaseOpen = true; 
                        }
                        else if (isUpperCaseOpen && upperN == 2)
                        {
                            isUpperCaseOpen = false;
                            upperN = 0;
                        }
                    }
                }

                if (chars[i] == '*' && !isDirectCaseOpen)
                {

                    uniN++;
                    if (!isUnicodeOpen && uniN == 1)
                    {
                        isUnicodeOpen = true;
                    }
                    else if (isUnicodeOpen && uniN == 2)
                    {
                        isUnicodeOpen = false;
                        uniN = 0;
                        string str3 = unicodeBuilder.ToString();
                        unicodeBuilder.Clear();
                        if (isUpperCaseOpen)
                        {
                            decodedBuilder2.Append(DecodingPhase2(str3, standart).ToUpper());
                        }
                        else
                        {
                            decodedBuilder2.Append(DecodingPhase2(str3, standart));
                        }
                        
                    }
                }

                if (chars[i] == ')')
                {
                    isDirectCaseOpen = false;
                }

                if (!isDirectCaseOpen)
                {
                    if (!isRegisterOpen && char.IsDigit(chars[i]))
                    {
                        n = 0;
                        isRegisterOpen = true;
                        arrayChar = (int)Char.GetNumericValue(chars[i]);
                        n++;

                        if (i < chars.Length - 1) //i,i,charLen
                        {
                            if (!char.IsDigit(chars[i + 1]) || chars[i] != chars[i + 1] || i + 1 == chars.Length)
                            {
                                char charValue = isUpperCaseOpen ? char.ToUpper(standart.Assignments[arrayChar][n - 1]) : standart.Assignments[arrayChar][n - 1];
                                if (isUnicodeOpen)
                                {
                                    unicodeBuilder.Append(charValue);
                                }
                                else
                                {
                                    decodedBuilder2.Append(charValue);
                                }                                
                                isRegisterOpen = false;
                            }
                        }
                    }
                    else if (isRegisterOpen)
                    {
                        if (char.IsDigit(chars[i]))
                        {
                            n++;

                            if (i < chars.Length - 1) //i,i,charLen
                            {
                                if (!char.IsDigit(chars[i + 1]) || chars[i] != chars[i + 1] || i + 1 == chars.Length)
                                {
                                    char charValue = isUpperCaseOpen ? char.ToUpper(standart.Assignments[arrayChar][n - 1]) : standart.Assignments[arrayChar][n - 1];
                                    if (isUnicodeOpen)
                                    {
                                        unicodeBuilder.Append(charValue);
                                    }
                                    else
                                    {
                                        decodedBuilder2.Append(charValue);
                                    }
                                    isRegisterOpen = false;
                                }
                            }
                        }
                    }
                }
                else if (isDirectCaseOpen)
                {
                    if (isUnicodeOpen)
                    {
                        unicodeBuilder.Append(chars[i]);
                    }
                    else
                    {
                        decodedBuilder2.Append(chars[i]);
                    }
                }
            }

            return decodedBuilder2.ToString();
        }

        private string DecodingPhase2(string str, Standart standart)
        {
            StringBuilder decodedBuilder = new StringBuilder();
            int uniNumber = 0;
            bool isUnicodeSuccessReal = false;

            if(str.Length % 4 == 0)
            {
                for (int j = 0; j < str.Length; j += 4)
                {
                    string hexCode = str.Substring(j, 4);

                    if (int.TryParse(hexCode, System.Globalization.NumberStyles.HexNumber, null, out int unicodeValue))
                    {
                        // Отримати верхню частину сурогатної пари (D83D)
                        if (char.IsHighSurrogate((char)unicodeValue))
                        {
                            isUnicodeSuccessReal = true;
                            uniNumber = unicodeValue;
                        }
                        // Отримати нижню частину сурогатної пари (DE01)
                        else if (char.IsLowSurrogate((char)unicodeValue) && isUnicodeSuccessReal)
                        {
                            unicodeValue = char.ConvertToUtf32((char)uniNumber, (char)unicodeValue);
                            decodedBuilder.Append(char.ConvertFromUtf32(unicodeValue));
                            isUnicodeSuccessReal = false;
                        }
                        // Одиничний код Unicode без сурогатних пар
                        else
                        {
                            decodedBuilder.Append(char.ConvertFromUtf32(unicodeValue));
                        }
                    }
                    else
                    {
                        // Обробка помилки або інші дії, які вам потрібно виконати
                    }
                }
            }
            return decodedBuilder.ToString();
        }
    }
}
