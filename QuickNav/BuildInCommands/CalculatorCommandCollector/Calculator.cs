using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Calculator // DO NOT CHANGE ANYTHING!!!
{
    public static class Settings
    {
        public static bool InsertLostTimesSymbols = true;
        public static bool Radians = true;
    }

    public interface IFormula
    {
        double Calc();
        string ToString();
        void OverwriteVariable(char name, IFormula value);
        void ReplaceVariable(char name, IFormula value);
        bool ContainsVariable(char varName);
        IFormula Clone();
    }

    public interface IFunction : IFormula
    {
        bool TryInit(string name, IFormula[] args);
    }

    public interface IOperator : IFormula
    {
        char OperatorName { get; }
        IFormula Formula1 { get; set; }
        IFormula Formula2 { get; set; }
        Priority Priority { get; }
        bool TryInit(char Operator, IFormula formula1, IFormula formula2);
    }

    public interface IBracket : IFormula
    {
        char OpenSymbol { get; }
        char CloseSymbol { get; }
        bool TryInit(char open, char close, IFormula formula);
    }

    public enum Priority
    {
        Dash,
        Dot
    }

    public class FNull : IFormula
    {
        public bool ContainsVariable(string varName)
        {
            throw new NotImplementedException("Method not implemented.");
        }

        public double Calc()
        {
            throw new NotImplementedException("Method not implemented.");
        }

        public override string ToString()
        {
            throw new NotImplementedException("Method not implemented.");
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            throw new NotImplementedException("Method not implemented.");
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            throw new NotImplementedException("Method not implemented.");
        }

        public IFormula Clone()
        {
            throw new NotImplementedException("Method not implemented.");
        }

        public bool ContainsVariable(char varName)
        {
            throw new NotImplementedException();
        }
    }

    public class FNumber : IFormula
    {
        public double Num { get; set; } = 0;

        public double Calc()
        {
            return Num;
        }

        public override string ToString()
        {
            return "(" + Num + ")";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            // No implementation needed for this method in FNumber class
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            // No implementation needed for this method in FNumber class
        }

        public IFormula Clone()
        {
            var fnum = new FNumber();
            fnum.Num = this.Num;
            return fnum;
        }

        public bool ContainsVariable(char varName)
        {
            return false;
        }
    }

    public class FVariable : IFormula
    {
        private IFormula _content = new FNumber();
        public char Symbol { get; set; } = 'x';

        public double Calc()
        {
            return _content.Calc();
        }

        public override string ToString()
        {
            return "(" + Symbol + ")";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            if ((name + "").ToLower() == (Symbol + "").ToLower())
                _content = value;
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (_content is FVariable fvar && (fvar.Symbol + "").ToLower() == (name + "").ToLower())
                _content = value;
        }

        public IFormula Clone()
        {
            var fvar = new FVariable();
            fvar.Symbol = Symbol;
            fvar._content = _content.Clone();
            return fvar;
        }

        public bool ContainsVariable(char varName)
        {
            return (Symbol + "").ToLower() == (varName + "").ToLower();
        }
    }

    public class FSubtraction : IOperator
    {
        public char OperatorName { get; } = '-';
        public IFormula Formula1 { get; set; } = new FNumber();
        public IFormula Formula2 { get; set; } = new FNumber();
        public Priority Priority { get; } = Priority.Dash;

        public bool TryInit(char Operator, IFormula formula1, IFormula formula2)
        {
            if (OperatorName == Operator)
            {
                Formula1 = formula1;
                Formula2 = formula2;
                return true;
            }
            return false;
        }

        public double Calc()
        {
            return Formula1.Calc() - Formula2.Calc();
        }

        public override string ToString()
        {
            return "(" + Formula1.ToString() + OperatorName + Formula2.ToString() + ")";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            Formula1.OverwriteVariable(name, value);
            Formula2.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (Formula1 is FVariable fvar1 && (fvar1.Symbol + "").ToLower() == (name + "").ToLower())
                Formula1 = value;
            if (Formula2 is FVariable fvar2 && (fvar2.Symbol + "").ToLower() == (name + "").ToLower())
                Formula2 = value;
        }

        public IFormula Clone()
        {
            var fsub = new FSubtraction();
            fsub.Formula1 = Formula1.Clone();
            fsub.Formula2 = Formula2.Clone();
            return fsub;
        }

        public bool ContainsVariable(char varName)
        {
            return Formula1.ContainsVariable(varName) || Formula2.ContainsVariable(varName);
        }
    }

    public class FAddition : IOperator
    {
        public char OperatorName { get; } = '+';
        public IFormula Formula1 { get; set; } = new FNumber();
        public IFormula Formula2 { get; set; } = new FNumber();
        public Priority Priority { get; } = Priority.Dash;

        public bool TryInit(char Operator, IFormula formula1, IFormula formula2)
        {
            if (OperatorName == Operator)
            {
                Formula1 = formula1;
                Formula2 = formula2;
                return true;
            }
            return false;
        }

        public double Calc()
        {
            return Formula1.Calc() + Formula2.Calc();
        }

        public override string ToString()
        {
            return "(" + Formula1.ToString() + OperatorName + Formula2.ToString() + ")";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            Formula1.OverwriteVariable(name, value);
            Formula2.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (Formula1 is FVariable fvar1 && (fvar1.Symbol + "").ToLower() == (name + "").ToLower())
                Formula1 = value;
            if (Formula2 is FVariable fvar2 && (fvar2.Symbol + "").ToLower() == (name + "").ToLower())
                Formula2 = value;
        }

        public IFormula Clone()
        {
            var fadd = new FAddition();
            fadd.Formula1 = Formula1.Clone();
            fadd.Formula2 = Formula2.Clone();
            return fadd;
        }

        public bool ContainsVariable(char varName)
        {
            return Formula1.ContainsVariable(varName) || Formula2.ContainsVariable(varName);
        }
    }

    public class FMultiplication : IOperator
    {
        public char OperatorName { get; } = '*';
        public IFormula Formula1 { get; set; } = new FNumber();
        public IFormula Formula2 { get; set; } = new FNumber();
        public Priority Priority { get; } = Priority.Dot;

        public bool TryInit(char Operator, IFormula formula1, IFormula formula2)
        {
            if (OperatorName == Operator)
            {
                Formula1 = formula1;
                Formula2 = formula2;
                return true;
            }
            return false;
        }

        public double Calc()
        {
            return Formula1.Calc() * Formula2.Calc();
        }

        public override string ToString()
        {
            return "(" + Formula1.ToString() + OperatorName + Formula2.ToString() + ")";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            Formula1.OverwriteVariable(name, value);
            Formula2.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (Formula1 is FVariable fvar1 && (fvar1.Symbol + "").ToLower() == (name + "").ToLower())
                Formula1 = value;
            if (Formula2 is FVariable fvar2 && (fvar2.Symbol + "").ToLower() == (name + "").ToLower())
                Formula2 = value;
        }

        public IFormula Clone()
        {
            var fmul = new FMultiplication();
            fmul.Formula1 = Formula1.Clone();
            fmul.Formula2 = Formula2.Clone();
            return fmul;
        }

        public bool ContainsVariable(char varName)
        {
            return Formula1.ContainsVariable(varName) || Formula2.ContainsVariable(varName);
        }
    }

    public class FDivision : IOperator
    {
        public char OperatorName { get; } = '/';
        public IFormula Formula1 { get; set; } = new FNumber();
        public IFormula Formula2 { get; set; } = new FNumber();
        public Priority Priority { get; } = Priority.Dot;

        public bool TryInit(char Operator, IFormula formula1, IFormula formula2)
        {
            if (OperatorName == Operator)
            {
                Formula1 = formula1;
                Formula2 = formula2;
                return true;
            }
            return false;
        }

        public double Calc()
        {
            return Formula1.Calc() / Formula2.Calc();
        }

        public override string ToString()
        {
            return "(" + Formula1.ToString() + OperatorName + Formula2.ToString() + ")";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            Formula1.OverwriteVariable(name, value);
            Formula2.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (Formula1 is FVariable fvar1 && (fvar1.Symbol + "").ToLower() == (name + "").ToLower())
                Formula1 = value;
            if (Formula2 is FVariable fvar2 && (fvar2.Symbol + "").ToLower() == (name + "").ToLower())
                Formula2 = value;
        }

        public IFormula Clone()
        {
            var fdiv = new FDivision();
            fdiv.Formula1 = Formula1.Clone();
            fdiv.Formula2 = Formula2.Clone();
            return fdiv;
        }

        public bool ContainsVariable(char varName)
        {
            return Formula1.ContainsVariable(varName) || Formula2.ContainsVariable(varName);
        }
    }

    public class FModulo : IOperator
    {
        public char OperatorName { get; } = '%';
        public IFormula Formula1 { get; set; } = new FNumber();
        public IFormula Formula2 { get; set; } = new FNumber();
        public Priority Priority { get; } = Priority.Dot;

        public bool TryInit(char Operator, IFormula formula1, IFormula formula2)
        {
            if (OperatorName == Operator)
            {
                Formula1 = formula1;
                Formula2 = formula2;
                return true;
            }
            return false;
        }

        public double Calc()
        {
            return Formula1.Calc() % Formula2.Calc();
        }

        public override string ToString()
        {
            return "(" + Formula1.ToString() + OperatorName + Formula2.ToString() + ")";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            Formula1.OverwriteVariable(name, value);
            Formula2.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (Formula1 is FVariable fvar1 && (fvar1.Symbol + "").ToLower() == (name + "").ToLower())
                Formula1 = value;
            if (Formula2 is FVariable fvar2 && (fvar2.Symbol + "").ToLower() == (name + "").ToLower())
                Formula2 = value;
        }

        public IFormula Clone()
        {
            var fmod = new FModulo();
            fmod.Formula1 = Formula1.Clone();
            fmod.Formula2 = Formula2.Clone();
            return fmod;
        }

        public bool ContainsVariable(char varName)
        {
            return Formula1.ContainsVariable(varName) || Formula2.ContainsVariable(varName);
        }
    }

    public class FExponentiation : IOperator
    {
        public char OperatorName { get; } = '^';
        public IFormula Formula1 { get; set; } = new FNumber();
        public IFormula Formula2 { get; set; } = new FNumber();
        public Priority Priority { get; } = Priority.Dot;

        public bool TryInit(char Operator, IFormula formula1, IFormula formula2)
        {
            if (OperatorName == Operator)
            {
                Formula1 = formula1;
                Formula2 = formula2;
                return true;
            }
            return false;
        }

        public double Calc()
        {
            return Math.Pow(Formula1.Calc(), Formula2.Calc());
        }

        public override string ToString()
        {
            return "(" + Formula1.ToString() + OperatorName + Formula2.ToString() + ")";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            Formula1.OverwriteVariable(name, value);
            Formula2.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (Formula1 is FVariable fvar1 && (fvar1.Symbol + "").ToLower() == (name + "").ToLower())
                Formula1 = value;
            if (Formula2 is FVariable fvar2 && (fvar2.Symbol + "").ToLower() == (name + "").ToLower())
                Formula2 = value;
        }

        public IFormula Clone()
        {
            var fexp = new FExponentiation();
            fexp.Formula1 = Formula1.Clone();
            fexp.Formula2 = Formula2.Clone();
            return fexp;
        }

        public bool ContainsVariable(char varName)
        {
            return Formula1.ContainsVariable(varName) || Formula2.ContainsVariable(varName);
        }
    }

    public class FSum : IFunction
    {
        public IFormula From { get; set; } = new FNumber();
        public IFormula To { get; set; } = new FNumber();
        public IFormula Formula { get; set; } = new FNumber();
        public char Symbol { get; set; } = 'x';

        public bool TryInit(string name, IFormula[] args)
        {
            if (name.ToLower() != "sum") return false;
            if (args.Length != 4) return false;
            if (!(args[0] is FVariable)) return false;

            Symbol = (args[0] as FVariable).Symbol;
            From = args[1];
            To = args[2];
            Formula = args[3];
            return true;
        }

        public double Calc()
        {
            double res = 0;
            for (double counter = Math.Floor(From.Calc()); counter <= Math.Floor(To.Calc()); counter++)
            {
                var fnum = new FNumber();
                fnum.Num = counter;
                Formula.OverwriteVariable(Symbol, fnum);
                res += Formula.Calc();
            }
            return res;
        }

        public override string ToString()
        {
            return $"sum({Symbol}, {From.ToString()}, {To.ToString()}, {Formula.ToString()})";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            From.OverwriteVariable(name, value);
            To.OverwriteVariable(name, value);
            Formula.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (From is FVariable fvar1 && (fvar1.Symbol + "").ToLower() == (name + "").ToLower())
                From = value;
            if (To is FVariable fvar2 && (fvar2.Symbol + "").ToLower() == (name + "").ToLower())
                To = value;
            if (Formula is FVariable fvar3 && (fvar3.Symbol + "").ToLower() == (name + "").ToLower())
                Formula = value;
        }

        public IFormula Clone()
        {
            var fsum = new FSum();
            fsum.Symbol = Symbol;
            fsum.From = From.Clone();
            fsum.To = To.Clone();
            fsum.Formula = Formula.Clone();
            return fsum;
        }

        public bool ContainsVariable(char varName)
        {
            return From.ContainsVariable(varName) || To.ContainsVariable(varName) || Formula.ContainsVariable(varName);
        }
    }

    public class FProduct : IFunction
    {
        public IFormula From { get; set; } = new FNumber();
        public IFormula To { get; set; } = new FNumber();
        public IFormula Formula { get; set; } = new FNumber();
        public char Symbol { get; set; } = 'x';

        public bool TryInit(string name, IFormula[] args)
        {
            if (name.ToLower() != "product") return false;
            if (args.Length != 4) return false;
            if (!(args[0] is FVariable)) return false;

            Symbol = (args[0] as FVariable).Symbol;
            From = args[1];
            To = args[2];
            Formula = args[3];
            return true;
        }

        public double Calc()
        {
            double res = 1;
            for (double counter = Math.Floor(From.Calc()); counter <= Math.Floor(To.Calc()); counter++)
            {
                var fnum = new FNumber();
                fnum.Num = counter;
                Formula.OverwriteVariable(Symbol, fnum);
                res *= Formula.Calc();
            }
            return res;
        }

        public override string ToString()
        {
            return $"product({Symbol}, {From.ToString()}, {To.ToString()}, {Formula.ToString()})";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            From.OverwriteVariable(name, value);
            To.OverwriteVariable(name, value);
            Formula.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (From is FVariable fvar1 && (fvar1.Symbol + "").ToLower() == (name + "").ToLower())
                From = value;
            if (To is FVariable fvar2 && (fvar2.Symbol + "").ToLower() == (name + "").ToLower())
                To = value;
            if (Formula is FVariable fvar3 && (fvar3.Symbol + "").ToLower() == (name + "").ToLower())
                Formula = value;
        }

        public IFormula Clone()
        {
            var fpro = new FProduct();
            fpro.Symbol = Symbol;
            fpro.From = From.Clone();
            fpro.To = To.Clone();
            fpro.Formula = Formula.Clone();
            return fpro;
        }

        public bool ContainsVariable(char varName)
        {
            return From.ContainsVariable(varName) || To.ContainsVariable(varName) || Formula.ContainsVariable(varName);
        }
    }

    public class FLogarithm : IFunction
    {
        public IFormula Base { get; set; } = new FNumber();
        public IFormula Num { get; set; } = new FNumber();

        public bool TryInit(string name, IFormula[] args)
        {
            if (name != "log") return false;
            if (args.Length != 2) return false;

            Base = args[0];
            Num = args[1];
            return true;
        }

        public double Calc()
        {
            return Math.Log(Num.Calc()) / Math.Log(Base.Calc());
        }

        public override string ToString()
        {
            return $"log({Base}, {Num})";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            Base.OverwriteVariable(name, value);
            Num.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (Base is FVariable fvar1 && (fvar1.Symbol + "").ToLower() == (name + "").ToLower())
                Base = value;
            if (Num is FVariable fvar2 && (fvar2.Symbol + "").ToLower() == (name + "").ToLower())
                Num = value;
        }

        public IFormula Clone()
        {
            var flog = new FLogarithm();
            flog.Base = Base.Clone();
            flog.Num = Num.Clone();
            return flog;
        }

        public bool ContainsVariable(char varName)
        {
            return Base.ContainsVariable(varName) || Num.ContainsVariable(varName);
        }
    }

    public class FAbsolute : IFunction
    {
        public IFormula Value { get; set; } = new FNumber();

        public bool TryInit(string name, IFormula[] args)
        {
            if (name != "abs") return false;
            if (args.Length != 1) return false;

            Value = args[0];
            return true;
        }

        public double Calc()
        {
            return Math.Abs(Value.Calc());
        }

        public override string ToString()
        {
            return $"abs({Value})";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            Value.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (Value is FVariable fvar && (fvar.Symbol + "").ToLower() == (name + "").ToLower())
                Value = value;
        }

        public IFormula Clone()
        {
            var fabs = new FAbsolute();
            fabs.Value = Value.Clone();
            return fabs;
        }

        public IFormula Derive(string varName)
        {
            return new FNull();
        }

        public bool ContainsVariable(char varName)
        {
            return Value.ContainsVariable(varName);
        }
    }

    public class FSine : IFunction
    {
        public IFormula Formula { get; set; } = new FNumber();

        public bool TryInit(string name, IFormula[] args)
        {
            if (name.ToLower() != "sin") return false;
            if (args.Length != 1) return false;

            Formula = args[0];
            return true;
        }

        public double Calc()
        {
            if (Settings.Radians)
                return Math.Sin(Formula.Calc());
            else
                return Math.Sin(Formula.Calc() * Math.PI / 180);
        }

        public override string ToString()
        {
            return $"sin({Formula})";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            Formula.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (Formula is FVariable fvar && (fvar.Symbol + "").ToLower() == (name + "").ToLower())
                Formula = value;
        }

        public IFormula Clone()
        {
            var fsin = new FSine();
            fsin.Formula = Formula.Clone();
            return fsin;
        }

        public bool ContainsVariable(char varName)
        {
            return Formula.ContainsVariable(varName);
        }
    }

    public class FCosine : IFunction
    {
        public IFormula Formula { get; set; } = new FNumber();

        public bool TryInit(string name, IFormula[] args)
        {
            if (name.ToLower() != "cos") return false;
            if (args.Length != 1) return false;

            Formula = args[0];
            return true;
        }

        public double Calc()
        {
            if (Settings.Radians)
                return Math.Cos(Formula.Calc());
            else
                return Math.Cos(Formula.Calc() * Math.PI / 180);
        }

        public override string ToString()
        {
            return $"cos({Formula})";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            Formula.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (Formula is FVariable fvar && (fvar.Symbol + "").ToLower() == (name + "").ToLower())
                Formula = value;
        }

        public IFormula Clone()
        {
            var fcos = new FCosine();
            fcos.Formula = Formula.Clone();
            return fcos;
        }

        public bool ContainsVariable(char varName)
        {
            return Formula.ContainsVariable(varName);
        }
    }

    public class FTangent : IFunction
    {
        public IFormula Formula { get; set; } = new FNumber();

        public bool TryInit(string name, IFormula[] args)
        {
            if (name.ToLower() != "tan") return false;
            if (args.Length != 1) return false;

            Formula = args[0];
            return true;
        }

        public double Calc()
        {
            if (Settings.Radians)
                return Math.Tan(Formula.Calc());
            else
                return Math.Tan(Formula.Calc() * Math.PI / 180);
        }

        public override string ToString()
        {
            return $"tan({Formula})";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            Formula.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (Formula is FVariable fvar && (fvar.Symbol + "").ToLower() == (name + "").ToLower())
                Formula = value;
        }

        public IFormula Clone()
        {
            var ftan = new FTangent();
            ftan.Formula = Formula.Clone();
            return ftan;
        }

        public bool ContainsVariable(char varName)
        {
            return Formula.ContainsVariable(varName);
        }
    }

    public class FAsine : IFunction
    {
        public IFormula Formula { get; set; } = new FNumber();

        public bool TryInit(string name, IFormula[] args)
        {
            if (name.ToLower() != "asin") return false;
            if (args.Length != 1) return false;

            Formula = args[0];
            return true;
        }

        public double Calc()
        {
            if (Settings.Radians)
                return Math.Asin(Formula.Calc());
            else
                return Math.Asin(Formula.Calc()) * 180 / Math.PI;
        }

        public override string ToString()
        {
            return $"asin({Formula})";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            Formula.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (Formula is FVariable fvar && (fvar.Symbol + "").ToLower() == (name + "").ToLower())
                Formula = value;
        }

        public IFormula Clone()
        {
            var fasin = new FAsine();
            fasin.Formula = Formula.Clone();
            return fasin;
        }

        public bool ContainsVariable(char varName)
        {
            return Formula.ContainsVariable(varName);
        }
    }

    public class FAcosine : IFunction
    {
        public IFormula Formula { get; set; } = new FNumber();

        public bool TryInit(string name, IFormula[] args)
        {
            if (name.ToLower() != "acos") return false;
            if (args.Length != 1) return false;

            Formula = args[0];
            return true;
        }

        public double Calc()
        {
            if (Settings.Radians)
                return Math.Acos(Formula.Calc());
            else
                return Math.Acos(Formula.Calc()) * 180 / Math.PI;
        }

        public override string ToString()
        {
            return $"acos({Formula})";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            Formula.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (Formula is FVariable fvar && (fvar.Symbol + "").ToLower() == (name + "").ToLower())
                Formula = value;
        }

        public IFormula Clone()
        {
            var facos = new FAcosine();
            facos.Formula = Formula.Clone();
            return facos;
        }

        public bool ContainsVariable(char varName)
        {
            return Formula.ContainsVariable(varName);
        }
    }

    public class FAtangent : IFunction
    {
        public IFormula Formula { get; set; } = new FNumber();

        public bool TryInit(string name, IFormula[] args)
        {
            if (name.ToLower() != "atan") return false;
            if (args.Length != 1) return false;

            Formula = args[0];
            return true;
        }

        public double Calc()
        {
            if (Settings.Radians)
                return Math.Atan(Formula.Calc());
            else
                return Math.Atan(Formula.Calc()) * 180 / Math.PI;
        }

        public override string ToString()
        {
            return $"atan({Formula})";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            Formula.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (Formula is FVariable fvar && (fvar.Symbol + "").ToLower() == (name + "").ToLower())
                Formula = value;
        }

        public IFormula Clone()
        {
            var fatan = new FAtangent();
            fatan.Formula = Formula.Clone();
            return fatan;
        }

        public bool ContainsVariable(char varName)
        {
            return Formula.ContainsVariable(varName);
        }
    }

    public class FSquareRoot : IFunction
    {
        public IFormula Formula { get; set; } = new FNumber();

        public bool TryInit(string name, IFormula[] args)
        {
            if (name.ToLower() != "sqrt") return false;
            if (args.Length != 1) return false;

            Formula = args[0];
            return true;
        }

        public double Calc()
        {
            return Math.Sqrt(Formula.Calc());
        }

        public override string ToString()
        {
            return $"sqrt({Formula})";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            Formula.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (Formula is FVariable fvar && (fvar.Symbol + "").ToLower() == (name + "").ToLower())
                Formula = value;
        }

        public IFormula Clone()
        {
            var fsqrt = new FSquareRoot();
            fsqrt.Formula = Formula.Clone();
            return fsqrt;
        }

        public bool ContainsVariable(char varName)
        {
            return Formula.ContainsVariable(varName);
        }
    }

    public class FSquareRootX : IFunction
    {
        public IFormula Base { get; set; } = new FNumber();
        public IFormula Formula { get; set; } = new FNumber();

        public bool TryInit(string name, IFormula[] args)
        {
            if (name.ToLower() != "sqrt") return false;
            if (args.Length != 2) return false;

            Base = args[0];
            Formula = args[1];
            return true;
        }

        public double Calc()
        {
            return Math.Pow(Formula.Calc(), 1 / Base.Calc());
        }

        public override string ToString()
        {
            return $"sqrt({Base}, {Formula})";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            Formula.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (Formula is FVariable fvar && (fvar.Symbol + "").ToLower() == (name + "").ToLower())
                Formula = value;
        }

        public IFormula Clone()
        {
            var fsqrt = new FSquareRootX();
            fsqrt.Formula = Formula.Clone();
            return fsqrt;
        }

        public bool ContainsVariable(char varName)
        {
            return Formula.ContainsVariable(varName) || Base.ContainsVariable(varName);
        }
    }

    public class FParentheses : IBracket
    {
        public char OpenSymbol { get; } = '(';
        public char CloseSymbol { get; } = ')';
        public IFormula Formula { get; set; } = new FNumber();

        public bool TryInit(char open, char close, IFormula formula)
        {
            if (open != OpenSymbol) return false;
            if (close != CloseSymbol) return false;
            Formula = formula;
            return true;
        }

        public double Calc()
        {
            return Formula.Calc();
        }

        public override string ToString()
        {
            return "(" + Formula.ToString() + ")";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            Formula.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (Formula is FVariable fvar && (fvar.Symbol + "").ToLower() == (name + "").ToLower())
                Formula = value;
        }

        public IFormula Clone()
        {
            var fpar = new FParentheses();
            fpar.Formula = Formula.Clone();
            return fpar;
        }

        public bool ContainsVariable(char varName)
        {
            return Formula.ContainsVariable(varName);
        }
    }

    public class FRound : IBracket
    {
        public char OpenSymbol { get; } = '[';
        public char CloseSymbol { get; } = ']';
        public IFormula Formula { get; set; } = new FNumber();

        public bool TryInit(char open, char close, IFormula formula)
        {
            if (open != OpenSymbol) return false;
            if (close != CloseSymbol) return false;
            Formula = formula;
            return true;
        }

        public double Calc()
        {
            return Math.Round(Formula.Calc());
        }

        public override string ToString()
        {
            return "[" + Formula.ToString() + "]";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            Formula.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (Formula is FVariable fvar && (fvar.Symbol + "").ToLower() == (name + "").ToLower())
                Formula = value;
        }

        public IFormula Clone()
        {
            var frou = new FRound();
            frou.Formula = Formula.Clone();
            return frou;
        }

        public bool ContainsVariable(char varName)
        {
            return Formula.ContainsVariable(varName);
        }
    }

    public class FRoundUp : IBracket
    {
        public char OpenSymbol { get; } = '⌈';
        public char CloseSymbol { get; } = '⌉';
        public IFormula Formula { get; set; } = new FNumber();

        public bool TryInit(char open, char close, IFormula formula)
        {
            if (open != OpenSymbol) return false;
            if (close != CloseSymbol) return false;
            Formula = formula;
            return true;
        }

        public double Calc()
        {
            return Math.Round(Formula.Calc() + 0.5);
        }

        public override string ToString()
        {
            return "⌈" + Formula.ToString() + "⌉";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            Formula.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (Formula is FVariable fvar && (fvar.Symbol + "").ToLower() == (name + "").ToLower())
                Formula = value;
        }

        public IFormula Clone()
        {
            var frouu = new FRoundUp();
            frouu.Formula = Formula.Clone();
            return frouu;
        }

        public bool ContainsVariable(char varName)
        {
            return Formula.ContainsVariable(varName);
        }
    }

    public class FRoundDown : IBracket
    {
        public char OpenSymbol { get; } = '⌊';
        public char CloseSymbol { get; } = '⌋';
        public IFormula Formula { get; set; } = new FNumber();

        public bool TryInit(char open, char close, IFormula formula)
        {
            if (open != OpenSymbol) return false;
            if (close != CloseSymbol) return false;
            Formula = formula;
            return true;
        }

        public double Calc()
        {
            return Math.Round(Formula.Calc() - 0.5);
        }

        public override string ToString()
        {
            return "⌊" + Formula.ToString() + "⌋";
        }

        public void OverwriteVariable(char name, IFormula value)
        {
            Formula.OverwriteVariable(name, value);
        }

        public void ReplaceVariable(char name, IFormula value)
        {
            if (Formula is FVariable fvar && (fvar.Symbol + "").ToLower() == (name + "").ToLower())
                Formula = value;
        }

        public IFormula Clone()
        {
            var froud = new FRoundDown();
            froud.Formula = Formula.Clone();
            return froud;
        }

        public bool ContainsVariable(char varName)
        {
            return Formula.ContainsVariable(varName);
        }
    }

    public static class Registry
    {
        public static List<IFunction> Functions = new List<IFunction>
        {
            new FSum(),
            new FProduct(),
            new FLogarithm(),
            new FAbsolute(),
            new FSine(),
            new FCosine(),
            new FTangent(),
            new FAsine(),
            new FAcosine(),
            new FAtangent(),
            new FSquareRoot(),
            new FSquareRootX(),
        };

        public static List<IOperator> Operators = new List<IOperator>
        {
            new FSubtraction(),
            new FAddition(),
            new FMultiplication(),
            new FDivision(),
            new FModulo(),
            new FExponentiation(),
        };

        public static List<IBracket> Brackets = new List<IBracket>
        {
            new FParentheses(),
            new FRound(),
            new FRoundUp(),
            new FRoundDown(),
        };
    }

    public static class Parser
    {
        public static IFormula Parse(string formula)
        {
            try
            {
                string abc = "abcdefghijklmnopqrstuvwxyz";
                string nums = "1234567890";

                formula = formula.ToLower();
                formula = formula.Replace(" ", "");
                formula = formula.Replace("\r", "").Replace("\n", "").Replace("\t", "");
                formula = formula.Replace("π", "(" + Math.PI + ")").Replace("\\pi", "(" + Math.PI + ")");
                formula = formula.Replace("\\e", "(" + Math.E + ")");
                formula = formula.Replace("\\", "/");
                formula = formula.Replace("÷", "/").Replace("×", "*");
                Random r = new Random();
                int index = formula.IndexOf('?');
                while (index != -1)
                {
                    formula = formula.Remove(index, 1);
                    formula = formula.Insert(index, "(" + r.NextDouble() + ")");
                    index = formula.IndexOf('?');
                }

                if (formula.StartsWith("-"))
                {
                    formula = "0" + formula;
                }

                string opStr = Registry.Operators.Select(op => op.OperatorName + "").Aggregate((acc, op) => acc + op);
                string brOpen = Registry.Brackets.Select(br => br.OpenSymbol + "").Aggregate((acc, br) => acc + br);
                string brClose = Registry.Brackets.Select(br => br.CloseSymbol + "").Aggregate((acc, br) => acc + br);

                int bracket = 0;
                bool readingFunc = false;
                bool firstParam = true;
                int lastCharIndex = -2;
                int lastNumberIndex = -2;
                string currentNumber = "";
                string currentName = "";
                List<object> objs = new List<object>();

                for (int i = 0; i < formula.Length; i++)
                {
                    char cc = formula[i];

                    if (readingFunc)
                    {
                        if (abc.Contains(cc))
                        {
                            currentName += cc;
                        }
                        if (cc == '(')
                        {
                            int bracketOld = bracket + 1;
                            string subFormula = "";
                            List<IFormula> args = new List<IFormula>();

                            do
                            {
                                if (cc == ',' && bracketOld == bracket)
                                {
                                    if (firstParam)
                                        subFormula = subFormula[1..];
                                    firstParam = false;
                                    args.Add(Parse(subFormula));
                                    subFormula = "";
                                }
                                else
                                {
                                    if (cc == '(') bracket++;
                                    if (cc == ')') bracket--;
                                    subFormula += cc;
                                }
                                i++;
                                if (i < formula.Length) cc = formula[i];
                            } while (bracket >= bracketOld);

                            if (subFormula.StartsWith("("))
                                subFormula = subFormula.Substring(1);
                            subFormula = subFormula.Substring(0, subFormula.Length - 1);
                            args.Add(Parse(subFormula));
                            i--;
                            cc = formula[i];
                            IFormula subFormulaObj = TryMatchFunction(currentName, args.ToArray());

                            objs.Add(subFormulaObj);
                            readingFunc = false;
                            currentName = "";
                            firstParam = true;
                        }
                    }
                    else
                    {
                        if (!abc.Contains(cc) && lastCharIndex == i - 1)
                        {
                            FVariable v = new FVariable();
                            v.Symbol = formula[i - 1];
                            currentName = "";
                            objs.Add(v);
                        }
                        if (!nums.Contains(cc) && cc != '.' && lastNumberIndex == i - 1)
                        {
                            FNumber n = new FNumber();
                            n.Num = ParseNumber(currentNumber);
                            objs.Add(n);
                            currentNumber = "";
                        }
                        if (brOpen.Contains(cc))
                        {
                            char open = cc;
                            char close = ')';
                            int bracketOld = bracket + 1;
                            string subFormula = "";

                            do
                            {
                                if (brOpen.Contains(cc))
                                    bracket++;
                                if (brClose.Contains(cc))
                                {
                                    bracket--;
                                    close = cc;
                                }
                                subFormula += cc;
                                i++;
                                if (i < formula.Length) cc = formula[i];
                            } while (bracket >= bracketOld);

                            subFormula = subFormula[1..^1];
                            IFormula subFormulaObj = TryMatchBracket(open, close, Parse(subFormula));

                            if (subFormulaObj is FNull)
                                subFormulaObj = new FNumber();

                            objs.Add(subFormulaObj);
                            i--;
                            cc = formula[i];
                        }
                        if (opStr.Contains(cc))
                        {
                            objs.Add(cc);
                        }
                        if (abc.Contains(cc))
                        {
                            currentName += cc;
                            if (lastCharIndex == i - 1)
                                readingFunc = true;
                            lastCharIndex = i;
                        }
                        if (nums.Contains(cc) || cc == '.')
                        {
                            currentNumber += cc;
                            lastNumberIndex = i;
                        }
                    }
                }

                if (lastNumberIndex == formula.Length - 1 && currentNumber != "")
                {
                    FNumber fnum = new FNumber();
                    fnum.Num = ParseNumber(currentNumber);
                    objs.Add(fnum);
                }
                if (lastCharIndex == formula.Length - 1)
                {
                    FVariable fvar = new FVariable();
                    fvar.Symbol = formula[formula.Length - 1];
                    objs.Add(fvar);
                }

                if (Settings.InsertLostTimesSymbols)
                {
                    bool operatorRequired = false;

                    for (int i = 0; i < objs.Count; i++)
                    {
                        if (operatorRequired)
                        {
                            if (!(objs[i] is char))
                            {
                                objs.Insert(i, '*');
                            }
                        }
                        operatorRequired = !operatorRequired;
                    }
                }

                List<char> priDot = new List<char>();
                List<char> priDash = new List<char>();

                foreach (var op in Registry.Operators)
                {
                    if (op.Priority == Priority.Dot)
                        priDot.Add(op.OperatorName);
                    else
                        priDash.Add(op.OperatorName);
                }

                int firstDot = FirstIndexOf(objs, priDot);

                while (firstDot != -1)
                {
                    IFormula form = TryMatchOperator((char)objs[firstDot], (IFormula)objs[firstDot - 1], (IFormula)objs[firstDot + 1]);

                    if (form is FNull)
                        throw new FormatException("Unknown formula format!");

                    objs.RemoveRange(firstDot, 2);
                    objs[firstDot - 1] = form;
                    firstDot = FirstIndexOf(objs, priDot);
                }

                int firstDash = FirstIndexOf(objs, priDash);

                while (firstDash != -1)
                {
                    IFormula form = TryMatchOperator((char)objs[firstDash], (IFormula)objs[firstDash - 1], (IFormula)objs[firstDash + 1]);

                    if (form is FNull)
                        throw new FormatException("Unknown formula format!");

                    objs.RemoveRange(firstDash, 2);
                    objs[firstDash - 1] = form;
                    firstDash = FirstIndexOf(objs, priDash);
                }

                if (objs.Count > 1)
                    throw new FormatException("Unknown formula format!");

                return (IFormula)objs[0];
            }
            catch (Exception e)
            {
                throw new FormatException("Unknown formula format!", e);
            }
        }

        public static double ParseNumber(string num)
        {
            string[] parts = num.Split('.');
            if (parts.Length == 1)
                return double.Parse(parts[0]);

            double before = double.Parse(parts[0]);
            double after = double.Parse(parts[1]);
            return before + (after / Math.Pow(10, parts[1].Length));
        }

        public static IFormula TryMatchFunction(string name, IFormula[] args)
        {
            IFormula res = new FNull();
            foreach (var instance in Registry.Functions)
            {
                var formula = instance.Clone() as IFunction;
                if (formula.TryInit(name, args))
                    res = formula;
            }
            return res;
        }

        public static IFormula TryMatchOperator(char symbol, IFormula formula1, IFormula formula2)
        {
            IFormula res = new FNull();
            foreach (var instance in Registry.Operators)
            {
                var formula = instance.Clone() as IOperator;
                if (formula.TryInit(symbol, formula1, formula2))
                    res = formula;
            }
            return res;
        }

        public static IFormula TryMatchBracket(char open, char close, IFormula content)
        {
            IFormula res = new FNull();
            foreach (var instance in Registry.Brackets)
            {
                var formula = instance.Clone() as IBracket;
                if (formula.TryInit(open, close, content))
                    res = formula;
            }
            return res;
        }

        public static int FirstIndexOf(List<object> objs, List<char> search)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                if (objs[i] is char && search.Contains((char)objs[i]))
                    return i;
            }
            return -1;
        }
    }
}