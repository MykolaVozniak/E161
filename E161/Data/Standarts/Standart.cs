namespace E161.Data.Standarts
{
    public abstract class Standart
    {
        //Language-independent keys
        public string Key1 => ".,?!1@'\"-()/:_;+&%*=<>€£$¥¤[]{}\\~^¿¡§#ABCDEF";
        public string Key0 => " 0\n";

        public string KeyChars => "0123456789-()#*";
        public string KeyCharsWithoutDirectCase => "0123456789-#*";
        public string KeyRoleChars => "-()#*";

        public abstract string Key2 { get; }
        public abstract string Key3 { get; }
        public abstract string Key4 { get; }
        public abstract string Key5 { get; }
        public abstract string Key6 { get; }
        public abstract string Key7 { get; }
        public abstract string Key8 { get; }
        public abstract string Key9 { get; }

        public string[] Assignments => new[] { Key0, Key1, Key2, Key3, Key4, Key5, Key6, Key7, Key8, Key9 };

    }
}
