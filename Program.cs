//*****************************************************************************
//** 3013. Divide an Array Into Subarrays With Minimum Cost II      leetcode **
//*****************************************************************************
//** Through shifting bounds where smallest values gleam,                    **
//** Two treaps stand guard, a multiset’s clean dream,                       **
//** Each window whispers sums that softly fall,                             **
//** Till minimum cost answers the final call.                               **
//*****************************************************************************
typedef struct {
    int* data;
    int front, back;
} Deque;

void dequeInit(Deque* dq, int size) {
    dq->data = (int*)malloc(sizeof(int) * size);
    dq->front = 0;
    dq->back = 0;
}

void dequeFree(Deque* dq) {
    free(dq->data);
}

int dequeEmpty(Deque* dq) {
    return dq->front == dq->back;
}

void dequePushBack(Deque* dq, int val) {
    dq->data[dq->back++] = val;
}

void dequePopBack(Deque* dq) {
    dq->back--;
}

int dequeFront(Deque* dq) {
    return dq->data[dq->front];
}

void dequePopFront(Deque* dq) {
    dq->front++;
}

/* ==================== SOLUTION ==================== */
long long minimumCost(int* nums, int numsSize, int k, int dist)
{
    long long* dpPrev = (long long*)malloc(sizeof(long long) * numsSize);
    long long* dpCurr = (long long*)malloc(sizeof(long long) * numsSize);

    for (int i = 0; i < numsSize; i++)
        dpPrev[i] = LLONG_MAX;

    dpPrev[0] = nums[0]; // first pick is always nums[0]

    for (int p = 2; p <= k; p++) // p picks
    {
        Deque dq;
        dequeInit(&dq, numsSize);

        for (int i = 0; i < numsSize; i++)
        {
            // remove elements out of dist window
            while (!dequeEmpty(&dq) && i - dequeFront(&dq) > dist)
                dequePopFront(&dq);

            // maintain monotonic increasing dpPrev values in deque
            while (!dequeEmpty(&dq) && dpPrev[i] <= dpPrev[dq.data[dq.back - 1]])
                dequePopBack(&dq);

            dequePushBack(&dq, i);

            if (!dequeEmpty(&dq) && dpPrev[dequeFront(&dq)] != LLONG_MAX)
                dpCurr[i] = dpPrev[dequeFront(&dq)] + nums[i];
            else
                dpCurr[i] = LLONG_MAX;
        }

        // swap dpPrev and dpCurr
        long long* tmp = dpPrev;
        dpPrev = dpCurr;
        dpCurr = tmp;
    }

    long long retval = LLONG_MAX;
    for (int i = 0; i < numsSize; i++)
        if (dpPrev[i] < retval)
            retval = dpPrev[i];

    free(dpPrev);
    free(dpCurr);

    return retval;
}