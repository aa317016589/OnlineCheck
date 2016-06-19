using System;
using System.Collections.Generic;

namespace OnlineCheck
{
    public class OnlineCheckManager
    {
        //加个单一实例

        public Dictionary<Int32, Queue<TeacherCheck>> PressReview; // 回评

        public IEnumerable<QuestionGroup> QuestionGroups;



        public void Init(IEnumerable<QuestionGroup> questionGroups, IEnumerable<Int32> teacherIds)
        {
            PressReview = new Dictionary<int, Queue<TeacherCheck>>();

            foreach (Int32 id in teacherIds)
            {
                PressReview.Add(id, new Queue<TeacherCheck>());
            }

        }



    }

}