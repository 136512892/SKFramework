using System;

namespace SK.Framework.Events
{
    interface IEventComponent
    {
        void Publish(int eventId);
        void Publish<T>(int eventId, T arg);
        void Publish<T1, T2>(int eventId, T1 arg1, T2 arg2);
        void Publish<T1, T2, T3>(int eventId, T1 arg1, T2 arg2, T3 arg3);
        void Publish<T1, T2, T3, T4>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
        void Publish<T1, T2, T3, T4, T5>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
        void Publish<T1, T2, T3, T4, T5, T6>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
        void Publish<T1, T2, T3, T4, T5, T6, T7>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
        void Publish<T1, T2, T3, T4, T5, T6, T7, T8>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
        void Publish<T1, T2, T3, T4, T5, T6, T7, T8, T9>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);
        void Publish<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);

        void Subscribe(int eventId, Action callback);
        void Subscribe<T>(int eventId, Action<T> callback);
        void Subscribe<T1, T2>(int eventId, Action<T1, T2> callback);
        void Subscribe<T1, T2, T3>(int eventId, Action<T1, T2, T3> callback);
        void Subscribe<T1, T2, T3, T4>(int eventId, Action<T1, T2, T3, T4> callback);
        void Subscribe<T1, T2, T3, T4, T5>(int eventId, Action<T1, T2, T3, T4, T5> callback);
        void Subscribe<T1, T2, T3, T4, T5, T6>(int eventId, Action<T1, T2, T3, T4, T5, T6> callback);
        void Subscribe<T1, T2, T3, T4, T5, T6, T7>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7> callback);
        void Subscribe<T1, T2, T3, T4, T5, T6, T7, T8>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7, T8> callback);
        void Subscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback);
        void Subscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback);

        bool Unsubscribe(int eventId, Action callback);
        bool Unsubscribe<T>(int eventId, Action<T> callback);
        bool Unsubscribe<T1, T2>(int eventId, Action<T1, T2> callback);
        bool Unsubscribe<T1, T2, T3>(int eventId, Action<T1, T2, T3> callback);
        bool Unsubscribe<T1, T2, T3, T4>(int eventId, Action<T1, T2, T3, T4> callback);
        bool Unsubscribe<T1, T2, T3, T4, T5>(int eventId, Action<T1, T2, T3, T4, T5> callback);
        bool Unsubscribe<T1, T2, T3, T4, T5, T6>(int eventId, Action<T1, T2, T3, T4, T5, T6> callback);
        bool Unsubscribe<T1, T2, T3, T4, T5, T6, T7>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7> callback);
        bool Unsubscribe<T1, T2, T3, T4, T5, T6, T7, T8>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7, T8> callback);
        bool Unsubscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback);
        bool Unsubscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback);


        void Publish<T>(T e) where T : EventArgs;
        void Subscribe(int eventId, Action<EventArgs> callback);
        bool Unsubscribe(int eventId, Action<EventArgs> callback);
    }
}