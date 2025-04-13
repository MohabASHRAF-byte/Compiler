using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JASON_Compiler
{
    public class Node
    {
        public List<Node> Children = new List<Node>();
        
        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public  Node root;
        
        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }
        Node Program()
        {
            Node program = new Node("Begin_Of_Program");
            
            program.Children.Add(Functions());
            MessageBox.Show("Success");
            return program;
        }

        Node Main_Function()
        {
            Node Main = new Node("Main_Function");
            //Main.Children.Add(Data_Type());
            Main.Children.Add(match(Token_Class.Main));
            Main.Children.Add(match(Token_Class.OpenParen));
            Main.Children.Add(match(Token_Class.CloseParen));
            Main.Children.Add(Function_Body());
            return Main;
        }

        Node Functions()
        {
            Node Fun_ = new Node("Functions");
            Fun_.Children.Add(Data_Type());
            Fun_.Children.Add(Fun());
            return Fun_;
            //bool less_than_count = InputPointer < TokenStream.Count;
            //bool token_Id = false;
            //bool token_Main = false;
            ////bool token_string = false;

            //if (less_than_count)
            //{
            //    token_Id = TokenStream[InputPointer].token_type == Token_Class.Identfire;
            //    token_Main = TokenStream[InputPointer].token_type == Token_Class.Main;
            //    //token_string = TokenStream[InputPointer].token_type == Token_Class.Data_type_String;
            //}
            //if (token_Id)
            //{
            //    Fun.Children.Add(Function_State());
            //    Fun.Children.Add(Functions());
            //    return Fun;
            //}
            //else if(token_Main)
            //{
            //    Fun.Children.Add(Main_Function());
            //    return Fun;
            //}
            //Errors.Error_List.Add("Parsing Error: Expected "
            //   + Token_Class.Identfire + " or " + "Expected "
            //   + Token_Class.Main + " and " +
            //   TokenStream[InputPointer].token_type.ToString() +
            //   "  found\r\n");
            //if (InputPointer + 1 < TokenStream.Count)
            //    InputPointer++;
            //return null;
            //return Fun;
        }
        Node Fun()
        {
            Node Function = new Node("Function");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_Id = false;
            bool token_Main = false;
            //bool token_string = false;

            if (less_than_count)
            {
                token_Id = TokenStream[InputPointer].token_type == Token_Class.Identfire;
                token_Main = TokenStream[InputPointer].token_type == Token_Class.Main;
                //token_string = TokenStream[InputPointer].token_type == Token_Class.Data_type_String;
            }
            if (token_Id)
            {
                Function.Children.Add(Function_State());
                Function.Children.Add(Functions());
                return Function;
            }
            else if (token_Main)
            {
                Function.Children.Add(Main_Function());
                return Function;
            }
            Errors.Error_List.Add("Parsing Error: Expected "
               + Token_Class.Identfire + " or " + "Expected "
               + Token_Class.Main + " and " +
               TokenStream[InputPointer].token_type.ToString() +
               "  found\r\n");
            if (InputPointer + 1 < TokenStream.Count)
                InputPointer++;
            return null;
        }
        Node Function_State()
        {
            Node Fun_State = new Node("Function_State");
            bool less_than_count = InputPointer < TokenStream.Count;
            //bool token_int = false;
            //bool token_float = false;
            //bool token_string = false;

            //if (less_than_count)
            //{

            //     token_int = TokenStream[InputPointer].token_type == Token_Class.Int;
            //     token_float = TokenStream[InputPointer].token_type == Token_Class.Float;
            //     token_string = TokenStream[InputPointer].token_type == Token_Class.Data_type_String;
            //}
            bool token_Id = false;
            //bool token_string = false;

            if (less_than_count)
            {
                token_Id = TokenStream[InputPointer].token_type == Token_Class.Identfire;
            }
                if (token_Id)
            {
                Fun_State.Children.Add(Function_Declar());
                Fun_State.Children.Add(Function_Body());
                return Fun_State;
            }
            Errors.Error_List.Add("Parsing Error: Expected "
                 + Token_Class.Identfire + " or " + "Expected "
                 + " and " +
                 TokenStream[InputPointer].token_type.ToString() +
                 "  found\r\n");
            if (InputPointer + 1 < TokenStream.Count)
                InputPointer++;
            return null;

        }
        Node Function_Declar()
        {
            Node Fun_Decl = new Node("Function_Declar");

            //Fun_Decl.Children.Add(Data_Type());
            Fun_Decl.Children.Add(Function_Name());
            Fun_Decl.Children.Add(match(Token_Class.OpenParen));
            Fun_Decl.Children.Add(Parameter_List());
            Fun_Decl.Children.Add(match(Token_Class.CloseParen));

            return Fun_Decl;
        }      
        Node Data_Type()
        {
            Node data_type = new Node("Data_Type");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_int = false;
            bool token_float = false;
            bool token_string = false;

            if (less_than_count)
            {

                token_int = TokenStream[InputPointer].token_type == Token_Class.Int;
                token_float = TokenStream[InputPointer].token_type == Token_Class.Float;
                token_string = TokenStream[InputPointer].token_type == Token_Class.Data_type_String;
            }
            if (token_int)
            {
                data_type.Children.Add(match(Token_Class.Int));
                return data_type;
            }
            else if (token_float)
            {
                data_type.Children.Add(match(Token_Class.Float));
                return data_type;
            }
            else if (token_string)
            {
                data_type.Children.Add(match(Token_Class.Data_type_String));
                return data_type;
            }
            Errors.Error_List.Add("Parsing Error: Expected "
                  + Token_Class.Int + " or " + "Expected "
                  + Token_Class.Float + " or " + "Expected "
                  + Token_Class.Data_type_String + " and " +
                  TokenStream[InputPointer].token_type.ToString() +
                  "  found\r\n");
            if (InputPointer + 1 < TokenStream.Count)
                InputPointer++;
            return null;

        }
        Node Function_Name()
        {
            Node Fun_Name = new Node("Function_Name");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_Id = false;

            if (less_than_count)
            {
                token_Id = TokenStream[InputPointer].token_type == Token_Class.Identfire;
            }
            if (token_Id)
            {
                Fun_Name.Children.Add(match(Token_Class.Identfire));
                return Fun_Name;
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                 + Token_Class.Identfire + " and " +
                 TokenStream[InputPointer].token_type.ToString() +
                 "  found\r\n");
                if (InputPointer + 1 < TokenStream.Count)
                    InputPointer++;
                return null;
            }
        }
        Node Parameter_List()
        {
            Node Para_list = new Node("Parameter_List");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_int = false;
            bool token_float = false;
            bool token_string = false;

            if (less_than_count)
            {
                token_int = TokenStream[InputPointer].token_type == Token_Class.Int;
                token_float = TokenStream[InputPointer].token_type == Token_Class.Float;
                token_string = TokenStream[InputPointer].token_type == Token_Class.Data_type_String;
            }

            if (token_int || token_float || token_string)
            {
                Para_list.Children.Add(Parameter());
                Para_list.Children.Add(Parameter_Lists());
                return Para_list;
            }
            else
            { return null; }


        }
        Node Parameter_Lists()
        {
            Node Para_lists = new Node("Parameter_Lists");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_Comma = false;
            if (less_than_count)
                token_Comma = TokenStream[InputPointer].token_type == Token_Class.Comma;

            if (token_Comma)
            {
                Para_lists.Children.Add(match(Token_Class.Comma));
                Para_lists.Children.Add(Parameter());
                Para_lists.Children.Add(Parameter_Lists());
                return Para_lists;
            }
            else
            { return null; }


        }
        Node Parameter()
        {
            Node Para = new Node("Parameter");

            Para.Children.Add(Data_Type());
            Para.Children.Add(match(Token_Class.Identfire));
            return Para;
        }
        Node Function_Body()
        {
            Node Fun_Body = new Node("Function_Body");

            Fun_Body.Children.Add(match(Token_Class.OpenBrac));
            Fun_Body.Children.Add(Statements());
            Fun_Body.Children.Add(Return_Statement());
            Fun_Body.Children.Add(match(Token_Class.CloseBrac));
            return Fun_Body;
        }
        Node Statement()
        {
            Node State = new Node("Statement");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_int = false, token_float = false, token_string = false;
            bool token_Id = false;
            bool token_write = false;
            bool token_read = false;
            bool token_if = false;
            bool token_repeat = false;
            //bool token_return = false;

            if (less_than_count)
            {
                token_int = TokenStream[InputPointer].token_type == Token_Class.Int;
                token_float = TokenStream[InputPointer].token_type == Token_Class.Float;
                token_string = TokenStream[InputPointer].token_type == Token_Class.Data_type_String;
                token_Id= TokenStream[InputPointer].token_type == Token_Class.Identfire;
                token_write = TokenStream[InputPointer].token_type == Token_Class.Write;
                token_read = TokenStream[InputPointer].token_type == Token_Class.Read;
                token_if = TokenStream[InputPointer].token_type == Token_Class.If;
                token_repeat = TokenStream[InputPointer].token_type == Token_Class.Repeat;
                //token_return = TokenStream[InputPointer].token_type == Token_Class.Return;
            }
            if(token_float||token_int||token_string)
            {
                State.Children.Add(Declaration_Statement());
                return State;
            }
            else if(token_Id)
            {
                
                State.Children.Add(Assignment_Statement());
                return State;
            }
            else if(token_write)
            {
                State.Children.Add(Write_Statement());
                return State;
            }
            else if(token_read)
            {
                State.Children.Add(Read_Statement());
                return State;
            }
            else if (token_if)
            {
                State.Children.Add(If_Statement());
                return State;   
            }
            else if (token_repeat)
            {
                State.Children.Add(Repeat_Statement());
                return State;
            }
            //else if (token_return)
            //{
            //    State.Children.Add(Assignment_Statement());
            //}
            Errors.Error_List.Add("Parsing Error: Expected "
           + Token_Class.Identfire + " or " + "Expected "
           + Token_Class.Number + " or " + "Expected "
           + Token_Class.String + " or " + "Expected "
           + Token_Class.Int + " or "
           + Token_Class.Write + " or " + "Expected "
           + Token_Class.Read + " or " + "Expected "
           + Token_Class.If + " or " + "Expected "
           + Token_Class.Repeat + " and "+

           TokenStream[InputPointer].token_type.ToString() +
           "  found\r\n");
            if(InputPointer+1<TokenStream.Count)
            InputPointer++;

            return null;
        }
        Node Statements()
        {
            Node States = new Node("Set_Statement");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_int = false, token_float = false, token_string = false;
            bool token_Id = false;
            bool token_write = false;
            bool token_read = false;
            bool token_if = false;
            bool token_repeat = false;
            //bool token_return = false;

            if (less_than_count)
            {
                token_int = TokenStream[InputPointer].token_type == Token_Class.Int;
                token_float = TokenStream[InputPointer].token_type == Token_Class.Float;
                token_string = TokenStream[InputPointer].token_type == Token_Class.Data_type_String;
                token_Id = TokenStream[InputPointer].token_type == Token_Class.Identfire;
                token_write = TokenStream[InputPointer].token_type == Token_Class.Write;
                token_read = TokenStream[InputPointer].token_type == Token_Class.Read;
                token_if = TokenStream[InputPointer].token_type == Token_Class.If;
                token_repeat = TokenStream[InputPointer].token_type == Token_Class.Repeat;
                //token_return = TokenStream[InputPointer].token_type == Token_Class.Return;
            }
            if (token_float || token_int 
                || token_string|| token_Id
                || token_write|| token_read
                || token_if|| token_repeat)
            {
                States.Children.Add(Statement());
                States.Children.Add(Statements());
                return States;
            }
           
            return null;
        }
        Node Read_Statement()
        {
            Node Read_State = new Node("Read_Statement");
            Read_State.Children.Add(match(Token_Class.Read));
            Read_State.Children.Add(match(Token_Class.Identfire));
            Read_State.Children.Add(match(Token_Class.Semicolon));
            return Read_State;
        }

        Node Write_Statement()
        {
            Node Write_State = new Node("Write_Statement");
            Write_State.Children.Add(match(Token_Class.Write));
            Write_State.Children.Add(Exp_Endl());
            Write_State.Children.Add(match(Token_Class.Semicolon));
            return Write_State;
        }
        Node Exp_Endl()
        {
            Node Exp_Endl = new Node("Exp_Endl");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_endl = false;
            bool token_string = false;
            bool token_Id = false;
            bool token_Number = false;

            if (less_than_count)
            {
                token_string = TokenStream[InputPointer].token_type == Token_Class.String;
                token_Id = TokenStream[InputPointer].token_type == Token_Class.Identfire;
                token_Number = TokenStream[InputPointer].token_type == Token_Class.Number;
                token_endl = TokenStream[InputPointer].token_type == Token_Class.Endl;
            }
            if(token_endl)
            {
                Exp_Endl.Children.Add(match(Token_Class.Endl));
                return Exp_Endl;
            }
            else if (token_string|| token_Number|| token_Id)
            {
                Exp_Endl.Children.Add(Expression());
                return Exp_Endl;
            }
            Errors.Error_List.Add("Parsing Error: Expected "
            + Token_Class.Identfire + " or " + "Expected "
            + Token_Class.Number + " or " + "Expected "
            + Token_Class.String + " or " + "Expected "
            + Token_Class.Endl +  " and " +

            TokenStream[InputPointer].token_type.ToString() +
            "  found\r\n");
            if (InputPointer + 1 < TokenStream.Count)
                InputPointer++;
            return null;
        }
     
        Node Return_Statement()
        {
            Node Return_State = new Node("Return_Statement");
            Return_State.Children.Add(match(Token_Class.Return));
            Return_State.Children.Add(Expression());
            Return_State.Children.Add(match(Token_Class.Semicolon));
            return Return_State;
        }
        Node Assignment_Statement()
        {
            Node assign_state = new Node("Assignment_Statement");
            assign_state.Children.Add(match(Token_Class.Identfire));
            assign_state.Children.Add(match(Token_Class.AssignmentOP));
            assign_state.Children.Add(Expression());
            assign_state.Children.Add(match(Token_Class.Semicolon));
            return assign_state;
        }

        Node Expression()
        {
            Node Exp = new Node("Expression");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_string = false;
            bool token_Id = false;
            bool token_Number = false;
            bool token_left_paren = false;

            if (less_than_count)
            {
                token_string = TokenStream[InputPointer].token_type == Token_Class.String;
                token_Id = TokenStream[InputPointer].token_type == Token_Class.Identfire;
                token_Number = TokenStream[InputPointer].token_type == Token_Class.Number;
                token_left_paren = TokenStream[InputPointer].token_type == Token_Class.OpenParen;
            }
            if(token_string)
            {
                Exp.Children.Add(match(Token_Class.String));
                return Exp;
            }
            else if(token_Id|| token_Number)
            {
                Exp.Children.Add(Term());
                Exp.Children.Add(Arithmatic_Part());
                return Exp;
            }
            else if(token_left_paren)
            {
                Exp.Children.Add(Equation());
                return Exp;
            }
            Errors.Error_List.Add("Parsing Error: Expected "
            + Token_Class.Identfire + " or " + "Expected "
            + Token_Class.Number + " or " + "Expected "
            + Token_Class.String + " or " + "Expected "
            + Token_Class.CloseParen + " and " +

            TokenStream[InputPointer].token_type.ToString() +
            "  found\r\n");
            if(InputPointer+1<TokenStream.Count)
                 InputPointer++;
            return null;
        }

        Node Equation()
        {
            Node Equ = new Node("Equation");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_Id = false;
            bool token_Number = false;
            bool token_left_paren = false;

            if (less_than_count)
            {
                token_Id = TokenStream[InputPointer].token_type == Token_Class.Identfire;
                token_Number = TokenStream[InputPointer].token_type == Token_Class.Number;
                token_left_paren = TokenStream[InputPointer].token_type == Token_Class.OpenParen;
            }

            if (token_Id || token_Number)
            {
                Equ.Children.Add(Term());
                Equ.Children.Add(Arithmatic_Term());
                return Equ;
            }
            else if (token_left_paren)
            {
                Equ.Children.Add(match(Token_Class.OpenParen));
                Equ.Children.Add(Term());
                Equ.Children.Add(Arithmatic_Term());
                Equ.Children.Add(match(Token_Class.CloseParen));
                Equ.Children.Add(Arithmatic_Terms());
                return Equ;
            }
            Errors.Error_List.Add("Parsing Error: Expected "
           + Token_Class.Identfire + " or " + "Expected "
           + Token_Class.Number + " or " + "Expected "
           + Token_Class.CloseParen + " and " +

           TokenStream[InputPointer].token_type.ToString() +
           "  found\r\n");
            if (InputPointer + 1 < TokenStream.Count)
                InputPointer++;
            return null;
        }

        Node Arithmatic_Part()
        {
            Node Arith_Part = new Node("Arithmatic_Part");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_plus = false;
            bool token_minus = false;
            bool token_multiply = false;
            bool token_divide = false;

            if (less_than_count)
            {
                token_plus = TokenStream[InputPointer].token_type == Token_Class.PlusOp ;
                token_minus = TokenStream[InputPointer].token_type == Token_Class.MinusOp;
                token_multiply = TokenStream[InputPointer].token_type == Token_Class.MultiplyOp;
                token_divide = TokenStream[InputPointer].token_type == Token_Class.DivideOp;
            }
            if(token_plus|| token_minus|| token_multiply|| token_divide)
            {
                Arith_Part.Children.Add(Arithmatic_Term());
                return Arith_Part;
            }

            return null;
        }

        Node Arithmatic_Term()
        {
            Node Arith_Term = new Node("Arithmatic_Term");
     
      
            Arith_Term.Children.Add(Arithmatic_Op());
            Arith_Term.Children.Add(Equation_Part());
            return Arith_Term;
       
        }

        Node Equation_Part()
        {
            Node Equ = new Node("Equation_Part");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_Id = false;
            bool token_Number = false;
            bool token_left_paren = false;

            if (less_than_count)
            {
                token_Id = TokenStream[InputPointer].token_type == Token_Class.Identfire;
                token_Number = TokenStream[InputPointer].token_type == Token_Class.Number;
                token_left_paren = TokenStream[InputPointer].token_type == Token_Class.OpenParen;
            }
     
            if (token_Id || token_Number)
            {
                Equ.Children.Add(Term());
                Equ.Children.Add(Arithmatic_Terms());
                return Equ;
            }
            else if (token_left_paren)
            {
                Equ.Children.Add(Equation());
                return Equ;
            }
            Errors.Error_List.Add("Parsing Error: Expected "
           + Token_Class.Identfire + " or " + "Expected "
           + Token_Class.Number + " or " + "Expected "
           + Token_Class.CloseParen + " and " +

           TokenStream[InputPointer].token_type.ToString() +
           "  found\r\n");
            if (InputPointer + 1 < TokenStream.Count)
                InputPointer++;
            return null; 
        }

        Node Arithmatic_Terms()
        {
            Node Arith_Term = new Node("Arithmatic_Terms");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_plus = false;
            bool token_minus = false;
            bool token_multiply = false;
            bool token_divide = false;

            if (less_than_count)
            {
                token_plus = TokenStream[InputPointer].token_type == Token_Class.PlusOp;
                token_minus = TokenStream[InputPointer].token_type == Token_Class.MinusOp;
                token_multiply = TokenStream[InputPointer].token_type == Token_Class.MultiplyOp;
                token_divide = TokenStream[InputPointer].token_type == Token_Class.DivideOp;
            }
            if (token_plus || token_minus||token_divide||token_multiply)
            {
                Arith_Term.Children.Add(Arithmatic_Term());
                return Arith_Term;
            }

            return null;
        }

        Node Arithmatic_Op()
        {
            Node Arith_Op = new Node("Arithmatic_Op");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_plus = false;
            bool token_minus = false;
            bool token_multiply = false;
            bool token_divide = false;

            if (less_than_count)
            {
                token_plus = TokenStream[InputPointer].token_type == Token_Class.PlusOp;
                token_minus = TokenStream[InputPointer].token_type == Token_Class.MinusOp;
                token_multiply = TokenStream[InputPointer].token_type == Token_Class.MultiplyOp;
                token_divide = TokenStream[InputPointer].token_type == Token_Class.DivideOp;
            }
            if(token_plus)
            {
                Arith_Op.Children.Add(match(Token_Class.PlusOp));
                return Arith_Op;
            }
            else if(token_minus)
            {
                Arith_Op.Children.Add(match(Token_Class.MinusOp));
                return Arith_Op;
            }
            else if (token_multiply)
            {
                Arith_Op.Children.Add(match(Token_Class.MultiplyOp));
                return Arith_Op;
            }
            else if(token_divide)
            {
                Arith_Op.Children.Add(match(Token_Class.DivideOp));
                return Arith_Op;
            }

            Errors.Error_List.Add("Parsing Error: Expected "
                + Token_Class.PlusOp + " or " + "Expected "
                + Token_Class.MinusOp + " or " + "Expected " 
                + Token_Class.DivideOp + " or " + "Expected "
                + Token_Class.MultiplyOp + " and " +
                TokenStream[InputPointer].token_type.ToString() +
                "  found\r\n");
            if (InputPointer + 1 < TokenStream.Count)
                InputPointer++;
            return null;
        }

        Node Term()
        {
            Node Ter = new Node("Term");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_Id = false;
            bool token_Number = false;

            if (less_than_count)
            {
                token_Id = TokenStream[InputPointer].token_type == Token_Class.Identfire;
                token_Number = TokenStream[InputPointer].token_type == Token_Class.Number;
            }
            if(token_Number)
            {
                Ter.Children.Add(match(Token_Class.Number));
                return Ter;
            }
            else if(token_Id)
            {
                Ter.Children.Add(match(Token_Class.Identfire));
                Ter.Children.Add(Fun_call());
                return Ter;
            }
            Errors.Error_List.Add("Parsing Error: Expected "
          + Token_Class.Identfire + " or " + "Expected "
          + Token_Class.Number 
          + " and " +

          TokenStream[InputPointer].token_type.ToString() +
          "  found\r\n");
            if (InputPointer + 1 < TokenStream.Count)
                InputPointer++;
            return null;

        }

        Node Fun_call()
        {
            Node Func_call = new Node("Fun_call");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_left_paren = false;

            if (less_than_count)
            {
                token_left_paren = TokenStream[InputPointer].token_type == Token_Class.OpenParen;
            }
            if (token_left_paren)
            {
                Func_call.Children.Add(match(Token_Class.OpenParen));
                Func_call.Children.Add(ID_LIST());
                Func_call.Children.Add(match(Token_Class.CloseParen));
                return Func_call;
            }
         
            
            return null;
        }

        Node ID_LIST()
        {
            //Node id_list = new Node("ID_LIST");
            //bool less_than_count = InputPointer < TokenStream.Count;
            //bool token_id = false;

            //if (less_than_count)
            //{
            //    token_id = TokenStream[InputPointer].token_type == Token_Class.Identfire;
            //}
            //if (token_id)
            //{
            //    id_list.Children.Add(match(Token_Class.Identfire));
            //    id_list.Children.Add(ID_LISTS());
                
            //    return id_list;
            //}

            //return null;
            Node id_list = new Node("ID_LIST");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_id = false;
            bool token_string = false;
            bool token_Number = false;

            if (less_than_count)
            {
                token_id = TokenStream[InputPointer].token_type == Token_Class.Identfire;
                token_string = TokenStream[InputPointer].token_type == Token_Class.String;
                token_Number = TokenStream[InputPointer].token_type == Token_Class.Number;
            }
            if (token_id)
            {
                id_list.Children.Add(match(Token_Class.Identfire));
                id_list.Children.Add(ID_LISTS());

                return id_list;
            }
            else if (token_string)
            {
                id_list.Children.Add(match(Token_Class.String));
                id_list.Children.Add(ID_LISTS());

                return id_list;
            }
            else if (token_Number)
            {
                id_list.Children.Add(match(Token_Class.Number));
                id_list.Children.Add(ID_LISTS());

                return id_list;
            }
            return null;
        }

        Node ID_LISTS()
        {
            Node id_lists = new Node("ID_LISTS");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_comma = false;

            //if (less_than_count)
            //{
            //    token_comma = TokenStream[InputPointer].token_type == Token_Class.Comma;
            //}
            //if (token_comma)
            //{
            //    id_lists.Children.Add(match(Token_Class.Comma));
            //    id_lists.Children.Add(match(Token_Class.Identfire));
            //    id_lists.Children.Add(ID_LISTS());

            //    return id_lists;
            //}


            //return null;
            bool token_id = false;
            bool token_string = false;
            bool token_Number = false;

            if (less_than_count)
            {
                token_id = TokenStream[InputPointer].token_type == Token_Class.Identfire;
                token_string = TokenStream[InputPointer].token_type == Token_Class.String;
                token_Number = TokenStream[InputPointer].token_type == Token_Class.Number;
                token_comma = TokenStream[InputPointer].token_type == Token_Class.Comma;
            }
            if (token_comma)
            {
                id_lists.Children.Add(match(Token_Class.Comma));
                {
                     less_than_count = InputPointer < TokenStream.Count;
                    if (less_than_count)
                    {
                        token_id = TokenStream[InputPointer].token_type == Token_Class.Identfire;
                        token_string = TokenStream[InputPointer].token_type == Token_Class.String;
                        token_Number = TokenStream[InputPointer].token_type == Token_Class.Number;
                        token_comma = TokenStream[InputPointer].token_type == Token_Class.Comma;
                    }
                }
                if(token_id)
                {
                    id_lists.Children.Add(match(Token_Class.Identfire));
                }
                else if(token_string)
                {
                    id_lists.Children.Add(match(Token_Class.String));
                }
                else if(token_Number)
                {
                    id_lists.Children.Add(match(Token_Class.Number));
                }
                id_lists.Children.Add(ID_LISTS());

                return id_lists;
            }
            return null;
        }

        Node Repeat_Statement()
        {
            Node Repeat = new Node("Repeat_Statement");
            Repeat.Children.Add(match(Token_Class.Repeat));
            Repeat.Children.Add(Statements());
            Repeat.Children.Add(match(Token_Class.Until));
            Repeat.Children.Add(Condition_Statement());

            return Repeat;
        }

        Node Condition_Statement()
        {
            Node Cond_State = new Node("Condition_Statement");
            Cond_State.Children.Add(Condition());
            Cond_State.Children.Add(Bool_Condition());

            return Cond_State;
        }
        Node Condition()
        {
            Node Cond = new Node("Condition");
            Cond.Children.Add(match(Token_Class.Identfire));
            Cond.Children.Add(Condition_Op());
            Cond.Children.Add(Term());

            return Cond;
        }
        Node Condition_Op()
        {
            Node Cond_Op = new Node("Condition_Op");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_less_than = false;
            bool token_greater_than = false; 
            bool token_is_equal = false;
            bool token_not_equal = false;
            if (less_than_count)
            {
                token_less_than = TokenStream[InputPointer].token_type == Token_Class.LessThan;
                token_greater_than = TokenStream[InputPointer].token_type == Token_Class.GreaterThan;
                token_is_equal = TokenStream[InputPointer].token_type == Token_Class.IsEqual;
                token_not_equal = TokenStream[InputPointer].token_type == Token_Class.NotEqual;
            }
            if(token_greater_than) 
            {
                Cond_Op.Children.Add(match(Token_Class.GreaterThan));
                return Cond_Op;
            }
            else if(token_is_equal) 
            {
                Cond_Op.Children.Add(match(Token_Class.IsEqual));
                return Cond_Op;
            }
            else if(token_less_than) 
            {
                Cond_Op.Children.Add(match(Token_Class.LessThan));
                return Cond_Op;
            }
            else if(token_not_equal) 
            {
                Cond_Op.Children.Add(match(Token_Class.NotEqual));
                return Cond_Op;
            }

            Errors.Error_List.Add("Parsing Error: Expected "
             + Token_Class.LessThan + " or " + "Expected "
             + Token_Class.GreaterThan + " or " + "Expected "
             + Token_Class.IsEqual + " or " + "Expected "
             + Token_Class.NotEqual + " and " +

             TokenStream[InputPointer].token_type.ToString() +
             "  found\r\n");
                if (InputPointer + 1 < TokenStream.Count)
                    InputPointer++;

            return null;
        }
        Node Bool_Condition()
        {
            Node Bool_Cond = new Node("Bool_Condition");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_and = false;
            bool token_or = false;
          
            if (less_than_count)
            {
                token_and = TokenStream[InputPointer].token_type == Token_Class.And;
                token_or = TokenStream[InputPointer].token_type == Token_Class.Or;
            }
            if (token_and || token_or)
            {
                Bool_Cond.Children.Add(Boolean_Op());
                Bool_Cond.Children.Add(Condition());
                Bool_Cond.Children.Add(Bool_Condition());

                return Bool_Cond;

            }
            return null;
        }
        Node Boolean_Op()
        {
            Node Bool_op = new Node("Boolean_Operator");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_and = false;
            bool token_or = false;

            if (less_than_count)
            {
                token_and = TokenStream[InputPointer].token_type == Token_Class.And;
                token_or = TokenStream[InputPointer].token_type == Token_Class.Or;
            }
            if (token_and )
            {
                Bool_op.Children.Add(match(Token_Class.And));
                return Bool_op;

            }
            else if(token_or)
            {
                Bool_op.Children.Add(match(Token_Class.Or));
                return Bool_op;
            }
            Errors.Error_List.Add("Parsing Error: Expected "
              + Token_Class.And+"Operator " + " or " + "Expected "
              + Token_Class.Or +"Operator" +" and " +
              TokenStream[InputPointer].token_type.ToString() +
              "  found\r\n");
            if (InputPointer + 1 < TokenStream.Count)
                InputPointer++;

            return null;
        }
        Node If_Statement()
        {
            Node If_State = new Node("If_Statement");
            If_State.Children.Add(match(Token_Class.If));
            If_State.Children.Add(Condition_Statement());
            If_State.Children.Add(match(Token_Class.Then));
            If_State.Children.Add(Statements());
            If_State.Children.Add(Close());

            return If_State;
        }
        Node Close()
        {
            Node Close_If = new Node("Close");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_else = false;
            bool token_else_If = false;
            bool token_end = false;
            if (less_than_count)
            {
                token_else = TokenStream[InputPointer].token_type == Token_Class.Else;
                token_else_If = TokenStream[InputPointer].token_type == Token_Class.Elseif;
                token_end = TokenStream[InputPointer].token_type == Token_Class.End;
            }
            if(token_else_If)
            {
                Close_If.Children.Add(Else_If_Statement());
                return Close_If;
            }
            else if(token_end)
            {
                Close_If.Children.Add(match(Token_Class.End));
                return Close_If;
            }
            else if (token_else)
            {
                Close_If.Children.Add(Else_Statement());
                return Close_If;
            }

            Errors.Error_List.Add("Parsing Error: Expected "
              + Token_Class.Elseif   + " or " + "Expected "
             + Token_Class.End + " or " + "Expected "
              + Token_Class.Else +  " and " +
              TokenStream[InputPointer].token_type.ToString() +
              "  found\r\n");
            if (InputPointer + 1 < TokenStream.Count)
                InputPointer++;

            return null;
        }
        Node Else_If_Statement()
        {
            Node Else_if_State = new Node("Else_If_Statement");
            Else_if_State.Children.Add(match(Token_Class.Elseif));
            Else_if_State.Children.Add(Condition_Statement());
            Else_if_State.Children.Add(match(Token_Class.Then));
            Else_if_State.Children.Add(Statements());
            Else_if_State.Children.Add(Close());

            return Else_if_State;
        }
        Node Else_Statement()
        {
            Node Else_State = new Node("Else_Statement");
            Else_State.Children.Add(match(Token_Class.Else));
            Else_State.Children.Add(Statements());
            Else_State.Children.Add(match(Token_Class.End));
            return Else_State;
        }
        Node Declaration_Statement()
        {
            Node Decl_State = new Node("Declaration_Statement");
            Decl_State.Children.Add(Data_Type());
            Decl_State.Children.Add(Decl());
            Decl_State.Children.Add(Decl_LIST());
            Decl_State.Children.Add(match(Token_Class.Semicolon));
            return Decl_State;
        }
        Node Decl()
        {
            Node Decl_ = new Node("Decl");

            Decl_.Children.Add(match(Token_Class.Identfire));
            Decl_.Children.Add(Assign());
            return Decl_;
        }
        Node Assign()
        {
            Node Assignment = new Node("Assign");
            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_assign = false;
           

            if (less_than_count)
            {
                token_assign = TokenStream[InputPointer].token_type == Token_Class.AssignmentOP;
               
            }
            if (token_assign)
            {
                Assignment.Children.Add(match(Token_Class.AssignmentOP));
                Assignment.Children.Add(Expression());
                return Assignment;
            }
          
         
            return null;
        }
        Node Decl_LIST()
        {
            Node Decl_List = new Node("Decl_LIST");

            bool less_than_count = InputPointer < TokenStream.Count;
            bool token_comma = false;


            if (less_than_count)
            {
                token_comma = TokenStream[InputPointer].token_type == Token_Class.Comma;

            }
            if (token_comma)
            {
                Decl_List.Children.Add(match(Token_Class.Comma));
                Decl_List.Children.Add(match(Token_Class.Identfire));
                Decl_List.Children.Add(Assign());
                Decl_List.Children.Add(Decl_LIST());
                return Decl_List;
            }

            return null;
        }
        


        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());

                    return newNode;
                }
                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString()  + "\r\n");
                InputPointer++;
                return null;
            }
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
