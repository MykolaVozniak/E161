using System.Text;

namespace E161.Data
{
    public class Algorythm
    {


        //switch (str[i], isUpperCaseOpen)
        //{
        //    case (char.IsUpper(str[i]), 0):
        //        Console.WriteLine("10, 0");
        //        break;
        //    default:
        //        break;
        //}


        //Encode
        public string Encode(string str) // ILoveSausages -> #444-555#-666-888-33-#7777#-2-88-2-4-33-7777
        {
            StringBuilder encodedBuilder = new StringBuilder();

            bool isUpperCaseOpen = false;
            int lastIndex = 0;

            for (int i = 0; i < str.Length; i++)
            {
               if 
                //UpperCase
                if (char.IsUpper(str[i]) && !isUpperCaseOpen)
                {
                    isUpperCaseOpen = true;
                    encodedBuilder.Append('#');
                    encodedBuilder.Append(char.ToLower(str[i]));
                }

                else if (char.IsUpper(str[i]) && isUpperCaseOpen)
                {
                    encodedBuilder.Append(char.ToLower(str[i]));
                }

                else if (isUpperCaseOpen)
                {
                    encodedBuilder.Remove(lastIndex-1, 1);
                    encodedBuilder.Append("#-");
                    encodedBuilder.Append(str[i]);
                    isUpperCaseOpen = false;
                }

                else
                {
                    encodedBuilder.Append(str[i]);
                }

                // Delimiter
                if (i > 0)
                {
                    encodedBuilder.Append('-');
                }
            }

            return encodedBuilder.ToString();
        }

        public string Decode(string str) // #444-555#-666-888-33-#7777#-2-88-2-4-33-7777 -> ILoveSausages
        {

            return str;
        }
    }
}
