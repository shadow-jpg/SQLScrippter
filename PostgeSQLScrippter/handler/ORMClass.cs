using System.Text;

namespace SqlScrippter.handler
{
    internal class ORMClass
    {
        public ORMClass()
        {
        }
        public class INT
        {
            private int? value = null;
            public INT()
            {

            }
            public INT(int value)
            {
                this.value = value;
            }

        }
        public class SHORT
        {
            private short? value = null;
            public SHORT()
            {

            }
            public SHORT(short value)
            {
                this.value = value;
            }

        }
        public class DOUBLE
        {
            private double? value = null;
            public DOUBLE()
            {

            }
            public DOUBLE(double value)
            {
                this.value = value;
            }

        }
        public class VARСHAR
        {
            private StringBuilder? value = null;
            public VARСHAR()
            {

            }
            public VARСHAR(StringBuilder value)
            {
                this.value = value;
            }
            public void add(StringBuilder value)
            {
                this.value.Append(value);
            }
        }
        public class TIME
        {
            private DateTime value = new DateTime();
            private bool isNull = true;
            public TIME()
            {
                isNull = true;
            }
            public TIME(DateTime? value)
            {
                if (value != null)
                    this.value = value.GetValueOrDefault();
            }
            public void addHours(int hours)
            {
                if (!isNull)
                    value.AddHours(hours);
            }
            public void addDays(int days)
            {
                if (!isNull)
                    value.AddDays(days);
            }
            public void addMinutes(int minutes)
            {
                if (!isNull)
                    value.AddMinutes(minutes);
            }
        }
    }
}
