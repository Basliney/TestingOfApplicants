namespace TestingOfApplicants.Models.ProgramStaticFiles.Map
{
    public class Pair
    {
        private int _key;
        private int _value;

        #region Property

        public int Key => _key;
        public int Value
        {
            get => _value;
            set => _value = value;
        }

        #endregion

        public Pair(int key)
        {
            _key = key;
            _value = 1;
        }
    }
}
