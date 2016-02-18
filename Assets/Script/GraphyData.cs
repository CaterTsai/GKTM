using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphyData
{
    #region Enum
    public enum eVertexType
    {
        eVertex_Normal = 0
        ,eVertex_Order1
        ,eVertex_Order2
        ,eVertex_Order3
        ,eVertex_Order4
    }
    #endregion

    public class GraphyData
    {
        #region Element
        private int _VertexNum = 0;
        public Dictionary<int, List<int>> _EdgeMap = new Dictionary<int, List<int>>(); //for checking connection        
        #endregion

        #region Property
        public int VertexNum { get { return _VertexNum; } }        
        #endregion

        #region Basic Method
        public GraphyData()
        {
        }
        #endregion

        #region Method
        //---------------------------------------------------
        public void removeVertex(int id)
        {
            _VertexNum--;

            var vList_ = _EdgeMap[id];

            foreach (var vid_ in vList_)
            {
                _EdgeMap[vid_].RemoveAll(item => item == id);
            }
            _EdgeMap.Remove(id);
        }

        //---------------------------------------------------
        public void removeEdge(int from, int to)
        {
            _EdgeMap[from].RemoveAll(item => item == to);
            _EdgeMap[to].RemoveAll(item => item == from);
        }

        //---------------------------------------------------
        public void generalK33()
        {
            //DEBUG
            _VertexNum = 6;

            //edge map
            for (int idx_ = 0; idx_ < 6; idx_++)
            {
                List<int> edgeList_ = new List<int>();

                if (idx_ < 3)
                {
                    edgeList_.Add(3);
                    edgeList_.Add(4);
                    edgeList_.Add(5);
                }
                else
                {
                    edgeList_.Add(0);
                    edgeList_.Add(1);
                    edgeList_.Add(2);
                }
                _EdgeMap.Add(idx_, edgeList_);
            }
            
        }
        #endregion

    }
}
