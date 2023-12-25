using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



public enum Token_Class
{
    Begin, Call, Declare, End, Do, Else, EndIf, EndUntil, EndWhile, If, Integer,
    Parameters, Procedure, Program, Read, Real, Set, Then, Until, While, Write,
    Dot, Semicolon, Comma, LParanthesis, RParanthesis, EqualOp, LessThanOp,
    GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, DivideOp,
    Idenifier, Constant, Comment, String, Float, Repeat, Elseif, Return, Endl, AssighmentOperator, lCurly,RCurly ,condationOr ,condationAnd
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

        public Scanner()
        {
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("end", Token_Class.End);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("endif", Token_Class.EndIf);
            ReservedWords.Add("int", Token_Class.Integer);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("while", Token_Class.While);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("elseif", Token_Class.Elseif);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("endl", Token_Class.Endl);
            // Operators.Add(".", Token_Class.Dot);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add(":=", Token_Class.AssighmentOperator);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("{", Token_Class.lCurly);
            Operators.Add("}", Token_Class.RCurly);
            Operators.Add("||", Token_Class.condationOr);
            Operators.Add("&&", Token_Class.condationAnd);


        }
    static bool IsNumber(char c){return c >= '0' && c <= '9';}
    static bool IsDelimiter(char c){return c == ' ' || c == '\r' || c == '\n' || c == '\t';}
    static bool IsEnglishChar(char c) {return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');}
    
    public void StartScanning(string SourceCode)
    {
  
        for(int i=0; i<SourceCode.Length;i++)
        {
            int j = i;
            char CurrentChar = SourceCode[i];
            string CurrentLexeme = "";
            // del continue

            if (IsDelimiter(CurrentChar))
                continue;

            if(i+1<SourceCode.Length){
                string[] arr = { "<>", "&&", "||",  ":=" };
                foreach (string s in arr){
                    if(s[0]==SourceCode[i] && s[1]==SourceCode[i+1]){
                        i++;
                        FindTokenClass(s);
                        goto loop_end;
                    } 
                }
            }
            // String
            if(CurrentChar == '"')
            {
                CurrentLexeme += CurrentChar;
                for(j = i + 1; j < SourceCode.Length ; j++)
                {
                    CurrentLexeme += SourceCode[j];
                    if(SourceCode[j]=='"' || SourceCode[j]=='\n')
                        break;
                }
                i = j;
            }
            // Comment

            else if ( i + 1 < SourceCode.Length && CurrentChar == '/' && SourceCode[ i + 1 ]=='*')
            {
                
                CurrentLexeme += "/*";
                for( j = i + 2 ; j < SourceCode.Length ; j++)
                {
                    CurrentLexeme += SourceCode[j];
                    if(j + 1 < SourceCode.Length && SourceCode[j]=='*' && SourceCode[j+1]=='/')
                    {
                        CurrentLexeme += "/";
                        break;
                    }
                }
                i = j + 1;
            }
            /**
            /
            */
            //if you read a character
            else if (IsEnglishChar(CurrentChar)) 
            {
                for(j = i ; j < SourceCode.Length ; j++)
                {
                    CurrentChar = SourceCode[j];

                    if(IsEnglishChar(CurrentChar) || IsNumber(CurrentChar))
                        CurrentLexeme += CurrentChar;
                    else
                     break;
                }
                i = j - 1;
            }
            // check if number 
            else if(IsNumber(CurrentChar))
            {
                CurrentLexeme += CurrentChar;
                for(j=i+1 ; j<SourceCode.Length;j++)
                {
                    CurrentChar = SourceCode[j];
                    if(IsNumber(CurrentChar) || IsEnglishChar(CurrentChar) || CurrentChar=='.')
                        CurrentLexeme += CurrentChar;
                    else
                        break;
                }
                i = j - 1;
            }
            // operator
            else if(Operators.ContainsKey(CurrentChar.ToString()) == true)
            {
                CurrentLexeme += CurrentChar;
            }
            // 
            else
            {
               CurrentLexeme+=CurrentChar;
            }
            FindTokenClass(CurrentLexeme);
            loop_end:
                continue;
        }
        
        JASON_Compiler.TokenStream = Tokens;
    }


    
        void FindTokenClass(string Lex)
        {
            // Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is it a reserved word?
            if(ReservedWords.ContainsKey(Lex)==true)
            {
                Tok.token_type=ReservedWords[Lex];
                Tokens.Add(Tok);
                return;
            }        

            //Is it an identifier?
            
            
            if(isIdentifier(Lex))
            {
                Tok.token_type=Token_Class.Idenifier;
                Tokens.Add(Tok);
                return;
            }

            //Is it a Constant?

            if(isConstant(Lex))
            {
                Tok.token_type=Token_Class.Constant;
                Tokens.Add(Tok);
                return;
            }

            // Comment
            if(isComment(Lex))
            {
                Tok.token_type=Token_Class.Comment;
                Tokens.Add(Tok);
                return;
            }


            // String
            if(isString(Lex))
            {
                Tok.token_type=Token_Class.String;
                Tokens.Add(Tok);
                return;
            }
            //Is it an operator?
            if(Lex == ":=")
            {
                Tok.token_type=Token_Class.AssighmentOperator;
                Tokens.Add(Tok);
                return;    
            }

            if(Lex == "<>")
            {
                Tok.token_type=Token_Class.NotEqualOp;
                Tokens.Add(Tok);
                return;
            }

            if(Operators.ContainsKey(Lex)==true)
            {
                Tok.token_type=Operators[Lex];
                Tokens.Add(Tok);
                return;
            }

            //Is it an undefined?
            Errors.Error_List.Add(Lex);

        }
        bool isIdentifier(string lex)
        {
            bool isValid=true;
            // Check if the lex is an identifier or not.
            Regex re = new Regex(@"^[a-zA-Z][a-zA-Z0-9]*$",RegexOptions.Compiled);
            isValid &= re.IsMatch(lex);
            return isValid;
        }
        bool isConstant(string lex)
        {
            bool isValid = true;
            // Check if the lex is a constant (Number) or not.
            Regex re = new Regex(@"^\d+(\.\d+)?$",RegexOptions.Compiled);
            isValid &= re.IsMatch(lex);
            return isValid;
        }

        bool isComment(string lex)
        {
            bool isValid = true;
            // Check if the lex is a constant (Number) or not.
            Regex re = new Regex(@"^/\*(.|[\s])*\*/$",RegexOptions.Compiled);
            isValid &= re.IsMatch(lex);
            return isValid;
        }

        bool isString(string lex)
        {
            bool isValid = true;
            Regex re = new Regex("^\".*\"$",RegexOptions.Compiled);
            isValid &= re.IsMatch(lex);
            return isValid;
        }
    }
}
