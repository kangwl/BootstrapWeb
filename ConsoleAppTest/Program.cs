using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleAppTest {
    class Program {
        static void Main(string[] args) {
            TestM testM = new TestM();
            Console.WriteLine(testM.GetName("2"));
            Console.Read();
        }


    }

    public class TestM {
        public string GetName(string act) {
            if (act == "1") {
                return "n1";
            }
            else if (act == "2") {
                return "n2";
            }
            else if (act == "3") {
                return "n3";
            }
            return "unknow";
        }
    }
}
