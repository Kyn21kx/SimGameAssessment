using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerExpressions : MonoBehaviour
{
    private const int MAX_QUEUE_LENGTH = 4;
    private const float DEFAULT_MAX_TIME_PER_EXPRESSION_S = 1.5f;
    private const string EXPRESSION_ANIM_PARAM = "Expression";

    [SerializeField]
    private Animator m_animator;
    private Queue<FacialExpression> m_expressionQueue;
    private float m_expressionTime;
    private float m_maxTimePerExpression = DEFAULT_MAX_TIME_PER_EXPRESSION_S;


    public bool TryEnqueueExpression(FacialExpression expression)
    {
        if (this.m_expressionQueue.Count >= MAX_QUEUE_LENGTH) return false;

        //Avoid adding the same expression twice, and just increment the duration
        bool hasElement = this.m_expressionQueue.TryPeek(out FacialExpression topElement);
        if (hasElement && topElement == expression)
        {
            //Add the difference of the frame
            this.m_maxTimePerExpression = this.m_expressionTime >= this.m_maxTimePerExpression
                ? this.m_maxTimePerExpression + Time.deltaTime
                : this.m_maxTimePerExpression;
            return false;
        }

        this.m_expressionQueue.Enqueue(expression);
        return true;
    }

    private void Start()
    {
        this.m_expressionTime = 0f;
        this.m_expressionQueue = new Queue<FacialExpression>(MAX_QUEUE_LENGTH);
    }

    private void Update()
    {
        //Do not move forward with expression stuff if there's nothing in the queue
        if (!this.m_expressionQueue.Any())
        {
            this.m_animator.SetInteger(EXPRESSION_ANIM_PARAM, (int)FacialExpression.Normal);
            return;
        }

        //Get the expression, execute it, and if it's passed its time, dequeue
        FacialExpression currExpression = this.m_expressionQueue.Peek();

        this.m_animator.SetInteger(EXPRESSION_ANIM_PARAM, (int)currExpression);

        //Handle the time passing
        this.m_expressionTime += Time.deltaTime;
        if (this.m_expressionTime >= this.m_maxTimePerExpression)
        {
            //Ignore both the out and return, we just don't want exceptions lol
            _ = this.m_expressionQueue.TryDequeue(out _);
            this.m_expressionTime = 0f;
        }
    }

}

