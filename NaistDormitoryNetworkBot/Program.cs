using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaistDormitoryNetworkBot
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length != 1)
            {
                System.Console.WriteLine("arg: 1 for \"in dormitory\" 2 for\"outside dormitroy\" ");
                return;
            }
            int mode;
            int.TryParse(args[0],out mode);
            switch (mode)
            {
                case 1:
                    BotInDormitory botInDormitory = new BotInDormitory();
                    botInDormitory.Run();
                    break;
                case 2:
                    BotOutsideDormitory botOutsideDormitory = new BotOutsideDormitory();
                    botOutsideDormitory.Run();
                    break;
                default:
                    System.Console.WriteLine("arg : 1 for \"in dormitory\" / 2 for\"out of dormitroy\" ");
                    break;
            }

            return;

        }
    }
}
