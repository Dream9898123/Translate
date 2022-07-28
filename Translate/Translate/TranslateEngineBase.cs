using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translate.Translate
{
    public class TranslateEngineBase
    {
        public virtual string GetResult(string translateValue)
        {
            string changeValue = translateValue;
            if(!string.IsNullOrEmpty(translateValue))
            {
                changeValue = "";
                string[] dealValue = translateValue.Split(' ');
                for(int i = 0; i < dealValue.Length; i++)
                {
                    if(i == 0)
                    {
                        changeValue += dealValue[i].ToLower();
                    }
                    else
                    {
                        changeValue += FirstCharToUpper(dealValue[i]);
                    }
                }
            }
            return changeValue;
        }
        private string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }
        public bool needLine;
    }
}
