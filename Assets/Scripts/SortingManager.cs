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
        InsertionSort(list, 0, list.Count - 1);
    }

    public static void InsertionSort<T>(List<T> list, int low, int high) where T : IComparable
    {
        for (int i = low + 1; i <= high; ++i)
        {
            T key = list[i];
            int j = i - 1;

            while (j >= low && list[j].CompareTo(key) > 0)
            {
                list[j + 1] = list[j];
                j--;
            }
            list[j + 1] = key;
        }
    }

    /// <summary>
    /// https://www.geeksforgeeks.org/dsa/selection-sort-algorithm-2/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void SelectionSort<T>(List<T> list) where T : IComparable
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            int minIdx = i;
            for (int j = i + 1; j < list.Count; j++)
                if (list[j].CompareTo(list[minIdx]) < 0)
                    minIdx = j;
            Swap(list, minIdx, i);
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

    private static void BitonicSort<T>(List<T> list, int low, int count, int dir) where T : IComparable
    {
        if (!IsPowerOfTwo(list.Count))
            throw new ArgumentException("BitonicSort: List size must be a power of two.");

        if (count > 1)
        {
            int k = count / 2;
            BitonicSort(list, low, k, 1);
            BitonicSort(list, low + k, k, 0);
            BitonicMerge(list, low, count, dir);
        }
    }

    private static void BitonicMerge<T>(List<T> list, int low, int count, int dir) where T : IComparable
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

    #region O(n log n)
    /// <summary>
    /// https://www.geeksforgeeks.org/dsa/merge-sort/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void MergeSort<T>(List<T> list) where T : IComparable
    {
        MergeSort(list, 0, list.Count - 1);
    }

    private static void MergeSort<T>(List<T> list, int left, int right) where T : IComparable
    {
        if (left < right)
        {
            int mid = (left + (right - 1)) / 2;
            MergeSort(list, left, mid);
            MergeSort(list, mid + 1, right);
            Merge(list, left, mid, right);
        }
    }

    private static void Merge<T>(List<T> list, int left, int mid, int right) where T : IComparable
    {
        int lSize = mid - left + 1;
        int rSize = right - mid;

        T[] Left = new T[lSize];
        T[] Right = new T[rSize];

        for (int i = 0; i < lSize; ++i)
            Left[i] = list[left + i];

        for (int j = 0; j < rSize; ++j)
            Right[j] = list[mid + 1 + j];

        int lIdx = 0;
        int rIdx = 0;

        int mIdx = left;
        while (lIdx < lSize && rIdx < rSize)
        {
            if (Left[lIdx].CompareTo(Right[rIdx]) <= 0)
            {
                list[mIdx] = Left[lIdx];
                lIdx++;
            }
            else
            {
                list[mIdx] = Right[rIdx];
                rIdx++;
            }
            mIdx++;
        }

        while (lIdx < lSize)
        {
            list[mIdx] = Left[lIdx];
            lIdx++;
            mIdx++;
        }

        while (rIdx < rSize)
        {
            list[mIdx] = Right[rIdx];
            rIdx++;
            mIdx++;
        }
    }

    /// <summary>
    /// https://www.geeksforgeeks.org/dsa/heap-sort/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void HeapSort<T>(List<T> list) where T : IComparable
    {
        HeapSort(list, 0, list.Count);
    }

    public static void HeapSort<T>(List<T> list, int low, int high) where T : IComparable
    {
        for (int i = (high - low) / 2 - 1 + low; i >= low; i--)
            Heapify(list, high, i);
        for (int i = high - 1; i > low; i--)
        {
            Swap(list, low, i);
            Heapify(list, i, low);
        }
    }

    private static void Heapify<T>(List<T> list, int length, int i) where T : IComparable
    {
        int largest = i;

        int left = 2 * i + 1;

        int right = 2 * i + 2;

        if (left < length && list[left].CompareTo(list[largest]) > 0)
            largest = left;

        if (right < length && list[right].CompareTo(list[largest]) > 0)
            largest = right;

        if (largest != i)
        {
            Swap(list, i, largest);
            Heapify(list, length, largest);
        }
    }

    /// <summary>
    /// https://www.geeksforgeeks.org/dsa/quick-sort-algorithm/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <exception cref="NotImplementedException"></exception>
    public static void QuickSort<T>(List<T> list) where T : IComparable
    {
        QuickSort(list, 0, list.Count - 1);
    }

    private static void QuickSort<T>(List<T> list, int low, int high) where T : IComparable
    {
        if (low < high)
        {
            int pi = Partition(list, low, high);
            QuickSort(list, low, pi - 1);
            QuickSort(list, pi + 1, high);
        }
    }

    private static int Partition<T>(List<T> list, int low, int high) where T : IComparable
    {
        T pivot = list[high];
        int i = (low - 1);

        for (int j = low; j < high; j++)
            if (list[j].CompareTo(pivot) < 0)
            {
                i++;
                Swap(list, i, j);
            }

        Swap(list, i + 1, high);
        return i + 1;
    }

    /// <summary>
    /// https://www.geeksforgeeks.org/dsa/introsort-or-introspective-sort/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <exception cref="NotImplementedException"></exception>
    public static void IntroSort<T>(List<T> list) where T : IComparable
    {
        int depthLimit = 2 * (int)Mathf.Floor(Mathf.Log(list.Count) / Mathf.Log(2));

        IntroSort(list, 0, list.Count - 1, depthLimit);
    }

    private static void IntroSort<T>(List<T> list, int low, int high, int depthLimit) where T : IComparable
    {
        if (high - low > 16)
        {
            if (depthLimit == 0)
            {
                HeapSort(list, low, high - 1);
                return;
            }

            depthLimit--;

            int pivot = FindPivot(list, low, low + ((high - low) / 2) + 1, high);

            Swap(list, pivot, high);

            int p = Partition(list, low, high);

            IntroSort(list, low, p - 1, depthLimit);
            IntroSort(list, p + 1, high, depthLimit);
        }
        else
            InsertionSort(list, low, high);
    }

    private static int FindPivot<T>(List<T> list, int i, int j, int k) where T : IComparable
    {
        T a = list[i];
        T b = list[j];
        T c = list[k];

        if ((a.CompareTo(b) > 0 && a.CompareTo(c) < 0) ||
            (a.CompareTo(b) < 0 && a.CompareTo(c) > 0))
            return i;
        else if ((b.CompareTo(a) > 0 && b.CompareTo(c) < 0) ||
                 (b.CompareTo(a) < 0 && b.CompareTo(c) > 0))
            return j;
        else
            return k;
    }

    /// <summary>
    /// https://www.geeksforgeeks.org/dsa/timsort/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void AdaptiveMergeSort<T>(List<T> list) where T : IComparable
    {
        const int RUN = 32;

        AdaptiveMergeSort(list, RUN);
    }

    private static void AdaptiveMergeSort<T>(List<T> list, int chunkSize) where T : IComparable
    {
        for (int i = 0; i < list.Count; i += chunkSize)
            InsertionSort(list, i, Mathf.Min(i + chunkSize - 1, list.Count - 1));

        for (int size = chunkSize; size < list.Count; size *= 2)
        {
            for (int left = 0; left < list.Count; left += size * 2)
            {
                int mid = left + size - 1;
                int right = Mathf.Min(left + 2 * size - 1, list.Count - 1);

                if (mid < right)
                    Merge(list, left, mid, right);
            }
        }
    }

    #endregion
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
