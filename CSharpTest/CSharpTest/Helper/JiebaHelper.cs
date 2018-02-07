using IWSData.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Data;
using JiebaNet.Analyser;
using JiebaNet.Segmenter;
using JiebaNet.Segmenter.PosSeg;
using System.Text.RegularExpressions;
using CSharpTest.Tools;
using CSharpTest.Model;

namespace CSharpTest.Helper
{
    public class JiebaHelper
    {
        #region 自动摘要
        /// <summary>
        /// 核心句序号
        /// </summary>
        public List<int> keySentenceList = new List<int>();
        /// <summary>
        /// 关键词列表
        /// </summary>
        public List<string> keywordList = new List<string>();
        /// <summary>
        /// 全文句子列表
        /// </summary>
        public List<string> AllsentenceList = new List<string>();

        /// <summary>
        /// 自动摘要
        /// </summary>
        /// <param name="categoryName">关键词组名</param>
        /// <param name="time">查询时间</param>
        /// <param name="projectId">项目Id</param>
        /// <param name="source">折线图返回自动摘要时使用</param>
        /// <returns></returns>
        public List<string> GetSummary(string categoryName, string time, string projectId,string source)
        {
            List<string> result = new List<string>();


            string text = "";       //自动摘要使用文本
            if (source == null)
            {
                //拆分projectId，判断是否为多项目
                var proObjIds = CommonTools.GetObjIdListFromStr(projectId);
                //获取起止时间
                DateTime endTime = new DateTime();
                DateTime.TryParse(time, out endTime);
                DateTime startTime = endTime.AddDays(-1);

                //获取词组ID
                var builderCatId = Builders<Dnl_KeywordCategory>.Filter;
                var filterCatId = builderCatId.Eq(x => x.Name, categoryName);
                filterCatId &= builderCatId.In(x => x.ProjectId, proObjIds);
                var categoryId = MongoDBHelper.Instance.GetDnl_KeywordCategory().Find(filterCatId).Project(x => x._id).FirstOrDefault();

                //获取关键词ID列表
                var builderKey = Builders<Dnl_KeywordMapping>.Filter;
                var filterKey = builderKey.Eq(x => x.CategoryId, categoryId);
                var keywordList = MongoDBHelper.Instance.GetDnl_KeywordMapping().Find(filterKey).Project(x => x.KeywordId.ToString()).ToList();

                //获取项目内已删除的链接Id
                var builderLinkMap = Builders<Dnl_LinkMapping_Baidu>.Filter;
                var filterLinkMap = builderLinkMap.In(x => x.ProjectId, proObjIds) & builderLinkMap.Eq(x => x.DataCleanStatus, (byte)2);
                var exLinkObjIds = MongoDBHelper.Instance.GetDnl_LinkMapping_Baidu().Find(filterLinkMap).Project(x => x.LinkId).ToList();       //项目中已删除的链接ID列表

                //获取链接标题及摘要
                var builder = Builders<Dnl_Link_Baidu>.Filter;
                var filter = builder.In(x => x.SearchkeywordId, keywordList) & builder.Ne(x => x.PublishTime, "");
                filter &= builder.Nin(x => x._id, exLinkObjIds);
                var queryDatas = MongoDBHelper.Instance.GetDnl_Link_Baidu().Find(filter).Project(x => new
                {
                    Title = x.Title,
                    Description = x.Description,
                    PublishTime = x.PublishTime
                }).ToList();

                //获取起止时间内数据
                List<TimeLink> LinkInfo = new List<TimeLink>();
                foreach (var gr in queryDatas)
                {
                    TimeLink v = new TimeLink();
                    v.Title = gr.Title;
                    v.Description = gr.Description;
                    DateTime dt = new DateTime();
                    DateTime.TryParse(gr.PublishTime, out dt);
                    v.PublishTime = dt;
                    LinkInfo.Add(v);
                }
                LinkInfo = LinkInfo.Where(x => x.PublishTime > startTime && x.PublishTime <= endTime).ToList();
                foreach (var x in LinkInfo)
                {
                    text += x.Title + "。" + System.Environment.NewLine;
                    text += x.Description + "。" + System.Environment.NewLine;
                }
            }
            else
            {
                text = source;
            }
            
            GetList(text, 3, 1);

            foreach (var i in keySentenceList)
            {
                result.Add(AllsentenceList[i]);
            }
            return result;
        }

        #region 函数集合：提取关键词，抽取核心句
        //初始化结巴分词组件
        JiebaSegmenter segmenter = new JiebaSegmenter();

        /// <summary>
        /// 获取num个核心句
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="num">核心句数</param>
        /// <param name="type">抽取类型</param>
        public void GetList(string text, int num, int type)
        {
            keywordList.Clear();

            //获取核心关键词列表
            switch (type)
            {
                case 1:
                    {
                        TfidfExtractor te = new TfidfExtractor();
                        keywordList = te.ExtractTags(text, num).ToList();
                    }
                    break;
                case 2:
                    {
                        TextRankExtractor te = new TextRankExtractor();
                        keywordList = te.ExtractTags(text, num).ToList();
                    }
                    break;
            }

            AllsentenceList.Clear();
            keySentenceList.Clear();


            //将文章拆为句子列表，并分词
            text = text.Replace(Environment.NewLine.ToString(), " 。");
            //text = text.Replace(" ", "");
            AllsentenceList = text.Split('。', '？').Where(x => !string.IsNullOrEmpty(x) && x != "undefined").Select(x => x.Trim()).ToList();
            List<Sentence> temp = new List<Sentence>();
            for (int i = 0; i < AllsentenceList.Count; i++)
            {
                AllsentenceList[i] = AllsentenceList[i] + "。";
                var sentence = segmenter.Cut(AllsentenceList[i]);
                Sentence v = new Sentence();
                v.Sen = string.Join(" ", sentence);
                v.Index = i;
                temp.Add(v);
            }
            GetSentenceList(keywordList, temp);
        }

        /// <summary>
        /// 获取所有关键词对应核心句
        /// </summary>
        /// <param name="keywordList">关键词列表</param>
        /// <param name="sentenceList">全文句子列表</param>
        /// <returns></returns>
        void GetSentenceList(List<string> keywordList, List<Sentence> sentenceList)
        {

            List<Sentence> delList = new List<Sentence>();
            //遍历关键词列表，获取核心句列表
            foreach (var keyword in keywordList)
            {
                //获取包含关键词的核心句
                List<Sentence> tempList = new List<Sentence>();
                foreach (var sentence in sentenceList)
                {
                    if (sentence.Sen.Contains(keyword))
                    {
                        Sentence v = new Sentence();
                        v.Sen = sentence.Sen;
                        v.Index = sentence.Index;
                        tempList.Add(v);
                    }
                }
                //将句子中关键词前面的部分删除
                for (int i = 0; i < tempList.Count; i++)
                {
                    int index = tempList[i].Sen.IndexOf(keyword);
                    tempList[i].Sen = tempList[i].Sen.Substring(index, tempList[i].Sen.Length - index);
                }
                int keySentence = GetKeySentence(keyword, tempList);
                if (keySentence != 10000)
                {
                    keySentenceList.Add(keySentence);
                }
            }

            //对核心句进行排序
            keySentenceList = keySentenceList.OrderBy(x => x).ToList();
        }

        /// <summary>
        /// 获取关键词对应核心句
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <param name="sentenceList">关键词对应的句子词组</param>
        /// <returns></returns>
        int GetKeySentence(string keyword, List<Sentence> sentenceList)
        {
            List<string> nextkeywordList = new List<string>();      //次级关键词
            ////匹配中文，英文字母和数字及下划线
            //Regex reg = new Regex("[\u4e00-\u9fa5_a-zA-Z0-9]+");

            //内容相同的句子数量
            int equalNum = 0;
            for (int i = 0; i < sentenceList.Count; i++)
            {
                if (sentenceList[0].Sen.Equals(sentenceList[i].Sen))
                {
                    equalNum++;
                }
            }
            //如果所有句子都相同，返回第一句
            if (equalNum == sentenceList.Count)
            {
                return ChooseReturnIndex(sentenceList);
            }

            //遍历句子，获取次级关键词列表，并缩短句子
            for (int i = 0; i < sentenceList.Count; i++)
            {
                //MatchCollection mc = reg.Matches(sentenceList[i].Sen);
                var strList = sentenceList[i].Sen.Split(' ').ToList();
                if (strList.Count > 1)
                {
                    //判断前后关键词是否相同，相同则获取下一个次级关键词
                    int keywordNum = 1;
                    while (strList[keywordNum].Equals(keyword))
                    {
                        keywordNum++;
                    }
                    string nextKeyword = strList[keywordNum];
                    //将句子中次级关键词前面的部分删除
                    int index = sentenceList[i].Sen.IndexOf(nextKeyword);
                    sentenceList[i].Sen = sentenceList[i].Sen.Substring(index, sentenceList[i].Sen.Length - index);
                    nextkeywordList.Add(nextKeyword);
                }
                else
                {
                    continue;
                }
            }

            if (nextkeywordList.Count <= 0)
            {
                return ChooseReturnIndex(sentenceList);
            }

            //计算次级关键词列表重复项，排除重复数据
            var keytmp = nextkeywordList.GroupBy(x => x).Select(x => new { Word = x.Key, Count = x.Count() }).ToList().OrderByDescending(x => x.Count).ToList();

            /* 如果次级关键词列表中没有重复项
             * 则认为这些句子出现频率相同
             * 从这些句子中选择一句作为该关键词对应核心句返回 */
            int count = 0;
            foreach (var x in keytmp)
            {
                if (x.Count == 1) count++;
                if (count == keytmp.Count)
                {

                    return ChooseReturnIndex(sentenceList);
                }
            }

            /* 如果句子长度比当前关键词短
             * 则将该句删除 */
            List<Sentence> delList = new List<Sentence>();      //移除句子列表
            for (int i = 0; i < sentenceList.Count; i++)
            {
                Sentence sentence = sentenceList[i];
                string wordtemp = keytmp[0].Word;               //频次最高的次级关键词
                /* 对比关键词与句子长度
                 * 如果句子长度比当前关键词短
                 * 则将该句删除 */
                if (wordtemp.Length > sentence.Sen.Length)
                {
                    delList.Add(sentence);
                }
                else
                {
                    //删除频率最高的次级关键词以外的句子
                    string compareWord = sentence.Sen.Substring(0, wordtemp.Length);
                    if (!wordtemp.Equals(compareWord))
                    {
                        delList.Add(sentence);
                    }
                }
            }
            foreach (var x in delList)
            {
                sentenceList.Remove(x);
            }
            if (sentenceList.Count <= 0) { return 10000; }
            int keySentence = GetKeySentence(keytmp[0].Word, sentenceList);
            return keySentence;
        }

        /// <summary>
        /// 判断应返回的核心句位置
        /// </summary>
        /// <param name="sentenceList">关键词对应的句子词组</param>
        /// <returns></returns>
        int ChooseReturnIndex(List<Sentence> sentenceList)
        {
            //遍历关键词对应的句子词组
            foreach (var sen in sentenceList)
            {
                int count2 = 0;
                /* 遍历已选取的核心句序号列表
                 * 如果关键词对应句组与已选取核心句不重复
                 * 则将返回该句序号 */
                foreach (var v in keySentenceList)
                {
                    if (!AllsentenceList[v].Equals(AllsentenceList[sen.Index]))
                    {
                        count2++;
                    }
                }
                if (count2 == keySentenceList.Count)
                { 
                    return sen.Index;
                }
            }
            //若所有句子均已重复，返回无效序号10000
            return 10000;
        }

        /// <summary>
        /// 句子信息
        /// </summary>
        class Sentence
        {
            /// <summary>
            /// 句子内容
            /// </summary>
            public string Sen { get; set; }
            /// <summary>
            /// 句子序号
            /// </summary>
            public int Index { get; set; }
        }

        class TimeLink
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime PublishTime { get; set; }
        }
        #endregion
        #endregion

        //结巴分词初始化
        PosSegmenter posSegementer = new PosSegmenter();

        /// <summary>
        /// 获取文章内名、动词词频
        /// </summary>
        /// <param name="text">要获取词频的文章</param>
        /// <param name="stopWords">停用词列表</param>
        /// <returns></returns>
        public FrequencyResult GetFrequency(string text, List<string> stopWords)
        {
            //临时数据列表
            List<string> noun = new List<string>();
            List<int> nounCount = new List<int>();
            List<string> verb = new List<string>();
            List<int> verbCount = new List<int>();
            int num = 0;
            int i = 0, j = 0;

            //对数据分词，并统计词频
            var segment = posSegementer.Cut(text).ToList();
            var wordList = segment.GroupBy(x => x.ToString()).Select(x => new { Word = x.Key.ToString(), Count = x.Count() }).OrderByDescending(x => x.Count).ToList();

            //提取名、动词词频
            Regex regN = new Regex("(?<word>[\u4E00-\u9FA5][\u4E00-\u9FA5]+?)/n[a-zA-Z]*");
            Regex regV = new Regex("(?<word>[\u4E00-\u9FA5][\u4E00-\u9FA5]+?)/v[a-zA-Z]*");
            foreach (var wordInfo in wordList)
            {
                if (num == 100) break;
                string word = regN.Match(wordInfo.Word).Groups["word"].Value;
                if (!string.IsNullOrEmpty(word) & i < 100)
                {
                    //排除已停用的词
                    if (stopWords.Contains(word))
                    {
                        continue;
                    }
                    int nc = wordInfo.Count;
                    noun.Add(word);
                    nounCount.Add(nc);
                    i++;
                    num++;
                }
                //else
                //{
                //    word = regV.Match(wordInfo.Word).Groups["word"].Value;
                //    if (!string.IsNullOrEmpty(word) & j < 10)
                //    {
                //        //排除已停用的词
                //        if (stopWords.Contains(word))
                //        {
                //            continue;
                //        }
                //        int nc = wordInfo.Count;
                //        verb.Add(word);
                //        verbCount.Add(nc);
                //        j++;
                //        num++;
                //    }
                //}
            }

            FrequencyResult result = new FrequencyResult();
            result.noun = noun;
            result.nounCount = nounCount;
            result.verb = verb;
            result.verbCount = verbCount;
            return result;
        }

        /// <summary>
        /// 动、名词词频统计结果
        /// </summary>
        public class FrequencyResult
        {
            public List<string> noun { get; set; }
            public List<int> nounCount { get; set; }
            public List<string> verb { get; set; }
            public List<int> verbCount { get; set; }
        }

    }

}