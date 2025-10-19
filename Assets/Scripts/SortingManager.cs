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

    #region O(n^2)
    /// <summary>
    /// https://www.geeksforgeeks.org/dsa/gnome-sort-a-stupid-one/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void GnomeSort<T>(List<T> list) where T : IComparable
    {
        int i = 0;

        while (i < list.Count)
        {
            if (i == 0 || list[i - 1].CompareTo(list[i]) <= 0)
                i++;
            else
            {
                Swap(list, i, i - 1);
                i--;
            }
        }
    }
    /// <summary>
    /// https://www.geeksforgeeks.org/dsa/bubble-sort-algorithm/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <exception cref="NotImplementedException"></exception>
    public static void BubbleSort<T>(List<T> list) where T : IComparable
    {
        bool swapped;

        for (int i = 0; i < list.Count - 1; i++)
        {
            swapped = false;

            for (int j = 0; j < list.Count - i - 1; j++)
            {
                if (list[j].CompareTo(list[j + 1]) > 0)
                {
                    Swap(list, j, j + 1);
                    swapped = true;
                }
            }

            if (!swapped)
                break;
        }
    }

    /// <summary>
    /// https://www.geeksforgeeks.org/dsa/cocktail-sort/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void CocktailSort<T>(List<T> list) where T : IComparable
    {
        bool swapped = true;

        var start = 0;
        var end = list.Count;

        while (swapped)
        {
            swapped = false;

            for (int i = start; i < end - 1; ++i)
            {
                if (list[i].CompareTo(list[i + 1]) > 0)
                {
                    Swap(list, i, i + 1);
                    swapped = true;
                }
            }

            if (!swapped)
                break;

            swapped = false;

            end = list.Count - 1;

            for (int i = end - 1; i >= start; i--)
            {
                if (list[i - 1].CompareTo(list[i]) > 0)
                {
                    Swap(list, i, i - 1);
                    swapped = true;
                }
            }

            start++;
        }
    }

    /// <summary>
    /// https://www.geeksforgeeks.org/dsa/insertion-sort-algorithm/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <exception cref="NotImplementedException"></exception>
    public static void InsertionSort<T>(List<T> list) where T : IComparable
    {
        for (int i = 1; i < list.Count; ++i)
        {
            T key = list[i];
            int j = i - 1;

            while (j >= 0 && list[j].CompareTo(key) > 0)
            {
                list[j + 1] = list[j];
                j--;
            }
            list[j + 1] = key;
        }
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

    private static void Swap<T>(List<T> list, int i, int j) where T : IComparable
    {
        T temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }

    #endregion
}
