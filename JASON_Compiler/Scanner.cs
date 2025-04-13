using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

public enum Token_Class
{
    //Begin, Call, Declare, End, Do, Else, EndIf, EndUntil, EndWhile, If, Integer,
    //Parameters, Procedure, Program, Read, Real, Set, Then, Until, While, Write,
    //Dot, Semicolon, Comma, LParanthesis, RParanthesis, EqualOp, LessThanOp,
    //GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, DivideOp,
    //Idenifier, Constant


    Int, Float, Data_type_String, String, Read, Write, Repeat, Until, If, Elseif, Else, Then,
    Return, Endl, End, PlusOp, MinusOp, MultiplyOp, DivideOp, Identfire, Number, LessThan,
    GreaterThan, IsEqual, NotEqual, And, Or, AssignmentOP,Main,
    OpenBracket,//  [
    CloseBracket,// ]
    OpenParen,//    (
    CloseParen, //  )
    OpenBrac,// {
    CloseBrac,//    }
    Semicolon,
    Comma

}
namespace JASON_Compiler
{


    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> ConditionOperators = new Dictionary<string, Token_Class>();

        Dictionary<string, Token_Class> BooleanOperators = new Dictionary<string, Token_Class>();
        public Scanner()
        {
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("int", Token_Class.Int);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.Data_type_String);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("elseif", Token_Class.Elseif);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("Identfire", Token_Class.Identfire);
            ReservedWords.Add("end", Token_Class.End);
            ReservedWords.Add("main", Token_Class.Main);

            ReservedWords.Add("[", Token_Class.OpenBracket);
            ReservedWords.Add("]", Token_Class.CloseBracket);
            ReservedWords.Add("(", Token_Class.OpenParen);
            ReservedWords.Add(")", Token_Class.CloseParen);
            ReservedWords.Add("{", Token_Class.OpenBrac);
            ReservedWords.Add("}", Token_Class.CloseBrac);
            ReservedWords.Add(";", Token_Class.Semicolon);
            ReservedWords.Add(",", Token_Class.Comma);



            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);



            ConditionOperators.Add("=", Token_Class.IsEqual);
            ConditionOperators.Add("<>", Token_Class.NotEqual);
            ConditionOperators.Add("<", Token_Class.LessThan);
            ConditionOperators.Add(">", Token_Class.GreaterThan);


            BooleanOperators.Add("&&", Token_Class.And);
            BooleanOperators.Add("||", Token_Class.Or);
        }

        public void StartScanning(string SourceCode)
        {
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                    continue;
                if (SourceCode[j] >= 'A' && SourceCode[j] <= 'z')
                {
                    j++;
                    while (j < SourceCode.Length)
                    {
                        if (SourceCode[j] >= 'A' && SourceCode[j] <= 'z' || SourceCode[j] >= '0' && SourceCode[j] <= '9') // x:=5 , /2
                        {
                            CurrentLexeme += SourceCode[j];
                        }
                        //else if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                        //    break;
                        //else
                        //    CurrentLexeme += SourceCode[j];
                        else
                            break;
                        j++;
                    }
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                    continue;
                }
                else if (SourceCode[j] >= '0' && SourceCode[j] <= '9')
                {
                    j++;
                    while (j < SourceCode.Length)
                    {
                        if (SourceCode[j] >= '0' && SourceCode[j] <= '9' || SourceCode[j] == '.')
                        {
                            CurrentLexeme += SourceCode[j];
                        }
                        else if (SourceCode[j] >= 'A' && SourceCode[j] <= 'z')
                        {
                            CurrentLexeme += SourceCode[j];
                            //if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                            //    break;
                            //else
                            //    CurrentLexeme += SourceCode[j];
                        }
                        else
                            break;
                        j++;
                    }
                    FindTokenClass(CurrentLexeme);
                    i = j - 1;
                    continue;

                }
                else if (CurrentChar == '{' || CurrentChar == '}' || CurrentChar == '[' || CurrentChar == ']'
                    || CurrentChar == '(' || CurrentChar == ')' || CurrentChar == ';' || CurrentChar == ',' || CurrentChar == ':')
                {
                    if (CurrentChar == ':' && j + 1 < SourceCode.Length)
                    {
                        j++;
                        CurrentLexeme += SourceCode[j];
                    }
                    FindTokenClass(CurrentLexeme);
                    i = j;
                    continue;
                }
                else if (CurrentChar == '"')
                {
                    j++;
                    while (j < SourceCode.Length)
                    {
                        CurrentLexeme += SourceCode[j].ToString();
                        j++;
                        if (SourceCode[j - 1] == '"')
                        {
                            break;
                        }
                    }
                    FindTokenClass(CurrentLexeme.Trim());
                    i = j - 1;
                    continue;
                }
                else if (CurrentChar == '/')
                {
                    bool closed = false, enter_condition = false;
                    j++;
                    if ((j < SourceCode.Length && SourceCode[j] != '*') && (SourceCode[j] < '0' || SourceCode[j] > '9'))
                    {
                        while (j < SourceCode.Length)
                        {
                            CurrentChar = SourceCode[j];
                            CurrentLexeme += SourceCode[j];
                            if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                            {
                                j++;
                                break;
                            }
                            j++;
                        }
                        FindTokenClass(CurrentLexeme);
                        i = j;
                        continue;
                    }
                    else if (j < SourceCode.Length && SourceCode[j] == '*')
                    {
                        enter_condition = true;
                        while (j < SourceCode.Length)
                        {
                            CurrentLexeme += SourceCode[j];
                            if (SourceCode[j - 1] == '*' && SourceCode[j] == '/')
                            {
                                closed = true;
                                j++;
                                break;
                            }
                            j++;
                        }

                    }
                    // if change it the comment will be in list error

                    if (closed == false && enter_condition == true)
                        Errors.Error_List.Add("undefined token: " + CurrentLexeme);
                    else if (closed == false && enter_condition == false)
                    {
                        if (Operators.ContainsKey(CurrentLexeme))
                        {
                            FindTokenClass(CurrentLexeme);
                            continue;
                        }
                    }

                    i = j - 1;
                    continue;
                }

                else
                {
                    if (Operators.ContainsKey(CurrentLexeme))
                    {
                        //j++;
                        //if (j<SourceCode.Length&&(SourceCode[j] < '0' || SourceCode[j] > '9'))
                        //{
                        //    while (j < SourceCode.Length)
                        //    {
                        //        CurrentChar = SourceCode[j];
                        //        CurrentLexeme += SourceCode[j];
                        //        if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                        //        {
                        //            j++;
                        //            break;
                        //        }
                        //        j++;
                        //    }

                        //}
                        FindTokenClass(CurrentLexeme);
                        //i = j-1;
                        continue;
                    }
                    else
                    {
                        j++;
                        while (j < SourceCode.Length)
                        {
                            if (!(SourceCode[j] == ' ' || SourceCode[j] == '\r' || SourceCode[j] == '\n'))
                            {
                                CurrentLexeme += SourceCode[j];
                                j++;
                            }
                            else { j--; break; }
                        }


                    }
                }
                FindTokenClass(CurrentLexeme.Trim());
                i = j;
                continue;
            }

            JASON_Compiler.TokenStream = Tokens;
        }
        void FindTokenClass(string Lex)
        {

            Token Tok = new Token();
            Tok.lex = Lex;

            if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
            }
            else if (ConditionOperators.ContainsKey(Lex))
            {
                Tok.token_type = ConditionOperators[Lex];
                Tokens.Add(Tok);
            }
            else if (BooleanOperators.ContainsKey(Lex))
            {
                Tok.token_type = BooleanOperators[Lex];
                Tokens.Add(Tok);
            }
            else if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
            }
            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Identfire;
                Tokens.Add(Tok);
            }
            else if (isString(Lex))
            {
                Tok.token_type = Token_Class.String;
                Tokens.Add(Tok);
            }
            else if (isNumber(Lex))
            {
                Tok.token_type = Token_Class.Number;
                Tokens.Add(Tok);
            }
            else if (isAssignmentOp(Lex))
            {
                Tok.token_type = Token_Class.AssignmentOP;
                Tokens.Add(Tok);
            }

            //Is it an undefined?
            else
            {
                Errors.Error_List.Add("undefined token: " + Lex);
            }
        }



        bool isIdentifier(string lex)
        {
            Regex ID = new Regex(@"^[A-Za-z][A-Za-z0-9]*$");
            if (ID.IsMatch(lex))
            {
                return true;
            }

            return false;
        }

        bool isString(string lex)
        {
            Regex String = new Regex(@"^"".*""$");
            if (String.IsMatch(lex))
            {
                return true;
            }

            return false;
        }


        bool isNumber(string lex)
        {
            Regex Number = new Regex(@"^[0-9]+(\.[0-9]+)?$");
            if (Number.IsMatch(lex))
            {
                return true;
            }
            return false;
        }
        bool isAssignmentOp(string lex)
        {

            if (lex == ":=")
            {
                return true;
            }
            return false;
        }



    }
}