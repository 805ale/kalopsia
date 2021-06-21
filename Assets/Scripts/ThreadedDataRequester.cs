using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class ThreadedDataRequester : MonoBehaviour
{
    static ThreadedDataRequester instance;
    Queue<ThreadInfo> dataQueue = new Queue<ThreadInfo>();

    private void Awake()
    {
        instance = FindObjectOfType<ThreadedDataRequester>();
    }

    // RequestheightMap method
    // Request map data using threads
    public static void RequestData(Func<object> generateData, Action<object> callback)
    {
        // thread start delegate, it represents the map data thread 
        ThreadStart threadStart = delegate
        {
            instance.DataThread(generateData, callback);
        };

        new Thread(threadStart).Start();

    }


    // heightMapThread method
    void DataThread(Func<object> generateData, Action<object> callback)
    {
        object data = generateData();
        // when one thread reaches this point, no other thread can execute it
        lock (dataQueue)
        {
            // add the heightMap along with the callback to a queue
            dataQueue.Enqueue(new ThreadInfo(callback, data));
        }
    }


    // Update method
    void Update()
    {
        if (dataQueue.Count > 0)
        {
            for (int i = 0; i < dataQueue.Count; i++)
            {
                ThreadInfo threadInfo = dataQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    //MapThreadInfo struct
    // map data and callback variable 
    struct ThreadInfo
    {
        public readonly Action<object> callback;
        public readonly object parameter;

        // constructor
        public ThreadInfo(Action<object> callback, object parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }
    }
}
