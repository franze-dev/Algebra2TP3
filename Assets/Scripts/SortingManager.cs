using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

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

    #region O(n log^2 n)
    /// <summary>
    /// https://www.geeksforgeeks.org/dsa/shell-sort/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void ShellSort<T>(List<T> list) where T : IComparable
    {
        for (int gap = list.Count / 2; gap > 0; gap /= 2)
        {
            for (int i = gap; i < list.Count; i++)
            {
                T temp = list[i];
                int j;
                for (j = i; j >= gap && list[j - gap].CompareTo(temp) > 0; j -= gap)
                    list[j] = list[j - gap];

                list[j] = temp;
            }
        }
    }

    /// <summary>
    /// https://www.geeksforgeeks.org/dsa/bitonic-sort/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void BitonicSort<T>(List<T> list) where T : IComparable
    {
        BitonicSort(list, 0, list.Count, 1);
    }

    public static void BitonicSort<T>(List<T> list, int low, int count, int dir) where T : IComparable
    {
        if (count > 1)
        {
            int k = count / 2;
            BitonicSort(list, low, k, 1);
            BitonicSort(list, low + k, k, 0);
            BitonicMerge(list, low, count, dir);
        }
    }

    public static void BitonicMerge<T>(List<T> list, int low, int count, int dir) where T : IComparable
    {
        if (count > 1)
        {
            int k = count / 2;
            for (int i = low; i < low + k; i++)
                CompSwap(list, i, i + k, dir);
            BitonicMerge(list, low, k, dir);
            BitonicMerge(list, low + k, k, dir);
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

    private static void CompSwap<T>(List<T> list, int i, int j, int dir) where T : IComparable
    {
        int res = list[i].CompareTo(list[j]) > 0 ? 1 : 0;

        if (dir == res)
            Swap(list, i, j);
    }
    #endregion
}
