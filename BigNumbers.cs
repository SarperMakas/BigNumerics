using System;
using System.Numerics;


namespace BigNumerics {
    public class BigNumber {

        private BigInteger whole;
        private BigInteger numerator;
        private BigInteger denominator;
        private int Sign;

        public static BigNumber One = new BigNumber(1);
        public static BigNumber MinusOne = new BigNumber(-1);
        public static BigNumber Zero = new BigNumber(0);

        #region Constructors
        public void SetBigNumber(BigNumber num) {
            whole = num.whole;
            numerator = num.numerator;
            denominator = num.denominator;
            Sign = num.Sign;
            Simplify();
        }

        #region BigInteger  (Int, Uint, long)
        // whole numerator/ denominator
        public static BigNumber FromBigInteger(BigInteger whole) {
            BigNumber num = new BigNumber();
            num.whole = whole - 1;
            num.numerator = 1;
            num.denominator = 1;
            num.Sign = whole.Sign;
            num.Simplify();
            return num;
        }
        public static BigNumber FromBigInteger(BigInteger numerator, BigInteger denominator) {
            BigNumber num = new BigNumber();
            num.whole = 0;
            num.numerator = numerator;
            num.denominator = denominator;
            num.Sign = numerator.Sign;

            if (num.denominator == 0) {
                throw new ArgumentException("Numerator musn't be 0");
            }

            num.Simplify();
            return num;
        }
        public static BigNumber FromBigInteger(BigInteger whole, BigInteger numerator, BigInteger denominator) {
            BigNumber num = new BigNumber();
            num.whole = whole;
            num.numerator = numerator;
            num.denominator = denominator;
            if (num.numerator <= 0 || num.denominator <= 0)
                throw new ArgumentException("Numerator or denominator must bigger than 3");
            num.GetSign();
            num.Simplify();
            return num;
        }

        public BigNumber() { }

        public BigNumber(int whole) => SetBigNumber(FromBigInteger(new BigInteger(whole)));
        public BigNumber(int numerator, int denominator) => SetBigNumber(FromBigInteger(new BigInteger(numerator), new BigInteger(denominator)));
        public BigNumber(int whole, int numerator, int denominator) => SetBigNumber(FromBigInteger(new BigInteger(whole), new BigInteger(numerator), new BigInteger(denominator)));

        public BigNumber(uint whole) => SetBigNumber(FromBigInteger(new BigInteger(whole)));
        public BigNumber(uint numerator, uint denominator) => SetBigNumber(FromBigInteger(new BigInteger(numerator), new BigInteger(denominator)));
        public BigNumber(uint whole, uint numerator, uint denominator) => SetBigNumber(FromBigInteger(new BigInteger(whole), new BigInteger(numerator), new BigInteger(denominator)));

        public BigNumber(long whole) => SetBigNumber(FromBigInteger(new BigInteger(whole)));
        public BigNumber(long numerator, long denominator) => SetBigNumber(FromBigInteger(new BigInteger(numerator), new BigInteger(denominator)));
        public BigNumber(long whole, long numerator, long denominator) => SetBigNumber(FromBigInteger(new BigInteger(whole), new BigInteger(numerator), new BigInteger(denominator)));

        public BigNumber(ulong whole) => SetBigNumber(FromBigInteger(new BigInteger(whole)));
        public BigNumber(ulong numerator, ulong denominator) => SetBigNumber(FromBigInteger(new BigInteger(numerator), new BigInteger(denominator)));
        public BigNumber(ulong whole, ulong numerator, ulong denominator) => SetBigNumber(FromBigInteger(new BigInteger(whole), new BigInteger(numerator), new BigInteger(denominator)));
        #endregion

        #region Float
        public BigNumber(float whole) {
            string[] arr;
            if (whole.ToString().Split(',').Length == 1) {
                BigNumber num = FromBigInteger(new BigInteger((int)whole));
                numerator = num.numerator;
                denominator = num.denominator;
                this.whole = num.whole;
                Sign = num.Sign;
                return;
            }

            arr = whole.ToString().Split(',');

            this.whole = BigInteger.Parse(arr[0]);
            this.numerator = BigInteger.Parse(arr[1]);
            this.denominator = new BigInteger(Math.Pow(10, arr[1].Length));

            if (this.numerator <= 0)
                throw new ArgumentException("Numerator or denominator must bigger than 3");
            Sign = GetSign();
            // Console.WriteLine($"Defenition: {ToString()}");
            Simplify();
            // Console.WriteLine($"After Defenition: {ToString()}");
        }
        #endregion
        #region Double
        public BigNumber(double whole) {

            string[] arr;
            if (whole.ToString().Split(',').Length == 1) {
                BigNumber num = FromBigInteger(new BigInteger((int)whole));
                numerator = num.numerator;
                denominator = num.denominator;
                this.whole = num.whole;
                Sign = num.Sign;
                return;
            }

            arr = whole.ToString().Split(',');

            this.whole = BigInteger.Parse(arr[0]);
            this.numerator = BigInteger.Parse(arr[1]);
            this.denominator = new BigInteger(Math.Pow(10, arr[1].Length));
            if (this.numerator <= 0)
                throw new ArgumentException("Numerator or denominator must bigger than 3");
            Sign = GetSign();
            Simplify();
        }
        #endregion
        #region Decimal
        public BigNumber(decimal whole) {

            string[] arr;
            if (whole.ToString().Split(',').Length == 1) {
                BigNumber num = FromBigInteger(new BigInteger((int)whole));
                numerator = num.numerator;
                denominator = num.denominator;
                this.whole = num.whole;
                Sign = num.Sign;
                return;
            }

            arr = whole.ToString().Split(',');

            this.whole = BigInteger.Parse(arr[0]);
            this.numerator = BigInteger.Parse(arr[1]);
            this.denominator = new BigInteger(Math.Pow(10, arr[1].Length));
            if (this.denominator <= 0)
                throw new ArgumentException("Numerator or denominator must bigger than 3");
            Sign = GetSign();
            Simplify();
        }
        #endregion

        #region String
        public BigNumber(string whole, string numerator, string denominator) {
            this.whole = BigInteger.Parse(whole);
            this.numerator = BigInteger.Parse(numerator);
            this.denominator = BigInteger.Parse(denominator);
            if (this.denominator <= 0)
                throw new ArgumentException("Numerator or denominator must bigger than 3");
            Sign = GetSign();
            Simplify();
        }

        public BigNumber(string number, char split = ',') {

            if (number.IndexOf(split) != -1) {
                if (number.Split(split).Length > 2) {
                    throw new ArgumentException("number can contain only 1 split character");
                } else {
                    string[] arr = number.Split(split);
                    whole = BigInteger.Parse(arr[0]);
                    numerator = BigInteger.Parse(arr[1]);
                    denominator = new BigInteger(Math.Pow(10, arr[1].Length));
                    Sign = GetSign();
                    Simplify();
                }
            } else {
                this.whole = BigInteger.Parse(number);
                numerator = 1;
                denominator = 1;
                Sign = GetSign();
                Simplify();
            }


        }
        #endregion
        #endregion

        #region Booleans
        #region Equal 
        public static bool operator ==(BigNumber a, BigNumber b) => Equal(a, b);
        public static bool operator ==(int a, BigNumber b) => Equal(new BigNumber(a), b);
        public static bool operator ==(BigNumber a, int b) => Equal(a, new BigNumber(b));
        public static bool operator ==(uint a, BigNumber b) => Equal(new BigNumber(a), b);
        public static bool operator ==(BigNumber a, uint b) => Equal(a, new BigNumber(b));
        public static bool operator ==(long a, BigNumber b) => Equal(new BigNumber(a), b);
        public static bool operator ==(BigNumber a, long b) => Equal(a, new BigNumber(b));
        public static bool operator ==(float a, BigNumber b) => Equal(new BigNumber(a), b);
        public static bool operator ==(BigNumber a, float b) => Equal(a, new BigNumber(b));
        public static bool operator ==(double a, BigNumber b) => Equal(new BigNumber(a), b);
        public static bool operator ==(BigNumber a, double b) => Equal(a, new BigNumber(b));
        public static bool operator ==(decimal a, BigNumber b) => Equal(new BigNumber(a), b);
        public static bool operator ==(BigNumber a, decimal b) => Equal(a, new BigNumber(b));
        #endregion

        #region Not Equal
        public static bool operator !=(BigNumber a, BigNumber b) => NotEqual(a, b);
        public static bool operator !=(int a, BigNumber b) => NotEqual(new BigNumber(a), b);
        public static bool operator !=(BigNumber a, int b) => NotEqual(a, new BigNumber(b));
        public static bool operator !=(uint a, BigNumber b) => NotEqual(new BigNumber(a), b);
        public static bool operator !=(BigNumber a, uint b) => NotEqual(a, new BigNumber(b));
        public static bool operator !=(long a, BigNumber b) => NotEqual(new BigNumber(a), b);
        public static bool operator !=(BigNumber a, long b) => NotEqual(a, new BigNumber(b));
        public static bool operator !=(float a, BigNumber b) => NotEqual(new BigNumber(a), b);
        public static bool operator !=(BigNumber a, float b) => NotEqual(a, new BigNumber(b));
        public static bool operator !=(double a, BigNumber b) => NotEqual(new BigNumber(a), b);
        public static bool operator !=(BigNumber a, double b) => NotEqual(a, new BigNumber(b));
        public static bool operator !=(decimal a, BigNumber b) => NotEqual(new BigNumber(a), b);
        public static bool operator !=(BigNumber a, decimal b) => NotEqual(a, new BigNumber(b));
        #endregion

        #region Smaller
        public static bool operator <(BigNumber a, BigNumber b) => Smaller(a, b);
        public static bool operator <(int a, BigNumber b) => Smaller(new BigNumber(a), b);
        public static bool operator <(BigNumber a, int b) => Smaller(a, new BigNumber(b));
        public static bool operator <(uint a, BigNumber b) => Smaller(new BigNumber(a), b);
        public static bool operator <(BigNumber a, uint b) => Smaller(a, new BigNumber(b));
        public static bool operator <(long a, BigNumber b) => Smaller(new BigNumber(a), b);
        public static bool operator <(BigNumber a, long b) => Smaller(a, new BigNumber(b));
        public static bool operator <(float a, BigNumber b) => Smaller(new BigNumber(a), b);
        public static bool operator <(BigNumber a, float b) => Smaller(a, new BigNumber(b));
        public static bool operator <(double a, BigNumber b) => Smaller(new BigNumber(a), b);
        public static bool operator <(BigNumber a, double b) => Smaller(a, new BigNumber(b));
        public static bool operator <(decimal a, BigNumber b) => Smaller(new BigNumber(a), b);
        public static bool operator <(BigNumber a, decimal b) => Smaller(a, new BigNumber(b));
        #endregion

        #region Smaller Or Equal
        public static bool operator <=(BigNumber a, BigNumber b) => SmallerOrEqual(a, b);
        public static bool operator <=(int a, BigNumber b) => SmallerOrEqual(new BigNumber(a), b);
        public static bool operator <=(BigNumber a, int b) => SmallerOrEqual(a, new BigNumber(b));
        public static bool operator <=(uint a, BigNumber b) => SmallerOrEqual(new BigNumber(a), b);
        public static bool operator <=(BigNumber a, uint b) => SmallerOrEqual(a, new BigNumber(b));
        public static bool operator <=(long a, BigNumber b) => SmallerOrEqual(new BigNumber(a), b);
        public static bool operator <=(BigNumber a, long b) => SmallerOrEqual(a, new BigNumber(b));
        public static bool operator <=(float a, BigNumber b) => SmallerOrEqual(new BigNumber(a), b);
        public static bool operator <=(BigNumber a, float b) => SmallerOrEqual(a, new BigNumber(b));
        public static bool operator <=(double a, BigNumber b) => SmallerOrEqual(new BigNumber(a), b);
        public static bool operator <=(BigNumber a, double b) => SmallerOrEqual(a, new BigNumber(b));
        public static bool operator <=(decimal a, BigNumber b) => SmallerOrEqual(new BigNumber(a), b);
        public static bool operator <=(BigNumber a, decimal b) => SmallerOrEqual(a, new BigNumber(b));
        #endregion

        #region Bigger
        public static bool operator >(BigNumber a, BigNumber b) => Bigger(a, b);
        public static bool operator >(int a, BigNumber b) => Bigger(new BigNumber(a), b);
        public static bool operator >(BigNumber a, int b) => Bigger(a, new BigNumber(b));
        public static bool operator >(uint a, BigNumber b) => Bigger(new BigNumber(a), b);
        public static bool operator >(BigNumber a, uint b) => Bigger(a, new BigNumber(b));
        public static bool operator >(long a, BigNumber b) => Bigger(new BigNumber(a), b);
        public static bool operator >(BigNumber a, long b) => Bigger(a, new BigNumber(b));
        public static bool operator >(float a, BigNumber b) => Bigger(new BigNumber(a), b);
        public static bool operator >(BigNumber a, float b) => Bigger(a, new BigNumber(b));
        public static bool operator >(double a, BigNumber b) => Bigger(new BigNumber(a), b);
        public static bool operator >(BigNumber a, double b) => Bigger(a, new BigNumber(b));
        public static bool operator >(decimal a, BigNumber b) => Bigger(new BigNumber(a), b);
        public static bool operator >(BigNumber a, decimal b) => Bigger(a, new BigNumber(b));
        #endregion

        #region Bigger Or Equal
        public static bool operator >=(BigNumber a, BigNumber b) => BiggerOrEqual(a, b);
        public static bool operator >=(int a, BigNumber b) => BiggerOrEqual(new BigNumber(a), b);
        public static bool operator >=(BigNumber a, int b) => BiggerOrEqual(a, new BigNumber(b));
        public static bool operator >=(uint a, BigNumber b) => BiggerOrEqual(new BigNumber(a), b);
        public static bool operator >=(BigNumber a, uint b) => BiggerOrEqual(a, new BigNumber(b));
        public static bool operator >=(long a, BigNumber b) => BiggerOrEqual(new BigNumber(a), b);
        public static bool operator >=(BigNumber a, long b) => BiggerOrEqual(a, new BigNumber(b));
        public static bool operator >=(float a, BigNumber b) => BiggerOrEqual(new BigNumber(a), b);
        public static bool operator >=(BigNumber a, float b) => BiggerOrEqual(a, new BigNumber(b));
        public static bool operator >=(double a, BigNumber b) => BiggerOrEqual(new BigNumber(a), b);
        public static bool operator >=(BigNumber a, double b) => BiggerOrEqual(a, new BigNumber(b));
        public static bool operator >=(decimal a, BigNumber b) => BiggerOrEqual(new BigNumber(a), b);
        public static bool operator >=(BigNumber a, decimal b) => BiggerOrEqual(a, new BigNumber(b));
        #endregion
        #endregion


        #region Operations

        #region Addition
        public static BigNumber operator +(BigNumber a) => a;
        public static BigNumber operator ++(BigNumber a) => Add(a, new BigNumber(1));
        public static BigNumber operator +(BigNumber a, BigNumber b) => Add(a, b);
        public static BigNumber operator +(int a, BigNumber b) => Add(new BigNumber(a), b);
        public static BigNumber operator +(BigNumber a, int b) => Add(a, new BigNumber(b));
        public static BigNumber operator +(uint a, BigNumber b) => Add(new BigNumber(a), b);
        public static BigNumber operator +(BigNumber a, uint b) => Add(a, new BigNumber(b));
        public static BigNumber operator +(long a, BigNumber b) => Add(new BigNumber(a), b);
        public static BigNumber operator +(BigNumber a, long b) => Add(a, new BigNumber(b));
        public static BigNumber operator +(float a, BigNumber b) => Add(new BigNumber(a), b);
        public static BigNumber operator +(BigNumber a, float b) => Add(a, new BigNumber(b));
        public static BigNumber operator +(double a, BigNumber b) => Add(new BigNumber(a), b);
        public static BigNumber operator +(BigNumber a, double b) => Add(a, new BigNumber(b));
        public static BigNumber operator +(decimal a, BigNumber b) => Add(new BigNumber(a), b);
        public static BigNumber operator +(BigNumber a, decimal b) => Add(a, new BigNumber(b));
        #endregion

        #region Subtraction
        public static BigNumber operator -(BigNumber a) => a * -1;
        public static BigNumber operator --(BigNumber a) => Subtract(a, new BigNumber(1));
        public static BigNumber operator -(BigNumber a, BigNumber b) => Subtract(a, b);
        public static BigNumber operator -(int a, BigNumber b) => Subtract(new BigNumber(a), b);
        public static BigNumber operator -(BigNumber a, int b) => Subtract(a, new BigNumber(b));
        public static BigNumber operator -(uint a, BigNumber b) => Subtract(new BigNumber(a), b);
        public static BigNumber operator -(BigNumber a, uint b) => Subtract(a, new BigNumber(b));
        public static BigNumber operator -(long a, BigNumber b) => Subtract(new BigNumber(a), b);
        public static BigNumber operator -(BigNumber a, long b) => Subtract(a, new BigNumber(b));
        public static BigNumber operator -(float a, BigNumber b) => Subtract(new BigNumber(a), b);
        public static BigNumber operator -(BigNumber a, float b) => Subtract(a, new BigNumber(b));
        public static BigNumber operator -(double a, BigNumber b) => Subtract(new BigNumber(a), b);
        public static BigNumber operator -(BigNumber a, double b) => Subtract(a, new BigNumber(b));
        public static BigNumber operator -(decimal a, BigNumber b) => Subtract(new BigNumber(a), b);
        public static BigNumber operator -(BigNumber a, decimal b) => Subtract(a, new BigNumber(b));
        #endregion

        #region Multiplycaiton
        public static BigNumber operator *(BigNumber a, BigNumber b) => Multiply(a, b);
        public static BigNumber operator *(int a, BigNumber b) => Multiply(new BigNumber(a), b);
        public static BigNumber operator *(BigNumber a, int b) => Multiply(a, new BigNumber(b));
        public static BigNumber operator *(uint a, BigNumber b) => Multiply(new BigNumber(a), b);
        public static BigNumber operator *(BigNumber a, uint b) => Multiply(a, new BigNumber(b));
        public static BigNumber operator *(long a, BigNumber b) => Multiply(new BigNumber(a), b);
        public static BigNumber operator *(BigNumber a, long b) => Multiply(a, new BigNumber(b));
        public static BigNumber operator *(float a, BigNumber b) => Multiply(new BigNumber(a), b);
        public static BigNumber operator *(BigNumber a, float b) => Multiply(a, new BigNumber(b));
        public static BigNumber operator *(double a, BigNumber b) => Multiply(new BigNumber(a), b);
        public static BigNumber operator *(BigNumber a, double b) => Multiply(a, new BigNumber(b));
        public static BigNumber operator *(decimal a, BigNumber b) => Multiply(new BigNumber(a), b);
        public static BigNumber operator *(BigNumber a, decimal b) => Multiply(a, new BigNumber(b));
        #endregion

        #region Divison
        public static BigNumber operator /(BigNumber a, BigNumber b) => Divide(a, b);
        public static BigNumber operator /(int a, BigNumber b) => Divide(new BigNumber(a), b);
        public static BigNumber operator /(BigNumber a, int b) => Divide(a, new BigNumber(b));
        public static BigNumber operator /(uint a, BigNumber b) => Divide(new BigNumber(a), b);
        public static BigNumber operator /(BigNumber a, uint b) => Divide(a, new BigNumber(b));
        public static BigNumber operator /(long a, BigNumber b) => Divide(new BigNumber(a), b);
        public static BigNumber operator /(BigNumber a, long b) => Divide(a, new BigNumber(b));
        public static BigNumber operator /(float a, BigNumber b) => Divide(new BigNumber(a), b);
        public static BigNumber operator /(BigNumber a, float b) => Divide(a, new BigNumber(b));
        public static BigNumber operator /(double a, BigNumber b) => Divide(new BigNumber(a), b);
        public static BigNumber operator /(BigNumber a, double b) => Divide(a, new BigNumber(b));
        public static BigNumber operator /(decimal a, BigNumber b) => Divide(new BigNumber(a), b);
        public static BigNumber operator /(BigNumber a, decimal b) => Divide(a, new BigNumber(b));
        #endregion

        #region Mod
        public static BigNumber operator %(BigNumber a, BigNumber b) => Mod(a, b);
        public static BigNumber operator %(int a, BigNumber b) => Mod(new BigNumber(a), b);
        public static BigNumber operator %(BigNumber a, int b) => Mod(a, new BigNumber(b));
        public static BigNumber operator %(uint a, BigNumber b) => Mod(new BigNumber(a), b);
        public static BigNumber operator %(BigNumber a, uint b) => Mod(a, new BigNumber(b));
        public static BigNumber operator %(long a, BigNumber b) => Mod(new BigNumber(a), b);
        public static BigNumber operator %(BigNumber a, long b) => Mod(a, new BigNumber(b));
        public static BigNumber operator %(float a, BigNumber b) => Mod(new BigNumber(a), b);
        public static BigNumber operator %(BigNumber a, float b) => Mod(a, new BigNumber(b));
        public static BigNumber operator %(double a, BigNumber b) => Mod(new BigNumber(a), b);
        public static BigNumber operator %(BigNumber a, double b) => Mod(a, new BigNumber(b));
        public static BigNumber operator %(decimal a, BigNumber b) => Mod(new BigNumber(a), b);
        public static BigNumber operator %(BigNumber a, decimal b) => Mod(a, new BigNumber(b));
        #endregion

        #endregion



        #region Funcs
        #region Operations

        public static BigNumber Add(BigNumber a, BigNumber b) {
            // To associative
            int aSign = (a.Sign == 0 || a.Sign == 1) ? 1 : -1;
            int bSign = (b.Sign == 0 || b.Sign == 1) ? 1 : -1;
            a.ToAssociative();
            b.ToAssociative();

            BigInteger a_numerator = BigInteger.Abs(a.numerator) * BigInteger.Abs(b.denominator) * aSign;
            BigInteger b_numerator = BigInteger.Abs(b.numerator) * BigInteger.Abs(a.denominator) * bSign;
            BigInteger new_denominator = a.denominator * b.denominator;


            a_numerator = aSign * BigInteger.Abs(a_numerator);
            b_numerator = bSign * BigInteger.Abs(b_numerator);
            return FromBigInteger(aSign * BigInteger.Abs(a_numerator) + BigInteger.Abs(b_numerator) * bSign, new_denominator);
        }
        public void Add(BigNumber other) {
            SetBigNumber(Add(this, other));
        }
        public static BigNumber Subtract(BigNumber a, BigNumber b) {
            return Add(a, b * -1);
        }
        public void Subtract(BigNumber other) {
            SetBigNumber(Subtract(this, other));
        }
        public static BigNumber Multiply(BigNumber a, BigNumber b) {
            a.ToAssociative();
            b.ToAssociative();
            BigNumber c = FromBigInteger(BigInteger.Abs(a.numerator * b.numerator), BigInteger.Abs(a.denominator * b.denominator));
            c.Sign = ((a.Sign == 0 || a.Sign == 1) ? 1 : -1) * ((b.Sign == 0 || b.Sign == 1) ? 1 : -1);
            return c;
        }
        public void Muliply(BigNumber other) {
            SetBigNumber(Multiply(this, other));
        }
        public static BigNumber Divide(BigNumber a, BigNumber b) {
            a.ToAssociative();
            b.ToAssociative();

            BigNumber c = FromBigInteger(b.denominator, b.numerator);
            return Multiply(a, c);
        }
        public void Divide(BigNumber other) {
            SetBigNumber(Divide(this, other));
        }
        public static BigNumber Mod(BigNumber a, BigNumber b) {
            a.ToAbs();
            b.ToAbs();

            while (a >= b)
                a -= b;

            a.ToAbs();
            return a;
        }

        public static BigNumber Pow(BigNumber a, BigInteger b) {
            BigNumber result = new BigNumber(1);
            for (BigInteger i = new BigInteger(0); i < b; i++) {
                result *= a;
            }
            return result;
        }
        public static BigNumber Pow(BigNumber a, int b) => Pow(a, new BigInteger(b));
        public static BigNumber Pow(BigNumber a, uint b) => Pow(a, new BigInteger(b));
        public static BigNumber Pow(BigNumber a, long b) => Pow(a, new BigInteger(b));

        public void Pow(BigInteger b) => SetBigNumber(Pow(this, b));
        public void Pow(int b) => SetBigNumber(Pow(this, new BigInteger(b)));
        public void Pow(uint b) => SetBigNumber(Pow(this, new BigInteger(b)));
        public void Pow(long b) => SetBigNumber(Pow(this, new BigInteger(b)));


        public static bool Equal(BigNumber a, BigNumber b) {
            a.ToAssociative();
            b.ToAssociative();

            bool w = a.whole == b.whole;
            bool n = a.numerator == b.numerator;
            bool d = a.denominator == b.denominator;

            return w && n && d;
        }
        public static bool NotEqual(BigNumber a, BigNumber b) {
            return !Equal(a, b);
        }
        public static bool Smaller(BigNumber a, BigNumber b) {
            a.ToAssociative();
            b.ToAssociative();

            a.numerator *= b.denominator;
            b.numerator *= a.denominator;

            BigInteger temp = a.denominator;
            a.denominator *= b.denominator;
            b.denominator *= temp;

            return a.numerator < b.numerator;
        }
        public static bool SmallerOrEqual(BigNumber a, BigNumber b) {
            a.ToAssociative();
            b.ToAssociative();

            a.numerator *= b.denominator;
            b.numerator *= a.denominator;

            BigInteger temp = a.denominator;
            a.denominator *= b.denominator;
            b.denominator *= temp;

            return a.numerator <= b.numerator;
        }
        public static bool Bigger(BigNumber a, BigNumber b) {
            a.ToAssociative();
            b.ToAssociative();

            a.numerator *= b.denominator;
            b.numerator *= a.denominator;

            BigInteger temp = a.denominator;
            a.denominator *= b.denominator;
            b.denominator *= temp;

            return a.numerator > b.numerator;
        }
        public static bool BiggerOrEqual(BigNumber a, BigNumber b) {
            a.ToAssociative(); // 10/3
            b.ToAssociative(); // 7/4
            a.numerator *= b.denominator; // 40
            b.numerator *= a.denominator; // 21

            BigInteger temp = a.denominator;
            a.denominator *= b.denominator;
            b.denominator *= temp;
            return a.numerator >= b.numerator;
        }


        #endregion
        private void ToAssociative() {
            numerator = whole * denominator + numerator;
            whole = 0;
        }

        public static BigInteger GreatestCommonFactor(BigInteger a, BigInteger b) {
            while (a != 0 && b != 0) {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }
            return a | b;
        }

        private void Simplify() {
            int whole_sign = (whole.Sign == 0 || whole.Sign == 1) ? 1 : -1;
            int numerator_sign = (numerator.Sign == 0 || numerator.Sign == 1) ? 1 : -1;
            int denominator_sign = (denominator.Sign == 0 || denominator.Sign == 1) ? 1 : -1;
            Sign = whole_sign * numerator_sign * denominator_sign;

            // set new whole
            BigInteger additionToWhole = numerator / denominator;
            whole += additionToWhole;
            numerator %= denominator;

            // simplify numerator and denominator

            if (numerator != 0) {

                BigInteger common = GreatestCommonFactor(BigInteger.Abs(numerator), BigInteger.Abs(denominator));
                numerator /= common;
                denominator /= common;
            }
        }

        public void ToAbs() {
            numerator = BigInteger.Abs(numerator);
            whole = BigInteger.Abs(whole);
            denominator = BigInteger.Abs(denominator);
            Sign = 1;
        }

        public static BigNumber Abs(BigNumber a) {
            a.ToAbs();
            return a;
        }

        public int GetSign() {
            int sign = 0;
            if (whole.Sign != 0 && numerator.Sign != 0 && denominator.Sign != 0) {
                sign = whole.Sign * numerator.Sign * denominator.Sign;
            } else if (whole.Sign != 0 && numerator != 0) {
                sign = whole.Sign * numerator.Sign;
            } else if (whole.Sign != 0 && denominator.Sign != 0) {
                sign = whole.Sign * denominator.Sign;
            } else if (numerator.Sign != 0 && denominator.Sign != 0) {
                sign = numerator.Sign * denominator.Sign;
            }
            return sign;
        }

        public override string ToString() {
            char sign = (Sign == -1) ? '-' : '+';
            Simplify();
            if (whole == 0)
                return $"{sign}{BigInteger.Abs(numerator)}/{BigInteger.Abs(denominator)}";
            if (numerator == 0)
                return $"{sign}{BigInteger.Abs(whole)}";
            return $"{sign}{BigInteger.Abs(whole)}, {BigInteger.Abs(numerator)}/{BigInteger.Abs(denominator)}";
        }

        public override bool Equals(object obj) {
            if (obj is null) {
                return false;
            } else if (obj is BigNumber) {
                BigNumber number = obj as BigNumber;
                return number == this;
            } else {
                return false;
            }
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
        #endregion
    }

}
