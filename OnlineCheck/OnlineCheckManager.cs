using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace OnlineCheck
{
    public class OnlineCheckManager
    {
        //加个单一实例

        private static OnlineCheckManager _singleton;

        private OnlineCheckManager()
        {
        }

        public static OnlineCheckManager Instance
        {
            get
            {
                if (_singleton == null)
                {
                    Interlocked.CompareExchange(ref _singleton, new OnlineCheckManager(), null);
                }
                return _singleton;
            }
        }


        public Dictionary<Int32, Queue<PressCheck>> PressReview; // 回评

        public IEnumerable<QuestionGroup> QuestionGroups;



        public void Init(IEnumerable<QuestionGroup> questionGroups, IEnumerable<Int32> teacherIds)
        {
            PressReview = new Dictionary<int, Queue<PressCheck>>();

            foreach (Int32 id in teacherIds)
            {
                PressReview.Add(id, new Queue<PressCheck>());
            }


            QuestionGroups = questionGroups;
        }

        private readonly Int32 Count = 10;


        public void Enqueue(Int32 teacherId, PressCheck pressCheck)
        {
            Queue<PressCheck> queue = PressReview[teacherId];

            PressCheck pc = queue.SingleOrDefault(s => s.Id == pressCheck.Id);

            if (pc != null)
            {
                pc.IsOver = true;

                pc.Score = pressCheck.Score;

                return;
            }


            if (queue.Count == Count)
            {
                while (true)
                {
                    PressCheck dpressCheck = queue.Dequeue();

                    if (!dpressCheck.IsOver)
                    {
                        break;
                        ;
                    }
                }
            }

            queue.Enqueue(pressCheck);
        }
    }
}