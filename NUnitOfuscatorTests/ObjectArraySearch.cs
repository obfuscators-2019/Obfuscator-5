namespace Tests
{
    public class ObjectArraySearch
    {
        object[] _matrix1 = null;

        public ObjectArraySearch(object[] matrix1)
        {
            _matrix1 = matrix1;
        }
        public bool ContentEquals(object[] matrix2)
        {
            if ((!(_matrix1 is null) && matrix2 is null) || (_matrix1 is null && !(matrix2 is null)))
                return false;
            else if (_matrix1 is null && matrix2 is null) return true;

            if (_matrix1.Length != matrix2.Length) return false;

            for (int i = 0; i < matrix2.Length; i++)
                if (!_matrix1[i].Equals(matrix2[i])) return false;

            return true;
        }
    }
}
