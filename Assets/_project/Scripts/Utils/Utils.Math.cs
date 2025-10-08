namespace _project.Scripts.Utils
{
    public static class MathUtils
    {
        public static int Mod(int x, int m) {
            return (x%m + m)%m;
        }
    }
}