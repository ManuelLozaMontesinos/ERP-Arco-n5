using Microsoft.SqlServer.Management.SqlParser.Parser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ESD.ERP.Web.Infrastructure
{
   public class FormulaParser
   {
      private string[] ReservedTokenList { get; set; }
      public List<FormulaVariable> VariableList { get; set; } = new List<FormulaVariable>();
      public FormulaParser() {
         ReservedTokenList = new string[]
         {
            "select","from","tableeval",
            "+","-","*","/","(",")","=",">","<",">=","<=","and","or","not",
            "in","case","when","then","else","end",
            "where","between","isnull"
         };
      }
      public List<FormulaVariable> Parse(string formula, string condicion)
      {

         List<FormulaVariable> resultList = new List<FormulaVariable>();

         string exprSQL = string.Concat("select",
            "\n", (string.IsNullOrWhiteSpace(formula) ? "*" : formula),
            "\n", "from tableeval",
            string.IsNullOrWhiteSpace(condicion) ? string.Empty :
            string.Concat("\nWHERE ", condicion));

         StringBuilder sb = new StringBuilder(exprSQL);

         sb.Replace("{", string.Empty);
         sb.Replace("}", string.Empty);

         ParseResult pr = Parser.Parse(sb.ToString());

         if (pr.Errors.Count() > 0)
            throw new Exception(SR.GetString("GEN_formula_error_sintaxis"));

         string[] tokenList = getEvalTokenList(exprSQL);

         foreach (string evalToken in tokenList)
         {
            string token = evalToken.ToLower();
            int position = Array.IndexOf(ReservedTokenList, token);
            if (position != -1)
               continue;

            Regex regex = new Regex("'.*?'");
            if (regex.IsMatch(token))
               continue;

            if(isNumeric(token))
               continue;

            if (VariableList.Exists(x => x.Key == token))
            {
               if (resultList.Exists(x => x.Key == token))
               {
                  FormulaVariable item = resultList.Find(x => x.Key == token);
                  resultList.Add(new FormulaVariable() { Key = item.Key, Type = item.Type });
               }
            }
            else
               throw new Exception(SR.GetString("GEN_formula_token_error", evalToken));
         }

         return resultList;

         //if (pr.Errors.Count() == 0)
         //   throw new Exception(SR.GetString("GEN_formula_condicion_error_sintaxis"));
      }
      private string[] getEvalTokenList(string exprSQL) {
         string[] list = exprSQL.Split(new char[]{' ','+','-','*','/','(',')',
            '>','<','=',',','\r','\n','\t'}, StringSplitOptions.RemoveEmptyEntries);

         return list;
      }
      private bool isNumeric(string s)
      {
         double retNum = 0;

         bool isNum = Double.TryParse(s, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out retNum);
         return isNum;
      }
      //private string resolveError(ParseResult pr)
      //{
      //   string errorMessage = string.Empty;

      //   foreach (Error error in pr.Errors)
      //   {
      //      switch (error.Message)
      //      {
      //         case "Incorrect syntax near 'FROM'.":
      //            errorMessage = "Error de sintaxis en la Fórmula";
      //            break;

      //         default:
      //            errorMessage = error.Message;
      //            break;
      //      }
      //      break;
      //   }

      //   return errorMessage;
      //}
   }
}
