namespace E161.Data.Standarts
{
    public abstract class Standart
    {
        public abstract string Name { get; }

        public abstract string Details { get; }
        //Language-independent signals
        public string Signal1 => ".,?!1@'\"-()/:_;+&%*=<>€£$¥¤[]{}\\~^¿¡§#"; //ABCDEF
        public string Signal0 => " 0\n";

        public string SignalChars => "0123456789-()#*";
        public string SignalCharsWithoutDirectCase => "0123456789-#*";
        public string RoleSignalChars => "-()#*";

        public abstract string Signal2 { get; }
        public abstract string Signal3 { get; }
        public abstract string Signal4 { get; }
        public abstract string Signal5 { get; }
        public abstract string Signal6 { get; }
        public abstract string Signal7 { get; }
        public abstract string Signal8 { get; }
        public abstract string Signal9 { get; }

        public string[] Assignments => new[] { Signal0, Signal1, Signal2, Signal3, Signal4, Signal5, Signal6, Signal7, Signal8, Signal9 };

    }
}
