using System;
using System.Collections.Generic;

public static class SortingManager
{
    #region O(n!)
    /// <summary>
    /// https://es.wikipedia.org/wiki/Stupid_sort
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void BogoSort<T>(List<T> list) where T : IComparable
    {
        while (!IsSorted(list))
            Shuffle(list);
    }
    #endregion

    
    #region Utils
    private static void Shuffle<T>(List<T> list) where T : IComparable
    {
        System.Random rng = new();
        int n = list.Count;

        for (int i = n - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);

            T old = list[i];
            list[i] = list[j];
            list[j] = old;
        }
    }

    private static bool IsSorted<T>(List<T> list) where T : IComparable
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            if (list[i].CompareTo(list[i + 1]) > 0)
                return false;
        }
        return true;
    }

    
    #endregion
}
