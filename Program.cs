using System.Text.Json;
using System.Text.Json.Nodes;

namespace ChemSolver
{
    internal class Program
    {   
        static void Main(string[] args)
        {
            Console.SetWindowSize(200, 50);

            int option = UI.Menu(new string[] {"Periodic Table", "Redox reaktion", "Oxidationstal"});
            switch (option) {
                case 0:
                    Chemistry.PeriodicTable();
                    break;
                case 1:
                    Chemistry.Redox();
                    break;
                case 2:
                    Chemistry.OxidationValue(showValue: true, molecule: "");
                    break;
            }
        }
    }

    internal class Chemistry
    {
        private static List<JsonElement> PeriodicTableElements = new List<JsonElement>();
        private static int InteractivePeriodicTable(string[] options, int length)
        {
            int cur = 1;
            bool loop = true;
            while (loop)
            {
                Console.Clear();
                Console.WriteLine(@"   _ (`-.    ('-.  _  .-')                       _ .-') _                            .-') _      ('-.    .-. .-')               ('-.        
  ( (OO  ) _(  OO)( \( -O )                     ( (  OO) )                          (  OO) )    ( OO ).-.\  ( OO )            _(  OO)       
 _.`     \(,------.,------.  ,-.-')  .-'),-----. \     .'_   ,-.-')   .-----.       /     '._   / . --. / ;-----.\  ,--.     (,------.      
(__...--'' |  .---'|   /`. ' |  |OO)( OO'  .-.  ',`'--..._)  |  |OO) '  .--./       |'--...__)  | \-.  \  | .-.  |  |  |.-')  |  .---'      
 |  /  | | |  |    |  /  | | |  |  \/   |  | |  ||  |  \  '  |  |  \ |  |('-.       '--.  .--'.-'-'  |  | | '-' /_) |  | OO ) |  |          
 |  |_.' |(|  '--. |  |_.' | |  |(_/\_) |  |\|  ||  |   ' |  |  |(_//_) |OO  )         |  |    \| |_.'  | | .-. `.  |  |`-' |(|  '--.       
 |  .___.' |  .--' |  .  '.',|  |_.'  \ |  | |  ||  |   / : ,|  |_.'||  |`-'|          |  |     |  .-.  | | |  \  |(|  '---.' |  .--'       
 |  |      |  `---.|  |\  \(_|  |      `'  '-'  '|  '--'  /(_|  |  (_'  '--'\          |  |     |  | |  | | '--'  / |      |  |  `---.      
 `--'      `------'`--' '--' `--'        `-----' `-------'   `--'     `-----'          `--'     `--' `--' `------'  `------'  `------'      
 ");
                for (int i = 0; i < options.Length; i++)
                {
                    Console.BackgroundColor = cur == i ? ConsoleColor.White : ConsoleColor.Black;
                    Console.ForegroundColor = cur == i ? ConsoleColor.Black : ConsoleColor.White;
                    Console.Write(i != 0 ? options[i] + " " : options[i]);
                    Console.Write(i % length == 0 && i != 0 ? "\n" : "");
                    Console.ResetColor();
                }

                ConsoleKey choice = Console.ReadKey(true).Key;
                switch (choice.ToString())
                {
                    case "Enter":
                        loop = false;
                        break;
                    case "DownArrow":
                        if (
                            cur + length > options.Length ||
                            options[cur + length] == "> " ||
                            options[cur + length] == "| " ||
                            options[cur + length] == " " ||
                            options[cur + length] == "  " ||
                            options[cur + length] == ""
                        )
                        {
                            continue;
                        }
                        cur += length;
                        break;
                    case "UpArrow":
                        if (
                            cur - length < 0 ||
                            options[cur - length] == " " ||
                            options[cur - length] == "  " ||
                            options[cur - length] == ""
                        )
                        {
                            continue; 
                        }
                        cur -= length;
                        break;
                    case "RightArrow":
                        if (
                            cur + 1 > options.Length ||
                            options[cur + 1] == " " ||
                            options[cur + 1] == "  " ||
                            options[cur + 1] == "| " ||
                            options[cur + 1] == "> "
                        )
                        {
                            continue;
                        }
                        cur += 1;
                        break;
                    case "LeftArrow":
                        if (
                            cur - 1 < 1 ||
                            options[cur - 1] == "" ||
                            options[cur - 1] == " " ||
                            options[cur - 1] == "  " ||
                            options[cur - 1] == "| " ||
                            options[cur - 1] == "> "
                        )
                        {
                            continue;
                        }
                        cur -= 1;
                        break;
                    case "Escape":
                    	return 999;
                }
            }
            return cur;
        }
        public static void PeriodicTable()
        {
            string[] table = {
                "", "H ", "  ", "  ", "  ", "  ", "  ", "  ", "  ", "  ", "    ", "   ", "   ", "  ", " ", " ", " ", " ", "He",
                "Li", "Be", "  ", "  ", "  ", "  ", "  ", "  ", " ", " ", "     ", " ", "B ", "C ", "N ", "O ", "F ", "Ne",
                "Na", "Mg", "  ", "  ", "  ", "  ", "  ", "  ", " ", " ", "     ", " ", "Al", "Si", "P ", "S ", "Cl", "Ar",
                "K ", "Ca", "Sc", "Ti", "V ", "Cr", "Mn", "Fe", "Co", "Ni", "Cu", "Zn", "Ga", "Ge", "As", "Se", "Br", "Kr",
                "Rb", "Sr", "Y ", "Zr", "Nb", "Mo", "Tc", "Ru", "Rh", "Pd", "Ag", "Cd", "In", "Sn", "Sb", "Te", "I ", "Xe",
                "Cs", "Ba", "| ", "Hf", "Ta", "W ", "Re", "Os", "Ir", "Pt", "Au", "Hg", "Tl", "Pb", "Bi", "Po", "At", "Rn",
                "Fr", "Ra", "| ", "Rf", "Db", "Sg", "Bh", "Hs", "Mt", "Ds", "Rg", "Cn", "Nh", "Fl", "Mc", "Lv", "Ts", "Og",
                "  ", "  ", "> ", "La", "Ce", "Pr", "Nd", "Pm", "Sm", "Eu", "Gd", "Tb", "Dy", "Ho", "Er", "Tm", "Yb", "Lu",
                "  ", "  ", "> ", "Ac", "Th", "Pa", "U ", "Np", "Pu", "Am", "Cm", "Bk", "Cf", "Es", "Fm", "Md", "No", "Lr"
            };
            
            bool loop = true;
            while (loop)
            {
                int idx = InteractivePeriodicTable(options: table, length: 18);
                if (idx == 999)
                {
                    loop = false;
                    continue;
                }
                string chosenElement = table[idx];
                chosenElement = UI.RemoveSpaces(chosenElement);

                PeriodicTableElements = JsonHandler.ReadJson(filename: "PeriodicTable.json");
                for (int i = 0; i < PeriodicTableElements.Count; i++)
                {
                    JsonNode jsonObject = JsonHandler.ParseJson(PeriodicTableElements[i].ToString());
                    if (jsonObject["symbol"]?.ToString() == chosenElement)
                    {
                        Console.Clear();
                        Console.WriteLine(jsonObject);
                    }
                }

                Console.ReadKey();
            }
        }
        private static int loweredValue(string m, int i, int val, int updateVal)
        {
            int _;
            string n = "";
            int j = 2;
            try {
                if (m[i + 1].ToString().ToLower() == "_")
                {   
                    try {
                        bool loop = true;
                        while (loop)
                        {
                            if (Int32.TryParse(m[i + j].ToString(), out _))
                            {
                                n += Convert.ToInt32(m[i + j].ToString());
                                j++;
                            }
                            else
                            {
                                loop = false;
                            }
                        }
                    }
                    catch {}
                    val += (updateVal) * Convert.ToInt32(n);
                }
                else {
                    val += (updateVal);
                }
            }
            catch (Exception)
            {
                val += (updateVal);
            }
            return val;
        }
        private static int Charge(string m)
        {
            int _;
            int j = 1;
            string n = "";
            for (int i = 0; i < m.Length; i++)
            {
                if (m[i] == '^')
                {
                    try
                    {
                        bool loop = true;
                        while (loop)
                        {
                            if (m[i + j] == '-')
                            {
                                n += m[i + j].ToString();
                                j++;
                            }
                            else if (Int32.TryParse(m[i + j].ToString(), out _))
                            {
                                n += m[i + j].ToString();
                                j++;
                            }
                            else 
                            {
                                loop = false;
                            }
                        }  
                    }
                    catch (Exception) {}
                }
            }
            return n.Length != 0 ? Convert.ToInt32(n) : 0;
        }

        private static bool IsAtom(string atom)
        {
            if (atom.Length == 0) return false;
            List<string> atoms = new List<string>();
            bool prevUpper = false;
            string tmpAtom = "";
            for (int i = 0; i < atom.Length; i++)
            {
                if (atom[i].ToString() == atom[i].ToString().ToUpper())
                {
                    if (prevUpper)
                    {
                        atoms.Add(tmpAtom);
                    }
                    tmpAtom += atom[i];
                    prevUpper = true;
                }
                else
                {
                    atoms.Add(tmpAtom + atom[i]);
                    prevUpper = false;
                }

            }
            bool state = false;
            for (int i = 0; i < atoms.Count; i++)
            {
                if (atoms[i] != "H" && atoms[i] != "O")
                {
                    state = true;
                }
            }
            return state;
        }

        public static int OxidationValue(bool showValue, string molecule)
        {
            if (molecule == "")
            {
                Console.Clear();
                Console.WriteLine(@"            ) (`-.             _ .-') _     ('-.     .-') _                            .-') _   .-')    .-') _      ('-.                    
                ( OO ).          ( (  OO) )   ( OO ).-.(  OO) )                          ( OO ) ) ( OO ). (  OO) )    ( OO ).-.                
    .-'),-----.(_/.  \_)-. ,-.-') \     .'_   / . --. //     '._ ,-.-')  .-'),-----. ,--./ ,--,' (_)---\_)/     '._   / . --. / ,--.           
    ( OO'  .-.  '\  `.'  /  |  |OO),`'--..._)  | \-.  \ |'--...__)|  |OO)( OO'  .-.  '|   \ |  |\ /    _ | |'--...__)  | \-.  \  |  |.-')       
    /   |  | |  | \     /\  |  |  \|  |  \  '.-'-'  |  |'--.  .--'|  |  \/   |  | |  ||    \|  | )\  :` `. '--.  .--'.-'-'  |  | |  | OO )      
    \_) |  |\|  |  \   \ |  |  |(_/|  |   ' | \| |_.'  |   |  |   |  |(_/\_) |  |\|  ||  .     |/  '..`''.)   |  |    \| |_.'  | |  |`-' |      
    \ |  | |  | .'    \_),|  |_.'|  |   / :  |  .-.  |   |  |  ,|  |_.'  \ |  | |  ||  |\    |  .-._)   \   |  |     |  .-.  |(|  '---.'      
    `'  '-'  '/  .'.  \(_|  |   |  '--'  /  |  | |  |   |  | (_|  |      `'  '-'  '|  | \   |  \       /   |  |     |  | |  | |      |       
        `-----''--'   '--' `--'   `-------'   `--' `--'   `--'   `--'        `-----' `--'  `--'   `-----'    `--'     `--' `--' `------'       
    Use _ to lower a value and ^ to upper a value
    Also note that the expression will look something like this: 
    NO_3^-1
    Notice how postive values such as Mn^2, is not +2 but just 2
    ");
                Console.Write("Molekyle: ");
            }
            string? m;
            if (molecule == "")
            {
                m = Console.ReadLine();
            }
            else
            {
                m = molecule;
            }
            
            if (m == null) 
            {
                return 0;
            }

            if (!IsAtom(m))
            {
                Console.WriteLine("Atom is not valid.");
                return 0;
            }

            int val = 0;
            string mCur;

            for (int i = 0; i < m.Length; i++)
            {
                mCur = m[i].ToString().ToLower();
                if (mCur == "o")
                {
                    val = loweredValue(m, i, val, -2);
                }
                else if (mCur == "h")
                {
                    val = loweredValue(m, i, val, 1);
                }
            }

            int charge = Charge(m);

            int c = charge - val;
            
            if (showValue)
            {
                Console.WriteLine("Oxidationstal: " + c);
            }

            return c;
        }

        private static List<int> ReverseList(List<int> list)
        {
            List<int> tmpList = new List<int>();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                tmpList.Add(list[i]);
            }
            return tmpList;
        }
        
        private static Tuple<string[], string[]> ElectronicAccounting(List<int> r1OxidationValues, List<int> r2OxidationValues, string[] r1Split, string[] r2Split)
        {
            List<int> change = new List<int>();
            int tmp;

            for (int i = 0; i < r1OxidationValues.Count; i++)
            {
                tmp = r1OxidationValues[i] - r2OxidationValues[i];
                if (tmp < 0)
                {
                    tmp = -tmp;
                }
                change.Add(tmp);
            }

            change = ReverseList(change);

            for (int i = 0; i < change.Count; i++)
            {
                if (change[i] == 0 || change[i] == 1)
                    continue;
                r1Split[i] = change[i] + r1Split[i];
            }

            for (int i = 0; i < change.Count; i++)
            {
                if (change[i] == 0 || change[i] == 1)
                    continue;
                r2Split[i] = change[i] + r2Split[i];
            }

            return Tuple.Create(r1Split, r2Split);
        } 

        public static void Redox()
        {
            Console.Clear();
            Console.WriteLine(@" _  .-')     ('-.  _ .-') _              ) (`-.           
( \( -O )  _(  OO)( (  OO) )              ( OO ).         
 ,------. (,------.\     .'_  .-'),-----.(_/.  \_)-.      
 |   /`. ' |  .---',`'--..._)( OO'  .-.  '\  `.'  /       
 |  /  | | |  |    |  |  \  '/   |  | |  | \     /\       
 |  |_.' |(|  '--. |  |   ' |\_) |  |\|  |  \   \ |       
 |  .  '.' |  .--' |  |   / :  \ |  | |  | .'    \_)      
 |  |\  \  |  `---.|  '--'  /   `'  '-'  '/  .'.  \       
 `--' '--' `------'`-------'      `-----''--'   '--'      
 Plase note that you must sperate each element with '+' or '->'
 Use _ to lower a value and ^ to upper a value
 Also note that the expression will look something like this: 
 Mn + NO_3^-1 -> Mn^2 + NO_2
 Notice how postive values such as Mn^2, is not +2 but just 2
 ");
            Console.Write("Reaktion: ");
            string? r = Console.ReadLine();

            bool sep = false;
            string r1 = "";
            string r2 = "";
            for (int i = 0; i < r?.Length; i++)
            {
                if (r[i] == '>' || r[i] == ' ') {}
                else if (r[i] == '-' && r[i+1] == '>') 
                {
                    sep = true;
                }
                else if (!sep) 
                {
                    r1 += r[i];
                }
                else {
                    r2 += r[i];
                }
            }

            string[] r1Split = r1.Split('+');
            string[] r2Split = r2.Split('+');

            if (r1Split.Length != r2Split.Length)
            {
                Console.WriteLine("ERROR: Not equal lengths...");
                return;
            }

            List<int> r1OxidationValues = new List<int>();
            List<int> r2OxidationValues = new List<int>();

            for (int i = 0; i < r1Split.Length; i++)
            {
                r1OxidationValues.Add(OxidationValue(showValue: false, molecule: r1Split[i]));
            }

            for (int i = 0; i < r2Split.Length; i++)
            {
                r2OxidationValues.Add(OxidationValue(showValue: false, molecule: r2Split[i]));
            }

            Tuple<string[], string[]> tmpTuple = ElectronicAccounting(r1OxidationValues, r2OxidationValues, r1Split, r2Split);
            r1Split = tmpTuple.Item1;
            r2Split = tmpTuple.Item2;

            for (int i = 0; i < r1Split.Length; i++)
            {
                Console.WriteLine(r1Split[i]);
                Console.WriteLine(r2Split[i]);
            }

            /*
                Todo:
                    Afstem Negativiteten:
                        Basisk Opløsning:
                            OH^-1
                        Sur Opløsning:
                            H^1
                        Afstem Det Sidste Med H_2O
            */
        }
    }

    internal class JsonHandler
    {
        public static List<JsonElement> ReadJson(string filename)
        {
            List<JsonElement> elements = new List<JsonElement>();
            FileStream file = File.OpenRead(filename);
            JsonElement json = JsonDocument.Parse(file).RootElement;
            foreach (JsonElement element in json.EnumerateArray())
            {
                elements.Add(element);
            }
            return elements;
        }

        public static JsonNode ParseJson(string el)
        {
            return JsonNode.Parse(el)!.AsObject();
        }
    }

    internal class UI
    {
        public static void Intro()
        {
            Console.ResetColor();
            Console.Clear();
            Console.WriteLine(@"            ('-. .-.   ('-.  _   .-')      .-')                               (`-.      ('-.  _  .-')        
           ( OO )  / _(  OO)( '.( OO )_   ( OO ).                           _(OO  )_  _(  OO)( \( -O )       
   .-----. ,--. ,--.(,------.,--.   ,--.)(_)---\_) .-'),-----.  ,--.    ,--(_/   ,. \(,------.,------.       
  '  .--./ |  | |  | |  .---'|   `.'   | /    _ | ( OO'  .-.  ' |  |.-')\   \   /(__/ |  .---'|   /`. '      
  |  |('-. |   .|  | |  |    |         | \  :` `. /   |  | |  | |  | OO )\   \ /   /  |  |    |  /  | |      
 /_) |OO  )|       |(|  '--. |  |'.'|  |  '..`''.)\_) |  |\|  | |  |`-' | \   '   /, (|  '--. |  |_.' |      
 ||  |`-'| |  .-.  | |  .--' |  |   |  | .-._)   \  \ |  | |  |(|  '---.'  \     /__) |  .--' |  .  '.'      
(_'  '--'\ |  | |  | |  `---.|  |   |  | \       /   `'  '-'  ' |      |    \   /     |  `---.|  |\  \       
   `-----' `--' `--' `------'`--'   `--'  `-----'      `-----'  `------'     `-'      `------'`--' '--'      
Made by: TacK & Soren

Use up arrow, down arrow and enter to navigate.
");
        }
        public static int Menu(string[] arr)
        {
            int cur = 0;
            
            bool loop = true;
            while (loop)
            {
                Intro();
                for (int i = 0; i < arr.Length; i++)
                {
                    Console.BackgroundColor = cur == i ? ConsoleColor.White : ConsoleColor.Black;
                    Console.ForegroundColor = cur == i ? ConsoleColor.Black : ConsoleColor.White;
                    Console.WriteLine(cur == i ? $"> {arr[i]}" : $"{arr[i]}");
                    Console.ResetColor();
                }

                ConsoleKey choice = Console.ReadKey(true).Key;
                switch (choice.ToString())
                {
                    case "Enter":
                        loop = false;
                        break;
                    case "DownArrow":
                        cur += cur + 1 != arr.Length ? 1 : 0;
                        break;
                    case "UpArrow":
                        cur -= cur != 0 ? 1 : 0;
                        break;
                }
            }
            return cur;
        }

        public static string RemoveSpaces(string str)
        {
            string tmp = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].ToString() != " ")
                {
                    tmp += str[i];
                }
            }
            return tmp;
        }
    }
}