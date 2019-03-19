using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLOBS2.Models
{
    public class ObservationSummaryData
    {
        public string strEngTask;
        public bool bTradingCardsUsed;
        public bool bTradingCardsMentioned;
        public bool bVideosUsed;
        public bool bVideosMentioned;
        public bool bEngArticleUsed;
        public bool bEngArticleMentioned;
        public bool bClassExamplesUsed;
        public bool bClassExampleMentioned;
        public bool bOtherUsed;
        public bool bOtherMentioned;
        public string strOther;
        public ObservationSummaryData()
        {
            strEngTask = "";
            bTradingCardsUsed = false;
            bTradingCardsMentioned = false;
            bVideosUsed = false;
            bVideosMentioned = false;
            bEngArticleUsed = false;
            bEngArticleMentioned = false;
            bClassExamplesUsed = false;
            bClassExampleMentioned = false;
            bOtherUsed = false;
            bOtherMentioned = false;
            strOther = "";
        }
    }
}
