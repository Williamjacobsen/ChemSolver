using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace ChemSolver
{
    internal class Program
    {   
        static void Main(string[] args)
        {
            Console.SetWindowSize(200, 50);

            int option = UI.Menu(new string[] {"Periodiske system", "Redoxreaktion", "Oxidationstal", "Organisk navngivning"});
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
                case 3:
                    Chemistry.OrganicNaming();
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

        private static int GetLoweredValue(string m, int i)
        {
            string tmp = "";
            int _;
            try
            {
                if (m[i + 1] == '_')
                {
                    for (int j = 2; j < m.Length; j++)
                    {
                        if (Int32.TryParse(m[i + j].ToString(), out _))
                        {
                            tmp += m[i + j].ToString();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    return 1;
                }
            } 
            catch (Exception) {
                if (tmp == "")
                {
                    return 1;
                }
            }

            return Convert.ToInt32(tmp);
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

        private static int ToPositive(int n)
        {
            if (n < 0)
            {
                n = -n;
            }
            return n;
        }

        private static string ArrayToString(string[] array)
        {
            string tmp = "";
            for (int i = 0; i < array.Length; i++)
            {
                tmp += array[i] + " ";
            }

            return tmp.Remove(tmp.Length - 1);
        }

        private static Tuple<string[], string[]> ChargeAccounting(string[] r1, string[] r2, int r1Charge, int r2Charge, string s)
        {
            string _r1 = ArrayToString(r1);
            string _r2 = ArrayToString(r2);
            if (s.ToString().ToLower()[0] == 's')
            {
                if (r1Charge < r2Charge)
                {
                    int difference = ToPositive(r1Charge - r2Charge);
                    _r1 = r1[0] + " " + r1[1] + " " + difference + "H^1";
                }
                else if (r1Charge > r2Charge)
                {
                    int difference = ToPositive(r2Charge - r1Charge);
                    _r2 = r2[0] + " " + r2[1] + " " + difference + "H^1";
                }
            }
            else if (s.ToString().ToLower()[0] == 'b')
            {
                if (r1Charge > r2Charge)
                {
                    int difference = ToPositive(r1Charge - r2Charge);
                    _r1 = r1[0] + " " + r1[1] + " " + difference + "OH^-1";
                }
                else if (r1Charge < r2Charge)
                {
                    int difference = ToPositive(r2Charge - r1Charge);
                    _r2 = r2[0] + " " + r2[1] + " " + difference + "OH^-1";
                }
            }

            return Tuple.Create(_r1.Split(" "), _r2.Split(" "));
        }

        private static int CountAtoms(string m, char atom)
        {
            int tmp = 0;
            for (int i = 0; i < m.Length; i++)
            {
                if (m[i] == atom)
                {
                    tmp += GetMultiplier(m) * GetLoweredValue(m, i);
                }
            }
            return tmp;
        }

        private static Tuple<string[], string[]> OH_Accounting(string[] r1, string[] r2)
        {
            string _r1 = ArrayToString(r1);
            string _r2 = ArrayToString(r2);

            int[] CountO = {0, 0};
            int[] CountH = {0, 0};

            for (int i = 0; i < r1.Length; i++)
            {
                CountO[0] += CountAtoms(r1[i], 'O');
                CountH[0] += CountAtoms(r1[i], 'H');
            }
            for (int i = 0; i < r2.Length; i++)
            {
                CountO[1] += CountAtoms(r2[i], 'O');
                CountH[1] += CountAtoms(r2[i], 'H');
            }

            if (CountO[0] < CountO[1])
            {
                _r1 += " " + (CountO[1] - CountO[0] > 1 ? CountO[1] - CountO[0] : "") + "H_2O";
            }
            else if (CountO[0] > CountO[1])
            {
                _r2 += " " + (CountO[0] - CountO[1] > 1 ? CountO[0] - CountO[1] : "") + "H_2O";
            }

            return Tuple.Create(_r1.Split(" "), _r2.Split(" "));
        }

        private static int GetMultiplier(string m)
        {
            string tmp = "";
            int _;
            for (int i = 0; i < m.Length; i++)
            {
                if (Int32.TryParse(m[i].ToString(), out _))
                {
                    tmp += m[i];
                }
                else
                {
                    break;
                }
            }
            if (tmp != "")
            {
                return Convert.ToInt32(tmp);
            }
            return 1;
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
                r1Split[i] = change[i] / CountAtoms(r1Split[i], r1Split[i][0]) + r1Split[i];
            }

            for (int i = 0; i < change.Count; i++)
            {
                if (change[i] == 0 || change[i] == 1)
                    continue;
                r2Split[i] = change[i] / CountAtoms(r2Split[i], r2Split[i][0]) + r2Split[i];
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
            Console.Write("Sur eller basisk opløsning (s/b): ");
            string? s = Console.ReadLine();

            if (string.IsNullOrEmpty(r) || string.IsNullOrEmpty(s))
            {
                return;
            }

            // examples : https://webkemi.dk/Reactions/Redox.htm 
            // test case:
            // r = "MnO_4^-1 + NO_2^-1 -> MnO_2 + NO_3^-1";
            // r = "Zn + Ag^1 -> Zn^2 + Ag";
            // r = "MnO_4^-1 + I^-1 -> Mn^3 + I_2";
            // s = "s";

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

            Console.WriteLine("\nReaktion: " + r);
            Console.WriteLine("Opløsning: " + s);

            List<int> r1OxidationValues = new List<int>();
            List<int> r2OxidationValues = new List<int>();

            Console.WriteLine("\n-OxidationValues-");
            for (int i = 0; i < r1Split.Length; i++)
            {
                r1OxidationValues.Add(OxidationValue(showValue: false, molecule: r1Split[i]));
                Console.WriteLine(r1Split[i] + " : " + r1OxidationValues[i].ToString());
            }

            for (int i = 0; i < r2Split.Length; i++)
            {
                r2OxidationValues.Add(OxidationValue(showValue: false, molecule: r2Split[i]));
                Console.WriteLine(r2Split[i] + " : " + r2OxidationValues[i].ToString());
            } 

            Console.WriteLine("\n-ElectronicAccounting-");
            Tuple<string[], string[]> tmpTuple = ElectronicAccounting(r1OxidationValues, r2OxidationValues, r1Split, r2Split);
            r1Split = tmpTuple.Item1;
            r2Split = tmpTuple.Item2;

            Console.WriteLine(ArrayToString(r1Split));
            Console.WriteLine(ArrayToString(r2Split));

            Console.WriteLine("\n-Charges-");
            int r1Charge = 0;
            int r2Charge = 0;
            for (int i = 0; i < r1Split.Length; i++)
            {
                r1Charge += GetMultiplier(r1Split[i]) * Charge(r1Split[i]);
            }
            Console.WriteLine(ArrayToString(r1Split) + " : " + r1Charge.ToString());

            for (int i = 0; i < r2Split.Length; i++)
            {
                r2Charge +=  GetMultiplier(r2Split[i]) * Charge(r2Split[i]);
            }
            Console.WriteLine(ArrayToString(r2Split) + " : " + r2Charge.ToString());

            Console.WriteLine("\n-ChargeAccounting-");
            tmpTuple = ChargeAccounting(r1Split, r2Split, r1Charge, r2Charge, s);
            r1Split = tmpTuple.Item1;
            r2Split = tmpTuple.Item2;

            Console.WriteLine(ArrayToString(r1Split));
            Console.WriteLine(ArrayToString(r2Split));

            Console.WriteLine("\n-OHAccounting-");
            tmpTuple = OH_Accounting(r1Split, r2Split);  
            r1Split = tmpTuple.Item1;
            r2Split = tmpTuple.Item2;          

            Console.WriteLine(ArrayToString(r1Split));
            Console.WriteLine(ArrayToString(r2Split));
        }

        public static void OrganicNaming()
        {
            Console.Clear();
            Console.WriteLine(@"             _  .-')                 ('-.         .-') _                             .-') _    ('-.     _   .-')                 .-') _                  
            ( \( -O )               ( OO ).-.    ( OO ) )                           ( OO ) )  ( OO ).-.( '.( OO )_              ( OO ) )                 
 .-'),-----. ,------.   ,----.      / . --. /,--./ ,--,' ,-.-')   .-----.       ,--./ ,--,'   / . --. / ,--.   ,--.) ,-.-') ,--./ ,--,'  ,----.          
( OO'  .-.  '|   /`. ' '  .-./-')   | \-.  \ |   \ |  |\ |  |OO) '  .--./       |   \ |  |\   | \-.  \  |   `.'   |  |  |OO)|   \ |  |\ '  .-./-')       
/   |  | |  ||  /  | | |  |_( O- ).-'-'  |  ||    \|  | )|  |  \ |  |('-.       |    \|  | ).-'-'  |  | |         |  |  |  \|    \|  | )|  |_( O- )      
\_) |  |\|  ||  |_.' | |  | .--, \ \| |_.'  ||  .     |/ |  |(_//_) |OO  )      |  .     |/  \| |_.'  | |  |'.'|  |  |  |(_/|  .     |/ |  | .--, \      
  \ |  | |  ||  .  '.'(|  | '. (_/  |  .-.  ||  |\    | ,|  |_.'||  |`-'|       |  |\    |    |  .-.  | |  |   |  | ,|  |_.'|  |\    | (|  | '. (_/      
   `'  '-'  '|  |\  \  |  '--'  |   |  | |  ||  | \   |(_|  |  (_'  '--'\       |  | \   |    |  | |  | |  |   |  |(_|  |   |  | \   |  |  '--'  |       
     `-----' `--' '--'  `------'    `--' `--'`--'  `--'  `--'     `-----'       `--'  `--'    `--' `--' `--'   `--'  `--'   `--'  `--'   `------'        ");

            Console.Write("Molekyle Struktur: ");
            string? OrganicChain = Console.ReadLine()?.ToLower();
            if (string.IsNullOrEmpty(OrganicChain))
            {
                return;
            }

            // test case: 
            //string OrganicChain = "CH(CH)CH(cc)CH(ch)C(c)CC".ToLower();
            //string OrganicChain = "CC=CCC".ToLower();
            Console.WriteLine(OrganicChain);

            string carbonChain = RemoveHydrogen(OrganicChain: OrganicChain);
            Console.WriteLine(carbonChain);

            Tuple<string, string> tmpHandleBanch = HandleBanch(carbonChain: carbonChain);
            OrganicChain = tmpHandleBanch.Item1;
            string branchNames = tmpHandleBanch.Item2;
            Console.WriteLine(OrganicChain);

            Tuple<int, int> tmpCountAtoms = CountCarbonAndBindings(OrganicChain: OrganicChain);
            int CountC = tmpCountAtoms.Item1;
            int CountBinding = tmpCountAtoms.Item2;

            Console.WriteLine(CountC);
            Console.WriteLine(CountBinding);

            string OrganicStartName = GetOrganicStartName(PureCarbonChain(OrganicChain).Length);

            string endWord = CalculateEndWord(carbonChain: carbonChain);
            Console.WriteLine(endWord);

            if (CountBinding == 0)
            {
                Console.WriteLine(branchNames + OrganicStartName + endWord);
                return;
            }

            Tuple<List<int>, List<int>> tmpOrganicBindingIndexLists = OrganicBindingIndexLists(carbonChain: carbonChain);
            List<int> firstBindings = tmpOrganicBindingIndexLists.Item1;
            List<int> lastBindings = tmpOrganicBindingIndexLists.Item2;
            List<int> listIndex = new List<int>();

            for (int i = 0; i < firstBindings.Count; i++)
            {
                if (firstBindings[i] != lastBindings[i])
                {
                    if (firstBindings[i] < lastBindings[i])
                    {
                        listIndex = firstBindings;
                        break;
                    }
                    else
                    {
                        listIndex = lastBindings;
                        break;
                    }
                }
            }

            string idxStr = "";
            for (int i = 0; i < listIndex.Count; i++)
            {
                idxStr += listIndex[i] + ",";
            }
            if (idxStr.Length > 0)
            {
                idxStr = idxStr.Remove(idxStr.Length - 1);
            }

            Console.WriteLine(branchNames + OrganicStartName + idxStr + GetMultipleName(CountBinding) + endWord);
        }

        private static string ExtractDitgitsFromString(string s)
        {
            string tmp = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsDigit(s[i]))
                {
                    tmp += s[i];
                }
            }
            return tmp;
        }
        
        private static string GetOrganicStartName(int _length)
        {
            return new string[]{ "", "meth", "eth", "prop", "but", "pent", "hex", "hept", "oct", "non", "dec" }[_length];
        }

        private static string GetMultipleName(int amount)
        {
            return new string[]{ "", "", "di", "tri", "tetra", "penta", "hexa", "hepta", "octa", "non", "dec" }[amount];
        }

        private static Tuple<int, int> CountCarbonAndBindings(string OrganicChain)
        {
            int CountC = 0;
            int CountBinding = 0;
            for (int i = 0; i < OrganicChain.Length; i++)
            {
                if (OrganicChain[i] == 'c')
                {
                    CountC++;
                }
                else if (OrganicChain[i] == '=' && OrganicChain[i + 1] == '=')
                {
                    // So that a triple binding will be counted as one
                }
                else if (OrganicChain[i] == '=')
                {
                    CountBinding++;
                }

            }
            return Tuple.Create(CountC, CountBinding);
        }

        private static string RemoveHydrogen(string OrganicChain)
        {
            string tmpOrganicStr = "";
            for (int i = 0; i < OrganicChain.Length; i++)
            {
                if (OrganicChain[i] != 'h')
                {
                    tmpOrganicStr += OrganicChain[i];
                }
            }
            return tmpOrganicStr;
        }

        private static string PureCarbonChain(string OrganicChain)
        {
            string tmpOrganicStr = "";
            for (int i = 0; i < OrganicChain.Length; i++)
            {
                if (OrganicChain[i] == 'c')
                {
                    tmpOrganicStr += OrganicChain[i];
                }
            }
            return tmpOrganicStr;
        }

        private static Tuple<List<int>, List<int>> OrganicBindingIndexLists(string carbonChain)
        {
            int idx = 0;
            int CountBinding = 0;
            List<int> firstBindings = new List<int>();
            List<int> lastBindings = new List<int>();
            for (int i = 0; i < carbonChain.Length; i++)
            {
                if (carbonChain[i] == '=')
                {
                    firstBindings.Add(i - CountBinding);
                    CountBinding++;
                }
            }
            for (int i = carbonChain.Length - 1; i > 0; i--)
            {
                if (carbonChain[i] == '=')
                {
                    lastBindings.Add(idx);
                }
                else
                {
                    idx++;
                }

            }
            return Tuple.Create(firstBindings, lastBindings);
        }

        private static List<int> SmallestIndexList(List<int> firstBindings, List<int> lastBindings)
        {

            List<int> numberIsSmallest = new List<int>();
            for (int i = 0; i < firstBindings.Count; i++)
            {
                if (firstBindings[i] < lastBindings[i])
                {
                    numberIsSmallest.Add(firstBindings[i]);
                }
                if (lastBindings[i] < firstBindings[i])
                {
                    numberIsSmallest.Add(lastBindings[i]);
                }
            }
            return numberIsSmallest;
        }

        private static string CalculateEndWord(string carbonChain)
        {
            string endWord = "an";
            try
            {
                for (int i = 0; i < carbonChain.Length; i++)
                {
                    if (carbonChain[i] == '=' && endWord != "yn")
                    {
                        endWord = "en";
                    }
                    if (carbonChain[i] == '=' && carbonChain[i + 1] == '=')
                    {
                        endWord = "yn";
                    }
                }
            }
            catch (Exception) { }
            return endWord;
        }

        private static Tuple<string, string> HandleBanch(string carbonChain)
        {
            List<int> branchIdx = new List<int>();
            List<string> branch = new List<string>();
            int carbonChainLength = 0;
            string tmp = "";
            bool isBranch = false;

            for (int i = 0; i < carbonChain.Length; i++)
            {
                if (!isBranch && carbonChain[i] == 'c')
                {
                    carbonChainLength++;
                }
                else if (carbonChain[i] == ')')
                {
                    isBranch = false;
                }
                else if (carbonChain[i] == '(')
                {
                    isBranch = true;
                    branchIdx.Add(carbonChainLength);
                    for (int j = i + 1; j < carbonChain.Length; j++)
                    {
                        if (carbonChain[j] != ')')
                        {
                            tmp += carbonChain[j];
                        }
                        else
                        {
                            branch.Add(tmp);
                            tmp = "";
                            break;
                        }
                    }
                }
            }

            string carbonChainBranchRemoved = Regex.Replace(carbonChain, @"\(.*?\)", "");

            List<Tuple<string, int>> branchNamesIdx = new List<Tuple<string, int>>();
            for (int i = 0; i < branchIdx.Count; i++)
            {
                branchNamesIdx.Add(Tuple.Create(GetOrganicStartName(branch[i].Length) + "yl", branchIdx[i]));
            }

            string idxStr = "";
            string lastBranchName = "";
            List<string> alreadyRanBranchNames = new List<string>();
            List<string> idxStrList = new List<string>();
            for (int i = 0; i < branchNamesIdx.Count; i++)
            {
                for (int j = 0; j < branchNamesIdx.Count; j++)
                {
                    if (lastBranchName == branchNamesIdx[j].Item1)
                    {
                        idxStr += branchNamesIdx[j].Item2 + ",";
                    }
                }
                bool isRan = false;
                for (int j = 0; j < alreadyRanBranchNames.Count; j++)
                {
                    if (alreadyRanBranchNames[j] == branchNamesIdx[i].Item1)
                    {
                        isRan = true;
                        lastBranchName = "";
                    }
                }
                if (!isRan)
                {
                    lastBranchName = branchNamesIdx[i].Item1;
                    alreadyRanBranchNames.Add(lastBranchName);
                }
                
                if (idxStr != "")
                {
                    idxStr = idxStr.Remove(idxStr.Length - 1);
                    idxStr += "-";
                    string nums = ExtractDitgitsFromString(idxStr);
                    idxStrList.Add(idxStr + GetMultipleName(nums.Length) + branchNamesIdx[i - 1].Item1);
                    idxStr = "";
                }
            }

            idxStrList.RemoveAll(v => v == "");
            string branchNames = string.Join("-", idxStrList);
            return Tuple.Create(carbonChainBranchRemoved, branchNames);
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
Made by: William Jacobsen & Søren Laursen

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