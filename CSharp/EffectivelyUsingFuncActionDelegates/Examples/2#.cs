namespace chsarp
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<int, string> m = Test.GetStatus;
            var t = TestFn(m, 10);
        }

        static string TestFn<TParam>(Func<TParam, string> m, TParam p)
        {
            try { return m(p); }
            catch (Exception exception)
            {
                return string.Format("Reserving \"{0}\" failure exception: {1}", p, exception);
            }
        }
    }

    static class Test
    {
        public static string GetStatus(int inp)
        {
            return string.Format("The ReservationID-{0}", inp);
        }
    }
}
